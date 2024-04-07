#if UNITY_2021_1_OR_NEWER
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.GameJolt
{
	public sealed class GameJoltSettings : ScriptableObject
	{
		[SerializeField]
		private int gameId = default;
		[SerializeField]
		private string privateGameKey = default;
		[SerializeField]
		private bool autoInitialize = true;
		[SerializeField]
		private bool autoShutdown = true;
		[SerializeField]
		private bool autoSignInFromWeb = true;
		[SerializeField]
		private bool autoSignInFromClient = true;

		[SerializeField]
		private bool autoStartSessions = true;
		[SerializeField]
		private bool autoCloseSessions = true;
		[SerializeField]
		private bool autoPingSessions = true;
		[SerializeField]
		private SessionStatus pingStatus = SessionStatus.Active;
		[SerializeField]
		private float pingInterval = 30f;

		private static GameJoltSettings instance;

		public static int GameId
		{
			get { return Instance.gameId; }
			set
			{
				if (Instance.gameId != value)
				{
					Instance.gameId = value;
					Instance.EditorSave();
				}
			}
		}

		public static string PrivateGameKey
		{
			get { return Instance.privateGameKey; }
			set
			{
				if (Instance.privateGameKey != value)
				{
					Instance.privateGameKey = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoInitialize
		{
			get { return Instance.autoInitialize; }
			set
			{
				if (Instance.autoInitialize != value)
				{
					Instance.autoInitialize = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoShutdown
		{
			get { return Instance.autoShutdown; }
			set
			{
				if (Instance.autoShutdown != value)
				{
					Instance.autoShutdown = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoSignInFromWeb
		{
			get { return Instance.autoSignInFromWeb; }
			set
			{
				if (Instance.autoSignInFromWeb != value)
				{
					Instance.autoSignInFromWeb = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoSignInFromClient
		{
			get { return Instance.autoSignInFromClient; }
			set
			{
				if (Instance.autoSignInFromClient != value)
				{
					Instance.autoSignInFromClient = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoStartSessions
		{
			get { return Instance.autoStartSessions; }
			set
			{
				if (Instance.autoStartSessions != value)
				{
					Instance.autoStartSessions = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoCloseSessions
		{
			get { return Instance.autoCloseSessions; }
			set
			{
				if (Instance.autoCloseSessions != value)
				{
					Instance.autoCloseSessions = value;
					Instance.EditorSave();
				}
			}
		}

		public static bool AutoPingSessions
		{
			get { return Instance.autoPingSessions; }
			set
			{
				if (Instance.autoPingSessions != value)
				{
					Instance.autoPingSessions = value;
					Instance.EditorSave();
				}
			}
		}

		public static SessionStatus PingStatus
		{
			get { return Instance.pingStatus; }
			set
			{
				if (Instance.pingStatus != value)
				{
					Instance.pingStatus = value;
					Instance.EditorSave();
				}
			}
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

				if (Mathf.Abs(Instance.pingInterval - v) > float.Epsilon)
				{
					Instance.pingInterval = v;
					Instance.EditorSave();
				}
			}
		}

#if UNITY_EDITOR
		public static bool AutoSignIn
		{
			get { return EditorPrefs.GetBool("GameJolt.NET.AutoSignIn", false); }
			set { EditorPrefs.SetBool("GameJolt.NET.AutoSignIn", value); }
		}

		public static string SignInUsername
		{
			get { return EditorPrefs.GetString("GameJolt.NET.SignInUsername", string.Empty); }
			set { EditorPrefs.SetString("GameJolt.NET.SignInUsername", value); }
		}

		public static string SignInToken
		{
			get { return EditorPrefs.GetString("GameJolt.NET.SignInToken", string.Empty); }
			set { EditorPrefs.SetString("GameJolt.NET.SignInToken", value); }
		}
#endif

		internal static GameJoltSettings Instance
		{
			get
			{
#if UNITY_EDITOR
				if (instance != null)
				{
					return instance;
				}

				instance = SettingsHelper.Load();
				instance.hideFlags = HideFlags.HideAndDontSave;
				return instance;
#else
				return instance;
#endif
			}
		}

#if !UNITY_EDITOR
		private void OnEnable()
		{
			instance = this;
		}
#endif

		[Conditional("UNITY_EDITOR")]
		private void EditorSave()
		{
#if UNITY_EDITOR
			SettingsHelper.Save(this);
#endif
		}
	}
}
#endif