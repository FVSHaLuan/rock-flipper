# Rock Flipper's GDD - Flipper Bots

Floating robots, move randomly around the playfield and flip rocks.

# Idle state
Still floating up and down but not moving and not flipping. 
Will switch to Active state after some time.

# Active state
Moves randomly, continuously does flipping every time interval. When performs a flip, ground rocks in range will be flipped.
Will switch to Idle state after some time.