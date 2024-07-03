#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.GameJolt
{
	internal static class SettingsHelper
	{
		internal const string ROOT_FOLDER = "ProjectSettings/Packages/" + PACKAGE_NAME;
		internal const string PACKAGE_NAME = "se.hertzole.gamejolt.net";
		internal const string SETTING_PATH = ROOT_FOLDER + "/GameJoltSettings.asset";

		public static void Save(GameJoltSettings settings)
		{
			if (!Directory.Exists(ROOT_FOLDER))
			{
				Directory.CreateDirectory(ROOT_FOLDER);
			}

			InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { settings }, SETTING_PATH, true);
		}

		public static GameJoltSettings Load()
		{
			GameJoltSettings settings = null;

			if (File.Exists(SETTING_PATH))
			{
				try
				{
					settings = (GameJoltSettings) InternalEditorUtility.LoadSerializedFileAndForget(SETTING_PATH)[0];
				}
				catch (Exception)
				{
					Debug.LogError("Could not load Game Jolt project settings. Settings will be reset.");
					settings = null;
				}
			}

			if (settings == null)
			{
				RemoveFile(SETTING_PATH);
				settings = ScriptableObject.CreateInstance<GameJoltSettings>();
				Save(settings);
			}

			return settings;
		}

		internal static void RemoveFile(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			FileAttributes attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) != 0)
			{
				File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
			}

			File.Delete(path);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT