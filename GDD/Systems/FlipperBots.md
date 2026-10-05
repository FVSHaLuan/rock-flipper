# Flipper Bots

**Flipper Bots** are floating robots, bought from the [side bar](../Screens/MainScreen.md#side-bar), that move randomly around the playfield and [flip](Rocks.md#flipping) rocks. A Flipper Bot is always in one of two states, Inactive or Active, switching between them after some time. All Flipper Bot stats are listed in [Flipper Bot Stats](../Stats/Flipper%20Bot%20Stats.md). Flipper Bots have a [max count](Rocks.md#tiers): the maximum number of Flipper Bots. There are no upgrades for an individual Flipper Bot.

## Purpose
Flipper Bots are the main idle device of the game.

## Inactive State
In **Inactive state**, a Flipper Bot is still floating up and down, but not moving and not flipping.

## Active State
In **Active state**, a Flipper Bot moves randomly and continuously performs a flip every time interval, flipping all ground rocks in range. A flip is performed even when no ground rocks are in range.

## Decisions

## Open Questions
* Which state does a newly bought Flipper Bot start in?
* Are the Inactive and Active durations fixed or random?
* Is movement constrained to the playfield? Can Flipper Bots overlap rocks or each other?
* What is the shape of the flip range?

## Parked Ideas

## Creative Guidance

## Brainstorm
