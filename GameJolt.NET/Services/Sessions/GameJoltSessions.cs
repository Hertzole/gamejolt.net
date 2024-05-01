#nullable enable

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Sessions are used to tell Game jolt when a user is playing a game, and what state they are in while playing (active
	///     or idle).
	/// </summary>
	public sealed class GameJoltSessions
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;
		private readonly GameJoltUsers users;

		/// <summary>
		///     Gets if there is a session open.
		/// </summary>
		public bool IsSessionOpen { get; private set; }

		internal GameJoltSessions(IGameJoltWebClient webClient, IGameJoltSerializer serializer, GameJoltUsers users)
		{
			this.webClient = webClient;
			this.serializer = serializer;
			this.users = users;
		}

		/// <summary>
		///     Invoked when a session is opened.
		/// </summary>
		public event Action? OnSessionOpened;
		/// <summary>
		///     Invoked when a session is closed.
		/// </summary>
		public event Action? OnSessionClosed;
		/// <summary>
		///     Invoked when a session is pinged.
		/// </summary>
		public event Action? OnSessionPinged;

		private const string ENDPOINT = "sessions/";
		internal const string OPEN_ENDPOINT = ENDPOINT + "open/";
		internal const string PING_ENDPOINT = ENDPOINT + "ping/";
		internal const string CLOSE_ENDPOINT = ENDPOINT + "close/";
		internal const string CHECK_ENDPOINT = ENDPOINT + "check/";

		internal const string SESSION_ALREADY_OPEN = "Session is already open.";
		internal const string CANT_CLOSE_SESSION = "Can't close session because there is no session open.";
		internal const string CANT_PING_SESSION = "Can't ping session because there is no session open.";

		/// <summary>
		///     Opens a session.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltSessionException">Returned if there is already a session open.</exception>
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
				AppendUser(builder, users.myUsername!, users.myToken!);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				Response response = serializer.DeserializeResponse<Response>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				IsSessionOpen = true;
				OnSessionOpened?.Invoke();
				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Closes the session.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// ///
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltSessionException">Returned if there is no session open.</exception>
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
				AppendUser(builder, users.myUsername!, users.myToken!);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				Response response = serializer.DeserializeResponse<Response>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				IsSessionOpen = false;
				OnSessionClosed?.Invoke();
				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Pings the session.
		/// </summary>
		/// <param name="status">The status of the session.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltSessionException">Returned if there is no session open.</exception>
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
				AppendUser(builder, users.myUsername!, users.myToken!);
				builder.Append("&status=");
				builder.Append(GetStatusString(status));

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);

				Response response = serializer.DeserializeResponse<Response>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				OnSessionPinged?.Invoke();
				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Checks if the session is active.
		/// </summary>
		/// <param name="cancellationToken"> Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and if the session is active.</returns>
		public async Task<GameJoltResult<bool>> CheckAsync(CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult<bool> result))
			{
				return result;
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(CHECK_ENDPOINT);
				AppendUser(builder, users.myUsername!, users.myToken!);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				Response response = serializer.DeserializeResponse<Response>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<bool>.Error(exception!);
				}

				// No assert here since the response 'Success' field is tied to the session status.

				return GameJoltResult<bool>.Success(response.Success);
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AppendUser(StringBuilder builder, string username, string token)
		{
			builder.Append("?username=");
			builder.Append(username);
			builder.Append("&user_token=");
			builder.Append(token);
		}

		internal static string GetStatusString(SessionStatus status)
		{
			return status == SessionStatus.Active ? "active" : "idle";
		}
	}
}