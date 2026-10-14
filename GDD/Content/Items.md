# Items

All [items](../Systems/Items.md) that differ in rules, one table per rarity; the `#` column tracks how many of each rarity's items are listed.

Number of distinct items per rarity:

| Rarity | Items |
|---|---|
| Common | 14 |
| Uncommon | 9 |
| Rare | 7 |
| Epic | 5 |
| Unique | 5 |

## Common
| # | Item | Effect |
|---|---|---|
| 1 | **Wallet** | +[landing cash](../Systems/Rocks.md#flipping) of every rock tier |
| 2 | **Spring** | +flipping speed of every rock tier (see [Rock Stats](../Stats/Rock%20Stats.md)) |
| 3 | **Hammer** | +[breaking cash multiplier](../Systems/Rocks.md#breaking) |
| 4 | **Dice** | +[critical landing chance](../Systems/Rocks.md#critical-landing) |
| 5 | **Magnifying Glass** | +critical cash multiplier |
| 6 | **Soap** | +chance for a new rock to be [pure](../Systems/Rocks.md#purity) |
| 7 | **Lantern** | +cash multiplier of pure rocks |
| 8 | **Key** | +[chest exp](../Systems/Chests.md#chest-bar) per landing |
| 9 | **Notebook** | +[level exp](../Systems/Levels.md#level-bar) per landing |
| 10 | **Wrench** | −[Flipper Bot](../Systems/FlipperBots.md) flip interval |
| 11 | **Battery** | +Flipper Bot active duration |
| 12 | **Alarm Clock** | −Flipper Bot inactive duration |
| 13 | **Sneakers** | +Flipper Bot move speed |

## Unique
| # | Item | Effect |
|---|---|---|
| 1 | **Pickaxe** | −[Max HP](../Systems/Rocks.md#tiers) of every rock tier (minimum 1) |

## Decisions

## Open Questions

## Parked Ideas
Each effect scales with item level.

### Uncommon
| Item | Effect |
|---|---|
| Rubber Ball | +bounce chance |
| Coffee Mug | −self-flip delay |
| Drum | +shockwave chance |
| Megaphone | +shockwave radius |
| Fishing Net | +mouse radius size |
| Magnet | +Flipper Bot flip range |
| Piggy Bank | +cash value per find (all rarities) |
| Compass | +Preferred Multiplier |
| Lockpick | −chest Max HP (minimum 1) |

### Rare
| Item | Effect |
|---|---|
| Four-Leaf Clover | Pure rocks get extra critical landing chance. |
| Sledgehammer | Chance a landing removes 2 HP instead of 1. |
| Crown | +chance for a pure rock to be pure again when it breaks |
| Treasure Map | Chance an opened chest drops an extra item. |
| Feather | Rocks flipped by the mouse get extra critical landing chance. |
| Bell | Leveling up fills part of the chest bar. |
| Trophy | Breaking a rock adds bonus chest exp. |

### Epic
| Item | Effect |
|---|---|
| Mirror | Chance a critical landing applies the critical cash multiplier twice. |
| Photocopier | Chance a full chest bar drops two chests. |
| Anvil | Chance a dropped chest is one rarity higher (never into Unique). |
| Crystal Ball | Chance a chest opens the moment it drops. |
| Halo | Chance a breaking landing pays breaking cash twice. |
| Gemstone | Every Nth landing is guaranteed critical; landings of all rocks count toward N (N shrinks, minimum 2). |

### Unique
| Item | Effect |
|---|---|
| Snow Globe | Inactive Flipper Bots still flip, at a long interval. |
| Glove | The mouse leaves a short trail that flips the rocks it touches. |
| Paper Plane | Flipper Bots may head toward the area with the most rocks. |
| Weather Vane | Using the Preferred background gives a bonus cash multiplier until it changes. |

## Creative Guidance
- it is OK to have items with effects that are useless until something is unlocked, but preferably use them in rarer tier.
- while the rule allow rarer tier items are just a bigger boost of the less rare ones, but unique effects are always better.
- Unique items are encouraged to have effects that modify gameplay rule in some way (must not significant to create a whole new system or mechanic)
- name of an item can be completely irrelevant to its effect. (sure it can be better if they have some connection)
- name of an item should be a thing (sword, clock,...)

## Brainstorm
- auto switch to preferred background
- drop additional chest after chest bar filled up
- chests drops additional item