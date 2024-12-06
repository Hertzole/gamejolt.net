#if UNITY_EDITOR && GAMEJOLT_UNITY
using UnityEditor;

namespace Hertzole.GameJolt
{
	partial class GameJoltSettings
	{
		public static bool AutoSignIn
		{
			get { return EditorPrefs.GetBool(AUTO_SIGN_IN_KEY, false); }
			set { EditorPrefs.SetBool(AUTO_SIGN_IN_KEY, value); }
		}

		public static string SignInUsername
		{
			get { return EditorPrefs.GetString(SIGN_IN_USERNAME_KEY, string.Empty); }
			set { EditorPrefs.SetString(SIGN_IN_USERNAME_KEY, value); }
		}

		public static string SignInToken
		{
			get { return EditorPrefs.GetString(SIGN_IN_TOKEN_KEY, string.Empty); }
			set { EditorPrefs.SetString(SIGN_IN_TOKEN_KEY, value); }
		}
		internal const string AUTO_SIGN_IN_KEY = "GameJolt.NET.AutoSignIn";
		internal const string SIGN_IN_USERNAME_KEY = "GameJolt.NET.SignInUsername";
		internal const string SIGN_IN_TOKEN_KEY = "GameJolt.NET.SignInToken";
	}
}
#endif // UNITY_EDITOR && GAMEJOLT_UNITY