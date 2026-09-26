# Rock Flipper's GDD - Rocks

Rocks lay around the playfield.
The game starts with 1 P0 rock, more rocks must be purchased.

# HP
Rocks have HPs, they are integers.

# Flipping
Throw a rock from the ground up into the air. It'll always **land** in a different position than the initial position. 

On landing, a rock earns **landing cash**.

## Breaking
When a rock lands, it'll lose 1 HP. When a rock's HP reaches 0, it breaks.

On breaking, a rock earns **breaking cash**.
Breaking cash is a multiple of landing cash.

After breaking, a new rock of the same tier with full HP replaces the broken rock. The new rock has a chance to be pure.

# Purity
Pure rocks earn a multiplied cash income.

# Tiers
Rocks come with different tiers. The tiers are named: P0, P1, P2,...

[WIP: P0 are just ordinary rocks, other tiers are being developed, but they generally have better qualities, could have special abilities, and cost more to buy and upgrade]