#if UNITY_64 // Just to make sure it works in any modern Unity version
#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using GameJolt.NET.Tests.Attributes;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameJolt.NET.Tests.Unity
{
	[SkipInitialization]
	[SingleThreaded]
	public class GameJoltManagerTest : BaseTest
	{
		private GameJoltManager? currentManager;

		private readonly string credentialsPath = Path.GetFullPath(Application.dataPath + "/../.gj-credentials");

		protected override async Task OnSetupAsync()
		{
			if (GameJoltManager.instance != null)
			{
				Object.Destroy(GameJoltManager.instance.gameObject);
				await WaitFramesAsync(1);
			}

			Assert.That(GameJoltManager.instance == null, Is.True, "GameJoltManager.instance is not null.");

			await base.OnSetupAsync();
		}

		protected override async Task OnTearDownAsync()
		{
			GameJoltSettings.AutoInitialize = false;

			if (currentManager != null)
			{
				Object.Destroy(currentManager.gameObject);
				await WaitFramesAsync(1);
			}

			Assert.That(GameJoltManager.instance == null, Is.True, "GameJoltManager.instance is not null.");

			if (File.Exists(credentialsPath))
			{
				File.Delete(credentialsPath);
			}

			await base.OnTearDownAsync();
		}

		[Test]
		public async Task Singleton_SingleInstance()
		{
			currentManager = CreateManager();

			await WaitFramesAsync(1);

			Assert.That(GameJoltManager.instance == null, Is.False, "GameJoltManager.instance is null.");
			Assert.That(GameJoltManager.instance!.isMainInstance, Is.True, "GameJoltManager.instance is not the main instance.");

			await WaitFramesAsync(1);

			GameJoltManager sceneInstance = FindObject<GameJoltManager>();

			Assert.That(sceneInstance == null, Is.False, "GameJoltManager in scene is null.");
			Assert.That(sceneInstance, Is.EqualTo(GameJoltManager.instance), "GameJoltManager in scene is not the same as the instance.");

			await WaitFramesAsync(1);

			GameJoltManager newInstance = new GameObject().AddComponent<GameJoltManager>();

			Assert.That(newInstance, Is.Not.EqualTo(GameJoltManager.instance), "New instance is the same as the main instance.");
			Assert.That(newInstance.isMainInstance, Is.False, "New instance is the main instance.");

			await WaitFramesAsync(1);

			Assert.That(newInstance == null, Is.True, "New instance was not destroyed.");
		}

		[Test]
		public async Task AutoInitialize_Initialized()
		{
			GameJoltSettings.AutoInitialize = true;
			bool invoked = false;

			GameJoltAPI.OnInitialized += Invoked;

			currentManager = CreateManager();

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");
			Assert.That(invoked, Is.True, "OnInitialized was not invoked.");

			GameJoltAPI.OnInitialized -= Invoked; // Clean up.
			return;

			void Invoked()
			{
				invoked = true;
			}
		}

		[Test]
		public async Task NoAutoInitialize_NotInitialized()
		{
			GameJoltSettings.AutoInitialize = false;
			bool invoked = false;

			GameJoltAPI.OnInitialized += Invoked;

			currentManager = CreateManager();

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI was initialized.");
			Assert.That(invoked, Is.False, "OnInitialized was invoked.");

			GameJoltAPI.OnInitialized -= Invoked; // Clean up.
			return;

			void Invoked()
			{
				invoked = true;
			}
		}

		[Test]
		public async Task AutoShutdown_NotInitialized()
		{
			GameJoltSettings.AutoShutdown = true;
			bool invokedShutdown = false;
			bool invokedShutdownComplete = false;

			GameJoltAPI.OnShutdown += InvokedShutdown;
			GameJoltAPI.OnShutdownComplete += InvokedShutdownComplete;

			currentManager = CreateManager();

			await WaitFramesAsync(1);

			GameJoltAPI.Initialize(0, "");

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			Object.Destroy(currentManager.gameObject);

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI is initialized.");
			Assert.That(invokedShutdown, Is.True, "OnShutdown was not invoked.");
			Assert.That(invokedShutdownComplete, Is.True, "OnShutdownComplete was not invoked.");

			GameJoltAPI.OnShutdown -= InvokedShutdown; // Clean up.
			GameJoltAPI.OnShutdownComplete -= InvokedShutdownComplete; // Clean up.

			return;

			void InvokedShutdown()
			{
				invokedShutdown = true;
			}

			void InvokedShutdownComplete()
			{
				invokedShutdownComplete = true;
			}
		}

		[Test]
		public async Task NoAutoShutdown_IsInitialized()
		{
			GameJoltSettings.AutoShutdown = false;
			bool invokedShutdown = false;
			bool invokedShutdownComplete = false;

			GameJoltAPI.OnShutdown += InvokedShutdown;
			GameJoltAPI.OnShutdownComplete += InvokedShutdownComplete;

			currentManager = CreateManager();

			await WaitFramesAsync(1);

			GameJoltAPI.Initialize(0, "");

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			Object.Destroy(currentManager.gameObject);

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");
			Assert.That(invokedShutdown, Is.False, "OnShutdown was invoked.");
			Assert.That(invokedShutdownComplete, Is.False, "OnShutdownComplete was invoked.");

			GameJoltAPI.OnShutdown -= InvokedShutdown; // Clean up.
			GameJoltAPI.OnShutdownComplete -= InvokedShutdownComplete; // Clean up.

			return;

			void InvokedShutdown()
			{
				invokedShutdown = true;
			}

			void InvokedShutdownComplete()
			{
				invokedShutdownComplete = true;
			}
		}

#if UNITY_EDITOR
		[Test]
		public async Task AutoSignIn_Editor()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoSignIn = true;

			currentManager = CreateManager();

			SetUpWebClientForAuth();

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Users.CurrentUser.HasValue, Is.True, "Current user is null.");
			Assert.That(GameJoltAPI.Users.CurrentUser!.Value.Username, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myUsername, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myToken, Is.EqualTo(Token), "Token is not correct.");
		}
