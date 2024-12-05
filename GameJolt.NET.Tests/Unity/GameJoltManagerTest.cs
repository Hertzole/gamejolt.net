#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

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
		private GameJoltManager manager = null!;
		private readonly string credentialsPath = Path.GetFullPath(Application.dataPath + "/../.gj-credentials");
		private const int AUTH_TIMEOUT = 5;

		protected override Task OnSetupAsync()
		{
			manager = new GameJoltManager();

			return base.OnSetupAsync();
		}

		protected override async Task PreTearDownAsync()
		{
			manager.Dispose();

			float maxTime = Time.time + 1;

			while (manager.hasInitialized)
			{
				await WaitFramesAsync(1);

				if (Time.time >= maxTime)
				{
					throw new TimeoutException("Dispose timed out");
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			GameJoltSettings.AutoInitialize = false;

			if (File.Exists(credentialsPath))
			{
				File.Delete(credentialsPath);
			}

			await base.OnTearDownAsync();
		}

		[Test]
		public void AutoInitialize_Initialized()
		{
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			bool invoked = false;

			GameJoltAPI.OnInitialized += Invoked;

			try
			{
				// Act
				manager.InitializeGameJolt();

				// Assert
				Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");
				Assert.That(invoked, Is.True, "OnInitialized was not invoked.");
				Assert.That(manager.hasInitialized, Is.True, "Manager HasInitialized is false.");
			}
			finally
			{
				GameJoltAPI.OnInitialized -= Invoked;
			}

			void Invoked()
			{
				invoked = true;
			}
		}

		[Test]
		public void NoAutoInitialize_NotInitialized()
		{
			// Arrange
			GameJoltSettings.AutoInitialize = false;
			bool invoked = false;

			GameJoltAPI.OnInitialized += Invoked;

			try
			{
				// Act
				manager.InitializeGameJolt();

				// Assert
				Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI was initialized.");
				Assert.That(invoked, Is.False, "OnInitialized was invoked.");
				Assert.That(manager.hasInitialized, Is.False, "Manager HasInitialized is true.");
			}
			finally
			{
				GameJoltAPI.OnInitialized -= Invoked;
			}

			void Invoked()
			{
				invoked = true;
			}
		}

		[Test]
		public void AutoShutdown_NotInitialized()
		{
			// Arrange
			GameJoltSettings.AutoShutdown = true;
			bool invokedShutdown = false;
			bool invokedShutdownComplete = false;

			GameJoltAPI.OnShutdown += InvokedShutdown;
			GameJoltAPI.OnShutdownComplete += InvokedShutdownComplete;

			try
			{
				// Act
				GameJoltAPI.Initialize(0, "");
				Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

				manager.Dispose();

				// Assert
				Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI is initialized.");
				Assert.That(invokedShutdown, Is.True, "OnShutdown was not invoked.");
				Assert.That(invokedShutdownComplete, Is.True, "OnShutdownComplete was not invoked.");
				Assert.That(manager.hasInitialized, Is.False, "Manager HasInitialized is true.");
			}
			finally
			{
				GameJoltAPI.OnShutdown -= InvokedShutdown;
				GameJoltAPI.OnShutdownComplete -= InvokedShutdownComplete;
			}

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
		public void NoAutoShutdown_IsInitialized()
		{
			// Arrange
			GameJoltSettings.AutoShutdown = false;
			bool invokedShutdown = false;
			bool invokedShutdownComplete = false;

			GameJoltAPI.OnShutdown += InvokedShutdown;
			GameJoltAPI.OnShutdownComplete += InvokedShutdownComplete;

			try
			{
				// Act
				GameJoltAPI.Initialize(0, "");
				Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");

				manager.Dispose();

				Assert.That(GameJoltAPI.IsInitialized, Is.True, "GameJoltAPI is not initialized.");
				Assert.That(invokedShutdown, Is.False, "OnShutdown was invoked.");
				Assert.That(invokedShutdownComplete, Is.False, "OnShutdownComplete was invoked.");
				Assert.That(manager.hasInitialized, Is.False, "Manager HasInitialized is true.");
			}
			finally
			{
				GameJoltAPI.OnShutdown -= InvokedShutdown; // Clean up.
				GameJoltAPI.OnShutdownComplete -= InvokedShutdownComplete; // Clean up.
			}

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
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			await PrepareSignIn(InitializationType.Editor);

			// Act
			manager.InitializeGameJolt();

			// Assert
			await AssertTimeout(() => GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.", TimeSpan.FromSeconds(AUTH_TIMEOUT));
			Assert.That(GameJoltAPI.Users.CurrentUser.HasValue, Is.True, "Current user is null.");
			Assert.That(GameJoltAPI.Users.CurrentUser!.Value.Username, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myUsername, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myToken, Is.EqualTo(Token), "Token is not correct.");
		}

#endif
		[Test]
		public async Task AutoSignIn_Client()
		{
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			await PrepareSignIn(InitializationType.Client);

			// Act
			manager.InitializeGameJolt();

			// Assert
			await AssertTimeout(() => GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.", TimeSpan.FromSeconds(AUTH_TIMEOUT));
			Assert.That(GameJoltAPI.Users.CurrentUser.HasValue, Is.True, "Current user is null.");
			Assert.That(GameJoltAPI.Users.CurrentUser!.Value.Username, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myUsername, Is.EqualTo(Username), "Username is not correct.");
			Assert.That(GameJoltAPI.Users.myToken, Is.EqualTo(Token), "Token is not correct.");
		}

		[Test]
		public async Task AutoStartSessions_FromAuthenticate_SessionStarted()
		{
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;

			// Act
			manager.InitializeGameJolt();
			await AuthenticateAsync();

			// Assert
			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
		}

		[Test]
		public async Task AutoStartSessions_FromInitialize_SessionStarted([Values] InitializationType initializationType)
		{
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			if (!await PrepareSignIn(initializationType))
			{
				return;
			}

			// Act
			manager.InitializeGameJolt();

			// Assert
			await AssertTimeout(() => GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.", TimeSpan.FromSeconds(AUTH_TIMEOUT));
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
		}

		[Test]
		public async Task AutoCloseSessions_SessionClosed([Values] InitializationType initializationType)
		{
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			GameJoltSettings.AutoCloseSessions = true;

			// Act
			manager.InitializeGameJolt();
			await AuthenticateAsync();

			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True, "User is not authenticated.");
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");

			manager.Dispose();

			await AssertTimeout(() => GameJoltAPI.Sessions.IsSessionOpen, Is.False, "Session is still open.", TimeSpan.FromSeconds(AUTH_TIMEOUT));
		}

		[Test]
		public async Task AutoPingSessions_SessionPinged()
		{
			// Arrange
			GameJoltSettings.AutoInitialize = true;
			GameJoltSettings.AutoStartSessions = true;
			GameJoltSettings.AutoPingSessions = true;
			GameJoltSettings.PingInterval = 1f;

			// Act
			manager.InitializeGameJolt();
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

			// Assert
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True, "Session is not open.");
			Assert.That(pinged, Is.True, "Session was not pinged.");
		}

		public enum InitializationType
		{
			Editor,
			Client
		}

		private async ValueTask<bool> PrepareSignIn(InitializationType initializationType)
		{
#if !UNITY_EDITOR
			if (initializationType == InitializationType.Editor)
			{
				// Skip outside editor.
				return false;
			}
#endif

			switch (initializationType)
			{
#if UNITY_EDITOR
				case InitializationType.Editor:
					GameJoltSettings.AutoSignIn = true;
					break;
#endif
				case InitializationType.Client:
					GameJoltSettings.AutoSignInFromClient = true;

					string credentialsFileContent = $"blah\n{Username}\n{Token}";

					await File.WriteAllTextAsync(credentialsPath, credentialsFileContent);

					Assert.That(File.Exists(credentialsPath), Is.True, "Credentials file does not exist.");
					break;
			}

			SetUpWebClientForAuth();

			return true;
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
#endif // DISABLE_GAMEJOLT