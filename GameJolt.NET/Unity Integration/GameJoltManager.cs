#if GAMEJOLT_UNITY && !DISABLE_GAMEJOLT
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltManager : IDisposable
	{
		internal bool hasInitialized = false;
		private bool hasStartedSession = false;

		private readonly Action onInitialized;

		private readonly CancellationToken cancellationToken;

		internal GameJoltManager(CancellationToken cancellationToken = default)
		{
			this.cancellationToken = cancellationToken;

			onInitialized = OnInitialized;

			GameJoltAPI.OnInitialized += onInitialized;
		}

		private async void OnInitialized()
		{
			hasInitialized = true;
			GameJoltAPI.Users.OnUserAuthenticated += OnUserAuthenticated;

			await InitializeSignInAsync();
		}

		private async ValueTask<bool> InitializeSignInAsync()
		{
#if UNITY_EDITOR
			if (await SignInEditorAsync())
			{
				return true;
			}
#endif // UNITY_EDITOR

			// Always enable in editor so it's easy to edit.
#if UNITY_EDITOR || UNITY_WEBGL
			if (await SignInWebAsync())
			{
				return true;
			}
#endif // UNITY_WEBGL

			return await SignInClientAsync();
		}

#if UNITY_EDITOR
		private async ValueTask<bool> SignInEditorAsync()
		{
			if (!GameJoltSettings.AutoSignIn)
			{
				return false;
			}

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync(GameJoltSettings.SignInUsername, GameJoltSettings.SignInToken, cancellationToken);
			if (result.HasError)
			{
				Debug.LogError("Failed to sign in: " + result.Exception);
				return false;
			}

			return true;
		}
#endif // UNITY_EDITOR

		// Always enable in editor so it's easy to edit.
#if UNITY_EDITOR || UNITY_WEBGL
		private async ValueTask<bool> SignInWebAsync()
		{
			// If it's the editor we usually can't sign in from the web.
			// So just create a scenario that is technically always true and return false. 
			// This is to make sure the code is easily editable in the editor.
			// But everything here will be stripped away outside of the editor and webgl builds.
#if UNITY_EDITOR
			if (Application.platform != RuntimePlatform.WebGLPlayer)
			{
				return false;
			}
#endif

			if (!GameJoltSettings.AutoSignInFromWeb)
			{
				return false;
			}

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync(Application.absoluteURL, cancellationToken);
			if (result.HasError)
			{
				Debug.LogError("Failed to sign in from web: " + result.Exception);
				return false;
			}

			return true;
		}
#endif // UNITY_EDITOR || UNITY_WEBGL

		private async ValueTask<bool> SignInClientAsync()
		{
			if (!GameJoltSettings.AutoSignInFromClient)
			{
				return false;
			}

			using (StringBuilderPool.Rent(out StringBuilder pathBuilder))
			{
				pathBuilder.Append(Application.dataPath);
				pathBuilder.Append("/../.gj-credentials");

				string credentialsPath = Path.GetFullPath(pathBuilder.ToString());

				if (File.Exists(credentialsPath))
				{
					string credentials =
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER || UNITY_2021_3_OR_NEWER
						await File.ReadAllTextAsync(credentialsPath, cancellationToken);
#else
						File.ReadAllText(credentialsPath);
#endif

					GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(credentials, cancellationToken);

					if (result.HasError)
					{
						Debug.LogError("Failed to sign in from client: " + result.Exception);
						return false;
					}

					return true;
				}
			}

			// If we reach this point, we couldn't sign in from the client.
			return false;
		}

		private void OnUserAuthenticated(GameJoltUser obj)
		{
			if (GameJoltSettings.AutoStartSessions)
			{
				_ = InitializeSessionsAsync();
			}
		}

		private async ValueTask InitializeSessionsAsync()
		{
			if (!GameJoltSettings.AutoStartSessions)
			{
				return;
			}

			GameJoltResult startResult = await GameJoltAPI.Sessions.OpenAsync(cancellationToken);

			if (startResult.HasError)
			{
				Debug.LogError("Failed to start session: " + startResult.Exception);
				return;
			}

			hasStartedSession = true;

			if (GameJoltSettings.AutoPingSessions)
			{
				// Start pinging the session.
				_ = PingSessionAsync();
			}
		}

		private async ValueTask PingSessionAsync()
		{
			while (hasInitialized && hasStartedSession && GameJoltSettings.AutoPingSessions)
			{
				GameJoltResult<bool> isSessionOpen = await GameJoltAPI.Sessions.CheckAsync(cancellationToken);
				if (!isSessionOpen.HasError && !isSessionOpen.Value)
				{
					// Session was closed by server. Try to reopen it.
					GameJoltResult openResult = await GameJoltAPI.Sessions.OpenAsync(cancellationToken);
					if (openResult.HasError)
					{
						Debug.LogError("Failed to reopen session: " + openResult.Exception);
						break;
					}
				}

				GameJoltResult pingResult = await GameJoltAPI.Sessions.PingAsync(GameJoltSettings.PingStatus, cancellationToken);
				if (pingResult.HasError)
				{
					Debug.LogError("Failed to ping session: " + pingResult.Exception);
					break;
				}

				await Task.Delay(TimeSpan.FromSeconds(GameJoltSettings.PingInterval), cancellationToken);
			}
		}

		public void InitializeGameJolt()
		{
			if (!GameJoltSettings.AutoInitialize)
			{
				return;
			}

			GameJoltAPI.Initialize(GameJoltSettings.GameId, GameJoltSettings.PrivateGameKey);
		}

		public async void Dispose()
		{
			GameJoltAPI.OnInitialized -= onInitialized;

			// If we haven't initialized, we don't need to do anything.
			if (!GameJoltAPI.IsInitialized)
			{
				hasInitialized = false;
				return;
			}

			GameJoltAPI.Users.OnUserAuthenticated -= OnUserAuthenticated;

			await DisposeSessions();

			if (GameJoltSettings.AutoShutdown)
			{
				GameJoltAPI.Shutdown();
			}

			hasInitialized = false;
			hasStartedSession = false;
		}

		private async ValueTask DisposeSessions()
		{
			if (!GameJoltSettings.AutoCloseSessions || !hasStartedSession || !GameJoltAPI.Sessions.IsSessionOpen)
			{
				return;
			}

			// Don't pass the cancellationToken here, we want to close the session no matter what.
			// We also check on the server if the session is open, as it might have been closed by the server.
			GameJoltResult<bool> sessionOpen = await GameJoltAPI.Sessions.CheckAsync();
			if (!sessionOpen.HasError && sessionOpen.Value)
			{
				// Don't pass the cancellationToken here, we want to close the session no matter what.
				GameJoltResult result = await GameJoltAPI.Sessions.CloseAsync();
				if (result.HasError)
				{
					Debug.LogError("Failed to close session: " + result.Exception);
				}
			}
		}
	}
}
#endif // GAMEJOLT_UNITY && !DISABLE_GAMEJOLT