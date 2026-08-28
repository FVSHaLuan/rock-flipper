# Scripts/FeatureBranching — platform/version/store branching + build pipeline

17 files. Explicitly reuse-oriented, but branded constants (`BSB_*` = this game's old codename prefix) and hardcoded build paths need renaming/reconfiguring per project.

## The three axes, precisely

- **Platform** (`PlatformBranch.cs`: `None, All, PC, Mac, Linux, Web, XBox, MicrosoftStore, Mobile`) — **compile-time only**, derived from Unity's `UNITY_STANDALONE_WIN/_OSX/_LINUX`, `UNITY_WEBGL`, `UNITY_WSA`, `UNITY_XBOXONE`, `UNITY_IOS||UNITY_ANDROID` build-target defines. Not independently toggleable — it just reflects the active build target.
- **Version** (`VersionBranch.cs`: `All, Full, Demo, Playtest`) — compile-time, driven by custom scripting-define symbols: `BSB_VER_DEMO`→Demo, `BSB_VER_PLAYTEST`→Playtest, else Full. `VersionBranchInfo` also exposes `IsBetaVersion` (a manual in-source `// #define BETA_VERSION`, **not** a menu-toggleable symbol like the others — easy to miss when cutting a beta build), `IsF2P` (`BSB_F2P`), `IsPrologue` (`BSB_VER_PROLOGUE && BSB_VER_DEMO` — Prologue is a demo sub-flag, not its own enum value; setting `BSB_VER_PROLOGUE` alone does nothing), `IsTargetedOrOnMobile` (compile-true on iOS/Android, else falls back to `Application.isMobilePlatform` at runtime).
- **Store** (`StoreInfo.cs`: `Unknown, Steam, GOG, MicrosoftStore, AppleAppStore, GooglePlay`) — **runtime-only**, detected via `SteamManager.Initialized` (GOG is scaffolded behind a disabled local `#define ENABLE_GOG`). The odd one out — a PC build could in principle be either storefront, so this can't be a compile-time branch.

## Declaring a new feature branch

1. **Single version+platform condition**: use a `FeatureBrancher`-derived component — `FeatureBranchingEvent` (fires `onActive`/`onInactive` UnityEvents) or `FeatureBranchingObject` (activates/deactivates a target GameObject, **and** implements `IGameObjectBuildStateSetter` — see `Scripts/Dev/CLAUDE.md`'s GameObjectBuildState section, since `FH/Set BuildState for GameObjects` bakes these too). Set `versionBranch`/`platformBranch` in the inspector; `.All` = wildcard.
2. **Multiple values per axis** (e.g. "PC or Mac", "Demo or Playtest"): use `FeatureBrancherMultiple` (`List<VersionBranch>`/`List<PlatformBranch>`, empty or containing `.All` = wildcard) — `FeatureBrancher` only supports one value per axis.
3. **Single hardcoded axis, no inspector config needed**: `MobileCheck`, `F2PCheck`, `BetaBranchDisplayer`, `DemoPrologueBrancher` — small purpose-built components.
4. **From code**: query `VersionBranchInfo.Current`/`.IsF2P`/`.IsDemo`/etc. or `PlatformBranchInfo.Current` directly (as `InterAds`, `VersionDisplayer`, `SetReporterState` do).
5. **New platform/version/store value**: add to the relevant enum; for Version, add a scripting-define symbol, wire it into `VersionBranchInfo.Current`'s `#if` chain, and add a toggle menu command in `VersionBranchUtilities`. Platform values come from Unity's own build-target defines, not independently toggleable.

## Build pipeline (`Editor/`)

- `BranchedBuildConfig.cs` — singleton ScriptableObject asset (`Assets/_AMainGame/Data/BranchedBuildConfig.asset`), the build entry point: `version`, per-OS output folders, `buildExecutableName`. `[ContextMenu("Build Current")] BuildCurrent()` builds via the active Unity Build Profile (legacy `BuildPlayerOptions` path is dead code, always throws), computes output path branched by `VersionBranchInfo.Current` × `PlatformBranchInfo.Current` (only PC/Mac/Linux/Web supported — Mobile/Xbox/MicrosoftStore throw `NotSupportedException`), sets `initiatedBuild = true`.
- `BranchedBuilder.cs` — `IPreprocessBuildWithReport`/`IPostprocessBuildWithReport` (`callbackOrder = int.MinValue + 1`). Preprocess: requires `BranchedBuildConfig` to exist, soft-checks the build was initiated through it (**currently `allowed = true` hardcoded — just warns, doesn't block**, so building via the normal Build Settings window still works, just without the safety rails), auto-corrects `PlayerSettings.bundleVersion`. Postprocess: writes `buildCfg.ini` (version/branch) into the output folder.
- `VersionBranchUtilities.cs` — `IPreprocessBuildWithReport` (`callbackOrder = int.MinValue`, runs before `BranchedBuilder`). **Also where the branch-switch menu commands live**: `FH/Version Branching/Switch to: FULL|PLAYTEST|DEMO|PROLOGUE` (toggles `BSB_VER_*` defines for the active build target group) and `Enable/Disable F2P` (toggles `BSB_F2P`). Its actual preprocess validation logic (output-path-matches-branch check + confirmation dialog) is **currently disabled** (`skipCheck = true` hardcoded, shows only a warning).

Practical flow: configure `BranchedBuildConfig` once → switch Version/F2P via the `FH/Version Branching/...` menus → trigger `BranchedBuildConfig.BuildCurrent()` (not the normal Build button) → `VersionBranchUtilities` then `BranchedBuilder` run as preprocessors → `buildCfg.ini` written after. **`FH/Set BuildState for GameObjects` (Dev folder) is a separate manual step, not auto-invoked by this pipeline** — run it before a real build if any `FeatureBranchingObject`/`GameObjectBuildState` scene state needs baking.

## Conventions / gotchas (this folder is meant to be copied into new projects)

- What needs updating per game: (a) `BSB_*` define names are branded with this game's old codename ("BSB") — rename for a new project; (b) `BranchedBuildConfig`'s default folder paths and `buildExecutableName` are this-project-specific; (c) `GetBuildPath()`'s platform switch needs extending for Mobile/Xbox/MicrosoftStore if a future game targets them.
- Store detection is deliberately runtime, not compile-time — don't try to make it a build define.
- `FeatureBranchingObject` doubles as a `GameObjectBuildState` bake target — reusing FeatureBranching in a new project means bringing the Dev folder's build-state baking menu along too, or its build-time behavior silently won't get baked.
- Both `VersionBranchUtilities.skipCheck` and `BranchedBuilder`'s `allowed` hardcode read as "temporarily relaxed for this project's release cadence" — worth re-enabling if release safety rails matter for a new project.
