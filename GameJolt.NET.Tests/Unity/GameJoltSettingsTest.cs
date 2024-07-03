#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_64
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameJolt.NET.Tests.Unity
{
	public sealed class GameJoltSettingsTest
	{
		private int gameId;
		private string privateGameKey;
		private bool autoInitialize;
		private bool autoShutdown;
		private bool autoSignInFromWeb;
		private bool autoSignInFromClient;
		private bool autoStartSessions;
		private bool autoCloseSessions;
		private bool autoPingSessions;
		private SessionStatus pingStatus;
		private float pingInterval;

		private readonly Faker faker = new Faker();

#if UNITY_EDITOR
		private bool autoSignIn;
		private string signInUsername;
		private string signInToken;
#endif

		[OneTimeSetUp]
		public void SetUp()
		{
			gameId = GameJoltSettings.GameId;
			privateGameKey = GameJoltSettings.PrivateGameKey;
			autoInitialize = GameJoltSettings.AutoInitialize;
			autoShutdown = GameJoltSettings.AutoShutdown;
			autoSignInFromWeb = GameJoltSettings.AutoSignInFromWeb;
			autoSignInFromClient = GameJoltSettings.AutoSignInFromClient;
			autoStartSessions = GameJoltSettings.AutoStartSessions;
			autoCloseSessions = GameJoltSettings.AutoCloseSessions;
			autoPingSessions = GameJoltSettings.AutoPingSessions;
			pingStatus = GameJoltSettings.PingStatus;
			pingInterval = GameJoltSettings.PingInterval;

#if UNITY_EDITOR
			autoSignIn = GameJoltSettings.AutoSignIn;
			signInUsername = GameJoltSettings.SignInUsername;
			signInToken = GameJoltSettings.SignInToken;
#endif
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			GameJoltSettings.GameId = gameId;
			GameJoltSettings.PrivateGameKey = privateGameKey;
			GameJoltSettings.AutoInitialize = autoInitialize;
			GameJoltSettings.AutoShutdown = autoShutdown;
			GameJoltSettings.AutoSignInFromWeb = autoSignInFromWeb;
			GameJoltSettings.AutoSignInFromClient = autoSignInFromClient;
			GameJoltSettings.AutoStartSessions = autoStartSessions;
			GameJoltSettings.AutoCloseSessions = autoCloseSessions;
			GameJoltSettings.AutoPingSessions = autoPingSessions;
			GameJoltSettings.PingStatus = pingStatus;
			GameJoltSettings.PingInterval = pingInterval;

#if UNITY_EDITOR
			GameJoltSettings.AutoSignIn = autoSignIn;
			GameJoltSettings.SignInUsername = signInUsername;
			GameJoltSettings.SignInToken = signInToken;
#endif
		}

		[Test]
		public void SetGameId()
		{
			int value = 0;
			while (value == 0)
			{
				value = faker.Random.Int();
			}

			GameJoltSettings.GameId = value;

			Assert.That(GameJoltSettings.GameId, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.gameId, Is.EqualTo(value));
		}

		[Test]
		public void SetPrivateGameKey()
		{
			string value = faker.Lorem.Sentence();
			GameJoltSettings.PrivateGameKey = value;

			Assert.That(GameJoltSettings.PrivateGameKey, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.privateGameKey, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoInitialize([Values] bool value)
		{
			GameJoltSettings.AutoInitialize = value;

			Assert.That(GameJoltSettings.AutoInitialize, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoInitialize, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoShutdown([Values] bool value)
		{
			GameJoltSettings.AutoShutdown = value;

			Assert.That(GameJoltSettings.AutoShutdown, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoShutdown, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoSignInFromWeb([Values] bool value)
		{
			GameJoltSettings.AutoSignInFromWeb = value;

			Assert.That(GameJoltSettings.AutoSignInFromWeb, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoSignInFromWeb, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoSignInFromClient([Values] bool value)
		{
			GameJoltSettings.AutoSignInFromClient = value;

			Assert.That(GameJoltSettings.AutoSignInFromClient, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoSignInFromClient, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoStartSessions([Values] bool value)
		{
			GameJoltSettings.AutoStartSessions = value;

			Assert.That(GameJoltSettings.AutoStartSessions, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoStartSessions, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoCloseSessions([Values] bool value)
		{
			GameJoltSettings.AutoCloseSessions = value;

			Assert.That(GameJoltSettings.AutoCloseSessions, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoCloseSessions, Is.EqualTo(value));
		}

		[Test]
		public void SetAutoPingSessions([Values] bool value)
		{
			GameJoltSettings.AutoPingSessions = value;

			Assert.That(GameJoltSettings.AutoPingSessions, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.autoPingSessions, Is.EqualTo(value));
		}

		[Test]
		public void SetPingStatus([Values] SessionStatus value)
		{
			GameJoltSettings.PingStatus = value;

			Assert.That(GameJoltSettings.PingStatus, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.pingStatus, Is.EqualTo(value));
		}

		[Test]
		public void SetPingInterval()
		{
			float value = faker.Random.Float(1f, 120f);

			GameJoltSettings.PingInterval = value;

			Assert.That(GameJoltSettings.PingInterval, Is.EqualTo(value));
			Assert.That(GameJoltSettings.Instance.pingInterval, Is.EqualTo(value));
		}

		[Test]
		public void PingInterval_Clamped()
		{
			float value = 0;

			GameJoltSettings.PingInterval = value;

			Assert.That(GameJoltSettings.PingInterval, Is.EqualTo(1f));

			value = 121;

			GameJoltSettings.PingInterval = value;

			Assert.That(GameJoltSettings.PingInterval, Is.EqualTo(120f));
		}

#if UNITY_EDITOR
		[Test]
		public void SetAutoSignIn([Values] bool value)
		{
			GameJoltSettings.AutoSignIn = value;

			Assert.That(GameJoltSettings.AutoSignIn, Is.EqualTo(value));
			Assert.That(EditorPrefs.GetBool(GameJoltSettings.AUTO_SIGN_IN_KEY), Is.EqualTo(value));
		}

		[Test]
		public void SetSignInUsername()
		{
			string value = faker.Internet.UserName();
			GameJoltSettings.SignInUsername = value;

			Assert.That(GameJoltSettings.SignInUsername, Is.EqualTo(value));
			Assert.That(EditorPrefs.GetString(GameJoltSettings.SIGN_IN_USERNAME_KEY), Is.EqualTo(value));
		}

		[Test]
		public void SetSignInToken()
		{
			string value = faker.Internet.Password();
			GameJoltSettings.SignInToken = value;

			Assert.That(GameJoltSettings.SignInToken, Is.EqualTo(value));
			Assert.That(EditorPrefs.GetString(GameJoltSettings.SIGN_IN_TOKEN_KEY), Is.EqualTo(value));
		}
#endif
	}
}
#endif
#endif // DISABLE_GAMEJOLT