#endif

		[Test]
		public async Task AutoSignIn_Client()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoSignInFromClient = true;

			string credentialsFileContent = $"blah\n{Username}\n{Token}";

			await File.WriteAllTextAsync(credentialsPath, credentialsFileContent);

			Assert.That(File.Exists(credentialsPath), Is.True, "Credentials file does not exist.");

			currentManager = CreateManager();

			SetUpWebClientForAuth();

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			// Wait for the authentication to finish. It may take some time.
			await Task.Delay(TimeSpan.FromSeconds(2));

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Users.CurrentUser.HasValue, Is.True, "Current user is null.");
			Assert.That(GameJoltAPI.Users.CurrentUser!.Value.Username, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myUsername, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myToken, Is.EqualTo(Token), "Token is not correct.");
		}

		[Test]
		public async Task AutoStartSessions_SessionStarted()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;

			currentManager = CreateManager();

			await WaitFramesAsync(1); // Wait for the manager to initialize.

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			await AuthenticateAsync();

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
		}

		[Test]
		public async Task AutoCloseSessions_SessionClosed()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			GameJoltSettings.AutoCloseSessions = true;

			currentManager = CreateManager();

			await WaitFramesAsync(1); // Wait for the manager to initialize.

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			await AuthenticateAsync();

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");

			Object.Destroy(currentManager.gameObject);

			await WaitFramesAsync(1);

			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False, "Session is still open.");
		}

		[Test]
		public async Task AutoPingSessions_SessionPinged()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			GameJoltSettings.AutoPingSessions = true;
			GameJoltSettings.PingInterval = 1f;

			currentManager = CreateManager();

			await WaitFramesAsync(1); // Wait for the manager to initialize.

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			await AuthenticateAsync();

			bool pinged = false;

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/ping") || arg.Contains("sessions/open"))
				{
					pinged = true;
					return FromResult(serializer.Serialize(new Response(true, null)));
				}

				if (arg.Contains("sessions/check"))
				{
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				return FromResult("");
			});

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");

			await Task.Delay(TimeSpan.FromSeconds(1.1));

			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
			Assert.That(pinged, Is.True, "Session was not pinged.");
		}

		[Test]
		public async Task Initialize_CreatesSingleton()
		{
			GameJoltManager[]? managers = FindObjects<GameJoltManager>();

			Assert.That(managers.Length, Is.EqualTo(0), "There are managers in the scene.");

			GameJoltManager.Initialize();

			await WaitFramesAsync(1);

			managers = FindObjects<GameJoltManager>();

			Assert.That(managers.Length, Is.EqualTo(1), "There are no managers in the scene.");

			currentManager = managers[0];
		}

		[Test]
		public async Task Initialize_ExistingSingleton()
		{
			await Initialize_CreatesSingleton();

			GameJoltManager.Initialize();

			GameJoltManager[]? managers = FindObjects<GameJoltManager>();

			Assert.That(managers.Length, Is.EqualTo(1), "There are no managers in the scene.");

			currentManager = managers[0];
		}

		private static GameJoltManager CreateManager()
		{
			return new GameObject().AddComponent<GameJoltManager>();
		}

		private static async Task WaitFramesAsync(int frames)
		{
			int expectedFrameCount = Time.frameCount + frames;

			while (Time.frameCount < expectedFrameCount)
			{
				await Task.Yield();
			}
		}

		private static T FindObject<T>() where T : Object
		{
#if UNITY_2023_1_OR_NEWER
			return Object.FindFirstObjectByType<T>();
#else
			return Object.FindObjectOfType<T>();
#endif
		}

		private static T[] FindObjects<T>() where T : Object
		{
#if UNITY_2023_1_OR_NEWER
			return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
			return Object.FindObjectsOfType<T>();
#endif
		}
	}
}
#endif