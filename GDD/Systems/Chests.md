# Chests

**Chests** are dropped by filling the chest bar, and are opened to drop [items](Items.md). All chest stats are listed in [Chest Stats](../Stats/Chest%20Stats.md).

## Purpose

## Chest Bar
* When a [rock](Rocks.md) lands, it adds **chest exp** to the **chest bar** (see [Main Screen](../Screens/MainScreen.md#overlay-ui-elements) for its position).
* When the chest bar fills up, it resets and drops a chest.

## Flipping & Breaking
* Chests have integer HP and are flipped the same way as rocks (see [Rocks → Flipping](Rocks.md#flipping)).
* When a chest's HP reaches 0, it breaks, which opens it.

## Opening
* **Opening** a chest means getting its contents out.
* Breaking is currently the only way to open a chest; the player might be able to open chests by other means in the future.
* When a chest opens, it drops one or more items, at least one of which has the same rarity as the chest.

## Rarity
Chests come in these **rarities**: Common, Uncommon, Rare, Epic, Unique. [Items](Items.md#rarity) use the same rarity system.

Chests of the same rarity share rarity-specific stats, such as Max HP.

Unique chests drop items unique to a [background](Backgrounds.md#unique-items); these items only drop from Unique chests.

## Decisions
* "Open" is the term for getting a chest's contents out.

## Open Questions
* Which rock rules also apply to chests?
  * Do chest landings add chest exp and [level exp](Levels.md#level-bar)?
  * Does a chest lose exactly 1 HP per landing? Does a chest landing earn landing cash?
  * Can chests make [critical landings](Rocks.md#critical-landing)?
  * Can chests be flipped by the [mouse](Mouse.md) and by [Flipper Bots](FlipperBots.md)?
* Does the chest bar get harder to fill over time?
* How is a dropped chest's rarity decided (random chance, upgradable odds)?
* Where does a dropped chest appear on the playfield? Is it a [gameplay element](../Screens/MainScreen.md#gameplay-elements)?
* Is there a cap on how many chests can be on the playfield at once?
* After a chest breaks, is it gone for good (unlike rocks, which are replaced)?
* What other ways to open chests might be added?
* How many items does a chest drop — does it depend on rarity?
* Can a Unique chest drop when only one background is unlocked? Which background's unique items does it drop — the one set as [active background](Backgrounds.md#unlocking)?

## Parked Ideas

## Creative Guidance

## Brainstorm
