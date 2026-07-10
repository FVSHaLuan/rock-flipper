using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AssetUpdaterMenu : IPreprocessBuildWithReport
{
    private static List<IAssetUpdater> updaters = new List<IAssetUpdater>();

    public int callbackOrder => 0;

    [MenuItem("FH/Update All Assets")]
    private static void UpdateAllAssetsMenu()
    {
        ///
        if (!EditorUtility.DisplayDialog("Are u sure?", "Update all game data now?", "OK", "Cancel"))
        {
            ///
            Debug.Log("Cancelled");

            ///
            return;
        }

        ///
        UpdateAllAssets();
    }

    private static void UpdateAllAssets()
    {
        EditorHelper.GetAllObjetsFromPath<IAssetUpdater>("Assets/", updaters);

        ///
        foreach (var updater in updaters)
        {
            ///
            updater.Editor_Update();

            ///
            Debug.LogFormat("Updated {0}", (updater as Object)?.name);
        }
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        ///
        UpdateAllAssets();
    }
}
