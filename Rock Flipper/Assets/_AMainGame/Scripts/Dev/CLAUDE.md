# Scripts/Dev — developer tooling

38 files. Editor/QA/debug-only, not shipped-gameplay code (much of it is `#if UNITY_EDITOR`-gated or gated behind `GameSetting.enabledTerminal`).

## Subfolders

- **Root** — `DevSetting.IsInternalEnvironment()` (dev-machine detection), `DevEntry` (singleton ScriptableObject asset at `Assets/_AMainGame/Data/Dev/DevEntry.asset`, wires unique-ID manager, demo/full player data objects, game balance, etc. — **editor-only, `Instance` throws in players**), `DevNote` (free-text scene comment), `CommonCheatLib` (`OnCheatSignalEmitted` event fired by the terminal `c` command; screenshot-mode lock via `BalancerWithObjects`), `CameraRenderExporter`, `GDebug.DoIfEnabledCheat/LogIfEnabledCheat` (gate on `enabledTerminal`), `VersionDisplayer` (on-screen branch tag).
- **DevPanel Components/** — `FpsDisplayer` (smoothed FPS counter).
- **Editor/** — misc editor menu commands (particle-light batch-disable, print object names, inspector-list clipboard round-trip, ScriptableObject creation).
- **GameObjectBuildState/** — system for baking a GameObject's canonical active/inactive state before shipping. `GameObjectBuildState : IGameObjectBuildStateSetter` records a default active state + expected name; `SetBuildState()` applies it (fails if renamed). `GameObjectBuildStateChecker` (editor, runtime safety net) errors if live state diverges from recorded default. `GameObjectStateConfiguration` snapshots/applies many `GameObjectBuildState`s at once (named presets, e.g. "screenshot mode"). `IGameObjectBuildStateSetter` is also implemented by `FeatureBranchingObject` (see FeatureBranching doc) — **`FH/Set BuildState for GameObjects` menu command walks every open scene and calls `SetBuildState()` on every implementer of this interface, from both folders.** This is a manual pre-build step, not automated.
- **PlayerDataSnapShot/** — `[ContextMenu] SaveCurrentState()`/`Restore()` for cloning/restoring `PlayerData` (`PlayerDataSnapShot` = full-game data, `PlayerDataSnapShotDemo` = demo data) — quick save-state snapshotting for testing.
- **TellADev/** — `TellADev.That(message)` static facade → `Entry.Instance.tellADevPopup.Show(...)`, an in-game "report a bug/note" popup, also used elsewhere as a runtime "you're doing something wrong" flag (e.g. F2P editor stubs call it if reached outside the editor).
- **Terminal/** — thin wrapper around the third-party **CommandTerminal** package. See below.
- **UniqueIds/** — `UniqueIdValidator` (duplicate-ID detector, used from `OnValidate`-style code project-wide), `UniqueIntManager` (editor-only `GetNextId()`, incrementing from `int.MinValue + 1`).
- **AssetUpdater/** — `IAssetUpdater.Editor_Update()` interface; `AssetUpdaterMenu` (`IPreprocessBuildWithReport` + `FH/Update All Assets` menu) scans all of `Assets/` for implementers and calls `Editor_Update()` on each — the single hook for "regenerate derived/cached data before shipping."

## Terminal — where every cheat/debug command lives

`Terminal/TerminalCommands.cs` — `internal static partial class`, commands registered via `[RegisterCommand]` attributes (auto-discovered by the CommandTerminal package, no manual registration list). Grouped in `#region`s: DevPanel, Screen, Settings, Common (`Cheat`/`c` fires `CommonCheatLib.OnCheatSignalEmitted`, `ScreenshotMode`, `SaveDataNow`, `SlowdownGameplay`, ...), Steam, Run (`CopySlot`, `AddCurrency`, `Spend`), Combat.

**To add a new command**: add `private static void Foo(CommandArg[] args)` decorated `[RegisterCommand(Help = "...")]` (optional `Name = "alias"`) to `TerminalCommands.cs`, in the appropriate `#region`. Args via `args[0].Int/.Bool/.String/.Float/.Double`.

- `TerminalEnabler.EnableTerminal()` — flips `GameSetting.enabledTerminal = true` + saves; the in-game way to unlock the terminal on builds where it starts disabled (e.g. hidden button/cheat code).
- `TerminalLoader` — always enabled in editor; in builds, enabled/disabled per `enabledTerminal`.
- `TerminalBatch` — runs a scripted sequence of terminal command strings from the inspector.

## Conventions / gotchas

- `GameObjectBuildState` requires its recorded `gameObjectName` to exactly match the live name — renaming an object with this component needs `Editor_CorrectName()` (context menu) rerun, or the next bake pass errors.
- "Screenshot mode" is a reference-counted lock (`CommonCheatLib.Add/RemoveScreenshotModeLock`), not a single toggle — multiple systems can request it independently.
- Namespacing is inconsistent (`Agame.Dev` vs. global vs. the unrelated `FMod` namespace on `SaveScreenshotToPC`) — don't read meaning into namespace choice, it's organic/legacy.
- `DevEntry` asset is at a fixed path (`Assets/_AMainGame/Data/Dev/DevEntry.asset`) — porting this folder to a new game means recreating that asset at the same relative path.
- Several files are `#if UNITY_EDITOR`-gated no-ops in player builds — safe to leave referenced from shipping scenes/prefabs.
