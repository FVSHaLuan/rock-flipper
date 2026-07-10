using BT.FeatureBranching;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Serialization;

public class BranchedBuildConfig : ScriptableObject
{
    public const string AssetPath = "Assets/_AMainGame/Data/BranchedBuildConfig.asset";

    private static BranchedBuildConfig instance;
    private static bool initiatedBuild;

    public static BranchedBuildConfig Instance
    {
        get
        {

            if (instance == null)
            {
                instance = AssetDatabase.LoadAssetAtPath<BranchedBuildConfig>(AssetPath);
            }

            ///
            return instance;
        }
    }

    [SerializeField]
    private string version = "0.0.0";
    [SerializeField, FormerlySerializedAs("buildFolder")]
#if UNITY_EDITOR_OSX
    [ReadOnly]
#endif
    private string windowsBuildFolder = "D:\\Builds\\BT Builds";
    [SerializeField]
#if UNITY_EDITOR_WIN
    [ReadOnly]
#endif
    private string macBuildFolder = "D:\\Builds\\BT Builds";
    [SerializeField]
    private string buildExecutableName = "Ballatory";

    private string buildFolder
    {
        get
        {

#if UNITY_EDITOR_WIN
            return windowsBuildFolder;
#elif UNITY_EDITOR_OSX
            return macBuildFolder;
#else
            throw new System.NotImplementedException("Not implemented for this editor");
#endif
        }
    }
    public string Version => version;

    [ContextMenu("Open Build Folder")]
    private void OpenBuildFolder()
    {
        if (Directory.Exists(buildFolder))
        {
            EditorUtility.RevealInFinder(buildFolder + "/");
        }
        else
        {
            EditorUtility.DisplayDialog("Error", $"Build folder does not exist: {buildFolder}", "OK");
        }
    }

    [ContextMenu("Open Target Folder")]
    private void OpenTargetFolder()
    {
        EditorUtility.RevealInFinder(GetBuildPath(out var platformPath));
    }

    [ContextMenu("Build Current"), EditorModeOnly]
    private void BuildCurrent()
    {
        ///
        var targetPath = GetBuildPath(out var platformPath);

        ///
        if (!Directory.Exists(platformPath))
        {
            Directory.CreateDirectory(platformPath);
        }

        ///
        BuildReport buildReport;

        ///
        var buildProfile = BuildProfile.GetActiveBuildProfile();
        if (buildProfile != null)
        {
            BuildPlayerWithProfileOptions options = new BuildPlayerWithProfileOptions
            {
                locationPathName = targetPath,
                options = BuildOptions.None,
                buildProfile = buildProfile,
            };

            ///        
            initiatedBuild = true;
            buildReport = BuildPipeline.BuildPlayer(options);
        }
        else
        {
            ///
            throw new BuildFailedException("Currently only support Build Profiles. Please choose one!");

            ///
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                locationPathName = targetPath
            };

            ///
            buildPlayerOptions = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(buildPlayerOptions);

            ///        
            initiatedBuild = true;
            buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        ///
        if (buildReport.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {targetPath}");
            EditorUtility.RevealInFinder(targetPath);
        }
        else
        {
            Debug.LogError($"Build failed: {buildReport.summary.result}");
        }
    }

    private string GetBuildPath(out string platformPath)
    {
        string branchFolder = VersionBranchInfo.Current == VersionBranch.Full ? "Release" : VersionBranchInfo.Current.ToString();
        string platformFolder;
        string targetName;
        switch (PlatformBranchInfo.Current)
        {
            case PlatformBranch.PC:
                platformFolder = "Windows";
                targetName = Instance.buildExecutableName + ".exe";
                break;
            case PlatformBranch.Mac:
                platformFolder = "Mac";
                targetName = $"{Instance.buildExecutableName}.app";
                break;
            case PlatformBranch.Linux:
                platformFolder = "Linux";
                targetName = $"{Instance.buildExecutableName}.x86_64";
                break;
            case PlatformBranch.Web:
                platformFolder = "Web";
                targetName = "";
                break;
            default:
                throw new System.NotSupportedException($"Platform branch not supported: {PlatformBranchInfo.Current}");
        }

        ///
        platformPath = Path.Combine(buildFolder, branchFolder, platformFolder);

        ///
        return Path.Combine(buildFolder, branchFolder, platformFolder, targetName);
    }

    public static bool CheckInitiatedBuild()
    {
        var rs = initiatedBuild;
        initiatedBuild = false;
        return rs;
    }
}
