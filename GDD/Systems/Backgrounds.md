# Backgrounds

Backgrounds are the bottom layer of the playfield (see [Main Screen](../Screens/MainScreen.md#background)). This doc covers how backgrounds are unlocked and how they affect gameplay, which is only through the [Preferred background](#preferred-background) and [unique items](#unique-items).

## Purpose
* Create significant visual variety.
* The Preferred background and unique items encourage the player to spend time on all backgrounds instead of sticking to the ones they like.

## Unlocking
* The background feature starts locked and is unlocked in the [Skill Tree](SkillTree.md).
* The player starts with 1 background unlocked, with the grassland [biome](../Screens/MainScreen.md#background).
* All backgrounds are listed in [Backgrounds](../Content/Backgrounds.md).
* Other backgrounds are unlocked in the [Backgrounds tab](../Screens/MainScreen.md#side-bar) by spending [stars](Currencies.md), one at a time and in random order: each unlock picks a random locked background.
  * The first unlock costs 0 stars; every later unlock costs 1 star.
* The player sets one of the unlocked backgrounds as the **active background** in the Backgrounds tab, and can switch it at any time for free.

## Preferred Background
* When at least 2 backgrounds are unlocked, at any time one of them is marked as the **Preferred background**. With only 1 background unlocked, there is no Preferred background and no Preferred Multiplier.
  * The first Preferred background is the first background unlocked after the starting one.
* The Preferred background changes every **Preferred Interval**, to one picked randomly from the other unlocked backgrounds, so it always changes to a different one.
* When the player sets the Preferred background as the active background, they get an income multiplier, the **Preferred Multiplier**, which applies to [cash](Currencies.md) income from all sources.
* The Preferred Multiplier increases as the player unlocks more backgrounds.
* All background stats are listed in [Background Stats](../Stats/Background%20Stats.md).

## Unique Items
* Some [items](Items.md) can only be found in specific backgrounds.
* Unique [chests](Chests.md#rarity) drop items unique to a background; these items only drop from Unique chests.

## Decisions

## Open Questions

## Parked Ideas

## Creative Guidance
* Just typical video game biomes (grassland, desert, snow, volcanic,...).
* Just create visual variety, doesn't transform gameplay; there are no gameplay elements tied to which biome the background presents.

## Brainstorm
