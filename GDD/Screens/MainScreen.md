# Main Screen

The main screen is split horizontally into the **playfield** (left 3/4) and the **side bar** (right 1/4).

```
+-------------------------------------------+-----------+
| Current cash                              | Skill Tree|
|                                           |-----------|
|                                           | Buy rock  |
|               PLAYFIELD (3/4)             | (per tier)|
|                                           | Buy       |
|                                           | Flipper   |
|                                           | Bot       |
|                                           |-----------|
| [Chest bar 1/5][      Level bar 4/5      ]| System    |
|                                           | Menu      |
+-------------------------------------------+-----------+
```

## Playfield
All the gameplay happens here. It has three layers, from bottom to top: background, gameplay elements, overlay UI elements. [The Rift](../Systems/TheRift.md) is drawn between the background and the gameplay elements.

### Background
* A solid color texture representing the current **biome** (e.g. green for grassland, brown for desert,...).
* A few unique **background elements** representing the biome (e.g. grass for grassland, cactuses for desert,...), scattered randomly all over the playfield.
* Background and biome are not the same concept but are closely tied. Biome is a development-only term used in the GDD; players only know about backgrounds.
* The background affects gameplay (see [Backgrounds](../Systems/Backgrounds.md)).

### Gameplay Elements
Rocks, Flipper Bots, [monoliths](../Systems/Monoliths.md),...
* Render order is sorted by y position: elements nearer the bottom of the screen ("near") are drawn on top of elements higher up ("far"), to simulate perspective.
* No size scaling: being near or far does not change an element's size.

### Overlay UI Elements
Drawn on top of everything else on the playfield.

| Element | Position | Size |
|---|---|---|
| Current cash | Top left | — |
| [Chest bar](../Systems/Chests.md#chest-bar) | Bottom left | 1/5 of the playfield's width |
| [Level bar](../Systems/Levels.md#level-bar) | Bottom right | 4/5 of the playfield's width |

The Chest bar and Level bar together span the full width of the playfield's bottom edge.

## Side Bar
A vertical column of buttons.

| Position | Button | Action |
|---|---|---|
| Top | Skill Tree | Opens the [Skill Tree](../Systems/SkillTree.md) screen |
| Middle | Buy Rock (one per rock tier) | Buys one more rock of that tier |
| Middle | Buy Flipper Bot | Buys one more Flipper Bot |
| Bottom | System Menu | Opens the **System Menu** screen |

## Open Questions
* Where do newly bought rocks / Flipper Bots appear on the playfield?
