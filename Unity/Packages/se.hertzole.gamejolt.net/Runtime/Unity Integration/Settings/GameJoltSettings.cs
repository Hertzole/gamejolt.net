#if UNITY_64
using System.Diagnostics;
using UnityEngine;

// NOTICE: This class is always included in the build, even if GameJolt is disabled.
// This is due to that Unity does not handle conditional compilation symbols very well when building for multiple platforms.
// So even if GameJolt is disabled, this class and settings object will be included in the build, although it will not do anything and be empty.

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Holds the Game Jolt settings.
	/// </summary>
	public sealed partial class GameJoltSettings : ScriptableObject
	{
		private static GameJoltSettings instance;

		internal static GameJoltSettings Instance
		{
			get
			{
#if UNITY_EDITOR
				GetInstanceInEditor();
#endif // UNITY_EDITOR
				return instance;
			}
		}

		[Conditional("UNITY_EDITOR")]
		private static void GetInstanceInEditor()
		{
#if UNITY_EDITOR
			if (instance != null)
			{
				return;
			}

			instance = SettingsHelper.Load();
			instance.hideFlags = HideFlags.HideAndDontSave;
#endif // UNITY_EDITOR
		}

#if !UNITY_EDITOR
		private void OnEnable()
		{
			instance = this;
		}
#endif
	}
}
#endif // UNITY_64