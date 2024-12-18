﻿#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// NOTE: This always runs, even if GameJolt is disabled. This is because Unity does not handle conditional compilation symbols very well when building for multiple platforms.
// For more information, see the notice in GameJoltSettings.cs.

namespace Hertzole.GameJolt.Editor
{
	internal sealed class GameJoltSettingsBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
	{
		private bool removeFromPreloadedAssets;

		private GameJoltSettings settingsInstance;

		public int callbackOrder
		{
			get { return -1_000_000; }
		}

		private const string SETTING_PATH = "Assets/" + SettingsHelper.PACKAGE_NAME + "_GameJoltSettings.asset";

		public void OnPreprocessBuild(BuildReport report)
		{
			// Don't do anything if the DISABLE_GAMEJOLT define is set.
			if (PlayerSettingsHelper.ContainsDefine("DISABLE_GAMEJOLT"))
			{
				return;
			}

			Application.logMessageReceivedThreaded += OnGetLog;

			removeFromPreloadedAssets = false;

			GameJoltSettings oldInstance = AssetDatabase.LoadAssetAtPath<GameJoltSettings>(SETTING_PATH);

			if (oldInstance != null)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oldInstance));
			}

			GameJoltSettings.Instance.hideFlags = HideFlags.None;

			AssetDatabase.CreateAsset(GameJoltSettings.Instance, SETTING_PATH);
			AssetDatabase.ImportAsset(SETTING_PATH);

			settingsInstance = AssetDatabase.LoadAssetAtPath<GameJoltSettings>(SETTING_PATH);

			Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
			bool wasDirty = IsPlayerSettingsDirty();

			if (!preloadedAssets.Contains(settingsInstance))
			{
				ArrayUtility.Add(ref preloadedAssets, settingsInstance);
				PlayerSettings.SetPreloadedAssets(preloadedAssets);

				removeFromPreloadedAssets = true;

				if (!wasDirty)
				{
					ClearPlayerSettingsDirtyFlag();
				}
			}

			EditorBuildSettings.AddConfigObject(SettingsHelper.PACKAGE_NAME, settingsInstance, true);
		}

		public void OnPostprocessBuild(BuildReport report)
		{
			// Don't do anything if the DISABLE_GAMEJOLT define is set.
			if (PlayerSettingsHelper.ContainsDefine("DISABLE_GAMEJOLT"))
			{
				return;
			}

			Application.logMessageReceivedThreaded -= OnGetLog;

			RemoveInstance();
		}

		private void RemoveInstance()
		{
			if (removeFromPreloadedAssets)
			{
				bool wasDirty = IsPlayerSettingsDirty();

				Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
				ArrayUtility.Remove(ref preloadedAssets, settingsInstance);
				PlayerSettings.SetPreloadedAssets(preloadedAssets);

				if (!wasDirty)
				{
					ClearPlayerSettingsDirtyFlag();
				}
			}

			if (EditorBuildSettings.TryGetConfigObject<GameJoltSettings>(SettingsHelper.PACKAGE_NAME, out _))
			{
				EditorBuildSettings.RemoveConfigObject(SettingsHelper.PACKAGE_NAME);
			}

			if (settingsInstance != null)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(settingsInstance));
			}

			settingsInstance = null;
		}

		private void OnBuildError()
		{
			RemoveInstance();
		}

		private void OnGetLog(string condition, string stacktrace, LogType type)
		{
			if (type == LogType.Error || type == LogType.Exception)
			{
				Application.logMessageReceivedThreaded -= OnGetLog;
				OnBuildError();
			}
		}

		private static bool IsPlayerSettingsDirty()
		{
			PlayerSettings[] settings = Resources.FindObjectsOfTypeAll<PlayerSettings>();
			if (settings != null && settings.Length > 0)
			{
				return EditorUtility.IsDirty(settings[0]);
			}

			return false;
		}

		private static void ClearPlayerSettingsDirtyFlag()
		{
			PlayerSettings[] settings = Resources.FindObjectsOfTypeAll<PlayerSettings>();
			if (settings != null && settings.Length > 0)
			{
				EditorUtility.ClearDirty(settings[0]);
			}
		}
	}
}
#endif // UNITY_EDITOR