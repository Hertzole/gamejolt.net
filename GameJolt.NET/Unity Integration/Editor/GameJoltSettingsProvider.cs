#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hertzole.GameJolt.Editor
{
	internal sealed class GameJoltSettingsProvider : SettingsProvider
	{
		public GameJoltSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

		private class Labels
		{
			public static readonly GUIContent gameSettings = new GUIContent("Game Settings");
			public static readonly GUIContent gameId = new GUIContent("Game ID", "The ID of your game on GameJolt.");
			public static readonly GUIContent privateGameKey = new GUIContent("Private Game Key", "The private key of your game on GameJolt.");
			public static readonly GUIContent autoInitialize = new GUIContent("Auto Initialize",
				"If true, the GameJoltManager will automatically initialize when the game starts. If false, you have to call GameJoltAPI.Initialize() yourself.");
			public static readonly GUIContent autoShutdown = new GUIContent("Auto Shutdown",
				"If true, the GameJoltManager will automatically shut down when the game stops. If false, you have to call GameJoltAPI.Shutdown() yourself.");
			public static readonly GUIContent autoSignInFromWeb = new GUIContent("Auto Sign-In From Web",
				"If true, the GameJoltManager will automatically sign in the user when the game starts in a web player.");
			public static readonly GUIContent autoSignInFromClient = new GUIContent("Auto Sign-In From Client",
				"If true, the GameJoltManager will automatically sign in the user when the game starts from the Game Jolt client.");

			public static readonly GUIContent editorSettings = new GUIContent("Editor Settings");
			public static readonly GUIContent
				autoSignIn = new GUIContent("Auto Sign-In", "Automatically sign in when the game starts, but only in the editor!");
			public static readonly GUIContent signInUsername = new GUIContent("Username",
				"The username to sign in with. This is only used in the editor and will not be saved to a shared file.");
			public static readonly GUIContent signInToken = new GUIContent("User Token",
				"The user token to sign in with. This is only used in the editor and will not be saved to a shared file.");
			public static readonly GUIContent sessionSettings = new GUIContent("Session Settings");
			public static readonly GUIContent autoStartSessions =
				new GUIContent("Auto Start Sessions", "Automatically start a session when the player has signed in.");
			public static readonly GUIContent autoCloseSessions =
				new GUIContent("Auto Close Sessions", "Automatically close the session when the player has signed out.");
			public static readonly GUIContent autoPingSessions =
				new GUIContent("Auto Ping Sessions", "Automatically ping the session when the a session is running.");
			public static readonly GUIContent pingStatus = new GUIContent("Ping Status", "The status to ping the session with.");
		}

		public override void OnGUI(string searchContext)
		{
			// Move the content to the right a bit.
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginVertical();

			// Add some space to the top.
			GUILayout.Space(9);

			// Set the label width to 250, 
			float oLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 250;

			GUILayout.Label(Labels.gameSettings, EditorStyles.boldLabel);
			int field = EditorGUILayout.IntField(Labels.gameId, GameJoltSettings.GameId);

			if (field < 0)
			{
				field = 0;
			}

			if (field != GameJoltSettings.GameId)
			{
				GameJoltSettings.GameId = field;
			}

			GameJoltSettings.PrivateGameKey = EditorGUILayout.TextField(Labels.privateGameKey, GameJoltSettings.PrivateGameKey);
			GameJoltSettings.AutoInitialize = EditorGUILayout.Toggle(Labels.autoInitialize, GameJoltSettings.AutoInitialize);
			GameJoltSettings.AutoShutdown = EditorGUILayout.Toggle(Labels.autoShutdown, GameJoltSettings.AutoShutdown);
			GameJoltSettings.AutoSignInFromWeb = EditorGUILayout.Toggle(Labels.autoSignInFromWeb, GameJoltSettings.AutoSignInFromWeb);
			GameJoltSettings.AutoSignInFromClient = EditorGUILayout.Toggle(Labels.autoSignInFromClient, GameJoltSettings.AutoSignInFromClient);

			EditorGUILayout.Space();
			GUILayout.Label(Labels.editorSettings, EditorStyles.boldLabel);
			
			EditorGUILayout.HelpBox("These settings are only used in the editor and will only be saved for you. They are not shared with anyone else.", MessageType.Info);

			GameJoltSettings.AutoSignIn = EditorGUILayout.Toggle(Labels.autoSignIn, GameJoltSettings.AutoSignIn);
			bool oEnabled = GUI.enabled;
			GUI.enabled = GameJoltSettings.AutoSignIn;
			GameJoltSettings.SignInUsername = EditorGUILayout.TextField(Labels.signInUsername, GameJoltSettings.SignInUsername);
			GameJoltSettings.SignInToken = EditorGUILayout.TextField(Labels.signInToken, GameJoltSettings.SignInToken);
			GUI.enabled = oEnabled;

			EditorGUILayout.Space();
			GUILayout.Label(Labels.sessionSettings, EditorStyles.boldLabel);

			GameJoltSettings.AutoStartSessions = EditorGUILayout.Toggle(Labels.autoStartSessions, GameJoltSettings.AutoStartSessions);
			GameJoltSettings.AutoCloseSessions = EditorGUILayout.Toggle(Labels.autoCloseSessions, GameJoltSettings.AutoCloseSessions);
			GameJoltSettings.AutoPingSessions = EditorGUILayout.Toggle(Labels.autoPingSessions, GameJoltSettings.AutoPingSessions);
			oEnabled = GUI.enabled;
			GUI.enabled = GameJoltSettings.AutoPingSessions;
			GameJoltSettings.PingStatus = (SessionStatus) EditorGUILayout.EnumPopup(Labels.pingStatus, GameJoltSettings.PingStatus);
			GameJoltSettings.PingInterval = EditorGUILayout.Slider("Ping Interval", GameJoltSettings.PingInterval, 1f, 120f);
			GUI.enabled = oEnabled;

			// Reset the label width.
			EditorGUIUtility.labelWidth = oLabelWidth;

			// End the vertical group.
			GUILayout.EndVertical();
			// End the horizontal group.
			GUILayout.EndHorizontal();
		}

		[SettingsProvider]
		private static SettingsProvider CreateProvider()
		{
			return new GameJoltSettingsProvider("Hertzole/GameJolt", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Labels>());
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT