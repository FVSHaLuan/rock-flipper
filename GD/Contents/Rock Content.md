# Rock Content

**Status**: designing
**Depends on**: [Rocks](../Systems/Rocks.md), [Skill Tree](../Systems/Skill%20Tree.md), [Currencies](../Systems/Currencies.md), [Chests](../Systems/Chests.md), [Levels](../Systems/Levels.md)

## Upgradable stats

> **[Proposal]** Every rock stat named in the system docs is upgradable. Each upgrade applies to one tier (P0, P1, P2,...), since tier-specific stats are shared by all rocks of that tier.

| Stat | What it controls | Upgrade direction | Source |
|---|---|---|---|
| Landing cash | Cash earned each time a rock lands | Increases | [Rocks](../Systems/Rocks.md) |
| Breaking cash multiplier | Breaking cash as a multiple of landing cash | Increases | [Rocks](../Systems/Rocks.md) |
| Max HP | Landings a rock survives before it breaks | Decreases (rocks break more often, so breaking cash comes more often) | [Rocks](../Systems/Rocks.md) |
| Pure chance | Chance that a replacement rock is pure | Increases | [Rocks](../Systems/Rocks.md) |
| Pure multiplier | Cash multiplier for pure rocks | Increases | [Rocks](../Systems/Rocks.md) |
| Chest exp per landing | Chest exp added to the chest bar per landing | Increases | [Chests](../Systems/Chests.md) |
| Level exp per landing | Level exp added to the level bar per landing | Increases | [Levels](../Systems/Levels.md) |
| Max rock count | The most rocks that can be owned; rock purchases stop at this cap | Increases | This doc |
| Critical landing chance | Chance that a landing is a critical landing | Increases | This doc |
| Critical landing multiplier | Landing cash multiplier on a critical landing | Increases | This doc |
| Critical landing damage | HP a rock loses on a critical landing (a normal landing loses 1) | Increases (rocks break more often, so breaking cash comes more often) | This doc |

Rock count is not a stat, but it also grows: more rocks of each tier are bought from the [Combat Screen](../Layouts/Combat%20Screen.md) side bar, up to the max rock count.

### Critical landing

Each landing has a chance, set by critical landing chance, to be a **critical landing**. A critical landing:
- earns landing cash × critical landing multiplier, and
- makes the rock lose critical landing damage HP instead of 1.

Critical landing damage is an integer, since HP is an integer.

## Parameters

| Parameter | Controls |
|---|---|
| Base value per stat, per tier | Starting value of each stat above |
| Change per upgrade level, per stat, per tier | How much each upgrade level adds (or removes, for Max HP) |
| Max upgrade level, per stat, per tier | When the upgrade is maxed (needed for the ending) |
| Upgrade cost, per level | Cost of the next level (TBD) |
| Rock purchase cost, per tier | Cost of the next rock of that tier (TBD) |
| Base max rock count | Max rock count before any upgrade |

All values `TBD`.

## Open questions

- Max HP floor: HP is an integer and drops by 1 per landing, so the lowest possible Max HP is 1 (the rock breaks on every landing). Is 1 the cap for the Max HP upgrade, or does the upgrade stop earlier?
- Are rock stat upgrades nodes in the [Skill Tree](../Systems/Skill%20Tree.md), a separate upgrade screen, or both?
- Which currency pays for each upgrade: Cash, Star, or depends on the stat?
- Can [Items](../Systems/Items.md) effects or the [Backgrounds](../Systems/Backgrounds.md) Preferred Multiplier also boost these stats? If so, do they stack with upgrades additively or multiplicatively?
- Do higher tiers add tier-specific upgradable stats (e.g. for special abilities)?
- Max rock count: one cap per tier, or one cap shared by all tiers? (The per-tier proposal above assumes per tier.)
- Max rock count: can the player hit the cap before the upgrade is available? If so, what does the side bar buy button show at the cap?
- Critical landing chance: capped at 100%, or does the upgrade stop earlier?
- Critical landing multiplier: does it stack with the pure multiplier on a pure rock? Additively or multiplicatively?
- Critical landing that breaks the rock: does the critical landing multiplier also apply to the breaking cash?
- Critical landing damage greater than the rock's remaining HP: the rock breaks at 0; is the extra damage just lost?
- Does a critical landing also multiply chest exp and level exp per landing?
- Does a critical landing get its own feedback (visual/sound/text) on the [Combat Screen](../Layouts/Combat%20Screen.md)?

## Decisions

- Max HP upgrades lower Max HP: rocks break more often, giving more frequent breaking cash.
- Max rock count is upgradable: rock purchases are capped, and the cap increases with upgrades.
- Critical landings exist, with three upgradable stats: critical landing chance, critical landing multiplier and critical landing damage.
