using UnityEditor;

public static class ImportScripts
{
	public static void DoThing()
	{
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
}