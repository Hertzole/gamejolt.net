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
			
			while (!EditorApplication.isPlaying)
			{
				await Task.Delay(50);
			}
		}
		
		protected static async Task ExitPlayModeAsync()
		{
			if (!EditorApplication.isPlaying)
			{
				throw new Exception("Editor is not in PlayMode");
			}
			
			await Task.Delay(50);
			
			EditorApplication.isPlaying = false;
			
			while (EditorApplication.isPlaying)
			{
				await Task.Delay(50);
			}
		}
	}
}
#endif // UNITY_EDITOR