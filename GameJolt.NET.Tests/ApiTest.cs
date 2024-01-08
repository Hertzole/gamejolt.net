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
	}
}