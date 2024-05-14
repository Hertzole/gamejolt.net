using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class ApiTest
	{
#if UNITY_64 // Just for the Unity version.
		[SetUp]
		public void SetUp()
		{
			Assert.That(GameJoltSettings.AutoInitialize, Is.False, "AutoInitialize is not supported when running tests.");
		}
#endif

		[Test]
		public void Shutdown_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(GameJoltAPI.Shutdown, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void AccessUsers_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(() => { _ = GameJoltAPI.Users; }, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void AccessSessions_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(() => { _ = GameJoltAPI.Sessions; }, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void AccessScores_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(() => { _ = GameJoltAPI.Scores; }, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void AccessTrophies_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(() => { _ = GameJoltAPI.Trophies; }, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void AccessDataStore_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(() => { _ = GameJoltAPI.DataStore; }, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void AccessFriends_NotInitialized_ThrowsException()
		{
			Assert.That(GameJoltAPI.IsInitialized, Is.False);
			Assert.That(() => { _ = GameJoltAPI.Friends; }, Throws.TypeOf<GameJoltInitializationException>());
		}

		[Test]
		public void Initialize_Success()
		{
			GameJoltAPI.Initialize(0, "");
			Assert.That(GameJoltAPI.IsInitialized, Is.True);
			GameJoltAPI.Shutdown(); // Clean up.
		}

		[Test]
		public void Initialize_OnInitialized_IsInvoked()
		{
			bool invoked = false;
			GameJoltAPI.OnInitialized += Initialized;

			GameJoltAPI.Initialize(0, "");

			Assert.That(invoked, Is.True);

			GameJoltAPI.Shutdown(); // Clean up.
			GameJoltAPI.OnInitialized -= Initialized;
			return;

			void Initialized()
			{
				invoked = true;
			}
		}

		[Test]
		public void Shutdown_OnShutdown_IsInvoked()
		{
			bool invoked = false;
			GameJoltAPI.OnShutdown += Shutdown;

			GameJoltAPI.Initialize(0, "");
			GameJoltAPI.Shutdown();
			GameJoltAPI.OnShutdown -= Shutdown;

			Assert.That(invoked, Is.True);
			return;

			void Shutdown()
			{
				invoked = true;
				// It should still be initialized until the shutdown is complete.
				Assert.That(GameJoltAPI.IsInitialized, Is.True);
			}
		}

		[Test]
		public void ShutdownComplete_OnShutdownComplete_IsInvoked()
		{
			bool invoked = false;

			GameJoltAPI.OnShutdownComplete += ShutdownComplete;

			GameJoltAPI.Initialize(0, "");
			GameJoltAPI.Shutdown();
			GameJoltAPI.OnShutdownComplete -= ShutdownComplete;

			Assert.That(invoked, Is.True);
			return;

			void ShutdownComplete()
			{
				invoked = true;
				Assert.That(GameJoltAPI.IsInitialized, Is.False);
			}
		}
	}
}