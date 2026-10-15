---
name: add-skill-node
description: Add one or more Skill Tree nodes end to end in Rock Flipper. Covers the BuildAgent subclass (C#), the BuildAgent prefab (icon, title, description) and the SkillGraphNode in MainSkillTree.asset. Use when asked to add, create or wire up a skill, skill node, upgrade node or build agent, or to make a stat upgradable in the skill tree. Also use when only part of the pipeline is needed (e.g. a new agent prefab for an Item).
---

# Add a skill node

A skill node is a chain of three assets, and each one references the previous one by GUID:

```
BuildAgent subclass (.cs)  ->  BuildAgent prefab (.prefab)  ->  SkillGraphNode (in MainSkillTree.asset)
   changes a BuildStats field     has SkillMetaData + SkillDescriptor     cost, levels, parents, buildValue
```

Do all three steps in this one context, in order. Don't hand individual steps to subagents: each step needs the GUID that the previous step produced.

The reference docs below hold the details. Read them before starting; this skill is the ordered checklist that ties them together:
- `Rock Flipper/Assets/_AMainGame/Data/Skill Trees/CLAUDE.md`: node anatomy, naming, grid and layout rules, BuildAgent prefab anatomy, and `unity-cli` eval snippets for creating graph nodes.
- `Rock Flipper/Assets/_AMainGame/Prefabs/Run/BuildAgents/CLAUDE.md`: prefab template, Title Group/Title rules, and the LocalizedString policy.
- `Rock Flipper/Assets/_AMainGame/Scripts/Run/CLAUDE.md`, sections "Stats / BuildAgent framework" and "Skill Tree".

## 0. Gather inputs

For each node, settle these before editing anything:

| Input | Default if the user doesn't say |
|---|---|
| Stat it changes (a `BuildStatsObject` / `RockTierBuildStats` / `MonolithBuildStats` field) | none: ask |
| Parent node(s) | right of `Skill Repo` (see Skill Trees CLAUDE.md, step 2) |
| `levelCount`, `buildValue` | ask, or use placeholder balance and say so in the summary |
| `currency`, `price` | `CASH`, `cashTier 0, a 0, b 1, c 1` (placeholder), and say so |
| `unlockingRequirement`, `minParentLevelEach`, `demoLimit` | `1`, `0`, `-1` |
| Icon / sub-icon | reuse what sibling prefabs of the same object use; sub-icons live in `Images/Run/Icons/Skill Subicons/<Category>/`. If none fits, leave it empty and list it in the summary |

**The stat must already exist and be consumed by gameplay.** If the field isn't in `BuildStatsObject` (or a sub-stats class), or nothing reads it, stop here. Making the stat exist is a gameplay feature, not part of this skill; confirm the scope with the user first.

Check the GDD: if the stat isn't listed in the matching `GDD/Stats/*.md` file, or the user stated a design rule (currency, level cap, unlock condition), follow the root `CLAUDE.md` "Keeping the GDD in sync" rule. Placeholder balance isn't design, so don't write it into the GDD.

## 1. BuildAgent subclass

Reuse first. Look in `Scripts/Run/Stats/Agents/Implementations/<Subsystem>/` for an agent that already changes this field. Rock-tier agents derive from `RockBuildAgent`, which has a serialized `rockTier`, so one class serves every tier. Only write a new class when nothing fits.

New class conventions (match the existing files):
- Namespace `Agame.Run.Stats.Agents`. File and class name follow `<Verb><Object><Stat>`, using `Increase`, `Reduce`, `Decrease`, `Enable` or `Unlock`. Examples: `IncreasePrismMaxBeamCount`, `ReduceFlipperBotIdleTime`.
- Folder: `Implementations/<Subsystem>/` (`Rocks`, `Rocks/Global`, `Monoliths`, `Flipper Bots`, `Mouse`, `Backgrounds`).
- Body is just the `Apply` override:
  - Numeric stat: `BuildStats.<field> += addingLevel * (<fieldType>)buildValuePerLevel;` (use `-=` for Reduce/Decrease).
  - Boolean unlock or enable: `if (currentLevel + addingLevel > 0) { ... = true; }`.
  - Rock tier: derive from `RockBuildAgent` and override `Apply(RockTierBuildStats rockTierBuildStats, ...)`.
- `Apply` must be additive in `addingLevel`, because it's replayed on load.

Then compile and generate the `.meta` through the `unity-cli` skill (refresh assets, then check the console for compile errors). Confirm that `<Class>.cs.meta` exists before moving on: the prefab needs the script GUID.

## 2. BuildAgent prefab

Reuse first. If a prefab with this agent and these target settings (tier, monolith) already exists under `Prefabs/Run/BuildAgents/`, use it.

Otherwise, through `unity-cli` (preferred over hand-editing YAML):
1. `AssetDatabase.CopyAsset` copies `Prefabs/Run/BuildAgents/SKill Build Agent.prefab` to `Prefabs/Run/BuildAgents/<Object folder>/<Name>.prefab`. The name follows the existing files, e.g. `Increase L VIII Max Beam Count`, `Increase P2 Rock Max Count`. Group by object, as in `Monolith/L VIII/` and `Rock/Increase Max Count/`.
2. `PrefabUtility.LoadPrefabContents`: add the agent component, destroy the `BuildAgentScriptPlaceholder`, then `SaveAsPrefabAsset` and `UnloadPrefabContents`.
3. Set fields with `SerializedObject`:
   - `SkillMetaData`: `icon`, `subIcon`, `titleGroup` (the object, e.g. `Monolith L VIII`, `All Rocks`, `Flipper Bots`), and `title` (the effect, without repeating the group, e.g. `Max Beam Count`). Use an existing `LocalizedString` only if one already matches. Never create new localization entries.
   - `SkillDescriptor`: `descriptionFormat` with `{0}` for the value, e.g. `+{0} Max Beam Count`, `-{0}s Shooting Interval`. For percentage stats stored as fractions, use `+{0}%` with `buildValueMultiplier: 100`.
   - Agent-specific fields such as `rockTier` or `monolithType`.
4. Confirm that `<Name>.prefab.meta` exists.

If the task is only "make an agent prefab for an Item", stop here.

## 3. SkillGraphNode in MainSkillTree.asset

Create the node through `unity-cli` eval, using the snippet in Skill Trees CLAUDE.md under "Scripting graph edits via unity-cli". Don't hand-edit the graph YAML.
- `m_Name`: short `[object] [property]`, unique across the graph (e.g. `L VIII Max Beams`, `P2 Crit Chance`).
- Position: on the 250-unit grid, one hop from the parent, in a direction that parent's other children don't already use. No overlaps.
- Connect `parent.output` to `child.input`, and set `buildAgent` to the prefab's `BuildAgent` component, plus the cost and level fields from step 0.
- `AssetDatabase.SaveAssets()`, then read the console. The graph editor flags "No icon", "No title group", "No title" and "No agent".

**Don't re-import into `Scenes/Run.unity`.** That's a scene edit (see root `CLAUDE.md`). When you finish, tell the user the Run scene's tree is out of date. Ask whether they'll run `SkillTree` → `Editor_ImportFromGraph (Dirty)` themselves or want you to.

## 4. Verify and report

- Every new `.cs` and `.prefab` has its `.meta`.
- The console has no new errors or warnings.
- Each node's `buildAgent` resolves to the intended prefab: open it with the node inspector's "View Build Agent", or read the GUID back.
- Don't enter Play mode to test without asking (root `CLAUDE.md`).

Summary for the user: list the nodes (name → parent, levels, buildValue, cost), say which values are placeholder balance, list any missing icons, list any GDD edits, and remind them the scene re-import is pending. Don't commit unless asked.
