#if !UNITY_2021_1_OR_NEWER
using System.Net.Http;
using System.Threading;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif

namespace Hertzole.GameJolt
{
	internal sealed class StandardWebClient : IGameJoltWebClient
	{
		private readonly HttpClient client = new HttpClient();

		public async StringTask GetStringAsync(string url, CancellationToken cancellationToken)
		{
			string signedUrl = GameJoltUrlBuilder.BuildUrl(url);
            
			string response = await client.GetStringAsync(signedUrl, cancellationToken).ConfigureAwait(false);
			if (string.IsNullOrEmpty(response))
			{
				throw new GameJoltException("Response was empty.");
			}

			return response!;
		}

		public void Dispose()
		{
			client.Dispose();
		}
	}
}
#endif