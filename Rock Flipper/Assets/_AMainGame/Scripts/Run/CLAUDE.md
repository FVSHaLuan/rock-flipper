# Scripts/Run — gameplay logic

The single most important folder for gameplay feature work. Everything here executes inside the "Run" scene. Fully game-specific (not reused across future games).

See also: [Scripts/CLAUDE.md](../CLAUDE.md), [Scripts/Common/CLAUDE.md](../Common/CLAUDE.md) (base classes), [Scripts/TimeScaleManager/CLAUDE.md](../TimeScaleManager/CLAUDE.md), [Scripts/PlayerData/CLAUDE.md](../PlayerData/CLAUDE.md) (cross-run save, vs. `RunData/` here which is per-run).

## Entry point & base class

- `RunEntry.cs` — scene singleton (`RunEntry.Instance`), owns serialized refs to every gameplay subsystem, rebuilds `BuildStatsObject` from Shop + Skill Tree + Items (`ApplyAllToBuildStats`).
- `ExtendedMonoBehaviourRun.cs` — base class most gameplay `MonoBehaviour`s derive from; exposes `RunEntry`, `RunData`, `BuildStats`, `Playfield`, `StateManager`, `IsTutorial`, and `TryInitThisCombat()` ("run once per combat" helper).
- Run-level state machine: `State Manager/RunStateManager.cs` / `RunState.cs` — states `Init, Combat, BeforePrestige, OnPrestige, BeforeCombatFromPrestige`; events `OnStateChanged`, `OnCombatStarted`, `OnBeforePrestige`, `OnPrestige`, `OnBeforeCombatFromPrestige`.

## Folder map

| Path | Responsibility |
|---|---|
| `Achievements/` | `GameAchievementsReporter.cs` — empty stub. |
| `Balancing/` | `GameBalance.cs` (empty placeholder ScriptableObject), `CashTiers.cs` (cost-tier presets + UI color), `StageConfig.cs` (looks like unused/legacy HP-scaling formula). |
| `Combat/` | The gameplay scene root — see subsystems below. |
| `Combat/Background/`, `Boundary/`, `Explosion/`, `Sfx/` | Background selection, playfield walls, pooled explosion VFX, sound. Background ids run from 0 to `GameConst.BackgroundCount - 1`, whether or not a `CombatBackground` exists in the scene for each id. Only code that touches the background itself (e.g. reads its name) checks `CombatBackgroundManager.HasBackground`; unlocking, selecting and counting backgrounds go by `GameConst.BackgroundCount` alone. One exception: in demo and playtest builds, `UnlockBackgroundButton` never picks a background whose `CombatBackground.isNaNInDemo` is ticked (`CombatBackgroundManager.IsBackgroundAvailableInCurrentBuild`), and shows "MORE IN FULL GAME" once only those are left. |
| `Combat/Flippable/` | `Flippable.cs` — the generic flip-animation state machine used by rocks (`TryFlipping`/`ForceFlipping`, arc motion via `Mathf.Sin`, fires `OnStartedFlipping`/`OnFinishedFlipping`/`OnUpdatedFlipping`). Every flip is tagged with a `FlipSource` (mouse, Flipper Bot, bounce, self-flip, new rock, shockwave), readable as `LastFlipSource` until the next flip; abilities use it to react only to some flips. Plus rotator/shadow/sorting cosmetic add-ons. |
| `Combat/Flipper Bot/` | `FlipperBot.cs`, `FlipperBotFlipper.cs` (circle-cast + flip), `FlipperBotMovement.cs` (wanders via `Vector2.MoveTowards`). |
| `Combat/Player Cursor/` | `PlayerCursor.cs` + `FlippableByPlayerCursor.cs` — click/hover-to-flip, circle/point cast each `LateUpdate`, per-object landing cooldown. |
| `Combat/Monoliths/` | Early stub: `Monolith.cs` (`ExtendedMonoBehaviourRun` holding a serialized `MonolithType`; activates/deactivates its own GameObject from `GetMonolithBuildStats(monolithType).unlocked` on `Start` and whenever the skill tree screen closes, so per-monolith scripts never check `unlocked` themselves) + `MonolithType.cs` enum (`LII, LIV, LVI, LVIII, LX, LXII`). Per-monolith scripts go in `Specifics/<Name>/` (so far only `Specifics/Prism/Prism.cs`, Prism = L VIII: every `prismShootingInterval` it beams a random airborne rock (found via `RockInstanceManager.ActiveRocks`); the hit effect `ZapRock` is still empty, pending the GDD). Used by `Prefabs/Run/Combat/Monolith/Monolith Base.prefab`. |
| `Combat/Playfield/` | `Playfield.cs` (partial class — bounds math, clamping, random points, edges). |
| `Combat/Rocks/` | The Rock system core — see below. |
| `Combat/Tutorials/` | Sequential onboarding task system (`TutorialTaskManager`/`Controller`). |
| `Currencies/` | Currency enum + per-currency config/state — see gotcha below. |
| `RunData/` | Persistent per-run save data — see below. |
| `Shop/` | Buy-count/upgrade UI logic — see below. |
| `Skill tree/` | XNode-graph-driven skill tree — see below. |
| `Stats/` | The `BuildAgent` stat-application framework shared by Skill Tree, Shop and Items. |
| `Dev/`, `Demo/`, `Compat/` | QA tools (incl. `AutoMouse`, see Conventions & gotchas), Steam-demo-build UI, save-compat version holder. |
| `ShortHands/` | `ShortHandManager.cs` — `{token}` text-replacement dictionary, currently empty/unused. |

## Rocks / Tiers / Pure Rocks

- `Combat/Rocks/Rock.cs` — per-rock MonoBehaviour: HP, tier, `IsPure`, cash-on-land/break, re-roll (`BreakCurrentRockAndSpawnNewOne`).
- `Combat/Rocks/RockTier.cs` — enum **`{ P0, P1, P2, P3 }`** — 4 tiers implemented. **Map "Tier I"→`P0`, "Tier II"→`P1`, etc.** when tiers are referred to by Roman numerals.
- `Combat/Rocks/PrototypeManager.cs` — ScriptableObject: rock prefabs per (tier, isPure); `GetRockPrototype(tier, isPure)`; validated via `PrototypeManager.Validate()` in editor (rejects duplicate/null/"Rock"-named prototypes).
- `Combat/Rocks/RockInstanceManager.cs` — spawn API: `SpawnAsOldRock` (combat start), `SpawnAsReplacement` (HP-hit-0 re-roll), `SpawnAsNewRock` (shop purchase); `Despawn` (return to pool). Tracks the rocks on the playfield as `ActiveRocks` — always spawn/despawn rocks through it so that list stays correct.
- `Combat/Rocks/RockPoolHandler.cs` — pooling via `FH.Core.Architecture.Pool.GeneralPoolMemberSimplified`.
- `Combat/Rocks/RocksSpawner.cs` — spawns initial rock counts per tier at combat start.
- `Stats/RockTierBuildStats.cs` — per-tier tunables: `count`, `maxCount`, `purity`, `landingCash`, `breakingCash`, `purityCashMultiplier`, `landingCooldown`.
- `Stats/MonolithBuildStats.cs` — per-monolith tunables shared by every monolith (currently just `unlocked`), one instance per `MonolithType` via `BuildStatsObject.GetMonolithBuildStats`. Monolith-specific stats (e.g. `prismShootingInterval`) stay as plain `BuildStatsObject` fields.
- Per-tier unique abilities live in `Combat/Rocks/Abilities/` (see Maturity below).
- **Pure Rocks**: purity is a per-tier chance (`RockTierBuildStats.purity`), rolled on spawn (`RocksSpawner`, `RockCountStatBuilderButton`) and on re-roll (`Rock.BreakCurrentRockAndSpawnNewOne` via `RockTier.GetPurityChance()`). It's a `bool isPure` flag + separate prototype prefab per tier — no separate "PureRock" class. Cash multiplied via `purityCashMultiplier`.

**To add a new Rock Tier**: extend `RockTier` enum → add a case to `BuildStatsObject.GetRockTierBuildStats` → add prototype prefabs (regular + pure) registered in `PrototypeManager` (run `Validate()`) → add rock-count/upgrade shop buttons + skill nodes wired to a `RockBuildAgent` subclass (see `Stats/Agents/Implementations/Rocks/IncreaseRockCount.cs` as the template).

## Flipping (player + bots)

- Flipper Bot upgradable stats today: only `flipperBotMovementSpeed` / `flipperBotFlippingInterval`.

## Currencies

- `Currencies/Currency.cs` — enum: `CASH, BLANK_BALL, RAW_PRESTIGE, PRESTIGE, BUCKET, FREE, AURA, BOSS, P7`. **Gotcha**: the game is Cash-only; treat everything besides `CASH` as legacy/reserved (likely vestigial from a shared template) unless you find real usage.
- `Currencies/CurrencyConfigManager.cs`/`CurrencyConfigAsset.cs` — ScriptableObject-per-currency config, `Editor_Sync` context menu auto-generates missing assets under `Assets/_AMainGame/Data/Currency Configs`.
- Project convention: `XxxDictionary` classes wrap Unity-serializable dictionaries (`CurrencyValueDictionary`, `CurrencyStateDictionary`).

## RunData (per-run save)

- `RunData/RunData.cs` — `[Serializable] partial class`: currency values, tutorial/prestige flags, skill node states, builder-button levels, background unlocks, play time.
- **Convention: split by partial file** — `RunData_Serialization.cs` (play-time bookkeeping), `RunData_FrameUpdate.cs` (batches `OnCurrencyValueModifiedThisFrame`, and `OnCurrencyAddedThisFrame` with the summed positive `AddCurrency` amounts), `RunData_Correction.cs` (save-migration hook, currently a no-op `CorrectData`). **Extend `RunData` via a new `RunData_<Aspect>.cs` partial rather than growing the main file.**
- **Play time** (seconds, real time, per save slot). `RunData.PlayTime` is the serialized value as of the last save. `RunData.PlayTimeNow` is the live value: `PlayTime` plus the real time since `LastSaveTime`, plus `PlayTimeAdjustment`. `RunData_Serialization.UpdatePlayTime` commits the elapsed time into `playTime` on every save (`RunAutoSaver`: every 60 s, 10 s in the editor).
  - **Gameplay code reads `PlayTimeNow`**, never `PlayTime`, which can lag by a full auto-save interval. For a timer measured in play time that must survive quitting, store a `PlayTimeNow` stamp in `RunData` and compare it to a later `PlayTimeNow` (`preferredBackgroundSetPlayTime` / `PreferredBackgroundTimeElapsed`, used by `PreferredBackgroundUpdater`).
  - **`PlayTimeNow` throws outside the Run scene** (`RunEntry.Instance == null`). The same goes for anything that calls it, such as the `PreferredBackgroundId` setter and `UnlockBackground`. Outside the Run scene (Home's `RunDataSlotView`), read `PlayTime`.
  - **It's real time, not gameplay time.** In builds it keeps counting while paused and ignores the time scale. Use the TimeScaleManager's gameplay time for in-combat timers and cooldowns. In the editor only, `PlaytimeAdjustmentModifier` (subtracts paused time) and `GameplayTimeScaleSlider` (adds the sped-up time) shift it through `PlayTimeAdjustment`, so the dev time-scale slider also fast-forwards play-time timers.
  - Never write `playTime`/`lastSaveTime` directly. Dev tools change play time through `PlayTimeAdjustment`. `PrestigePlayTimeNow` follows the same rules; since Prestige is still scaffolded, nothing resets it yet.
- `RunData/RunDataObject.cs` — `WritableScriptableObject<RunData>` (the actual save-slot asset). `RunDataManager.cs` — 4 fixed save slots.

## Stats / BuildAgent framework (shared by Skill Tree + Shop + Items)

- **Only create a new `BuildAgent` subclass when the user explicitly asks for one.** When a feature needs a tunable stat, add the `BuildStatsObject` field (and read it in gameplay code), but leave the agent that upgrades it, and its wiring to Shop buttons or skill nodes, for the user to request.
- `Stats/BuildStatsObject.cs` — the live mutable stat blob for the current run; rebuilt from base + shop + skill tree + items every prestige.
- **`BuildStatsObject` holds stats for every system, so each field and getter name states its target** (`mouseRadius`, `flipperBotCount`, `prismShootingInterval`, `globalRockLandingCash`, `GetRockLandingCash`). Fields inside the per-instance classes (`RockTierBuildStats`, `MonolithBuildStats`, `ChestRarityBuildStats`) are already scoped by their class. When renaming a serialized field, add `[FormerlySerializedAs("oldName")]`.
- **Reacting to a `BuildStatsObject` change: listen to `RunEntry.skillTreeScreen.OnClosed` by default** (and also read the stat on `Start`/`OnEnable`), instead of adding a per-stat event. Skill-tree upgrades reach the player once the screen closes, so re-reading the stat there covers them (see `Monolith`, `Rock`, `RockStatBuilderButton`, `FlipperBotCountStatBuilderButton`). Add a dedicated `BuildStatsObject` event only when it works better. For example, `OnUnlockedSkillTree` fires from a Shop purchase, when the skill tree screen isn't open. Another case is a change that must show while the screen is still open.
- `Stats/Agents/BuildAgent.cs` — abstract base: `Apply(currentLevel, addingLevel, buildValuePerLevel)`. **`GetDescriptionText`/`TryToReportAchievement`/`TryToReportMaxedAchievement` are hard-stubbed** (`Debug.LogError`/fixed string) — not wired to real implementations despite being called from `SkillNode`.
- **Effects on every rock tier use a global stat, never a loop over tiers.** Add a `globalRock<X>` field under `BuildStatsObject`'s "Rocks - Global" header, a `BuildStatsObject.GetRock<X>(RockTier)` getter that adds it to the tier's value, read the getter in gameplay code, and put the agent in `Stats/Agents/Implementations/Rocks/Global/` (e.g. `globalRockLandingCash` / `GetRockLandingCash` / `IncreaseGlobalRockLandingCash`).
- `Stats/Agents/Implementations/Rocks/RockBuildAgent.cs` (abstract, tier-scoped) + `IncreaseRockCount.cs` (concrete) — **the pattern to copy for a new rock-tier upgrade agent.**
- `Stats/Agents/Implementations/UnlockSkillTree.cs` — sets `BuildStatsObject.UnlockedSkillTree = true` (fires `OnUnlockedSkillTree`), which `Shop/SkillTreeButton.cs` uses to make the Shop's "open skill tree" button interactable. Wire this to a one-time Shop purchase (`StatBuilderButton`), not a skill-tree node — a node inside the tree can't unlock the tree it lives in.
- `Stats/Agents/Skills/SkillBuildAgent.cs` is `[Obsolete]` but still has live subclasses (`...WithSimpleTooltip`, `...Percentage`) — legacy path, don't extend further.
- `Stats/IStatBuilder.cs` — interface implemented by both `StatBuilderButton` and skill-tree classes so `SideBarStatBuilder.Apply()` can treat them uniformly.

## Shop

- `Shop/SideBarStatBuilder.cs` — collects `IStatBuilder` children, calls `ApplyToBuildStats()` on each (called from `RunEntry.ApplyAllToBuildStats`).
- `Shop/StatBuilderButtons/StatBuilderButton.cs` — **composition-over-inheritance**: a button prefab is assembled from small marker components (`ICostConfig`, `IBuildValueConfig`, `IMaxLevelConfig`, optional `IStatBuilderButtonSpecific`), resolved via `GetComponentInParent<T>()`. Build new shop buttons by attaching these to a prefab, not by subclassing `StatBuilderButton`.
- **"Dumb" prefix convention** = simplest/static-value implementation of an interface (`DumbCostConfig`, `DumbBuildValueConfig`, `DumbMaxLevelConfig`, `DumbBuildAgent`) — often a placeholder rather than final content.
- `Shop/StatBuilderButtons/Specifics/RockStatBuilderButton.cs`/`RockCountStatBuilderButton.cs` — concrete "buy rock count" button, also spawns rock instances on purchase.
- `Shop/Items/` — **each item is a data-only prefab** (never spawned) with an `Item` component (plain `MonoBehaviour`) (`itemId`, name, `ChestRarity`, icon; `itemId` auto-derived from `itemName`, or the prefab file name if that's empty: "Lucky Pebble" → `item_lucky_pebble`. The id follows renames until the item is registered in an `ItemManager`, then it's frozen. Resync it by hand with the "Assign Item Id From Name" context menu). **Every production item prefab is a prefab variant of `Prefabs/Run/Items/ItemBase.prefab`**, saved under `Prefabs/Run/Items/<Rarity>/`; the base itself has no `itemId` and is never registered, and a new variant gets its own ID instead of inheriting one. `ItemManager` (ScriptableObject, `RunEntry.itemManager`) lists the prefabs in `itemPrefabs` and validates them in editor (null/duplicate, not an `ItemBase` variant, empty or duplicate ID). Per-item progress is `ItemState` in `RunData`, keyed by `itemId` — never change an existing item's ID.
- **Item effects**: an item prefab carries no `BuildAgent` component. `Item.buildAgent` references a `BuildAgent` prefab from `Prefabs/Run/BuildAgents/`, the same agent prefabs skill nodes reference (`SkillGraphNode.buildAgent`); reuse an existing one, or add a new agent prefab there when none fits. `Item.buildValue` is the per-level build value. `ItemManager.ApplyToBuildStats` applies each item's current item level during the BuildStats rebuild, and `RunData.AddItemExp` applies the gained levels when an item levels up. `ItemManager` validation fails for an item with no `buildAgent` set, unless it's a placeholder.
- **Finding items**: when a chest opens (`Chest.Open`), it finds one random item of its own rarity (`ItemManager.GetRandomItem`, uniform over the items where `Item.IsDroppableInCurrentBuild`) and adds `GameBalance.GetItemExpPerFind(rarity)` item exp to it via `RunData.AddItemExp`. Full builds drop any item, placeholders included; demo and playtest builds drop only non-placeholder items with `Item.isAvailableInDemo` ticked. If no item qualifies, the chest logs a warning and drops nothing.
- **Placeholder items**: `ItemManager` always holds the full item count per rarity from the GDD ([Content/Items.md](../../../../../GDD/Content/Items.md)); slots without a designed item are filled by placeholder prefabs `_Placeholder_<NN>` (`Item.isPlaceholder` ticked, no `buildAgent`, id `item_placeholder_<NN>`, numbered across all rarities). **To add an official item, convert a placeholder of the same rarity instead of creating a new prefab**: rename the prefab file, untick `isPlaceholder`, fill in name/icon/`buildAgent`/`buildValue`, then run "Assign Item Id From Name" (the id is frozen because the placeholder is registered). Placeholder ids are the one exception to "never change an item's ID". If the GDD count for a rarity grows, add placeholders with the next unused number; never reuse a number.

## Skill Tree

- `Skill tree/Graph/SkillTreeGraph.cs`/`SkillGraphNode.cs` — **XNode**-based node-graph asset (edited via `Skill tree/Graph/Editor/*`): cost (`levelCount`, one `currency`, and a `TieredExponentialPrice` `price` evaluated per level by `GetNextLevelCost`), `BuildAgent` ref + `buildValue`, unlock requirements, demo-build limits.
- `Skill tree/SkillNode.cs` — runtime `MonoBehaviour` counterpart of a graph node; click-to-upgrade, spends via `RunData.SpendCurrency`, unlocks children.
- `Skill tree/SkillTree.cs` — owns 3 parallel graphs (`mainSkillTreeGraph`, `laserSkillTreeGraph`, `lightningSkillTreeGraph`).
- `Skill tree/Special Crusher Configs/` — a **branching alternate sub-tree mechanic** ("Special Crusher": `SpecialCrusherId { Demo, None, Laser, Lightning }`, `SkillTree.SetActiveSpecialTree`). Currently only 2 variants exist. Not connected to the `Combat/Monoliths/` stub.
- `Skill tree/SkillNodeOverrider.cs` — abstract hook to override max-ability/cost/click behavior for non-standard nodes; all virtuals default to "no override".

## Maturity — what's actually implemented

**Solid & working**: Rocks/Tiers/Pure Rocks, Flippable animation, Player Cursor input, Flipper Bots (basic — no smart targeting/battery/charging), Shop stat-builder buttons, Skill Tree incl. branching Special Crusher sub-trees, Currency/RunData persistence, tutorial task sequencing, combat backgrounds, save-slot management.

**Scaffolded but incomplete**: **Prestige** — `RunStateManager` has full state transitions/events but the actual data-reset calls are **commented out** (`// RunData.Prestige();` etc. in `RunStateManager.cs`); `RunData.CorrectData` is an empty stub. Don't assume Prestige resets anything without verifying. Also: `GameAchievementsReporter` (empty), several `BuildAgent` description/achievement hooks (hardcoded stubs), `GameBalance`/`ShortHandManager` (empty).

**Stub only**: Monoliths — `Combat/Monoliths/` has a `Monolith` component + `MonolithType` enum and a base prefab; only Prism (L VIII) has behavior so far (targeting/laser, no hit effect yet).

**Not present at all in code**: The Rift. Pure design-doc concept with zero implementation (see `GDD/Systems/`).

**Per-tier unique abilities** exist for Bouncy (P1), Restless (P2) and Shockwave (P3) under `Combat/Rocks/Abilities/` (each a `RockExtender` on the tier's prefabs, tuned by `BuildStatsObject` fields).

## Conventions & gotchas

- Editor tooling is baked into runtime classes via `#if UNITY_EDITOR` + `[ContextMenu]` (`PrototypeManager.Validate`, `CurrencyConfigManager.Editor_Sync`, `SkillGraphNode.Editor_LogCosts`, `CashTiers.Editor_AutoColors`) — follow this pattern for new data validators rather than writing separate Editor-only scripts.
- Pooling: rocks, tooltips, VFX all go through `FH.Core.Architecture.Pool` (`GeneralPoolMemberSimplified`, `TakeInstance`/`TryReturnToPoolAndDeactivate`) — don't `Instantiate`/`Destroy` pooled prefab types directly.
- Partial-class convention (`RunData`, `Playfield`) — split large types by `TypeName_Aspect.cs`, don't grow one giant file.
- **Gameplay code must not know about `AutoMouse`** (`Dev/AutoMouse.cs`, the dev toggle that plays the mouse for you). The dependency is one-way: `AutoMouse` uses gameplay APIs, and gameplay never references it. That means no `AutoMouse` fields, type checks, or AutoMouse-specific flags or branches. If `AutoMouse` needs a hook, add a general-purpose public API on the gameplay side that makes sense without it, like `PlayerCursor.AddFlippingLock`/`FindHits` or `ChestInstanceManager.ActiveChests`, and call it from `AutoMouse`.
