using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif

namespace Hertzole.GameJolt
{
	internal sealed partial class GameJoltWebClient : IGameJoltWebClient
	{
		private const string BASE_URL = "https://api.gamejolt.com/api/game/v1_2/";

		public async StringTask GetStringAsync(string url, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(BASE_URL);
				builder.Append(url);
#if NETSTANDARD2_1_OR_GREATER || UNITY_2021_3_OR_NEWER || NET5_0_OR_GREATER
				builder.Append(!url.Contains("/?", StringComparison.OrdinalIgnoreCase) ? '?' : '&');
#else
				builder.Append(!url.Contains("/?") ? '?' : '&');
#endif
				builder.Append("game_id=");
				builder.Append(GameJoltAPI.GameId);
				builder.Append("&format=json");

				string baseString = builder.ToString();

				builder.Append(GameJoltAPI.PrivateKey);

				using (MD5 md5 = MD5.Create())
				{
					byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
					builder.Clear();

					builder.Append(baseString);
					builder.Append("&signature=");

					for (int i = 0; i < hash.Length; i++)
					{
						builder.Append(hash[i].ToString("x2"));
					}
				}

				return await SendGetRequestAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
			}
		}

		private partial StringTask SendGetRequestAsync(string url, CancellationToken cancellationToken);

		public void Dispose()
		{
#if !UNITY_2021_1_OR_NEWER
			client.Dispose();
#endif
		}
	}
}