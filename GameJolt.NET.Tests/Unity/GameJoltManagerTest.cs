#if UNITY_64 // Just to make sure it works in any modern Unity version
#nullable enable
using System.Collections;
using System.IO;
using GameJolt.NET.Tests.Attributes;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GameJolt.NET.Tests.Unity
{
	[SkipInitialization]
	public class GameJoltManagerTest : BaseTest
	{
		private GameJoltManager? currentManager;

		private readonly string credentialsPath = Path.GetFullPath(Application.dataPath + "/../.gj-credentials");

		[UnitySetUp]
		public IEnumerator UnitySetUp()
		{
			if (GameJoltManager.instance != null)
			{
				Object.Destroy(GameJoltManager.instance.gameObject);
				yield return null;
			}

			Assert.That(GameJoltManager.instance == null, Is.True, "GameJoltManager.instance is not null.");
		}

		[UnityTearDown]
		public IEnumerator UnityTearDown()
		{
			GameJoltSettings.AutoInitialize = false;

			if (currentManager != null)
			{
				Object.Destroy(currentManager.gameObject);
				yield return null;
			}

			Assert.That(GameJoltManager.instance == null, Is.True, "GameJoltManager.instance is not null.");

			if (File.Exists(credentialsPath))
			{
				File.Delete(credentialsPath);
			}
		}

		[UnityTest]
		public IEnumerator Singleton_SingleInstance()
		{
			currentManager = CreateManager();

			yield return null;

			Assert.That(GameJoltManager.instance == null, Is.False, "GameJoltManager.instance is null.");
			Assert.That(GameJoltManager.instance!.isMainInstance, Is.True, "GameJoltManager.instance is not the main instance.");

			yield return null;

			GameJoltManager sceneInstance = Object.FindObjectOfType<GameJoltManager>();

			Assert.That(sceneInstance == null, Is.False, "GameJoltManager in scene is null.");
			Assert.That(sceneInstance, Is.EqualTo(GameJoltManager.instance), "GameJoltManager in scene is not the same as the instance.");

			yield return null;

			GameJoltManager newInstance = new GameObject().AddComponent<GameJoltManager>();

			Assert.That(newInstance, Is.Not.EqualTo(GameJoltManager.instance), "New instance is the same as the main instance.");
			Assert.That(newInstance.isMainInstance, Is.False, "New instance is the main instance.");

			yield return null;

			Assert.That(newInstance == null, Is.True, "New instance was not destroyed.");
		}

		[UnityTest]
		public IEnumerator AutoInitialize_Initialized()
		{
			GameJoltSettings.AutoInitialize = true;
			bool invoked = false;

			GameJoltAPI.OnInitialized += Invoked;

			currentManager = CreateManager();

			yield return null;

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");
			Assert.That(invoked, Is.True, "OnInitialized was not invoked.");

			GameJoltAPI.OnInitialized -= Invoked; // Clean up.
			yield break;

			void Invoked()
			{
				invoked = true;
			}
		}

		[UnityTest]
		public IEnumerator NoAutoInitialize_NotInitialized()
		{
			GameJoltSettings.AutoInitialize = false;
			bool invoked = false;

			GameJoltAPI.OnInitialized += Invoked;

			currentManager = CreateManager();

			yield return null;

			Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI was initialized.");
			Assert.That(invoked, Is.False, "OnInitialized was invoked.");

			GameJoltAPI.OnInitialized -= Invoked; // Clean up.
			yield break;

			void Invoked()
			{
				invoked = true;
			}
		}

		[UnityTest]
		public IEnumerator AutoShutdown_NotInitialized()
		{
			GameJoltSettings.AutoShutdown = true;
			bool invokedShutdown = false;
			bool invokedShutdownComplete = false;

			GameJoltAPI.OnShutdown += InvokedShutdown;
			GameJoltAPI.OnShutdownComplete += InvokedShutdownComplete;

			currentManager = CreateManager();

			yield return null;

			GameJoltAPI.Initialize(0, "");

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			Object.Destroy(currentManager.gameObject);

			yield return null;

			Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI is initialized.");
			Assert.That(invokedShutdown, Is.True, "OnShutdown was not invoked.");
			Assert.That(invokedShutdownComplete, Is.True, "OnShutdownComplete was not invoked.");

			GameJoltAPI.OnShutdown -= InvokedShutdown; // Clean up.
			GameJoltAPI.OnShutdownComplete -= InvokedShutdownComplete; // Clean up.

			yield break;

			void InvokedShutdown()
			{
				invokedShutdown = true;
			}

			void InvokedShutdownComplete()
			{
				invokedShutdownComplete = true;
			}
		}

		[UnityTest]
		public IEnumerator NoAutoShutdown_IsInitialized()
		{
			GameJoltSettings.AutoShutdown = false;
			bool invokedShutdown = false;
			bool invokedShutdownComplete = false;

			GameJoltAPI.OnShutdown += InvokedShutdown;
			GameJoltAPI.OnShutdownComplete += InvokedShutdownComplete;

			currentManager = CreateManager();

			yield return null;

			GameJoltAPI.Initialize(0, "");

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			Object.Destroy(currentManager.gameObject);

			yield return null;

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");
			Assert.That(invokedShutdown, Is.False, "OnShutdown was invoked.");
			Assert.That(invokedShutdownComplete, Is.False, "OnShutdownComplete was invoked.");

			GameJoltAPI.OnShutdown -= InvokedShutdown; // Clean up.
			GameJoltAPI.OnShutdownComplete -= InvokedShutdownComplete; // Clean up.

			yield break;

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
		[UnityTest]
		public IEnumerator AutoSignIn_Editor()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoSignIn = true;

			currentManager = CreateManager();

			SetUpWebClientForAuth();

			yield return null;

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			yield return null;

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Users.CurrentUser.HasValue, Is.True, "Current user is null.");
			Assert.That(GameJoltAPI.Users.CurrentUser!.Value.Username, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myUsername, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myToken, Is.EqualTo(Token), "Token is not correct.");
		}
#endif

		[UnityTest]
		public IEnumerator AutoSignIn_Client()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoSignInFromClient = true;

			string credentialsFileContent = $"blah\n{Username}\n{Token}";

			File.WriteAllText(credentialsPath, credentialsFileContent);

			currentManager = CreateManager();

			SetUpWebClientForAuth();

			yield return null;

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			yield return null;

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Users.CurrentUser.HasValue, Is.True, "Current user is null.");
			Assert.That(GameJoltAPI.Users.CurrentUser!.Value.Username, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myUsername, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myToken, Is.EqualTo(Token), "Token is not correct.");
		}

		[UnityTest]
		public IEnumerator AutoStartSessions_SessionStarted()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;

			currentManager = CreateManager();

			yield return null; // Wait for the manager to initialize.

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			yield return AuthenticateAsync();

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
		}

		[UnityTest]
		public IEnumerator AutoCloseSessions_SessionClosed()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			GameJoltSettings.AutoCloseSessions = true;

			currentManager = CreateManager();

			yield return null; // Wait for the manager to initialize.

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			yield return AuthenticateAsync();

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");

			Object.Destroy(currentManager.gameObject);

			yield return null;

			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False, "Session is still open.");
		}

		[UnityTest]
		public IEnumerator AutoPingSessions_SessionPinged()
		{
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			GameJoltSettings.AutoPingSessions = true;
			GameJoltSettings.PingInterval = 1f;

			currentManager = CreateManager();

			yield return null; // Wait for the manager to initialize.

			Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

			yield return AuthenticateAsync();

			bool pinged = false;

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/ping") || arg.Contains("sessions/open"))
				{
					pinged = true;
					return FromResult(serializer.Serialize(new SessionResponse(true, null)));
				}

				return FromResult("");
			});

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");

			yield return new WaitForSeconds(1.1f);

			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
			Assert.That(pinged, Is.True, "Session was not pinged.");
		}

		[UnityTest]
		public IEnumerator Initialize_CreatesSingleton()
		{
			GameJoltManager[]? managers = Object.FindObjectsOfType<GameJoltManager>();

			Assert.That(managers.Length, Is.EqualTo(0), "There are managers in the scene.");

			GameJoltManager.Initialize();

			yield return null;

			managers = Object.FindObjectsOfType<GameJoltManager>();

			Assert.That(managers.Length, Is.EqualTo(1), "There are no managers in the scene.");

			currentManager = managers[0];
		}

		[UnityTest]
		public IEnumerator Initialize_ExistingSingleton()
		{
			yield return Initialize_CreatesSingleton();

			GameJoltManager.Initialize();

			GameJoltManager[]? managers = Object.FindObjectsOfType<GameJoltManager>();

			Assert.That(managers.Length, Is.EqualTo(1), "There are no managers in the scene.");

			currentManager = managers[0];
		}

		private static GameJoltManager CreateManager()
		{
			return new GameObject().AddComponent<GameJoltManager>();
		}
	}
}
#endif