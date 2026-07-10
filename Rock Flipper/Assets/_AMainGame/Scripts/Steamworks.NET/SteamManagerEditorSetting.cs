#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SteamManagerEditorSetting : ScriptableObject
{
    private const string Path = "Assets/_Exp/SteamManagerEditorSetting.asset";

    [SerializeField]
    private bool enabled;

#if UNITY_EDITOR
    public static SteamManagerEditorSetting Instance
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<SteamManagerEditorSetting>(Path);
        }
    }

    public bool Enabled => enabled;
#endif
}

