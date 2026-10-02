using Agame.FeatureBranching;
using Agame.Run;
using Agame.Run.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    // Resource paths
    public const string EntryResourcePath = "OV Entry";

    // Scene names
    public const string SceneHomeName = "Main";
    public const string SceneRunName = "Run";
    public const string SceneFakeOSName = "FakeOS";

    // Tags    
    public const string TagBoundary = "Boundary";
    public const string TagEditorOnly = "EditorOnly";
    public const string TagBall = "Ball";
    public const string TagMultButton = "MultButton";

    // Layers    
    public const string LayerCore = "Core";

    // Content
    public const int BackgroundCount = 6;
    public const int MaxStage = 131;

    // Steam    
    public static uint MainSteamAppId => 5151920U;
    public static uint PlayTestSteamAppId => 5186480U;
    public static uint DemoSteamAppId => 5373190U;
    public static uint SteamAppId
    {
        get
        {
            switch (VersionBranchInfo.Current)
            {
                case VersionBranch.All:
                    throw new System.NotImplementedException("Invalid version branch: All");
                case VersionBranch.Full:
                    return MainSteamAppId;
                case VersionBranch.Demo:
                    return DemoSteamAppId;
                case VersionBranch.Playtest:
                    return PlayTestSteamAppId;
                default:
                    throw new System.NotImplementedException("Unknown version branch");
            }
        }
    }
    public static uint SoundtrackAppId => 3920690U;

    // Demo
    public const int DemoMaxStage = 29;
    public const int DemoMaxSkillPoint = 150;

    // IAP
    public const string PremiumProductId = "premium";

    // URLs
    public static string SteamPageUrl => $"https://store.steampowered.com/app/{MainSteamAppId}?utm_source=ingame";
#if UNITY_ANDROID
    public static string SteamPageUrlMobile => $"https://store.steampowered.com/app/{MainSteamAppId}?utm_source=ingame_android";
#elif UNITY_IOS
    public static string SteamPageUrlMobile => $"https://store.steampowered.com/app/{MainSteamAppId}?utm_source=ingame_ios";
#else
    public static string SteamPageUrlMobile => $"https://store.steampowered.com/app/{MainSteamAppId}?utm_source=ingame_mobile";
#endif
    public const string NewsletterUrl = "https://minifun.games/newsletter/";
}
