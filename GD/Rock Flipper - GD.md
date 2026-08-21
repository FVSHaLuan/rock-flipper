# Rock Flipper's game design document

\---

### 1\. Overview

* **Title**: Rock Flipper
* **Monetization**: one time purchase (upfront)
* **Genres**: Incremental, idle, casual, indie
* **Store**: Steam

\---

### 2\. Concept \& Fantasy

* **Core fantasy**: operating some kind of sci-fi operation/experiment that involves throwing up rocks into the air, when it drops it earns money (flipping). Along with doing that, player also builds a spaceship to "escape", it's also the game's premise and long-term goal.
* **Theme**: sci-fi, subtle, rocks/mineral/ore, casual, funny, plot twist, 2D flat art, space
* **Why incremental**: players don't need muscular/reflective/reflexive skills or strategic thinking, just play along from the start to the end of the game and watch the game's mechanics, systems, stories,...to unfold
* Emotional core: growth, evolution, revelation (plot twist), discovery, completion, relaxation

\---

### 3\. Core Gameplay Loop

* **Early game**: players have to click or hover mouse on rocks to flip them
* **Mid to end game**: the game plays itself via helpers (agents that flip the rocks without players' input), special effects that trigger the rock flipping,...
* Either via manual or auto flipping, every tens of seconds, there would be new things for players to buy (either a skill upgrade, a helper agent, a new rock count,...)
* No offline earning/progress

\---

### 4\. Systems

#### 4.1 Currencies

Only one currency, Cash ($)

#### 4.2 Rocks

* Rocks:

  * spawned around the screen
* Rock flipping:

  * rock goes up, reaches a certain height then falls down and land to a different position
  * upon landing, a rock earns some cash and loses some HP
* Rock tiers:

  * rocks have tier: I, II, III, IV,...
  * Higher tier rocks earn more cash
  * In each tier other than I, rocks have a unique ability
* Pure Rocks:

  * each rock tier has an additional version called "pure rock", pure rocks earn a multiplied cash of the normal rocks of the same tier
  * when a rock is spawned, it has a chance to be a pure rock
  * upon landing, if a rock's HP reaches 0, it'll be re-determined to be a pure or normal rock again

#### 4.3 Flipper Bots

* agents that go around, flip rocks that they touch

#### 4.4 Shop

A side bar in the right

* Buy rock count for a rock tier
* Buy helpers

#### 4.5 Skill Tree

* Node style skill tree, buy/upgrade one node lead to unlocking of other nodes (directly connected to it)
* Most of the game progression happens in here

#### 4.6 Biomes

Unlock linearly.

Each biome has a cash income multiplier (**multiplier**), with the later biome has better multiplier than the ones before it.

Each biome has a hardness multiplier (**hardness**) which defines how hard it is to get collectibles, destroy environmental elements, raise totems, get artifacts on that biome. The later biome has bigger hardness than the ones before it.

The first biome has x1 multiplier.

##### 4.6.1 Environmental Elements

Themed elements tied to a biome.

* Non-interactable: only for decoration
* Interactable: have HP, lose HP when rocks land near it. When an element's HP reaches 0, trigger some effect and disappear. New element takes some time to spawn.

##### 4.6.2 Collectibles

Upon landing, rocks have a chance to find a collectible.

Can be sold for cash.

Some tied to a biome, some can be found anywhere.

##### 4.6.3 Totems

Each biome has 1 (or more) totems that are hidden at first.

As the rocks flipping on that biome, the totems slowly raise up.

When the totems fully raised, they give the biome's ultimate boost.

##### 4.6.4 Artifacts

* Special items that give boosts

##### 4.6.5 Challenge Scroll

* Special items that give a challenge and reward when completed

#### 4.7 Prestige

\---

### 5\. Content

#### 5.1 Rock Tiers

Designer note: need to specify special ability for each tier

##### 5.1.1 Tier 0

##### 5.1.1 Tier 1

##### 5.1.1 Tier 2

##### 5.1.1 Tier 3

##### 5.1.1 Tier 4

##### 5.1.1 Tier 5

##### 5.1.1 Tier 6

#### 5.2 Helpers

#### 5.3 Biomes

##### 5.3.1 Normal Biome 0 (starter)

##### 5.3.2 Normal Biome 1

##### 5.3.3 Normal Biome 2

##### 5.3.4 Normal Biome 3

##### 5.3.5 Normal Biome 4

##### 5.3.6 Special Biome - Secret Base

It's where the spaceship is built.

##### 5.3.7 Special Biome - Void

After launching the spaceship, players land here. This serves as an ending screen.

### 6\. Progression overview

6.1 Key Milestones

* New game
* Unlock Skill Tree
* Unlock biome 1 -> 2
* Unlock biome Secret Base
* Unlock biome 3 -> 4
* Launch the spaceship -> end game

### 7\. Skills

#### 7.1 Rocks

#### 7.2 Helpers

* Max count
* Movement speed
* Charging time
* Battery capacity
* Flip interval
* Flip range
* Flip strength (how many rocks flipped at once)
* Smart targeting: random -> move to rocks -> re-evaluate target in realtime

