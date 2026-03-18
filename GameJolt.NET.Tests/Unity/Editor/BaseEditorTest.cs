#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEditor;

namespace GameJolt.NET.Tests.Unity.Editor
{
	public abstract class BaseEditorTest : BaseTest
	{
		protected override async Task OnTearDownAsync()
		{
			if (EditorApplication.isPlaying)
			{
				await ExitPlayModeAsync();
			}
		}

		protected static async Task EnterPlayModeAsync()
		{
			bool hasEnteredPlayMode = false;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			try
			{
				if (EditorApplication.isPlaying)
				{
					throw new Exception("Editor is already in PlayMode");
				}

				if (EditorUtility.scriptCompilationFailed)
				{
					throw new Exception("Script compilation failed");
				}

				await Task.Delay(50);

				EditorApplication.UnlockReloadAssemblies();
				EditorApplication.isPlaying = true;

				do
				{
					await Task.Delay(100);
				} while (!hasEnteredPlayMode);
			}
			finally
			{
				EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			}

			void OnPlayModeStateChanged(PlayModeStateChange obj)
			{
				if (obj == PlayModeStateChange.EnteredPlayMode)
				{
					hasEnteredPlayMode = true;
				}
			}
		}

		protected static async Task ExitPlayModeAsync()
		{
			bool hasEnteredEditMode = false;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			try
			{
				if (!EditorApplication.isPlaying)
				{
					throw new Exception("Editor is not in PlayMode");
				}

				await Task.Delay(50);

				EditorApplication.isPlaying = false;

				do
				{
					await Task.Delay(100);
				} while (!hasEnteredEditMode);
			}
			finally
			{
				EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			}

			void OnPlayModeStateChanged(PlayModeStateChange obj)
			{
				if (obj == PlayModeStateChange.EnteredEditMode)
				{
					hasEnteredEditMode = true;
				}
			}
		}
	}
}
#endif // UNITY_EDITOR
#endif // !DISABLE_GAMEJOLT