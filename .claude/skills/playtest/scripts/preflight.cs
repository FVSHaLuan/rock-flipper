// Safe in Edit mode. Reports what a playtest would start from.
var sb = new System.Text.StringBuilder();
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
sb.AppendLine("activeScene: " + scene.path + (scene.isDirty ? " (UNSAVED CHANGES)" : ""));
sb.AppendLine("isPlaying: " + UnityEditor.EditorApplication.isPlaying + ", isCompiling: " + UnityEditor.EditorApplication.isCompiling);
sb.AppendLine("versionBranch: " + Agame.FeatureBranching.VersionBranchInfo.Current);
sb.AppendLine("persistentDataPath: " + UnityEngine.Application.persistentDataPath);

var test = UnityEditor.AssetDatabase.LoadAssetAtPath<Agame.Run.Stats.TestBuildStats>("Assets/_Exp/BuildStatsTest.asset");
sb.AppendLine("testBuildStats: " + (test != null && test.Enabled ? "ENABLED (" + test.BuildStats.name + ") - run starts from it, not from BuildStats - Default" : "off"));

// save files that Play mode reads and writes
var dir = UnityEngine.Application.persistentDataPath;
if (System.IO.Directory.Exists(dir))
{
    foreach (var f in System.IO.Directory.GetFiles(dir, "WritableData_*.FH"))
    {
        sb.AppendLine("  save file: " + System.IO.Path.GetFileName(f) + " (" + new System.IO.FileInfo(f).Length + " B)");
    }
}

// which asset each save file belongs to. PlayerData (holds lastSlotId, the slot the Run scene opens) is
// PlayerData_Demo on the Demo branch, PlayerData otherwise. GameSetting is the settings file.
foreach (var guid in UnityEditor.AssetDatabase.FindAssets("t:PlayerDataObject t:GameSettingObject t:RunDataObject"))
{
    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
    var so = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadMainAssetAtPath(path));
    sb.AppendLine("  asset " + System.IO.Path.GetFileNameWithoutExtension(path) + " -> WritableData_" + so.FindProperty("standAloneKey").stringValue + ".FH");
}
return sb.ToString();
