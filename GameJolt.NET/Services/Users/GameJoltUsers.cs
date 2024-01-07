#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
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

		public bool IsAuthenticated { get; private set; }

		public GameJoltUser? CurrentUser { get; private set; }
		private const string ENDPOINT = "users/";

		public event Action<GameJoltUser>? OnUserAuthenticated;

		internal GameJoltUsers(IGameJoltWebClient webClient, IGameJoltSerializer serializer)
		{
			this.webClient = webClient;
			this.serializer = serializer;
		}

		public async Task<GameJoltResult> AuthenticateAsync(string username, string token, CancellationToken cancellationToken = default)
		{
			myUsername = username;
			myToken = token;

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("auth/?username=");
				builder.Append(username);
				builder.Append("&user_token=");
				builder.Append(token);

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);

				AuthResponse response = serializer.Deserialize<AuthResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				GameJoltResult<GameJoltUser> fetchResponse = await FetchUserAsync(myUsername, cancellationToken).ConfigureAwait(false);
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

		public Task<GameJoltResult> AuthenticateFromUrlAsync(string url, CancellationToken cancellationToken = default)
		{
			return AuthenticateFromUrlAsync(new Uri(url), cancellationToken);
		}

		public async Task<GameJoltResult> AuthenticateFromUrlAsync(Uri url, CancellationToken cancellationToken = default)
		{
			if (url.Host.EndsWith("gamejolt.com", StringComparison.OrdinalIgnoreCase) || url.Host.EndsWith("gamejolt.net", StringComparison.OrdinalIgnoreCase))
			{
				if (QueryParser.TryGetToken(url.Query, "gjapi_username", out string? username) &&
				    QueryParser.TryGetToken(url.Query, "gjapi_token", out string? token))
				{
					return await AuthenticateAsync(username!, token!, cancellationToken).ConfigureAwait(false);
				}
			}

			return GameJoltResult.Error(new ArgumentException("Invalid URL.", nameof(url)));
		}

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

		public async Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(string[] lines, CancellationToken cancellationToken = default)
		{
			if (lines.Length < 3)
			{
				return GameJoltResult.Error(new ArgumentException("Invalid credentials file.", nameof(lines)));
			}

			string username = lines[1];
			string token = lines[2];

			return await AuthenticateAsync(username, token, cancellationToken).ConfigureAwait(false);
		}

		public async Task<GameJoltResult<GameJoltUser>> FetchUserAsync(string username, CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?username=");
				builder.Append(username);

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser>.Error(exception);
				}

				return GameJoltResult<GameJoltUser>.Success(response.Users[0].ToPublicUser());
			}
		}

		public async Task<GameJoltResult<GameJoltUser[]>> FetchUsersAsync(IEnumerable<string> usernames, CancellationToken cancellationToken = default)
		{
			if (usernames == null)
			{
				throw new ArgumentNullException(nameof(usernames));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?username=");
				builder.Append(usernames.ToCommaSeparatedString());

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser[]>.Error(exception);
				}

				GameJoltUser[] users = new GameJoltUser[response.Users.Length];
				for (int i = 0; i < response.Users.Length; i++)
				{
					users[i] = response.Users[i].ToPublicUser();
				}

				return GameJoltResult<GameJoltUser[]>.Success(users);
			}
		}

		public async Task<GameJoltResult<GameJoltUser>> FetchUserAsync(int userId, CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?user_id=");
				builder.Append(userId);

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser>.Error(exception);
				}

				return GameJoltResult<GameJoltUser>.Success(response.Users[0].ToPublicUser());
			}
		}

		public async Task<GameJoltResult<GameJoltUser[]>> FetchUsersAsync(IEnumerable<int> userIds, CancellationToken cancellationToken = default)
		{
			if (userIds == null)
			{
				throw new ArgumentNullException(nameof(userIds));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?user_id=");
				builder.Append(userIds.ToCommaSeparatedString());

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				UsersFetchResponse response = serializer.Deserialize<UsersFetchResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltUser[]>.Error(exception);
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