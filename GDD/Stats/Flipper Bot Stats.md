# Flipper Bot Stats

All stats of [Flipper Bots](../Systems/FlipperBots.md).

| Stat | Scope | Upgrade Direction |
|---|---|---|
| Inactive duration | Global | _(-)_ |
| Active duration | Global | _(+)_ |
| Flip interval | Global | _(-)_ |
| Flip range | Global | _(+)_ |
| Max count | Global | _(+)_ |
| Move speed | Global | _(+)_ |

## Parked Ideas

### Movement
| Stat | Scope | Upgrade Direction | Idea |
|---|---|---|---|
| Rock seeking | _Global_ | _(\*)_ | Bots move toward rocks instead of randomly. |
| Chest seeking | _Global_ | _(\*)_ | Bots prioritize moving toward [chests](../Systems/Chests.md). |

### Flip Output
| Stat | Scope | Upgrade Direction | Idea |
|---|---|---|---|
| Double flip chance | _Global_ | _(+)_ | Chance for a flip to be performed twice. |
| Bot cash multiplier | _Global_ | _(+)_ | Multiplies landing cash of rocks flipped by bots. |
| Bot critical landing chance | _Global_ | _(+)_ | Extra [critical landing chance](../Systems/Rocks.md#critical-landing) for landings from bot flips. |
| Bot chest exp multiplier | _Global_ | _(+)_ | Multiplies chest exp added by landings from bot flips. |

### Inactive / Active Cycle
| Stat | Scope | Upgrade Direction | Idea |
|---|---|---|---|
| Always Active | _Global_ | _(\*)_ | Removes the Inactive state; bots stay Active permanently. |
| Wake on hover | _Global_ | _(\*)_ | Hovering the [mouse](../Systems/Mouse.md) over an Inactive bot switches it to Active. |
| Overdrive chance | _Global_ | _(+)_ | Chance that a bot turning Active gets a shorter flip interval for that Active period. |
