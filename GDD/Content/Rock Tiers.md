# Rock Tiers

All rock [tiers](../Systems/Rocks.md#tiers) and what makes each one differ in rules.

| Tier | Name | Unique rules / abilities |
|---|---|---|
| P0 | | None: P0 rocks are just ordinary rocks. |
| P1 | [Bouncy](#p1-bouncy) | Has a chance to bounce and land once more after landing. |
| P2 | [Restless](#p2-restless) | Has a chance to flip itself after a duration on the ground. |
| P3 | [Shockwave](#p3-shockwave) | Has a chance to trigger a shockwave on landing, which flips ground rocks around it. |

## P1: Bouncy
**Bouncy** is the P1 tier. After landing, a Bouncy rock has a **bounce chance** to **bounce**: it lands once more. Bounce chance is upgradable (see [Rock Stats](../Stats/Rock%20Stats.md#bouncy-p1)).
* A landing from any flip (e.g. by the [mouse](../Systems/Mouse.md) or a [Flipper Bot](../Systems/FlipperBots.md)) can bounce. A bounce landing is just like a normal landing, except that initially it can't bounce again.
* **Rebound** is the unlockable ability for a bounce landing to bounce again. Once Rebound is unlocked, a bounce landing has the same bounce chance to bounce again.
* A Bouncy rock that breaks can't bounce.

## P2: Restless
**Restless** is the P2 tier. After a Restless rock has been on the ground for its **self-flip delay**, it has a **self-flip chance** to **self-flip**: it flips itself, with no [mouse](../Systems/Mouse.md) or [Flipper Bot](../Systems/FlipperBots.md) needed. Self-flip delay and self-flip chance are upgradable (see [Rock Stats](../Stats/Rock%20Stats.md#restless-p2)).
* Only continuous time on the ground counts: whenever the rock is in the air, whatever flipped it, the count restarts from zero.
* If the self-flip chance fails, the count restarts, and the rock tries again after another self-flip delay on the ground.
* A self-flip is a normal flip.

## P3: Shockwave
**Shockwave** is the P3 tier. On landing, a Shockwave rock has a **shockwave chance** to trigger a **shockwave**: it flips the rocks on the ground within the **shockwave radius** around it, up to **max shockwave flips** rocks. Shockwave chance, shockwave radius and max shockwave flips are upgradable (see [Rock Stats](../Stats/Rock%20Stats.md#shockwave-p3)).
* It's the only tier that flips other rocks.

## Decisions
* Restless rocks self-flip after a duration on the ground, not at a regular interval.
* A Shockwave rock triggers a shockwave on a chance, not on every landing.

## Open Questions
* What special abilities can higher tiers have?
* Are bounce chance and shockwave chance capped at 100%, or does anything happen beyond it?
* Shockwave:
  * When more rocks are within the shockwave radius than max shockwave flips, which ones are flipped?
  * Is a flip by a shockwave a normal flip (landing cash, HP loss, critical landing, bounce)?
  * Can a Shockwave rock flipped by a shockwave trigger a shockwave itself (chain reaction)?
  * Does any landing trigger a shockwave chance, whatever flipped the rock (mouse, Flipper Bot, shockwave), including the landing that breaks it?

## Parked Ideas
| Tier | Name | Unique rules / abilities |
|---|---|---|
| P4 | Purifier | Each landing from a primary flip makes lower-tier ground rocks around it glow, which makes them pure for a while. |
| P5 | Giant | Oversized. Each landing from a primary flip causes a quake: every other ground rock loses 1 HP, and rocks that reach 0 HP break normally. |
| ? | Ember | A critical landing from a primary flip sets it on fire: its next landings are all critical, each with a bigger critical cash multiplier than the last; landings on fire don't relight it, and the fire and its escalation pass to the replacement rock. Adds new stats, burn length, (+), and burn step, (+). |
| ? | Geode | Each landing from a primary flip has a chance to crack it: it breaks immediately, whatever its HP, and its breaking cash is multiplied by a jackpot multiplier. Adds new stats, crack chance (capped below 100%), (+), and jackpot multiplier, (+). |
| ? | Shatter | On breaking, it bursts into fragments that each land once, earning this tier's landing cash (pure multiplier included, can be critical) but no chest exp or level exp, then vanish; fragments aren't rocks. Adds a new stat, fragment count, (+). |
| ? | Flock | Each landing earns extra landing cash for every other rock of this tier thrown by the same flip; the bonus is part of landing cash, so critical landings and purity multiply it. Adds a new stat, flock bonus, (+). |
| ? | Crowbar | Each landing from a primary flip makes chests on the ground within range lose 1 HP. Only worth it if Flipper Bots can't flip chests. Adds a new stat, pry range, (+). |

## Creative Guidance
The higher tier should feel better than the lower ones.

## Brainstorm
