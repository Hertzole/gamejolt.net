#nullable enable

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	public sealed class GameJoltSessions
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;
		private readonly GameJoltUsers users;

		public bool IsSessionOpen { get; private set; }

		internal GameJoltSessions(IGameJoltWebClient webClient, IGameJoltSerializer serializer, GameJoltUsers users)
		{
			this.webClient = webClient;
			this.serializer = serializer;
			this.users = users;
		}

		private const string ENDPOINT = "sessions/";
		private const string OPEN_ENDPOINT = ENDPOINT + "open/";
		private const string PING_ENDPOINT = ENDPOINT + "ping/";
		private const string CLOSE_ENDPOINT = ENDPOINT + "close/";
		private const string CHECK_ENDPOINT = ENDPOINT + "check/";

		internal const string SESSION_ALREADY_OPEN = "Session is already open.";
		internal const string CANT_CLOSE_SESSION = "Can't close session because there is no session open.";
		internal const string CANT_PING_SESSION = "Can't ping session because there is no session open.";

		public async Task<GameJoltResult> OpenAsync(CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			if (IsSessionOpen)
			{
				return GameJoltResult.Error(new GameJoltSessionException(SESSION_ALREADY_OPEN));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(OPEN_ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");
				builder.Append(users.myToken);

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				SessionResponse response = serializer.Deserialize<SessionResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				IsSessionOpen = true;
				return GameJoltResult.Success();
			}
		}

		public async Task<GameJoltResult> CloseAsync(CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			if (!IsSessionOpen)
			{
				return GameJoltResult.Error(new GameJoltSessionException(CANT_CLOSE_SESSION));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(CLOSE_ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");
				builder.Append(users.myToken);

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				SessionResponse response = serializer.Deserialize<SessionResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				IsSessionOpen = false;
				return GameJoltResult.Success();
			}
		}

		public async Task<GameJoltResult> PingAsync(SessionStatus status, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			if (!IsSessionOpen)
			{
				return GameJoltResult.Error(new GameJoltSessionException(CANT_PING_SESSION));
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(PING_ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");
				builder.Append(users.myToken);
				builder.Append("&status=");
				builder.Append(GetStatusString(status));

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);

				SessionResponse response = serializer.Deserialize<SessionResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				return GameJoltResult.Success();
			}
		}

		public async Task<GameJoltResult<bool>> CheckAsync(CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult<bool> result))
			{
				return result;
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(CHECK_ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");

				string? json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				SessionResponse response = serializer.Deserialize<SessionResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<bool>.Error(exception!);
				}

				// No assert here since the response 'Success' field is tied to the session status.

				return GameJoltResult<bool>.Success(response.Success);
			}
		}

		private static string GetStatusString(SessionStatus status)
		{
			return status == SessionStatus.Active ? "active" : "idle";
		}

		internal void Shutdown() { }
	}
}