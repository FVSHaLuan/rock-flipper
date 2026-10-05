# Items

**Items** are dropped from [chests](Chests.md). Finding an item earns cash and progresses its item level. All item stats are listed in [Item Stats](../Stats/Item%20Stats.md).

## Purpose

## Rarity
* Items use the same [rarity](Chests.md#rarity) system as chests. Unique items only drop from Unique chests.
* Rarity is an intrinsic property of an item, not a variant: each item has exactly one rarity (e.g. the Sword is Uncommon; there is no Common Sword or Rare Sword).
* When an item is found, the player earns [cash](Currencies.md); rarer items earn more cash.

## Item Progression
* Each item has an **item level** and an **item level bar**.
* The very first find of an item unlocks it at item level 0 and adds **item exp** to its item level bar.
* Finding more of the same item adds item exp to its item level bar.
* When the item level bar fills up, the item levels up, and the bar resets and becomes harder to fill.
* Once an item reaches its max level, finding more of it only earns cash (the same cash as a normal find); it no longer levels up.
* An item itself is not upgradable: its max level, item exp per find and item effect stats are fixed. The only exception is cash value per find, which is upgradable per rarity.

## Item Effect
Items have **effects** that scale with their item level.

## Decisions
* Item terms are item level and item exp, to keep them distinct from the player's level.

## Open Questions
* What kinds of effects do items have (e.g. boosts to rocks, chests, cash)? Are they always active once found?

## Parked Ideas

## Creative Guidance
* Most item effects just improve stats; rarer items simply give bigger boosts.
* No item should change or transform gameplay significantly, if at all.
* A few items may have a small impact on the rules of other systems, but it shouldn't be too significant.

## Brainstorm
