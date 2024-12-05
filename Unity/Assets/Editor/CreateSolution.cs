#if UNITY_EDITOR

using Unity.CodeEditor;
using UnityEditor;

namespace GitTools
{
	public static class Solution
	{
		public static void Sync()
		{
			AssetDatabase.Refresh();
			CodeEditor.CurrentEditor.SyncAll();
		}
	}
}
#endif