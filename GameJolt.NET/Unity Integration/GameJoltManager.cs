#if UNITY_2021_1_OR_NEWER
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Hertzole.GameJolt
{
	public sealed class GameJoltManager : MonoBehaviour
	{
		internal bool isMainInstance;

		internal static GameJoltManager instance;

		private void Awake()
		{
			isMainInstance = false;

			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			// This is used in OnDestroy to only close the session if this is the main instance.
			isMainInstance = true;

			instance = this;
		}

		private void Start()
		{
			// If this is not the main instance, we don't want to do anything else.
			if (!isMainInstance)
			{
				return;
			}

			if (GameJoltSettings.AutoInitialize)
			{
				GameJoltAPI.Initialize(GameJoltSettings.GameId, GameJoltSettings.PrivateGameKey);
			}
		}

		private void OnEnable()
		{
			if (!isMainInstance)
			{
				return;
			}

			GameJoltAPI.OnInitialized += OnInitialized;
			GameJoltAPI.OnShutdown += OnShutdown;
		}

		private void OnDisable()
		{
			if (!isMainInstance)
			{
				return;
			}

			GameJoltAPI.OnInitialized -= OnInitialized;
			GameJoltAPI.OnShutdown -= OnShutdown;
		}

		private async void OnDestroy()
		{
			// There's no built-in destroy cancellation token pre Unity 2022.2, so we make our own.
#if !UNITY_2022_2_OR_NEWER
			destroyCancellationTokenSource.Cancel();
#endif

			// If this is not the main instance, we don't want to do anything else.
			if (!isMainInstance)
			{
				return;
			}
			
			// No need to do anything if the API isn't initialized.
			if (!GameJoltAPI.IsInitialized)
			{
				return;
			}

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

		private async void OnInitialized()
		{
			GameJoltAPI.Users.OnUserAuthenticated += OnUserAuthenticated;

#if UNITY_EDITOR
			if (GameJoltSettings.AutoSignIn)
			{
				GameJoltResult result =
					await GameJoltAPI.Users.AuthenticateAsync(GameJoltSettings.SignInUsername, GameJoltSettings.SignInToken, destroyCancellationToken);

				if (result.HasError)
				{
					Debug.LogError("Failed to sign in from editor: " + result.Exception);
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
					Debug.LogError("Failed to sign in from web: " + result.Exception);
				}
			}
#else
			if (GameJoltSettings.AutoSignInFromClient)
			{
				using (StringBuilderPool.Rent(out StringBuilder pathBuilder))
				{
					pathBuilder.Append(Application.dataPath);
					pathBuilder.Append("/../.gj-credentials");

					string credentialsPath = Path.GetFullPath(pathBuilder.ToString());
					if (File.Exists(credentialsPath))
					{
						string credentials =
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER
							await File.ReadAllTextAsync(credentialsPath, destroyCancellationToken);
#else
							File.ReadAllText(credentialsPath);
#endif

						GameJoltResult result =
							await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(credentials, destroyCancellationToken);

						if (result.HasError)
						{
							Debug.LogError("Failed to sign in from client: " + result.Exception);
						}
					}
				}
			}
#endif
		}

		private void OnShutdown()
		{
			GameJoltAPI.Users.OnUserAuthenticated -= OnUserAuthenticated;
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
		internal static void Initialize()
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
				await Task.Delay(TimeSpan.FromSeconds(GameJoltSettings.PingInterval), cancellationToken);
			}
		}

		// There's no built-in destroy cancellation token pre Unity 2022.2, so we make our own.
#if !UNITY_2022_2_OR_NEWER
		private readonly CancellationTokenSource destroyCancellationTokenSource = new CancellationTokenSource();
		private CancellationToken destroyCancellationToken
		{
			get { return destroyCancellationTokenSource.Token; }
		}
#endif
	}
}
#endif