#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Hertzole.GameJolt.Editor
{
	[CustomEditor(typeof(GameJoltManager))]
	internal sealed class GameJoltManagerEditor : UnityEditor.Editor
	{
		public override bool RequiresConstantRepaint()
		{
			return true;
		}

		public override void OnInspectorGUI()
		{
			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("Current Game Jolt state information will be shown here when playing.", MessageType.Info);
				return;
			}

			EditorGUILayout.Toggle("Is Initialized", GameJoltAPI.IsInitialized);

			EditorGUILayout.Space();

			if (!GameJoltAPI.IsInitialized)
			{
				return;
			}

			EditorGUILayout.LabelField("User Information", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Authenticated", GameJoltAPI.Users.IsAuthenticated);
			EditorGUILayout.TextField("Username", GameJoltAPI.Users.IsAuthenticated ? GameJoltAPI.Users.CurrentUser!.Value.Username : "Not Authenticated");

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Session Information", EditorStyles.boldLabel);

			bool oEnabled = GUI.enabled;

			GUI.enabled = GameJoltAPI.Users.IsAuthenticated;
			EditorGUILayout.Toggle("Is Session Open", GameJoltAPI.Sessions.IsSessionOpen);
			GUI.enabled = oEnabled;
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT