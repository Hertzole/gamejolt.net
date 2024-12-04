#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_6000_0_OR_NEWER
using UnityEditor.Build.Profile;
#endif

namespace Hertzole.GameJolt.Editor
{
	internal static class PlayerSettingsHelper
	{
		public static bool ContainsDefine(string define)
		{
			HashSet<string> defines = new HashSet<string>();
			// Get for the current platform.
			PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup),
				out string[] buildTargetGroupDefines);

			foreach (string s in buildTargetGroupDefines)
			{
				defines.Add(s);
			}

			// Get for the active build target group.
			foreach (string s in EditorUserBuildSettings.activeScriptCompilationDefines)
			{
				defines.Add(s);
			}

#if UNITY_6000_0_OR_NEWER
			// Get for the active build profile.
			foreach (string s in BuildProfile.GetActiveBuildProfile().scriptingDefines)
			{
				defines.Add(s);
			}
#endif

			return defines.Contains(define);
		}
	}
}
#endif // UNITY_EDITOR