using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class ApiTest
	{
		[Test]
		public void Shutdown_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(GameJoltAPI.Shutdown);
		}

		[Test]
		public void AccessUsers_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(() => { _ = GameJoltAPI.Users; });
		}

		[Test]
		public void AccessSessions_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(() => { _ = GameJoltAPI.Sessions; });
		}

		[Test]
		public void AccessScores_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(() => { _ = GameJoltAPI.Scores; });
		}

		[Test]
		public void AccessTrophies_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(() => { _ = GameJoltAPI.Trophies; });
		}

		[Test]
		public void AccessDataStore_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(() => { _ = GameJoltAPI.DataStore; });
		}

		[Test]
		public void AccessFriends_NotInitialized_ThrowsException()
		{
			Assert.Throws<GameJoltInitializationException>(() => { _ = GameJoltAPI.Friends; });
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