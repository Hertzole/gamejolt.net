#nullable enable

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	public sealed class GameJoltFriends
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;
		private readonly GameJoltUsers users;

		internal GameJoltFriends(IGameJoltWebClient webClient, IGameJoltSerializer serializer, GameJoltUsers users)
		{
			this.webClient = webClient;
			this.serializer = serializer;
			this.users = users;
		}

		internal const string ENDPOINT = "friends/";

		public async Task<GameJoltResult<int[]>> FetchAsync(CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult<int[]> result))
			{
				return result;
			}

			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(ENDPOINT);
				sb.Append("?username=");
				sb.Append(users.myUsername);
				sb.Append("&user_token=");
				sb.Append(users.myToken);

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken).ConfigureAwait(false);
				FetchFriendsResponse response = serializer.Deserialize<FetchFriendsResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<int[]>.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful, but Success was false.");

				int[] ids = response.friends.Length > 0 ? new int[response.friends.Length] : Array.Empty<int>();

				for (int i = 0; i < response.friends.Length; i++)
				{
					ids[i] = response.friends[i].id;
				}

				return GameJoltResult<int[]>.Success(ids);
			}
		}
	}
}