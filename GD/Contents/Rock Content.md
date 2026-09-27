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

Rock count is not a stat, but it also grows: more rocks of each tier are bought from the [Combat Screen](../Layouts/Combat%20Screen.md) side bar.

## Parameters

| Parameter | Controls |
|---|---|
| Base value per stat, per tier | Starting value of each stat above |
| Change per upgrade level, per stat, per tier | How much each upgrade level adds (or removes, for Max HP) |
| Max upgrade level, per stat, per tier | When the upgrade is maxed (needed for the ending) |
| Upgrade cost, per level | Cost of the next level (TBD) |
| Rock purchase cost, per tier | Cost of the next rock of that tier (TBD) |

All values `TBD`.

## Open questions

- Max HP floor: HP is an integer and drops by 1 per landing, so the lowest possible Max HP is 1 (the rock breaks on every landing). Is 1 the cap for the Max HP upgrade, or does the upgrade stop earlier?
- Are rock stat upgrades nodes in the [Skill Tree](../Systems/Skill%20Tree.md), a separate upgrade screen, or both?
- Which currency pays for each upgrade: Cash, Star, or depends on the stat?
- Can [Items](../Systems/Items.md) effects or the [Backgrounds](../Systems/Backgrounds.md) Preferred Multiplier also boost these stats? If so, do they stack with upgrades additively or multiplicatively?
- Do higher tiers add tier-specific upgradable stats (e.g. for special abilities)?

## Decisions

- Max HP upgrades lower Max HP: rocks break more often, giving more frequent breaking cash.
