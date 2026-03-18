#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Used to get and manage users.
	/// </summary>
	public sealed class GameJoltUsers
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;

		internal string? myUsername;
		internal string? myToken;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
		private static readonly string[] credentialsSplit = { "\n", "\r\n" };
#else
		private static readonly string[][] credentialsSplit =
		{
			new[] { "\n" },
			new[] { "\r\n" }
		};
#endif

		/// <summary>
		///     Gets if the user is authenticated.
		/// </summary>
		[MemberNotNullWhen(true, nameof(myUsername), nameof(myToken))]
		public bool IsAuthenticated { get; private set; }

		/// <summary>
		///     Gets the current user. This is only set if the user is authenticated. If the user is not authenticated, this will
		///     be null.
		/// </summary>
		public GameJoltUser? CurrentUser { get; private set; }
		internal const string ENDPOINT = "users/";
		internal const string AUTH_ENDPOINT = ENDPOINT + "auth/";

		/// <summary>
		///     Called when the user is authenticated.
		/// </summary>
		public event Action<GameJoltUser>? OnUserAuthenticated;

		internal GameJoltUsers(IGameJoltWebClient webClient, IGameJoltSerializer serializer)
		{
			this.webClient = webClient;
			this.serializer = serializer;
		}

		/// <summary>
		///     Authenticates the user with the given username and token. This method will also fetch the user's data and set the
		///     <see cref="CurrentUser" /> property if successful.
		/// </summary>
		/// <param name="username">The user's username.</param>
		/// <param name="token">The user's token.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">
		///     Thrown if <paramref name="username" /> or <paramref name="token" /> is empty or whitespace.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///     Thrown if <paramref name="username" /> or <paramref name="token" /> is <see langword="null" />.
		/// </exception>
		public async Task<GameJoltResult> AuthenticateAsync(string username, string token, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNullOrWhiteSpace(username, nameof(username));
			Guard.IsNotNullOrWhiteSpace(token, nameof(token));

			myUsername = username;
			myToken = token;

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(AUTH_ENDPOINT + "?username=");
				builder.Append(username);
				builder.Append("&user_token=");
				builder.Append(token);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);

				Response response = serializer.DeserializeResponse<Response>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				GameJoltResult<GameJoltUser> fetchResponse = await GetUserAsync(myUsername, cancellationToken);
				if (!fetchResponse.HasError)
				{
					CurrentUser = fetchResponse.Value;
					IsAuthenticated = true;
					OnUserAuthenticated?.Invoke(fetchResponse.Value);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Authenticates the user from a URL. The URL must contain the query parameters <c>qjapi_username</c> and
		///     <c>gjapi_token</c>.
		///     This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if successful.
		/// </summary>
		/// <param name="url">The URL to authenticate from.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="url" /> is empty or whitespace.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="url" /> is <see langword="null" />.</exception>
		/// <exception cref="FormatException">Thrown if <paramref name="url" /> is not a valid URL.</exception>
		public Task<GameJoltResult> AuthenticateFromUrlAsync(string url, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNullOrWhiteSpace(url, nameof(url));

			return AuthenticateFromUrlAsync(new Uri(url), cancellationToken);
		}

		/// <summary>
		///     Authenticates the user from a URL. The URL must contain the query parameters <c>qjapi_username</c> and
		///     <c>gjapi_token</c>.
		///     This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if successful.
		/// </summary>
		/// <param name="url">The URL to authenticate from.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">Returned if <paramref name="url" /> is invalid.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="url" /> is <see langword="null" />.</exception>
		public async Task<GameJoltResult> AuthenticateFromUrlAsync(Uri url, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(url, nameof(url));

			if ((url.Host.EndsWith("gamejolt.com", StringComparison.OrdinalIgnoreCase) || url.Host.EndsWith("gamejolt.net", StringComparison.OrdinalIgnoreCase))
			    && QueryParser.TryGetToken(url.Query, "gjapi_username", out string? username) &&
			    QueryParser.TryGetToken(url.Query, "gjapi_token", out string? token))
			{
				return await AuthenticateAsync(username, token, cancellationToken);
			}

			return GameJoltResult.Error(new ArgumentException("Invalid URL.", nameof(url)));
		}

		/// <summary>
		///     Authenticates the user from a credentials file. The file must contain the username on the second line and the token
		///     on the third line. This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if
		///     successful.
		/// </summary>
		/// <param name="gjCredentialsContent">The credentials file content.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="gjCredentialsContent" /> is empty or whitespace.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="gjCredentialsContent" /> is <see langword="null" />.</exception>
		public Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(string gjCredentialsContent, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNullOrWhiteSpace(gjCredentialsContent, nameof(gjCredentialsContent));

			string[] lines = Array.Empty<string>();

			// We may need to split on \r\n instead of just \n. So we try both.
			for (int i = 0; i < credentialsSplit.Length; i++)
			{
				lines = gjCredentialsContent.Split(credentialsSplit[i], StringSplitOptions.RemoveEmptyEntries);
				if (lines.Length >= 3)
				{
					break;
				}
			}

			return AuthenticateFromCredentialsFileAsync(lines, cancellationToken);
		}

		/// <summary>
		///     Authenticates the user from a credentials file. The file must contain the username on the second line and the token
		///     on the third line. This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if
		///     successful.
		/// </summary>
		/// <param name="lines">The credentials file lines.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="lines" /> is empty or has less than 3 lines.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="lines" /> is <see langword="null" />.</exception>
		public async Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(string[] lines, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNullOrEmpty(lines, nameof(lines));
			Guard.HasSizeGreaterThanOrEqualTo(lines, 3, nameof(lines));

			string username = lines[1];
			string token = lines[2];

			return await AuthenticateAsync(username, token, cancellationToken);
		}

		/// <summary>
		///     Fetches the user with the given username. This method does not require the user to be authenticated.
		/// </summary>
		/// <param name="username">The username of the user whose data you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the user's data.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="username" /> is empty or whitespace.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="username" /> is <see langword="null" />.</exception>
		public async Task<GameJoltResult<GameJoltUser>> GetUserAsync(string username, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNullOrWhiteSpace(username, nameof(username));

			return await GetUserAsync(username, null, cancellationToken);
		}

		/// <summary>
		///     Fetches the user with the given user ID. This method does not require the user to be authenticated.
		/// </summary>
		/// <param name="userId">The user ID of the user whose data you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the user's data.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		public async Task<GameJoltResult<GameJoltUser>> GetUserAsync(int userId, CancellationToken cancellationToken = default)
		{
			return await GetUserAsync(null, userId, cancellationToken);
		}

		private async Task<GameJoltResult<GameJoltUser>> GetUserAsync(string? username, int? userId, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);

				if (username != null)
				{
					builder.Append("?username=");
					builder.Append(username);
				}

				if (userId != null)
				{
					builder.Append("?user_id=");
					builder.Append(userId.Value);
				}

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				UsersFetchResponse response = serializer.DeserializeResponse<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser>.Error(exception);
				}

				if (response.Users.Length == 0)
				{
					return GameJoltResult<GameJoltUser>.Error(new GameJoltInvalidUserException());
				}

				return GameJoltResult<GameJoltUser>.Success(response.Users[0].ToPublicUser());
			}
		}

		/// <summary>
		///     Fetches the users with the given usernames. This method does not require the user to be authenticated.
		/// </summary>
		/// <param name="usernames">The usernames of the users whose data you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns> The result of the request and the users' data.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="usernames" /> is <see langword="null" />.</exception>
		public async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(IEnumerable<string> usernames, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(usernames, nameof(usernames));

			using (ListPool<GameJoltUser>.Rent(out List<GameJoltUser> results))
			{
				GameJoltResult result = await GetUsersInternalAsync(usernames, null, results, cancellationToken);

				if (result.HasError)
				{
					return GameJoltResult<GameJoltUser[]>.Error(result.Exception);
				}

				return GameJoltResult<GameJoltUser[]>.Success(results.ToArray());
			}
		}

		/// <summary>
		///     Fetches the users with the given usernames and adds them to the provided <paramref name="results" /> list. This
		///     method does not require the user to be authenticated.
		/// </summary>
		/// <param name="usernames">The usernames of the users whose data you'd like to fetch.</param>
		/// <param name="results">The results buffer where the users will be added to.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns> The result of the request and the users' data.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentNullException">
		///     Thrown if <paramref name="usernames" /> or <paramref name="results" /> is <see langword="null" />.
		/// </exception>
		public async Task<GameJoltResult> GetUsersAsync(IEnumerable<string> usernames,
			IList<GameJoltUser> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(usernames, nameof(usernames));
			Guard.IsNotNull(results, nameof(results));

			return await GetUsersInternalAsync(usernames, null, results, cancellationToken);
		}

		/// <summary>
		///     Fetches the users with the given user IDs. This method does not require the user to be authenticated.
		/// </summary>
		/// <param name="userIds">The user IDs of the users whose data you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns> The result of the request and the users' data.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="userIds" /> is <see langword="null" />.</exception>
		public async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(userIds, nameof(userIds));

			using (ListPool<GameJoltUser>.Rent(out List<GameJoltUser> results))
			{
				GameJoltResult result = await GetUsersInternalAsync(null, userIds, results, cancellationToken);

				if (result.HasError)
				{
					return GameJoltResult<GameJoltUser[]>.Error(result.Exception);
				}

				return GameJoltResult<GameJoltUser[]>.Success(results.ToArray());
			}
		}

		/// <summary>
		///     Fetches the users with the given user IDs and adds them to the provided <paramref name="results" /> list. This
		///     method does not require the user to be authenticated.
		/// </summary>
		/// <param name="userIds">The user IDs of the users whose data you'd like to fetch.</param>
		/// <param name="results">The results buffer where the users will be added to.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns> The result of the request and the users' data.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentNullException">
		///     Thrown if <paramref name="userIds" /> or <paramref name="results" /> is <see langword="null" />.
		/// </exception>
		public async Task<GameJoltResult> GetUsersAsync(IEnumerable<int> userIds, IList<GameJoltUser> results, CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(userIds, nameof(userIds));
			Guard.IsNotNull(results, nameof(results));

			return await GetUsersInternalAsync(null, userIds, results, cancellationToken);
		}

		private async Task<GameJoltResult> GetUsersInternalAsync(IEnumerable<string>? usernames,
			IEnumerable<int>? userIds,
			IList<GameJoltUser> buffer,
			CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);

				if (usernames != null)
				{
					builder.Append("?username=");
					builder.Append(usernames.ToCommaSeparatedString());
				}

				if (userIds != null)
				{
					builder.Append("?user_id=");
					builder.Append(userIds.ToCommaSeparatedString());
				}

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				UsersFetchResponse response = serializer.DeserializeResponse<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				if (response.Users.Length == 0)
				{
					return GameJoltResult.Error(new GameJoltInvalidUserException());
				}

				buffer.ClearAndEnsureCapacity(response.Users.Length);

				for (int i = 0; i < response.Users.Length; i++)
				{
					buffer.Add(response.Users[i].ToPublicUser());
				}

				return GameJoltResult.Success();
			}
		}

		[MemberNotNullWhen(false, nameof(myUsername), nameof(myToken))]
		internal bool IsNotAuthenticated([NotNullWhen(true)] out Exception? exception)
		{
			if (!IsAuthenticated)
			{
				exception = new GameJoltAuthorizedException();
				return true;
			}

			exception = null;
			return false;
		}

		internal void Shutdown()
		{
			CurrentUser = null;
			myUsername = null;
			myToken = null;
			IsAuthenticated = false;
		}
	}
}
#endif // DISABLE_GAMEJOLT