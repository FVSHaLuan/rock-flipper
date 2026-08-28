# Scripts/Common — app-wide composition root

27 files. Foundational: this is where `Entry` (the app-wide singleton nearly everything reaches through) and `ExtendedMonoBehaviour` (the base class nearly everything derives from) live. Read this before wiring a new manager into the game.

See also: [Scripts/CLAUDE.md](../CLAUDE.md), `Assets/FHC` (the framework `MonoBehaviourWithInit` comes from).

## `Entry` (`Entry.cs`, `Entry_Utilities.cs`) — the composition root

A `partial class Entry : MonoBehaviour`, `DontDestroyOnLoad`, one instance for the whole app lifetime.

- Statics: `Entry.Instance` (has an editor-only `Resources.Load` fallback for `[ExecuteInEditMode]` tooling), `Entry.BareInstance` (raw, no fallback — **this is what `ExtendedMonoBehaviour` polls**), `Entry.EditorInstance` (editor-only). Static event `Entry.OnHadInstance`, fired once at the end of `Awake()`.
- `Awake()`: sets `Instance`, loads `PlayerData`/`GameSetting` from the serialized data objects, builds `GeneralPool`, fires `OnHadInstance`, then (play mode only) `TrySaveInstallTime()` + `DoMandatoryInits()`.
- `Update()`: ticks any `Entry.InactiveUpdatable` in `updatableObjects` that's currently inactive — a way to keep logic running on disabled objects.
- Aggregates ~40 manager references as public fields, populated via Inspector on the `Entry` prefab (`Resources/OV Entry`, path in `GameConst.EntryResourcePath`). Grouped by `[Header]`: Core (`conversionCamera`, `executionHelper`, `concurrentActivationManager`, `gameBalance`, `cashTiers`), 0. Common Data (`playerDataObject`/`playerDataObjectDemo`, `playerDataSaver`, `gameSettingObject`, `compatManager`), 1. Time (`timeScaleManager`), 2. Sound (`audioManager`, `backgroundMusicManager`, `uiSoundManager`, `pooledAudioManager`), 3. UI Systems (`uiScreenManager`, `uiSelectedEventManager`, `buttonPromptManager`, `mouseCursorVisibilityManager`, `visualSceneLoader`, `loadingScreenAnimator`, `clickParticleManager`, ...), 4. Input (`inputManager`, `anyKeyDetector`, `completeInputBlocker`), 5. Dev (`tellADevPopup`), 6. Incremental (`runDataManager`, `currencyConfigManager`), 7. Game platform (`handleSteamOverlay`, `achievementReporter`, `steamStoreStateDetector`).
- `Entry.PlayerDataObject` (property) silently switches between `playerDataObject`/`playerDataObjectDemo` based on `VersionBranchInfo.Current == Demo` — **always read through this property, never the raw serialized fields.**
- `EntryLoader.cs` — bootstrap: if `Entry.Instance == null`, instantiates the `Entry` prefab from Resources. Put one in any scene you might play standalone (bypassing a proper bootstrap flow).

## `CommonEntry` — second-tier per-scene singleton

`Agame.CommonEntry : MonoBehaviourWithInit`, `CommonEntry.CommonInstance`. Holds cross-scene "common UI" refs (`generalDialog`, `settingPopup`, `mainUIScreen`, `toolTipManager`, etc.) and wires them into `Entry.Instance.GeneralDialog`/`SettingPopup` as a side effect of being set. `HomeEntry`/`RunEntry` (in `Scripts/Home`, `Scripts/Run`) both extend this. **Note**: some managers (e.g. `toolTipManager`) live on `CommonEntry`, not `Entry` — check which before wiring a new dependency.

## `ExtendedMonoBehaviour` — the lazy-init pattern (exact mechanics)

Base: `MonoBehaviourWithInit` (`Assets/FHC/Core/ExtendedMonoBehaviour/MonoBehaviourWithInit.cs`).

1. In the **C# constructor** (runs even outside Play mode), calls `TryGetReferences()` once.
2. `TryGetReferences()` sets `entry = Entry.BareInstance` (not `Entry.Instance` — no editor fallback here). If non-null: caches `playerData`, `generalPool`, `gameSetting`, sets `foundReferences = true`.
3. If not found (very common — instantiation order isn't guaranteed relative to `Entry.Awake()`), subscribes `Entry.OnHadInstance += Entry_OnHadInstance`.
4. When `Entry.Awake()` fires `OnHadInstance`, every waiting instance re-runs `TryGetReferences()` and finally caches its refs. Finalizer unsubscribes to avoid leaks.
5. Separately, `MonoBehaviourWithInit` provides `Init()`/`TryInit()`/`Inited`: override `protected virtual bool Init()` for one-time setup (return `true` on success) and/or `ExtendedAwake()` for logic that runs every Awake regardless. `ExtendedMonoBehaviour.TryInit()` is `sealed override` — it first requires `TryGetReferences()` to have succeeded (returns `false` immediately otherwise), then defers to base.

**Practical rule**: derive gameplay/UI components from `ExtendedMonoBehaviour` (or `ExtendedMonoBehaviourWithTime`/`ExtendedMonoBehaviourWithUniqueId`), read `entry`/`playerData`/`generalPool`/`gameSetting` via the protected properties instead of `Entry.Instance` directly, and put init logic in `override protected bool Init()` rather than raw `Awake()`.

## Other notable files

- `ExtendedMonoBehaviourWithTime.cs` — adds per-component `TimeScaleMode` + `GameplayDeltaTime`/`GameplayTime` shortcuts (see `Scripts/TimeScaleManager/CLAUDE.md`); UI (`RectTransform`) components default to `GameplayUnscaledTime` in `Reset()`.
- `ExtendedMonoBehaviourWithUniqueId.cs` / `ScriptableObjectWithUniqueId.cs` — editor-assigned unique IDs via `DevEntry.Instance.uniqueIntManager`, duplicate detection in `OnValidate`.
- `GameScene.cs` — `enum { Other, Home, Run, FakeOS }`.
- `GameConst.cs` — static const bag: resource paths, scene names, tags/layers, per-branch Steam app IDs, IAP id, URLs.
- `MiniStorage.cs` — `[Serializable]` generic per-key-type dictionary bag, used inside `PlayerData.GeneralStorage` for ad-hoc persisted values.
- `VisualSceneLoader/VisualSceneLoader.cs` — fade-out/`SceneManager.LoadScene`/fade-in; `Load(GameScene, customText=null)`; blocks input via `completeInputBlocker` for the duration.
- `LoadingScreenAnimator/LoadingScreenAnimator.cs` — drives the loading-screen Animator; adds/removes an input-block lock while active.

## Conventions / gotchas

- Prefer `Entry.BareInstance` over `Entry.Instance` in runtime code to avoid accidentally instantiating things in edit mode.
- `EntryLoader` is the idiomatic way to guarantee `Entry` exists when opening a scene directly.
- `LimitWidthOnWideScreen.cs` is `[Obsolete]` — don't build on it.
- `MiniStorage.SetFloat(string, string)`/`GetFloat(string, string)` are misleadingly-named — they actually operate on `stringDictionary` (should read `SetString`/`GetString`). The compiler resolves the right overload by signature, but don't go looking for a `SetString` method that doesn't exist by that name.
