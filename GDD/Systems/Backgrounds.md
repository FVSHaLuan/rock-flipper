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
  * Whenever a background is unlocked, it immediately becomes the Preferred background. So the first Preferred background is the first background unlocked after the starting one.
* The Preferred background changes every **Preferred Interval**, to one picked randomly from the other unlocked backgrounds, so it always changes to a different one.
* When the player sets the Preferred background as the active background, they get an income multiplier, the **Preferred Multiplier**, which applies to [cash](Currencies.md) income from all sources.
* The Preferred Multiplier increases as the player unlocks more backgrounds.
* All background stats are listed in [Background Stats](../Stats/Background%20Stats.md).

## Unique Items
* Unique [items](Items.md#rarity) aren't tied to a specific background: each background drops a Unique item of a random kind.
* Unique items only drop from Unique [chests](Chests.md#rarity).
* Unique chests can only drop once the background feature is unlocked.
* Each background drops only 1 Unique chest, and only while it is the active background.
  * If the active background has already dropped its Unique chest, no Unique chest drops.
  * Once every background has dropped its Unique chest, Unique chests no longer drop.
* A Unique chest's item is decided as soon as the chest drops. Unique chests only drop while some Unique items haven't been found yet and aren't held by an unopened Unique chest.
* A background can only drop its Unique chest once it has been the active background for a minimum time in total, added up across every time it was active.
  * After that, the longer it has been the active background, the more likely a dropped chest is Unique.
  * Time only counts once the background feature is unlocked; the starting background begins with a time offset.

## Decisions

## Open Questions

## Parked Ideas

## Creative Guidance
* Just typical video game biomes (grassland, desert, snow, volcanic,...).
* Just create visual variety, doesn't transform gameplay; there are no gameplay elements tied to which biome the background presents.

## Brainstorm
