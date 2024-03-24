#if UNITY_2021_1_OR_NEWER
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Hertzole.GameJolt
{
	public sealed class GameJoltManager : MonoBehaviour
	{
		private static GameJoltManager instance;
		
		// There's no built-in destroy cancellation token pre Unity 2022.2, so we make our own.
#if !UNITY_2022_2_OR_NEWER
		private readonly CancellationTokenSource destroyCancellationTokenSource = new CancellationTokenSource();
		private CancellationToken destroyCancellationToken { get { return destroyCancellationTokenSource.Token; } }
#endif

		private void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;

			if (GameJoltSettings.AutoInitialize)
			{
				GameJoltAPI.Initialize(GameJoltSettings.GameId, GameJoltSettings.PrivateGameKey);
			}

			GameJoltAPI.Users.OnUserAuthenticated += OnUserAuthenticated;
		}

		private async void Start()
		{
#if UNITY_EDITOR
			if (GameJoltSettings.AutoSignIn)
			{
				GameJoltResult result =
					await GameJoltAPI.Users.AuthenticateAsync(GameJoltSettings.SignInUsername, GameJoltSettings.SignInToken, destroyCancellationToken);

				if (result.HasError)
				{
					Debug.LogError("Failed to sign in: " + result.Exception);
				}

				return;
			}

#endif

#if UNITY_WEBGL
			if (GameJoltSettings.AutoSignInFromWeb)
			{
				GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync(Application.absoluteURL, destroyCancellationToken);
				if (result.HasError)
				{
					Debug.LogError("Failed to sign in: " + result.Exception);
				}
			}
#else
			if (GameJoltSettings.AutoSignInFromClient)
			{
				string credentials = File.ReadAllText(Path.GetFullPath(Application.dataPath + "/../" + ".gj-credentials"));
				
				GameJoltResult result =
					await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(credentials, destroyCancellationToken);

				if (result.HasError)
				{
					Debug.LogError("Failed to sign in: " + result.Exception);
				}
			}
#endif
		}

		private async void OnDestroy()
		{
			// There's no built-in destroy cancellation token pre Unity 2022.2, so we make our own.
#if !UNITY_2022_2_OR_NEWER
			destroyCancellationTokenSource.Cancel();
#endif
			
			GameJoltAPI.Users.OnUserAuthenticated -= OnUserAuthenticated;

			if (GameJoltSettings.AutoCloseSessions && GameJoltAPI.Sessions.IsSessionOpen)
			{
				// Don't pass the destroyCancellationToken here, we want to close the session no matter what.
				GameJoltResult result = await GameJoltAPI.Sessions.CloseAsync();
				if (result.HasError)
				{
					Debug.LogError("Failed to close session: " + result.Exception);
				}
			}

			if (GameJoltSettings.AutoShutdown)
			{
				GameJoltAPI.Shutdown();
			}
		}

		private async void OnUserAuthenticated(GameJoltUser obj)
		{
			if (GameJoltSettings.AutoStartSessions)
			{
				GameJoltResult startResult = await GameJoltAPI.Sessions.OpenAsync(destroyCancellationToken);

				if (startResult.HasError)
				{
					Debug.LogError("Failed to start session: " + startResult.Exception);
					return;
				}

				if (GameJoltSettings.AutoPingSessions)
				{
					_ = PingSession(destroyCancellationToken);
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
			if (instance == null && FindAnyObjectByType<GameJoltManager>() == null)
			{
				new GameObject("GameJolt").AddComponent<GameJoltManager>();
			}
		}

		private static async Task PingSession(CancellationToken cancellationToken)
		{
			while (GameJoltAPI.IsInitialized)
			{
				await GameJoltAPI.Sessions.PingAsync(GameJoltSettings.PingStatus, cancellationToken);
				await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
			}
		}
	}
}
#endif