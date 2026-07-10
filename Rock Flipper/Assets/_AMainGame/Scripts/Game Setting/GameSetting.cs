using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSetting
{
    public static bool ForceMouseAndKeyboard { get; set; }

    [field: NonSerialized]
    public event System.Action OnLightDarknessValueChanged;
    [field: NonSerialized]
    public event System.Action OnTargetFPSChanged;
    [field: NonSerialized]
    public event System.Action OnDemoMusicUsageChanged;
    [field: NonSerialized]
    public event System.Action OnCoreThemeChanged;

    public bool enabledTerminal = false;
    public bool enabledSteamStats = true;

    [Header("Cheat")]
    public bool revealAllItemInDataBase = false;

    [Header("Gameplay")]
    public float screenShakingEffect = 0.8f;
    public float coreHitSoundEffectVolume = 0.8f;
    public bool pauseWhenLostFocus = true;
    public bool enabledRewardedAds = true;
    public bool holdToBlow = true;
    public bool enabledCoreFloatingDamageEffect = true;
    public bool enabledDamageLeaderboard = false;

    [Header("Graphics")]
    public bool enabledCard3DInspectorEffect = true;
    [SerializeField]
    private bool disableLighting;
    [SerializeField]
    private bool disableLighting_WebGL = true;
    [SerializeField]
    private bool colorblindMode;
    [SerializeField]
    private float lightDarkness = 1;
    [SerializeField]
    private int targetFPS = 60;

    [Header("Input")]
    [SerializeField]
    private InputDetectionMode inputDetectionMode;

    [Header("Meta")]
    public bool hideVersionInfo;
    public bool trailerMode;
    public bool showDemoLockedItem = true;
    public bool enableReporterOnMobile = false;

    [Header("Tutorials")]
    [SerializeField]
    private List<string> passedTutorialIds;

    [Header("Audio")]
    [SerializeField]
    private bool useDemoMusic;

    [NonSerialized]
    private HashSet<string> passedTutorialIdSet;

    public bool UseDemoMusic
    {
        get => useDemoMusic;
        set
        {
            useDemoMusic = value;
            OnDemoMusicUsageChanged?.Invoke();
        }
    }

    public float LightDarkness
    {
        get => lightDarkness;

        set
        {
            lightDarkness = value;

            ///
            OnLightDarknessValueChanged?.Invoke();
        }
    }

    public bool DisableLighting
    {
        get
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return disableLighting_WebGL;
            }
            else
            {
                return disableLighting;
            }
        }

        set
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                disableLighting_WebGL = value;
            }
            else
            {
                disableLighting = value;
            }
        }
    }
    public InputDetectionMode InputDetectionMode
    {
        get => ForceMouseAndKeyboard ? InputDetectionMode.MouseAndKeyBoard : inputDetectionMode;

        set
        {
            ///
            if (inputDetectionMode == value
                || ForceMouseAndKeyboard)
            {
                return;
            }

            ///
            inputDetectionMode = value;

            ///
            Entry.Instance.inputManager.InvokeOnActiveInputDeviceChanged();
        }
    }
    public bool EnabledTerminalOrInEditor => enabledTerminal || Application.isEditor;
    public bool ColorblindMode { get => colorblindMode; set => colorblindMode = value; }
    public int PassedTutorialCount => passedTutorialIds == null ? 0 : passedTutorialIds.Count;

    public int TargetFPS
    {
        get => targetFPS;
        set
        {
            ///
            if (targetFPS == value)
            {
                return;
            }

            ///
            targetFPS = value;

            ///
            OnTargetFPSChanged?.Invoke();
        }
    }

    #region Tutorials
    private void TryInitPassedTutorialIdSet()
    {
        ///
        if (passedTutorialIds == null)
        {
            passedTutorialIds = new List<string>();
        }

        ///
        if (passedTutorialIdSet != null
            && passedTutorialIdSet.Count == passedTutorialIds.Count)
        {
            return;
        }

        ///
        if (passedTutorialIdSet == null)
        {
            passedTutorialIdSet = new HashSet<string>();
        }
        passedTutorialIdSet.Clear();

        ///
        foreach (var item in passedTutorialIds)
        {
            passedTutorialIdSet.Add(item);
        }
    }

    public void ResetTutorial()
    {
        passedTutorialIds?.Clear();
        passedTutorialIdSet?.Clear();
    }

    public bool HasTutorialPassed(string tutorialId)
    {
        TryInitPassedTutorialIdSet();

        ///
        return passedTutorialIdSet.Contains(tutorialId);
    }

    public void SetTutorialAsPassed(string tutorialId)
    {
        TryInitPassedTutorialIdSet();

        ///
        if (passedTutorialIdSet.Add(tutorialId))
        {
            passedTutorialIds.Add(tutorialId);
        }
    }
    #endregion Tutorials
}
