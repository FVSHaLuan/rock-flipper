# Rock Flipper's GDD - Combat Screen

The main screen where all the actions happen.

The screen is divided horizonally, 3/4 on the left is the **playfield**, the remaining on the right is the **side bar**.

# Playfield

This is where all the actions happens.

## Background
The bottom layer of the playfield.

Consists of a solid color texture respresenting the biome (e.g: green for grassland, brown for dessert,...)

There are a few unique background elements representing the biome (e.g: grass for grassland, cactuses for dessert,...), the elements are contributed randomly all over the playfield.

## Gameplay elements
On top of the background are gameplay elements (rocks, flipper bots, monoliths,...)

Elements' rendering orders are sorted by their y positions, near elements (near the bottom) are on top far elements (far from the bottom) to simulate perspective.

Being near or far doesn't affect an object's size.


## Overlay UI elements
UI elements are on top of everthing else on this screen.
* **Current cash**: top left of the playfield
* **Chest bar**: bottom left of the playfield, 1/5 length of the playfield's width.
* **Level bar**: bottom right of the playfield, 4/5 length of the playfield's width.

# Side bar
- **Skill Tree button**: on the top, to open **Skill Tree screen**
- **System Menu button**: on the bottom, to open **System Menu screen**
- Middle buttons: buy more rocks of each tier