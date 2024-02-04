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

			EditorGUILayout.LabelField("User Information", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Authenticated", GameJoltAPI.Users.IsAuthenticated);
			EditorGUILayout.TextField("Username", GameJoltAPI.Users.IsAuthenticated ? GameJoltAPI.Users.CurrentUser!.Value.Username : "Not Authenticated");

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Session Information", EditorStyles.boldLabel);

			EditorGUILayout.Toggle("Is Session Open", GameJoltAPI.Sessions.IsSessionOpen);
		}
	}
}
#endif