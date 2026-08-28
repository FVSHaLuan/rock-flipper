# Scripts/ — folder map

~34 subfolders. Two are game-specific; almost everything else is a reusable system meant to carry over to the owner's future games (per project convention). Folders with real architectural weight get their own `CLAUDE.md` (linked below); everything else is a short entry here.

## Game-specific (not reusable as-is)

| Folder | Doc | What |
|---|---|---|
| **Run** | [Run/CLAUDE.md](Run/CLAUDE.md) | All gameplay logic — rocks, tiers, flipper bots, shop, skill tree, run state/prestige. Fully game-specific. Start here for gameplay features. |
| **Home** | [Home/CLAUDE.md](Home/CLAUDE.md) | Front-end/hub scene (save slots, DLC button, demo notices). The *pattern* (scene-entry singleton, save-slot picker) is reusable; the *content* (stats shown, product IDs) is specific to this game. |

## Have their own CLAUDE.md (real complexity / non-obvious conventions)

| Folder | Doc | Why it earned one |
|---|---|---|
| **Common** | [Common/CLAUDE.md](Common/CLAUDE.md) | Houses `Entry` (the app-wide composition-root singleton) and `ExtendedMonoBehaviour` (base class nearly everything derives from). Foundational — read this if anything about app-wide wiring is confusing. |
| **Dev** | [Dev/CLAUDE.md](Dev/CLAUDE.md) | Terminal/cheat commands, dev panel, build-state baking, unique-ID system. Editor/QA tooling, not shipped gameplay. |
| **F2P** | [F2P/CLAUDE.md](F2P/CLAUDE.md) | Ads (interstitial/rewarded) + IAP abstraction. Explicitly reuse-oriented: interface + editor-stub + platform impl split. |
| **FeatureBranching** | [FeatureBranching/CLAUDE.md](FeatureBranching/CLAUDE.md) | Declarative platform/version/store branching + the build pipeline (`BranchedBuilder`). Explicitly reuse-oriented, though branded constants (`BSB_*`) need renaming per-project. |
| **Helpers** | [Helpers/CLAUDE.md](Helpers/CLAUDE.md) | **242 files** — the biggest reusable folder by far (pure-C# math/formatting utilities + Unity-specific extensions/components). Check here before writing a new utility. |
| **PlayerData** | [PlayerData/CLAUDE.md](PlayerData/CLAUDE.md) | Cross-run save-data model. Small file count but the partial-class save/versioning/correction convention is safety-critical — read before adding a persisted field. |
| **TimeScaleManager** | [TimeScaleManager/CLAUDE.md](TimeScaleManager/CLAUDE.md) | Pause vs. gameplay-slowdown vs. unscaled-time system. Non-obvious semantics, easy to get wrong. |
| **UI Systems** | [UI Systems/CLAUDE.md](UI%20Systems/CLAUDE.md) | Screen-stack navigation, gamepad/KBM button-prompt glyph system, tooltips, menus. Read before building any new screen or menu. |

## Everything else (self-explanatory or thin/vendored — no dedicated file)

| Folder | One-liner |
|---|---|
| Audio | `AudioManager`/`AudioChannel` + pooled SFX player + music manager. Original, built on `AudioSource` directly. |
| BoundExtension | Tiny UI-layout padding struct + combine helpers (own asmdef `BSB.BoundExtension`). |
| Common Visual | Single `VisualDefinitions` shared-reference container. |
| Community | Contributor/credits list data + view. |
| ConcurrentActivation | Rate-limits how often a pooled object "activates" within a time window (throttles VFX/SFX bursts); 4 key-strategy variants. |
| Conditions | Abstract `Condition : ScriptableObjectWithInit` — activates/deactivates on first/last subscriber. |
| Demo | Single `DemoHub` class — demo-build behavior switch. |
| ExecutionHelper | Coroutine/callback dispatch + main-thread marshaling (`UnityThreadHelper`). |
| ExternalCredits | Third-party asset credit data for the credits screen. |
| Game Helpers | Big grab-bag (53 files) of small independent MonoBehaviours — rotators, movers, blinkers, spawners, time-triggers. One component per file, names are self-descriptive. |
| Game Platform | Achievements/DLC/stat-reporting abstraction with a Steam-specific concrete layer (`SteamDLCValidator`, wraps the vendored `com.rlabrecque.steamworks.net`). Generic-vs-Steam split isn't obvious from filenames alone. |
| Game Setting | `GameSetting` data model + ~15 thin per-setting UI binding components (volume, FPS, resolution, ...). |
| Game Time | Two `WaitFor...` custom coroutine yield instructions (gameplay-unscaled / real time). |
| Game logic controller | Pause-on-focus-lost + save-on-system-menu, two standalone behaviours. |
| Input | Wraps the new Input System: active-device detection (mouse/kb vs gamepad type), device-switch events, flicker-debounce (`InputDeviceFlickeringResolver`). Feeds `UI Systems/ButtonPrompt`'s glyph swap. |
| Marketing | Steam store-page discount/release-state detection + countdown box. |
| Meta | URL launcher + Steam wishlist call-to-action box. |
| Mouse Cursor Visibility | Shows/hides the OS cursor based on game state/input device. |
| ObjectsLayout | Abstract `ObjectsLayout` (layout-position strategy) + `RingObjectsLayout` implementation (own asmdef `BSB.ObjectsLayout`). |
| Pooling | `GeneralPool`/`EntryGeneralPool` — thin customization over the real pooling engine, which lives in `Assets/FHC/Core/Architecture/Pool` (`MultiPrototypesPool<T>`, `GeneralPoolMemberSimplified`). |
| Runtime Initialization | Single `[RuntimeInitializeOnLoadMethod]` script (hides cursor on load). |
| ScreenReader | Screen/resolution reading + coordinate transform (`TransformationLib`). |
| SerializableDictionary | **Vendored** — the well-known community Unity serializable-dictionary implementation (PDF manual included). Standard usage, not custom. |
| Steamworks.NET (wrapper) | `SteamManager.cs` is stock Steamworks.NET boilerplate; a few thin game-specific wrappers (overlay, leaderboard, SteamId display) on top. The actual SDK lives at `Assets/com.rlabrecque.steamworks.net`. |
| Tutorials | `Tutorial`/`TutorialUnit` step definitions + popup display. |
| UI | Broad widget library (39 files: sliders, dialog/popup system, HUD camera, loading screen, `ButtonScroller/EnhancedScroller` wrapper). Mostly self-descriptive; the `General Dialog` launcher/result pattern is the one thing worth knowing (ask if unclear — not separately documented). |
| Visual Effects | Camera shake, camera stabilizer, 3D card-inspection effect. |
| VisualAttachment | "Attach a visual to a pooled object on a named slot" host/guest pattern (`IAttachmentHost`/`AttachmentGuest`/`AttachmentSlot`). |

Vendored/third-party asset folders live outside `_AMainGame` entirely (see root `CLAUDE.md`) — don't expect docs for those.
