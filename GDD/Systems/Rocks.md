# Rocks

**Rocks** lay around the playfield. The game starts with 1 P0 rock; more rocks must be purchased. All rock stats are listed in [Rock Stats](../Stats/Rock%20Stats.md).

## Purpose

## HP
Rocks have integer **HP**.

## Flipping
**Flipping** a rock throws it from the ground up into the air.
* A flipped rock always **lands** in a different position than its initial position.
* On landing, the rock earns **landing cash**.

### Critical Landing
Each time a rock lands, there is a **critical landing chance** that the landing is a **critical landing**, which earns landing cash multiplied by the **critical cash multiplier**.
* Critical landing chance and critical cash multiplier each have a per-tier value and a global value; a rock uses the sum of its tier's value and the global value.
* The landing that breaks a rock can be a critical landing; its breaking cash is then also multiplied by the critical cash multiplier.

### Breaking
* Each time a rock lands, it loses 1 HP.
* When a rock's HP reaches 0, it **breaks** and earns **breaking cash**, a multiple of landing cash.
* After breaking, a new rock of the same tier with full HP replaces the broken rock. The new rock has a chance to be pure.

## Purity
**Pure** rocks earn a multiplied cash income.
* When a pure rock breaks, there is a global chance for it to be pure again. This chance is rolled first; if it fails, the replacement rock still gets its tier's chance for a new rock to be pure.

## Tiers
Rocks come in different **tiers**, named P0, P1, P2,... Rocks in the same tier share tier-specific stats: **Max HP**, landing cash, **breaking cash multiplier**,...

Each tier has a **max count**: the maximum number of rocks of that tier.

There are no upgrades for an individual rock.

All tiers are listed in [Rock Tiers](../Content/Rock%20Tiers.md).

> WIP: Tiers other than P0 are still being developed, but they generally:
> * have better qualities,
> * could have special abilities,
> * cost more to buy and upgrade.

## Decisions

## Open Questions
* Can anything besides the [mouse](Mouse.md), [Flipper Bots](FlipperBots.md) and [Restless](../Content/Rock%20Tiers.md#p2-restless) self-flips and [Shockwave](../Content/Rock%20Tiers.md#p3-shockwave) shockwaves flip rocks? Can a rock be flipped while already in the air?
* Is a rock's landing position constrained to the playfield? Can it land on / under or overlap other elements (e.g. [monoliths](Monoliths.md))?
* On the breaking landing, does the rock earn both landing cash and breaking cash, or only breaking cash?
* Critical landing:
  * Is the critical landing chance the same for rocks flipped by the mouse and by Flipper Bots?
  * Is critical landing chance capped at 100%, or does anything happen beyond it?
  * Does a critical landing do anything besides multiplying landing cash?
* What exactly does flipping speed speed up (e.g. the time a flipped rock spends in the air before landing)?
* Does the replacement rock appear at the broken rock's landing position, or somewhere else?
* Purity:
  * Can only replacement rocks be pure, or can purchased / starting rocks also be pure?
  * Does purity multiply landing cash, breaking cash, or both?
* Is there any source of HP loss or gain other than landing (e.g. regeneration, upgrades)?
* What happens when a tier, or the [Flipper Bots](FlipperBots.md), reaches its max count (e.g. no more rocks of that tier / Flipper Bots can be purchased)?

## Parked Ideas

## Creative Guidance

## Brainstorm
