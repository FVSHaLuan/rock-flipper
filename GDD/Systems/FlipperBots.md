# Flipper Bots

**Flipper Bots** are floating robots, bought from the [Shop tab](../Screens/MainScreen.md#shop-tab) of the side bar, that move around the playfield and [flip](Rocks.md#flipping) rocks. A Flipper Bot is always in one of two states, Inactive or Active, switching between them after some time. All Flipper Bot stats are listed in [Flipper Bot Stats](../Stats/Flipper%20Bot%20Stats.md). Flipper Bots have a [max count](Rocks.md#tiers): the maximum number of Flipper Bots. There are no upgrades for an individual Flipper Bot.

## Purpose
Flipper Bots are the main idle device of the game.

## Inactive State
In **Inactive state**, a Flipper Bot is still floating up and down, but not moving and not flipping.

## Active State
In **Active state**, a Flipper Bot moves and continuously performs a flip every time interval. A flip is performed even when no ground rocks are in range.

By default, Flipper Bots can't flip [chests](Chests.md). Chest flipping, unlocked in the [Skill Tree](SkillTree.md), lets them flip chests.

By default, Flipper Bots move randomly. **Smart Pathing**, unlocked in the Skill Tree, replaces random movement: Flipper Bots move in a way optimized to flip the most rocks. Once chest flipping is unlocked, Smart Pathing also counts chests, treating rocks and chests identically.

## Strength and Flip Range
Each flip flips ground rocks (and chests, once chest flipping is unlocked) within the **flip range**: the radius of the circle around the Flipper Bot in which it finds rocks and chests to flip. A flip flips up to **strength** rocks and chests at once: when more are in range than its strength, the ones to flip are picked at random. Strength and flip range are upgradable.

## Decisions

## Open Questions
* Which state does a newly bought Flipper Bot start in?
* Are the Inactive and Active durations fixed or random?
* Is movement constrained to the playfield? Can Flipper Bots overlap rocks or each other?

## Parked Ideas

## Creative Guidance

## Brainstorm
