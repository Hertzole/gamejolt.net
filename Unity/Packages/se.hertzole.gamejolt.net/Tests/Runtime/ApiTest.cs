using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class ApiTest
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
		public void Initialize_OnInitialized_IsInvoked()
		{
			bool invoked = false;
			GameJoltAPI.OnInitialized += () => invoked = true;

			GameJoltAPI.Initialize(0, "");

			Assert.That(invoked, Is.True);

			GameJoltAPI.Shutdown(); // Clean up.
		}

		[Test]
		public void Shutdown_OnShutdown_IsInvoked()
		{
			bool invoked = false;
			GameJoltAPI.OnShutdown += () =>
			{
				invoked = true;
				// It should still be initialized until the shutdown is complete.
				Assert.That(GameJoltAPI.IsInitialized, Is.True);
			};

			GameJoltAPI.Initialize(0, "");
			GameJoltAPI.Shutdown();

			Assert.That(invoked, Is.True);
		}

		[Test]
		public void ShutdownComplete_OnShutdownComplete_IsInvoked()
		{
			bool invoked = false;
			GameJoltAPI.OnShutdownComplete += () =>
			{
				invoked = true;
				Assert.That(GameJoltAPI.IsInitialized, Is.False);
			};

			GameJoltAPI.Initialize(0, "");
			GameJoltAPI.Shutdown();

			Assert.That(invoked, Is.True);
		}
	}
}