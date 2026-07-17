using Agame.FeatureBranching;
using Google.Android.AppBundle.Editor;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BranchedBuilder : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => int.MinValue + 1;

    public void OnPostprocessBuild(BuildReport report)
    {
        ///
        //if (Instance == null)
        //{
        //    EditorUtility.DisplayDialog("Error", $"BranchedBuildConfig instance not found at {AssetPath}", "OK");
        //    throw new BuildFailedException($"BranchedBuildConfig instance not found at {AssetPath}");
        //}

        ///
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"version = {PlayerSettings.bundleVersion}");
        sb.AppendLine($"branch = {VersionBranchInfo.Current}");

        ///
        var buildPath = report.summary.outputPath;
        string buildFolder = Directory.Exists(buildPath)
          ? buildPath
          : Path.GetDirectoryName(buildPath);
        string filePath = Path.Combine(buildFolder, "buildCfg.ini");

        ///
        System.IO.File.WriteAllText(System.IO.Path.Combine(report.summary.outputPath, filePath), sb.ToString());

        ///
        Debug.Log($"[BranchedBuildConfig] Wrote build config to {filePath}");
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        ///
        var config = BranchedBuildConfig.Instance;
        if (config == null)
        {
            EditorUtility.DisplayDialog("Error", $"BranchedBuildConfig instance not found at {BranchedBuildConfig.AssetPath}", "OK");
            throw new BuildFailedException($"BranchedBuildConfig instance not found at {BranchedBuildConfig.AssetPath}");
        }

        ///
        if (!BranchedBuildConfig.CheckInitiatedBuild())
        {
            bool allowed = true;
            if (!allowed)
            {
                EditorUtility.DisplayDialog("Error", "Must initiate build from BranchedBuildConfig", "Cancel Build");
                // ping the instance
                Selection.activeObject = config;
                throw new BuildFailedException($"Didn't initiate build from BranchedBuildConfig (can be found at {BranchedBuildConfig.AssetPath})");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Must initiate build from BranchedBuildConfig.\r\nBut you're allowed for now!", "Continue Build");
            }
        }

        // Correct version
        if (Application.version != config.Version)
        {
            EditorUtility.DisplayDialog("Version Mismatched", $"Corrected to {config.Version}", "OK");
            PlayerSettings.bundleVersion = config.Version;
        }
    }
}
