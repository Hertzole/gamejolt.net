#if UNITY_EDITOR
using System.IO;
using Hertzole.GameJolt;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GameJolt.NET.Tests.Unity.Editor
{
	public sealed class SettingsHelperTest
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

		private GameJoltSettings settingsInstance;
		public GameJoltSettings SettingsInstance
		{
			get { return settingsInstance; }
			set
			{
				if (settingsInstance != null && settingsInstance != value)
				{
					Object.DestroyImmediate(settingsInstance);
				}

				settingsInstance = value;
			}
		}

		[SetUp]
		public void Setup()
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

			SettingsHelper.RemoveFile(SettingsHelper.SETTING_PATH);

			if (Directory.Exists(SettingsHelper.ROOT_FOLDER))
			{
				Directory.Delete(SettingsHelper.ROOT_FOLDER, true);
			}

			LogAssert.ignoreFailingMessages = false;
		}

		[TearDown]
		public void Teardown()
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

			SettingsHelper.RemoveFile(SettingsHelper.SETTING_PATH);

			if (Directory.Exists(SettingsHelper.ROOT_FOLDER))
			{
				Directory.Delete(SettingsHelper.ROOT_FOLDER, true);
			}

			if (SettingsInstance != null)
			{
				Object.DestroyImmediate(SettingsInstance);
			}

			LogAssert.ignoreFailingMessages = false;
		}

		[Test]
		public void Save_NoDirectory_Success()
		{
			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.False);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.False);

			SettingsInstance = GetSettings();
			SettingsInstance.gameId = 1234;

			SettingsHelper.Save(SettingsInstance);

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);

			SettingsInstance = SettingsHelper.Load();

			Assert.That(SettingsInstance == null, Is.False);
			Assert.That(SettingsInstance.gameId, Is.EqualTo(1234));
		}

		[Test]
		public void Save_ExistingDirectory_Success()
		{
			Directory.CreateDirectory(SettingsHelper.ROOT_FOLDER);

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.False);

			SettingsInstance = GetSettings();
			SettingsInstance.gameId = 1234;

			SettingsHelper.Save(SettingsInstance);

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);

			SettingsInstance = SettingsHelper.Load();

			Assert.That(SettingsInstance == null, Is.False);
			Assert.That(SettingsInstance.gameId, Is.EqualTo(1234));
		}

		[Test]
		public void Load_NoExisting_CreatesNew()
		{
			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.False);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.False);

			SettingsInstance = SettingsHelper.Load();

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);
			Assert.That(SettingsInstance == null, Is.False);
			Assert.That(SettingsInstance.gameId, Is.EqualTo(0));
		}

		[Test]
		public void Load_Existing_Success()
		{
			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.False);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.False);

			SettingsInstance = GetSettings();
			SettingsInstance.gameId = 1234;
			SettingsHelper.Save(SettingsInstance);

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);

			SettingsInstance = SettingsHelper.Load();

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);
			Assert.That(SettingsInstance == null, Is.False);
			Assert.That(SettingsInstance.gameId, Is.EqualTo(1234));
		}

		[Test]
		public void Load_InvalidFile_Success()
		{
			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.False);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.False);

			SettingsInstance = GetSettings();
			SettingsInstance.gameId = 1234;
			SettingsHelper.Save(SettingsInstance);

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);

			File.WriteAllText(SettingsHelper.SETTING_PATH, "This is not a valid file.");

			// Ignore error logs.
			LogAssert.ignoreFailingMessages = true;

			SettingsInstance = SettingsHelper.Load();

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);
			Assert.That(SettingsInstance == null, Is.False);
			Assert.That(SettingsInstance.gameId, Is.EqualTo(0));
		}

		[Test]
		public void Load_InvalidReadOnlyFile_Success()
		{
			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.False);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.False);

			SettingsInstance = GetSettings();
			SettingsInstance.gameId = 1234;
			SettingsHelper.Save(SettingsInstance);

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);

			File.WriteAllText(SettingsHelper.SETTING_PATH, "This is not a valid file.");
			File.SetAttributes(SettingsHelper.SETTING_PATH, FileAttributes.ReadOnly);

			// Ignore error logs.
			LogAssert.ignoreFailingMessages = true;

			SettingsInstance = SettingsHelper.Load();

			Assert.That(Directory.Exists(SettingsHelper.ROOT_FOLDER), Is.True);
			Assert.That(File.Exists(SettingsHelper.SETTING_PATH), Is.True);
			Assert.That(SettingsInstance == null, Is.False);
			Assert.That(SettingsInstance.gameId, Is.EqualTo(0));
		}

		private GameJoltSettings GetSettings()
		{
			if (SettingsInstance == null)
			{
				SettingsInstance = ScriptableObject.CreateInstance<GameJoltSettings>();
			}

			return SettingsInstance;
		}
	}
}
#endif