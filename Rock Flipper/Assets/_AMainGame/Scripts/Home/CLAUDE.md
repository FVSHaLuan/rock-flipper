# Scripts/Home — front-end / hub scene

The scene the player lands on outside of a run: save-slot picker, DLC button, demo notice, ad trigger. Flat folder, 6 files, no subfolders, namespace `Agame.Home`.

See also: [Scripts/CLAUDE.md](../CLAUDE.md), [Scripts/Common/CLAUDE.md](../Common/CLAUDE.md) (the `Entry`/`ExtendedMonoBehaviour` pattern this folder follows), [Scripts/Run/CLAUDE.md](../Run/CLAUDE.md) (where the player is sent from here).

## Files

- `HomeEntry.cs` — `HomeEntry : CommonEntry`, scene singleton (`HomeEntry.Instance`). Sets `CommonEntry.CommonInstance` and `Entry.ActiveGameScene = GameScene.Home`. Mirrors `RunEntry` in `Scripts/Run`.
- `ExtendedMonoBehaviourHome.cs` — thin base: adds `protected HomeEntry HomeEntry => HomeEntry.Instance`. Everything else here extends it (except `SteamPromotionPanel`, which extends `ExtendedMonoBehaviour` directly).
- `RunDataSlotView.cs` — the save-slot UI widget. Click → `entry.runDataManager.ActiveRunDataIndex = slotIndex` then `entry.visualSceneLoader.Load(GameScene.Run)`. Handles delete-slot and incompatible-save warnings (`entry.compatManager.CurrentCompatVersion`).
- `GetSoundTrackButton.cs` — polls `SteamApps.BIsDlcInstalled` to show/hide the soundtrack DLC button; compiled out via `#if !DISABLESTEAMWORKS`.
- `ShowDemoNoticePopup.cs` — one-time "this is a demo" popup, gated by `VersionBranchInfo.IsDemo || IsPlaytest` + a static `showed` flag.
- `ShowInterAdsInHomeScreen.cs` — skips the interstitial on first Home load (fresh launch), shows `InterAds.Show(null)` on later visits (i.e. after a run).
- `SteamPromotionPanel.cs` — empty stub, no members.

## Known-broken

**`RunDataSlotView.ViewDataCounting(RunData, float)` throws `NotImplementedException` unconditionally** — the real "counting up" stat-reveal logic is commented out below it. Any save slot with data will throw when its view updates. Verify/fix this before relying on save-slot stat display.

## Architecture patterns

- **Per-scene singleton "Entry" pattern**: `HomeEntry`/`RunEntry` both `: CommonEntry`, scene root, static `Instance`, registers as `CommonEntry.CommonInstance`, stamps `Entry.ActiveGameScene`. Two-tier: `Entry` (persistent, cross-scene, `Scripts/Common/Entry.cs`) + `CommonEntry`/`HomeEntry`/`RunEntry` (per-scene, replaced on load).
- **Scene tracking**: `GameScene` enum (`Other, Home, Run, FakeOS`, in `Scripts/Common/GameScene.cs`). Transitions via `entry.visualSceneLoader.Load(GameScene.X)`.
- **Event-driven UI activation**: `UIScreen.OnBecomeActive` (see `Scripts/UI Systems/CLAUDE.md`) instead of polling — used by `ShowDemoNoticePopup` to wait for `HomeEntry.mainUIScreen`.
- **Static "do once per process" gating**: `ShowDemoNoticePopup` (`static bool showed`) and `ShowInterAdsInHomeScreen` (`static int count`) — statics survive scene reload within the same app session even though the components get destroyed/recreated.
- **Feature branching**: `VersionBranchInfo.IsDemo`/`IsPlaytest` gates demo behavior; `#if !DISABLESTEAMWORKS` gates Steamworks-dependent paths. See `Scripts/FeatureBranching/CLAUDE.md`.

## Reusable vs. Rock-Flipper-specific

The *pattern* is reusable, the *content* is interleaved into the same files (not cleanly separated):

- **Reusable shape**: the `XEntry : CommonEntry` singleton pattern, save-slot picker with counting-reveal/delete/version-warning, "ad on return to home"/"DLC button"/"demo notice once" patterns (same F2P/Steam-storefront tricks likely to recur).
- **Game-specific content**: `RunDataSlotView`'s actual stat fields (`playTimeText`, `skillPointsText`, `bossLevelText` — reference `RunData.BossCoreLevel`, `GameBalance.FinalBossLevel`), `GameConst.SoundtrackAppId`, the Home→Run transition semantics tied to this game's save model.

## Conventions / gotchas

- Sibling namespaces to watch for: `Agame.Run`, `Agame` (root, `Common`), `Agame.UI`, `Agame.F2P`, `Agame.FeatureBranching`, `Agame.Balancing`.
- Comment style: bare `///` divider lines above logical blocks (not XML-doc) — match this in `HomeEntry`/`RunDataSlotView`/etc.
- `HomeEntry`'s `OnSetAsInstance` (inherited from `CommonEntry`) has a side effect: setting `CommonInstance` wires `Entry.Instance.GeneralDialog`/`SettingPopup` — non-obvious control flow when tracing dialog wiring.
- Scripts here assume `Entry.Instance` may not exist yet when constructed; `ExtendedMonoBehaviour` falls back to `Entry.OnHadInstance` (see `Scripts/Common/CLAUDE.md`) — safe to reference in scenes loaded before `Entry` exists, functional only once that event fires.
