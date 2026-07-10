using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using BT.UI.ButtonPrompts;
using BT.FeatureBranching;
using BT.UI;
using BT;
using BT.Dev;
using BT.Balancing;
using BT.Demo;
using BT.Steamworks;
using BT.GamePlatform;
using BT.Marketing;
using BT.Localization;
using BT.Run;

public partial class Entry : MonoBehaviour
{
    public static event Action OnHadInstance;

    private static Entry instance;

    public static Entry BareInstance => instance;

    public static Entry Instance
    {
        get
        {
#if UNITY_EDITOR
            if (instance == null && !Application.isPlaying)
            {
                instance = Resources.Load<Entry>(GameConst.EntryResourcePath);
            }
#endif

            ///
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    public static Entry EditorInstance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                instance = FindFirstObjectByType<Entry>();
            }
#endif
            ///
            return instance;
        }
    }

    [Space]
    public Camera conversionCamera;
    public ExecutionHelper executionHelper;
    public ConcurrentActivationManager concurrentActivationManager;
    public Transform pooledObjectsRoot;
    public VisualDefinitions visualDefinitions;
    public GameBalance gameBalance;
    public CashTiers cashTiers;
    public DemoHub demoHub;
    
    [Header("0. Common Data")]
    [SerializeField]
    private PlayerDataObject playerDataObject;
    [SerializeField]
    private PlayerDataObject playerDataObjectDemo;
    public PlayerDataSaver playerDataSaver;
    public GameSettingObject gameSettingObject;
    public LocalizedStrings localizedStrings;
    public CompatManager compatManager;

    [Header("1. Time")]
    public TimeScaleManager timeScaleManager;

    [Header("2. Sound")]
    public AudioManager audioManager;
    public BackgroundMusicManager backgroundMusicManager;
    public UISoundManager uiSoundManager;
    public PooledAudioManager pooledAudioManager;

    [Header("3. UI Systems")]
    public ActiveContextButtonPromptManager activeContextButtonPromptManager;
    public UIScreenManager uiScreenManager;
    public UISelectedEventManager uiSelectedEventManager;
    public ButtonPromptManager buttonPromptManager;
    public MouseCursorVisibilityManager mouseCursorVisibilityManager;
    public UIBackgroundManager uiBackgroundManager;
    /// <summary>
    /// Nothing can be on top of this background
    /// </summary>
    public UIBackgroundManager absoluteUIBackgroundManager;
    public VisualSceneLoader visualSceneLoader;
    public LoadingScreenAnimator loadingScreenAnimator;
    public ClickParticleManager clickParticleManager;
    public GameObject quittingPrompt;

    [Header("4. Input")]
    public InputManager inputManager;
    public AnyKeyDetector anyKeyDetector;
    public CompleteInputBlocker completeInputBlocker;
    public InputSystemUIInputModule inputSystemUIInputModule;

    [Header("5. Dev")]
    public TellADevPopup tellADevPopup;

    [Header("6. Incoremental")]
    public RunDataManager runDataManager;
    public CurrencyConfigManager currencyConfigManager;

    [Header("7. Game platform")]
    public HandleSteamOverlay handleSteamOverlay;
    public AchievementReporter achievementReporter;
    public SteamStoreStateDetector steamStoreStateDetector;
    public StatReporter statReporter;
    public GamePlatformStatStorer gamePlatformStatStorer;

    [Space]
    public List<InactiveUpdatable> updatableObjects = new List<InactiveUpdatable>();

    public static GameScene ActiveGameScene { get; set; } = GameScene.Other;

    public GeneralDialog GeneralDialog { get; set; }
    public UIScreen SettingPopup { get; set; }

    ///
    public EntryGeneralPool GeneralPool { get; private set; }

    public TimeSpan LastOfflineTimeSpan { get; private set; } = TimeSpan.MinValue;

    public bool IsFirstLaunch { get; private set; }

    // Cached Objects
    public PlayerData PlayerData { get; private set; }
    public GameSetting GameSetting { get; private set; }

    public PlayerDataObject PlayerDataObject
    {
#if UNITY_EDITOR
        get => VersionBranchInfo.Current == VersionBranch.Demo ? playerDataObjectDemo : playerDataObject;
#elif BSB_VER_DEMO
        get => playerDataObjectDemo;
#else
        get => playerDataObject;
#endif
    }

    public abstract class InactiveUpdatable : MonoBehaviour
    {
        public abstract void InactiveUpdate();
    }

    public void Awake()
    {
        ///
        DontDestroyOnLoad(gameObject);

        ///
        Instance = this;

        ///
        PlayerData = PlayerDataObject.Data;
        GameSetting = gameSettingObject.Data;

        ///
        InitReferences();

        ///
        GeneralPool = new EntryGeneralPool(pooledObjectsRoot);

        ///
        OnHadInstance?.Invoke();

        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Debug.Log("Entry - Editor causes this Awake");
            return;
        }
#endif

        ///
        TrySaveInstallTime();

        ///
        enabled = true;

        ///
        DoMandatoryInits();
    }

    public void Update()
    {
        foreach (var item in updatableObjects)
        {
            if (!item.isActiveAndEnabled)
            {
                item.InactiveUpdate();
            }
        }
    }

    private void TrySaveInstallTime()
    {
        if (PlayerDataObject.Data.TrySetNowAsInstallTime())
        {
            ///
            IsFirstLaunch = true;

            ///
            PlayerDataObject.SaveData();
        }
        else
        {
            IsFirstLaunch = false;
        }
    }

    private void InitReferences()
    {

    }

    private void DoMandatoryInits()
    {

    }
}

