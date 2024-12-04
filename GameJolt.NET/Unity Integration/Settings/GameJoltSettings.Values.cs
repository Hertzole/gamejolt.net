#if UNITY_64 // Only to make sure this is used in Unity
#if UNITY_EDITOR || !DISABLE_GAMEJOLT
#define ENABLE_GAMEJOLT // Always enable GameJolt in the editor so it can be worked on without switching platform. Disable it in the build if needed.
#endif // UNITY_EDITOR || !DISABLE_GAMEJOLT

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Hertzole.GameJolt
{
#if ENABLE_GAMEJOLT
	partial class GameJoltSettings
	{
		[SerializeField]
		internal int gameId = default;
		[SerializeField]
		internal string privateGameKey = default;
		[SerializeField]
		internal bool autoInitialize = true;
		[SerializeField]
		internal bool autoShutdown = true;
		[SerializeField]
		internal bool autoSignInFromWeb = true;
		[SerializeField]
		internal bool autoSignInFromClient = true;

		[SerializeField]
		internal bool autoStartSessions = true;
		[SerializeField]
		internal bool autoCloseSessions = true;
		[SerializeField]
		internal bool autoPingSessions = true;
		[SerializeField]
		internal SessionStatus pingStatus = SessionStatus.Active;
		[SerializeField]
		internal float pingInterval = 30f;

		public static int GameId
		{
			get { return Instance.gameId; }
			set { SetValue(ref Instance.gameId, value); }
		}

		public static string PrivateGameKey
		{
			get { return Instance.privateGameKey; }
			set { SetValue(ref Instance.privateGameKey, value); }
		}

		public static bool AutoInitialize
		{
			get { return Instance.autoInitialize; }
			set { SetValue(ref Instance.autoInitialize, value); }
		}

		public static bool AutoShutdown
		{
			get { return Instance.autoShutdown; }
			set { SetValue(ref Instance.autoShutdown, value); }
		}

		public static bool AutoSignInFromWeb
		{
			get { return Instance.autoSignInFromWeb; }
			set { SetValue(ref Instance.autoSignInFromWeb, value); }
		}

		public static bool AutoSignInFromClient
		{
			get { return Instance.autoSignInFromClient; }
			set { SetValue(ref Instance.autoSignInFromClient, value); }
		}

		public static bool AutoStartSessions
		{
			get { return Instance.autoStartSessions; }
			set { SetValue(ref Instance.autoStartSessions, value); }
		}

		public static bool AutoCloseSessions
		{
			get { return Instance.autoCloseSessions; }
			set { SetValue(ref Instance.autoCloseSessions, value); }
		}

		public static bool AutoPingSessions
		{
			get { return Instance.autoPingSessions; }
			set { SetValue(ref Instance.autoPingSessions, value); }
		}

		public static SessionStatus PingStatus
		{
			get { return Instance.pingStatus; }
			set { SetValue(ref Instance.pingStatus, value); }
		}

		public static float PingInterval
		{
			get { return Instance.pingInterval; }
			set
			{
				float v = Mathf.Clamp(value, 1f, 120f);
				if (value < 1f || value > 120f)
				{
					Debug.LogWarning($"Ping interval must be between 1 and 120 seconds. Clamping value to {v}");
				}

				SetValue(ref Instance.pingInterval, v, (a, b) => Mathf.Abs(a - b) > float.Epsilon);
			}
		}

		private static void SetValue<T>(ref T field, T value, Func<T, T, bool> comparer = null)
		{
			bool changed = comparer?.Invoke(field, value) ?? !EqualityComparer<T>.Default.Equals(field, value);

			if (changed)
			{
				field = value;
				Instance.EditorSave();
			}
		}

		[Conditional("UNITY_EDITOR")]
		private void EditorSave()
		{
#if UNITY_EDITOR
			SettingsHelper.Save(this);
#endif
		}
	}
#endif // ENABLE_GAMEJOLT
}
#endif // UNITY_64