# Skill Tree

Most of the game's progression happens via the **Skill Tree** (see [Pillars](../Pillars.md#core-mechanics)), which has its own screen, opened from the [side bar](../Screens/MainScreen.md#side-bar).

## Purpose

## Nodes
* The Skill Tree consists of **nodes**. Each node is a **skill**: it unlocks or upgrades things. Only Skill Tree nodes are skills.
* Leveling up a skill is an [upgrade](../Pillars.md#core-loop), but not every upgrade is a skill (e.g. buying more rocks or Flipper Bots on the side bar).
* A skill can have levels (**skill level**).

## Structure
* There is one **root skill**: it has no **parent skills** and is available from the start.
* Every other skill must have parent skills.
* The conditions for a skill to become available based on its parent skills (e.g. whether all or any parent skills are required) are configured per node.

## Known Skills
Skills mentioned in other docs:
* Unlocking [backgrounds](Backgrounds.md#unlocking), with [stars](Currencies.md).
* Unlocking [monoliths](Monoliths.md), with stars.
* Upgrading each monolith's [ability](Monoliths.md#ability).

## Decisions
* "Skill level" is used to keep it distinct from the player's level and item level.

## Open Questions
* Does a skill's cost increase with each skill level?
* Do skills have a max skill level?
* What does the root skill do?
* Which of these are unlocked or upgraded in the Skill Tree (or elsewhere, e.g. via [items](Items.md#item-effect)), and with which currency?
  * The [hover ability and mouse radius](Mouse.md)
  * [Critical landing](Rocks.md#critical-landing) itself (present from the start, or unlocked?), critical landing chance, critical cash multiplier
  * Higher [rock tiers](Rocks.md#tiers)
  * [Flipper Bot](FlipperBots.md) upgrades (if any)
  * [The Rift](TheRift.md) (present from the start, or unlocked?)
  * Monolith ability upgrades: stars or cash?

## Parked Ideas

## Creative Guidance

## Brainstorm
