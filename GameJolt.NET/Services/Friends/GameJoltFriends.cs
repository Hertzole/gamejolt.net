#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections.Generic;
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
			using (ListPool<int>.Rent(out List<int> results))
			{
				GameJoltResult result = await GetFriendsInternalAsync(results, cancellationToken).ConfigureAwait(false);
				if (result.HasError)
				{
					return GameJoltResult<int[]>.Error(result.Exception);
				}

				return GameJoltResult<int[]>.Success(results.ToArray());
			}
		}

		/// <summary>
		///     Lists all the friends of the authenticated user and adds their IDs to the provided <paramref name="results" />
		///     list. This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="results">The results buffer where the IDs will be added to. This will be cleared before use.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="results" /> is <see langword="null"/>.</exception>
		public Task<GameJoltResult> GetFriendsAsync(IList<int> results, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(results, nameof(results));

			return GetFriendsInternalAsync(results, cancellationToken);
		}

		private async Task<GameJoltResult> GetFriendsInternalAsync(IList<int> results, CancellationToken cancellationToken = default)
		{
			if (users.IsNotAuthenticated(out Exception? authException))
			{
				return GameJoltResult.Error(authException);
			}

			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(ENDPOINT + "?username=");
				sb.Append(users.myUsername);
				sb.Append("&user_token=");
				sb.Append(users.myToken);

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				FetchFriendsResponse response = serializer.DeserializeResponse<FetchFriendsResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				Debug.Assert(response.Success, "Response was successful, but Success was false.");

				results.ClearAndEnsureCapacity(response.friends.Length);
				for (int i = 0; i < response.friends.Length; i++)
				{
					results.Add(response.friends[i].id);
				}

				return GameJoltResult.Success();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT