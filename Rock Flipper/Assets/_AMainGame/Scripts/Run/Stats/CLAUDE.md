# Scripts/Run/Stats — BuildStatsObject and its prefabs

How the run's stat blob is built, where its starting values live, and how to change them. See [Run/CLAUDE.md](../CLAUDE.md#stats--buildagent-framework-shared-by-skill-tree--shop--items) for the BuildAgent conventions (naming, global stats, when to add an agent).

## What it is

`BuildStatsObject` is a **`MonoBehaviour` on a prefab**, not a ScriptableObject. Every tunable in a run lives on it: plain fields (mouse, Flipper Bots, Prism, rock abilities, `globalRock*`) plus per-instance blocks (`rock_P0..P3` → `RockTierBuildStats`, `monolith_*` → `MonolithBuildStats`, `chest_*` → `ChestRarityBuildStats` in `Combat/Chests/`). Gameplay reads it through `ExtendedMonoBehaviourRun.BuildStats` (= `RunEntry.Instance.BuildStats`).

## Lifecycle (RunEntry)

1. **Base**: `RunEntry.TryInitBaseBuildStats` instantiates one source prefab into `BaseBuildStats`:
   - editor only: the first entry of `Assets/_Exp/BuildStatsTest.asset` (`TestBuildStats`, gitignored), if that asset exists and `enabled` is ticked. A warning logs "Using test BuildStats". **If live values don't match the prefab, check this first.**
   - otherwise `buildStatsDefault` if `RunData.FinishedTutorial`, else `buildStatsTutorial` (both serialized on `RunEntry` in `Scenes/Run.unity`).
2. **Live**: `BuildStats` is `Instantiate(BaseBuildStats)`, a runtime clone in the scene.
3. **Apply**: `RunEntry.ApplyAllToBuildStats` (on `ExtendedAwake`, and on every rebuild) replays every saved level through its agent: Shop buttons (`StatBuilderButton.ApplyToBuildStats` → `Apply(0, level, value)`), then the skill tree, then items.
4. **Incremental**: a purchase applies only the new levels on top of the live clone (`StatBuilderButton.LevelUp`, `SkillNode` → `Apply(currentLevel, 1, value)`, `RunData.AddItemExp`). Nothing is persisted on the stat blob itself; the save holds only levels (`RunData`), and the stats are re-derived from prefab + levels.
5. **Rebuild**: `RunEntry.RebuildBuildStats` destroys the clone and redoes step 3. Triggered by `RunStateManager.StartCombat` when marked invalid (prestige), `Dev/SkillTreeReloader`, and `SkillNode_Editor`.

Consequences:
- **Only `BuildAgent`s write to `BuildStatsObject`.** No other gameplay code assigns its fields today; keep it that way. Runtime state (owned counts on the field, cooldowns) belongs in `RunData` or the gameplay component.
- Agents must be **additive and order-independent**: `Apply(0, n, v)` must equal `n` calls of `Apply(i, 1, v)`, because the rebuild uses the first and purchases the second. Agents that also record a cap breakpoint (`IncreaseRockMaxCount`, `IncreaseFlipperBotMaxCount`) loop per level for this reason.
- `MaxCountCapBreakpoints` is **non-serialized** runtime state rebuilt by those agents; it never comes from the prefab.
- Editing the live clone in Play mode (`RunEntry` context menu `Editor_EditBuildStats`) is a throwaway tweak: the next rebuild or scene load discards it.

## The prefabs (`Data/BuildStats/`)

| Prefab | Used by | Real content |
|---|---|---|
| `BuildStats - Default.prefab` | `RunEntry.buildStatsDefault` | **The single source of starting values.** A plain prefab; all values are its own. |
| `BuildStats - Tutorial.prefab` | `RunEntry.buildStatsTutorial` (before `FinishedTutorial`) | Prefab **variant** of Default. Its only overrides are for fields from an older template (`maxHP`, `maxFuel`, `spawnRate`, ...) that no longer exist, so in practice it **equals Default**. |
| `BuildStats - Max.prefab` | Nothing references it | Variant of Default; likewise only dead overrides (`crushDamage`, `laserCrusher*`, ...). Effectively Default, not a "max" build. Don't rely on it. |

Because Tutorial and Max are variants, any value changed on Default flows into them unless they override it. If you ever give the Tutorial its own value, that override becomes the one place that differs; mention it when you do.

## Code initializers are NOT the live values

The C# field initializers (`= 0.5f`, `maxHp = 5`, ...) only matter when a field isn't serialized in the prefab yet. Once Unity re-saves the prefab, the prefab value wins. Many differ today, e.g. `rock_P0.maxHp` 15 (prefab) vs 5 (code), `flipperBotMovementSpeedFactor` 0.2 vs 1, `chest_*.maxHP` 5 vs 2. So:

- **To read a current starting value, read `BuildStats - Default.prefab`**, not the `.cs` file. If the field is missing from the prefab YAML, the code initializer is in effect (as of writing: `flipperBotFlipRange` 0.5 and `flipperBotStrength` 1).
- **When adding a new field**, give it the initializer you want as the starting value, so the prefab picks it up when Unity next serializes it. Then confirm it shows up in Default (see below).
- **Changing an existing field's initializer changes nothing in game** if the prefab already serializes that field. Change the prefab.

## Changing values in the future

Pick the layer by what the change means:

| Want | Change |
|---|---|
| Starting value of a stat (balance at level 0) | `BuildStats - Default.prefab` |
| How much one upgrade gives | `buildValue` on the Shop button (`IBuildValueConfig`), the `SkillGraphNode` in the skill tree graph, or the `Item` — not the BuildStats prefab |
| Upgrade cost / level cap | the button's `ICostConfig`/`IMaxLevelConfig`, or the node's `price`/`levelCount` |
| Temporary value for local testing | `_Exp/BuildStatsTest.asset` pointing at your own copy (never shipped), or the Play-mode clone |

How to edit the Default prefab:
- **Prefer `unity-cli`** (root CLAUDE.md): load the prefab, set the field through `SerializedObject`/`FindProperty` (e.g. `rock_P0.maxHp`, `chest_Epic.itemFindCash`), apply, `AssetDatabase.SaveAssets()`. This also writes any fields the YAML is missing.
- Hand-editing the YAML is acceptable for a scalar that's **already present** in `BuildStats - Default.prefab` (a plain prefab, so values are under the `MonoBehaviour` block by field name; doubles/floats in plain decimal). Don't hand-add missing fields or edit the variants' `m_Modifications` by hand.
- Never edit Tutorial/Max to change a value meant for the normal game; edit Default.
- After renaming a serialized field, keep `[FormerlySerializedAs("old")]` so the prefab's value carries over (the variants' dead overrides are keyed by old names and do nothing).
- A starting-value change is a design/balance change: if the GDD states that value (stat files under `GDD/`), update it per root CLAUDE.md, or point out the mismatch.

## Adding a stat (summary)

Field on `BuildStatsObject` under the right `[Header]`, named after its target system → initializer = intended start value → read it in gameplay (react via `skillTreeScreen.OnClosed`, see Run/CLAUDE.md) → make sure it's serialized into Default → agent only if the user asks for one. For a stat on every rock tier, use a `globalRock<X>` field plus `GetRock<X>(tier)`; for per-tier/per-monolith/per-rarity values, add the field to `RockTierBuildStats`/`MonolithBuildStats`/`ChestRarityBuildStats` (one prefab block per instance to fill in).
