#nullable enable

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Used to get information about the friends of the authenticated user.
	/// </summary>
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

		/// <summary>
		///     List all the friends of the authenticated user. This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and a list of the user's friends.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult<int[]>> GetFriendsAsync(CancellationToken cancellationToken = default)
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

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				FetchFriendsResponse response = serializer.DeserializeResponse<FetchFriendsResponse>(json);

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