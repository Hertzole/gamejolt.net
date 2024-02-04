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
	///     Used to get and manage users.
	/// </summary>
	public sealed class GameJoltUsers
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;

		internal string? myUsername;
		internal string? myToken;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
		private static readonly string[] credentialsSplit = { "\n", Environment.NewLine };
#else
		private static readonly string[][] credentialsSplit =
		{
			new[] { "\n" },
			new[] { Environment.NewLine }
		};
#endif

		/// <summary>
		///     Gets if the user is authenticated.
		/// </summary>
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
		public async Task<GameJoltResult> AuthenticateAsync(string username, string token, CancellationToken cancellationToken = default)
		{
			myUsername = username;
			myToken = token;

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(AUTH_ENDPOINT);
				builder.Append("?username=");
				builder.Append(username);
				builder.Append("&user_token=");
				builder.Append(token);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);

				AuthResponse response = serializer.Deserialize<AuthResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
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
		///     Authenticates the user from a URL. The URL must contain the query parameters 'qjapi_username' and 'gjapi_token'.
		///     This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if successful.
		/// </summary>
		/// <param name="url">The URL to authenticate from.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		public Task<GameJoltResult> AuthenticateFromUrlAsync(string url, CancellationToken cancellationToken = default)
		{
			return AuthenticateFromUrlAsync(new Uri(url), cancellationToken);
		}

		/// <summary>
		///     Authenticates the user from a URL. The URL must contain the query parameters 'qjapi_username' and 'gjapi_token'.
		///     This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if successful.
		/// </summary>
		/// <param name="url">The URL to authenticate from.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		public async Task<GameJoltResult> AuthenticateFromUrlAsync(Uri url, CancellationToken cancellationToken = default)
		{
			if ((url.Host.EndsWith("gamejolt.com", StringComparison.OrdinalIgnoreCase) || url.Host.EndsWith("gamejolt.net", StringComparison.OrdinalIgnoreCase))
			    && QueryParser.TryGetToken(url.Query, "gjapi_username", out string? username) &&
			    QueryParser.TryGetToken(url.Query, "gjapi_token", out string? token))
			{
				return await AuthenticateAsync(username!, token!, cancellationToken);
			}

			return GameJoltResult.Error(new ArgumentException("Invalid URL.", nameof(url)));
		}

		/// <summary>
		///     Authenticates the user from a credentials file. The file must contain the username on the second line and the token
		///     on the third line. This method will also fetch the user's data and set the <see cref="CurrentUser" /> property if
		///     successful.
		/// </summary>
		/// <param name="gjCredentials">The credentials file content.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		/// <exception cref="ArgumentException">Returned if the credentials file is invalid.</exception>
		public Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(string gjCredentials, CancellationToken cancellationToken = default)
		{
			string[] lines = Array.Empty<string>();

			// We may need to split on \r\n instead of just \n. So we try both.
			for (int i = 0; i < credentialsSplit.Length; i++)
			{
				lines = gjCredentials.Split(credentialsSplit[i], StringSplitOptions.RemoveEmptyEntries);
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
		/// <exception cref="ArgumentException">Returned if the credentials file is invalid.</exception>
		public async Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(string[] lines, CancellationToken cancellationToken = default)
		{
			if (lines.Length < 3)
			{
				return GameJoltResult.Error(new ArgumentException("Invalid credentials file.", nameof(lines)));
			}

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
		public async Task<GameJoltResult<GameJoltUser>> GetUserAsync(string username, CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?username=");
				builder.Append(username);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser>.Error(exception!);
				}

				return GameJoltResult<GameJoltUser>.Success(response.Users[0].ToPublicUser());
			}
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
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?user_id=");
				builder.Append(userId);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser>.Error(exception!);
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
		/// <exception cref="ArgumentNullException">Returned if <paramref name="usernames" /> is null.</exception>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		public async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(IEnumerable<string> usernames, CancellationToken cancellationToken = default)
		{
			if (usernames == null)
			{
				return GameJoltResult<GameJoltUser[]>.Error(new ArgumentNullException(nameof(usernames)));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?username=");
				builder.Append(usernames.ToCommaSeparatedString());

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser[]>.Error(exception!);
				}

				GameJoltUser[] users = new GameJoltUser[response.Users.Length];
				for (int i = 0; i < response.Users.Length; i++)
				{
					users[i] = response.Users[i].ToPublicUser();
				}

				return GameJoltResult<GameJoltUser[]>.Success(users);
			}
		}

		/// <summary>
		///     Fetches the users with the given user IDs. This method does not require the user to be authenticated.
		/// </summary>
		/// <param name="userIds">The user IDs of the users whose data you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns> The result of the request and the users' data.</returns>
		/// <exception cref="ArgumentNullException">Returned if <paramref name="userIds" /> is null.</exception>
		/// <exception cref="GameJoltInvalidUserException">Returned if the user does not exist.</exception>
		public async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default)
		{
			if (userIds == null)
			{
				return GameJoltResult<GameJoltUser[]>.Error(new ArgumentNullException(nameof(userIds)));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?user_id=");
				builder.Append(userIds.ToCommaSeparatedString());

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser[]>.Error(exception!);
				}

				GameJoltUser[] users = new GameJoltUser[response.Users.Length];
				for (int i = 0; i < response.Users.Length; i++)
				{
					users[i] = response.Users[i].ToPublicUser();
				}

				return GameJoltResult<GameJoltUser[]>.Success(users);
			}
		}

		internal bool IsAuthenticatedInternal(out GameJoltResult result)
		{
			if (!IsAuthenticated)
			{
				result = GameJoltResult.Error(new GameJoltAuthorizedException());
				return false;
			}

			result = default;
			return true;
		}

		internal bool IsAuthenticatedInternal<T>(out GameJoltResult<T> result)
		{
			if (!IsAuthenticated)
			{
				result = GameJoltResult<T>.Error(new GameJoltAuthorizedException());
				return false;
			}

			result = default;
			return true;
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