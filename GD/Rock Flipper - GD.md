# Rock Flipper's game design document

\---

### 1\. Overview

* **Title**: Rock Flipper
* **Monetization**: one time purchase (upfront)
* **Genres**: Incremental, idle, casual, indie
* **Store**: Steam

\---

### 2\. Concept \& Fantasy

* **Core fantasy**: operating some kind of sci-fi/fantasy operation/experiment that involves throwing up rocks into the air, when it drops it earns money (flipping). 
* **Theme**: sci-fi, fantasy, subtle, rocks/mineral/ore, casual, funny, plot twist, 2D flat art, space
* **Why incremental**: players don't need muscular/reflective/reflexive skills or strategic thinking, just play along from the start to the end of the game and watch the game's mechanics, systems, stories,...to unfold
* Emotional core: growth, evolution, revelation (plot twist), discovery, completion, relaxation

\---

### 3\. Core Gameplay Loop

* **Early game**: players have to click or hover mouse on rocks to flip them
* **Mid to end game**: the game plays itself via helpers (Flipper Bots) (agents that flip the rocks without players' input), special effects that trigger the rock flipping,...
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

##### 4.6.1 Environmental Elements

Themed elements tied to a biome.

* Non-interactable: only for decoration
* Interactable: have HP, lose HP when rocks land near it. When an element's HP reaches 0, trigger some effect and disappear. New element takes some time to spawn.

##### 4.6.2 Collectibles

Upon landing, rocks have a chance to find a collectible.

Can be sold for cash.

Some tied to a biome, some can be found anywhere.

##### 4.6.3 Artifacts

* Special items that give boosts

##### 4.6.4 Challenge Scrolls

* Special items that give a challenge and reward when completed

#### 4.7 Prestige

\---

#### 4.8 Monoliths

* There 6 of them floating in the center
* They can be unlocked one by one in the skill tree
* Each of them has a unique ability and many skills in the tree to enhance the ability

#### 4.9 The Rift

* A sci-fi-y/magical rift at the center

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

#### 5.2 Flipper Bots

#### 5.3 Biomes

##### 5.3.1 Normal Biome 0 (starter)

##### 5.3.2 Normal Biome 1

##### 5.3.3 Normal Biome 2

##### 5.3.4 Normal Biome 3

##### 5.3.5 Normal Biome 4

##### 5.3.6 Special Biome - Void

### 6\. Progression overview

6.1 Key Milestones

* New game
* Unlock Skill Tree
* Unlock some biomes
* Unlock some monoliths
* Unlock The Rift

### 7\. Skills

#### 7.1 Rocks

#### 7.2 Flipper Bots

* Max count
* Movement speed
* Charging time
* Battery capacity
* Flip interval
* Flip range
* Flip strength (how many rocks flipped at once)
* Smart targeting: random -> move to rocks -> re-evaluate target in realtime

