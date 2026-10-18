# Translation Glossary

The fixed translations of Rock Flipper's game terms, so AI translation stays consistent without a human reviewer (plan item 4 in [CLAUDE.md](CLAUDE.md)). Built on 2026-10-18 from [GDD/Glossary.md](../GDD/Glossary.md) and the English string tables.

- **This file**: how to use the glossary, what not to translate, conventions per locale, and what each term means.
- **[Glossary.tsv](Glossary.tsv)**: the translations. One row per English term, one column per locale in the locales' sort order (`zh`, `ru`, `pt-BR`, `de`, `ja`, `ko`, `es`, `fr`, `tr`, `zh-TW`, `pl`, `uk`, `it`, `th`, `cs`, `vi`, `es-419`). The `es-419` column was added later on 2026-10-18. Same text conventions as the `translate` skill's TSV: tab-separated, `#` lines are comments, `\u200B` is a zero-width space.

## Using It
- **Use the listed term** every time the English term appears, in labels, titles and descriptions alike.
- **Inflect as the grammar requires**: case, number, gender agreement, verb form. The cell gives the base form, usually the singular noun.
- **Capitalization** follows the surrounding text and the locale's existing style, not the cell.
- **" / " in a cell** separates forms of the same term. Most often it's verb / noun (Flip, Break, Level Up). Use the one the sentence needs.
- **Compound terms not listed** (e.g. "Temporary Critical Landing Chance", "Flip Chests") are built from the listed parts.
- **A term that's missing**: pick a translation under the length rule below, add it to both files, and report it.
- **Changing a fixed translation** means every entry already using it is out of date. Only the owner decides that.
- **Length**: each translation is as close to the English term's length as an accurate option allows (see Decisions in [CLAUDE.md](CLAUDE.md)).
- **Adopted from `Common Strings`**: Cash, Tier, Level, Multiplier, Skill Points, Count, Speed and Maxed were already translated and shipped, so their rows copy those translations exactly. Skill copies the singular of the shipped Skills.

## Do Not Translate
- **Names**: the game title `Rock Flipper`; `Steam`, `Steam Deck`.
- **Tier codes** `P0`–`P5`: only the word "Rock" around them is translated (see the `P1 Rock` row).
- **Monolith names** `L II`, `L IV`, `L VI`, `L VIII`, `L X`, `L XII` (Roman numerals): only the word "Monolith" is translated (see the `Monolith L VIII` row).
- **Currency prefixes** `$` (cash) and `ST` (stars), e.g. `$1.5K`, `ST275`.
- **`HP`**: kept as `HP` in every locale except `fr`, which uses `PV` (*points de vie*).
- **Number suffixes** `K`, `M`, `B`, `T` (see Decisions in [CLAUDE.md](CLAUDE.md)).

## Conventions
- **Seconds unit** in "+{0}s Active Time" and similar: `zh`, `zh-TW` and `ja` use `秒`, `ko` `초`, `ru` and `uk` `с`, `th` `วิ`. Every other locale keeps `s`.
- **Percent sign** `%`: kept. `fr` puts a space before it (`+5 %`), `de`, `cs` and `tr` follow their usual typography (`+5 %`, `+5 %`, `+%5`).
- **Gender agreement**: the tier names Bouncy and Restless are adjectives agreeing with "Rock" (feminine in `es`, `es-419`, `pt-BR`, `fr`, `it`; masculine in `ru`, `uk`, `pl`, `cs`). The rarities agree with the noun they describe. The cells give the form for "Item" (masculine in `es`, `es-419`, `fr`, `it`, `pt-BR`, `ru`, `uk`, `pl`, `cs`); a chest takes the chest noun's gender (e.g. `pl` *zwykła skrzynia*).
- **Register**: `de` uses *du*, `es` *tú*, `fr` *vous* (as the existing translations do). `es-419` uses *tú*, and *ustedes* for the plural, never *vosotros* (set by the owner on 2026-10-18).
- **Thai**: multi-word cells already carry their `\u200B` breaks. Keep them when using the term.

### Per Locale
- **`de`**: Compounds use *-tempo* for speed (*Wurftempo*, *Bewegungstempo*); the standalone Speed label stays *Geschwindigkeit*.
- **`ru`, `uk`**: Flipper Bot is transliterated (*Флиппер-бот*, *Фліпер-бот*), while the flip itself is translated (*подбросить*, *підкинути*).
- **`fr`, `it`, `pl`, `pt-BR`**: Beam uses *faisceau*, *fascio*, *wiązka*, *feixe*, so it doesn't clash with Radius (*rayon*, *raggio*, *promień*, *raio*).
- **`zh`, `zh-TW`**: Upgrade is 强化/強化 and Level Up is 升级/升級, so the two never share a word. `zh-TW` uses Taiwan usage (滑鼠, 雷射), not a character conversion of `zh`.
- **`es-419`**: Latin American vocabulary where it differs from `es`: Mouse stays *mouse* (not *ratón*), *billetera*, *resorte*, *tenis*, *lista de deseados*, *agregar* (not *añadir*), *costo*, *disculpa*. Bounce is *rebote* (*bote* reads as "boat"), so Rebound is *multirrebote*. Every other term matches `es`. Spanish sentence case, not English title case.
- **`ja`, `ko`**: Flip-based terms use katakana and hangul loanwords (フリップ, 플립). Chance compounds in `ja` use the 〜率 suffix.

## Terms
Meanings come from the GDD and the string tables' translator notes. "Shown as" gives the in-game English when it differs from the GDD name.

### Rocks
| Term | Meaning | Notes |
|---|---|---|
| Rock | An object lying on the playfield, flipped to earn cash. | Small stones, not boulders. |
| All Rocks | Applies to rocks of every tier. | Skill title group: "All Rocks - Max HP". |
| P1 Rock | Pattern for a rock of one tier: P0 Rock to P5 Rock. | Tier code stays as is. |
| Tier | A rock category (P0, P1, P2…). | Adopted from `Common Strings`. |
| Bouncy | Name of the P1 tier. | An adjective agreeing with "Rock". Not shown in game yet. |
| Restless | Name of the P2 tier. | Same as Bouncy. |
| Shockwave | Name of the P3 tier; also its effect, which flips the rocks on the ground around it on landing. | |
| Flip | Throwing a rock from the ground up into the air: the core action. | verb / noun. Not "turn over". |
| Landing | A flipped rock coming back down. | |
| Critical Landing | A lucky landing that earns multiplied landing cash. | |
| Break | What happens to a rock or chest when its HP reaches 0. | verb / noun. "on break", "on destroyed" in descriptions. |
| HP | An integer stat of rocks and chests (hit points). | See Do Not Translate. |
| Max HP | The HP a rock or chest starts with. | |
| Pure | A pure rock earns multiplied cash. | Adjective; "Pure rock". |
| Purity | The chance for a rock to be pure. | Skill title: "P0 Rock - Purity". |
| Bounce | A P1 rock landing once more after a landing. | |
| Rebound | Ability: a bounce landing can bounce again. | Must differ from Bounce. |
| Self-flip | A P2 rock flipping itself into the air. | |
| Cache | Ability: a P2 rock flipped by anything other than a self-flip self-flips after landing. | Named after a computer's cache (the IT term). Must differ from Cash (キャッシュ, 캐시 in `ja`, `ko`). Not in the tables yet. |

### Rock Stats
| Term | Meaning | Notes |
|---|---|---|
| Landing Cash | Cash a rock earns each time it lands. | |
| Breaking Cash | Cash a rock earns when it breaks. | |
| Breaking Cash Multiplier | Sets breaking cash as a multiple of landing cash. | |
| Critical Cash Multiplier | The multiplier on landing cash on a critical landing. | |
| Critical Landing Cash | The extra cash (percent) a rock earns on a critical landing. | |
| Critical Landing Chance | The chance that a landing is a critical landing. | |
| Pure Cash Multiplier | The cash multiplier of pure rocks. | Descriptions say "Pure rock Cash Multiplier". |
| Flipping Speed | How fast a rock moves once flipped. | Speed of flight, not of the action. |
| Landing Cooldown | How long a rock waits after landing before it can be flipped again. | |
| Max Rock Count | The maximum number of rocks of a tier. | |
| Bounce Chance | The chance that a P1 rock bounces after landing. | |
| Self-flip Chance | The chance that a P2 rock self-flips once its delay is over. | |
| Self-flip Delay | How long a P2 rock stays on the ground before it may self-flip. | |
| Shockwave Chance | The chance that a P3 rock triggers a shockwave on landing. | Not in the tables yet. |
| Shockwave Radius | The radius within which a shockwave flips rocks. | Not in the tables yet. |
| Max Shockwave Flips | The most rocks one shockwave can flip. | Not in the tables yet. |
| Chance | The probability of something happening. | Used in all "… Chance" terms. |
| Radius | A circle's radius around something. | Skill title: "Mouse - Radius". |

### Flipper Bots
| Term | Meaning | Notes |
|---|---|---|
| Flipper Bot | A floating robot that flips rocks for the player. | |
| Active Time | How long a Flipper Bot stays active (moving and flipping). | GDD: active state / active duration. |
| Idle Time | How long a Flipper Bot stays idle (not moving or flipping). | GDD: inactive state / inactive duration. |
| Flip Range | The radius around a Flipper Bot within which it flips rocks and chests. | |
| Strength | The most rocks and chests a Flipper Bot flips at once. | Not physical strength. |
| Smart Pathing | Flipper Bots move to where they flip the most rocks instead of randomly. | Skill name. |
| Movement Speed | How fast Flipper Bots move. | GDD: move speed. |
| Flipping Interval | The time between two flips of a Flipper Bot. | GDD: flip interval. |
| Max Count | The maximum number of Flipper Bots (or of one rock tier). | |

### Mouse
| Term | Meaning | Notes |
|---|---|---|
| Mouse | The player's mouse cursor, used to flip rocks. | |
| Hover | Ability: flip rocks by hovering the cursor over them, without clicking. | Skill title (noun). |
| Area Flip | Ability: the mouse flips every rock within a radius at once. | Skill title; its description calls it Mouse Radius. |
| Mouse Radius | The radius of Area Flip around the cursor; also the ability's name. | |

### Monoliths
| Term | Meaning | Notes |
|---|---|---|
| Monolith | A floating structure at the center of the playfield. | |
| Monolith L VIII | Pattern for a named monolith (L II to L XII). | Name stays as is. |
| Beam | A laser beam fired by L VIII. | Must differ from Radius. |
| Laser Beam | The full form of Beam. | |
| Max Beam Count | The most beams L VIII fires each time it shoots. | |
| Min Beam Count | The fewest beams L VIII fires each time it shoots. | |
| Shooting Interval | The time between two shots of L VIII. | |
| Temporary Multiplier | A rock's cash multiplier that resets each time it's flipped. | Base for every "Temporary …" stat. |
| The Rift | A black-hole-like circle at the center of the playfield. | WIP in the GDD; not in game yet. |

### Chests and Items
| Term | Meaning | Notes |
|---|---|---|
| Chest | An object dropped when the chest bar fills, opened to drop items. | A treasure chest. |
| Chest Bar | The bar that drops a chest when full. | |
| Chest Exp | What fills the chest bar. | |
| Open | Getting a chest's contents out. | Verb. |
| Drop | A chest or item appearing ("a chest drops"). | Verb. |
| Item | Something dropped from opened chests. | The side bar tab "Items" is its plural. |
| Item Level | An item's level; its effect scales with it. | |
| Rarity | A chest or item category. | |
| Common, Uncommon, Rare, Epic, Unique | The rarities, lowest to highest. | Unique items are one of a kind. |
| Alarm Clock | Item (Common). | Not in the tables yet. |
| Battery | Item (Common). | Not in the tables yet. |
| Dice | Item (Common): a pair of dice. | |
| Hammer | Item (Common): the tool. | |
| Key | Item (Common). | Not in the tables yet. Must differ from Wrench. |
| Lantern | Item (Common): the lamp. | |
| Magnifying Glass | Item (Common). | |
| Notebook | Item (Common): a paper notebook. | Not in the tables yet. |
| Pickaxe | Item (Unique): the mining tool. | |
| Sneakers | Item (Common): sports shoes. | Not in the tables yet. |
| Soap | Item (Common): a bar of soap. | |
| Spring | Item (Common): a coiled metal spring. | Not the season. |
| Wallet | Item (Common). | |
| Wrench | Item (Common): the tool. | Not in the tables yet. |

### Backgrounds
| Term | Meaning | Notes |
|---|---|---|
| Background | The playfield's landscape, which the player unlocks and picks. | |
| BK.GR | Background, shortened to fit the side bar tab. | Also "BK" in "Next Preferred BK:". Shortened only where the full word is long. |
| Active Background | The background the player has set as active. | |
| Preferred | Marks the one background that currently multiplies cash income. | Label; changes at an interval. |
| Preferred Background | The background currently marked Preferred. | |
| Preferred Multiplier | The cash income multiplier for using the Preferred background. | |
| Preferred Interval | How often the Preferred background changes. | Not in the tables yet. |
| All Preferred | Skill: every background gets the Preferred Multiplier. | Not in the tables yet. |
| Auto Preferred | Skill: switches to each new Preferred background automatically. | Not in the tables yet. |
| Desert, Grassland, Mars, Tundra, Volcanic | Background names. | Mars is the planet. |

### Currencies and Progression
| Term | Meaning | Notes |
|---|---|---|
| Cash | The currency spent on most upgrades. | Adopted. Prefix `$`. |
| Cash Income | The cash earned over time. | |
| Star | The hard-to-earn currency, one per level-up. | Prefix `ST`. Not in the tables yet. |
| Multiplier | A factor applied to a value. | Adopted. |
| Level | The player's level, raised by filling the level bar. Also item and skill levels. | Adopted. |
| Lvl. | Short form of Level before a number ("Lvl. 3/5"). | Keep short. |
| Level Up | Gaining a level. | verb / noun. Must differ from Upgrade. |
| Level Bar | The bar that levels the player up when full. | |
| Exp | Short for experience points ("Exp: 40/100"). | Keep short. |
| Level Exp | What fills the level bar. | |
| Skill | A Skill Tree node, which unlocks or upgrades something. | Singular of the adopted Skills. |
| Skill Tree | The system of skills where most progression happens. | |
| Skill Level | A skill's level. | |
| Skill Points | The number of skill points. | Adopted. |
| Shop | The side bar tab for buying rocks and Flipper Bots and unlocking the Skill Tree. | |
| Upgrade | Spending currency to progress. | Must differ from Level Up. |
| Unlock | Making something available. | Verb; matches the shipped "Not unlocked". |
| Enable | Skill title verb for a skill that turns a feature on ("Backgrounds - Enable", "Enable Mouse Radius"). | Verb, in the form used as a skill title. |
| Count, Speed | Generic words used in stat names. | Adopted. |
| Maxed | At the maximum level. | Adopted. |
| Demo | The demo version of the game. | |
| Full Game | The paid full version, as opposed to the Demo ("More in full game"). | |
| Wishlist | Steam's wishlist ("Wishlist the full game on Steam"). | Steam's own term in each language. |

## Not Included
- **Development-only terms** that players never see: Biome, Prism, Playfield, Gameplay elements, Background elements, Overlay UI elements, Node, Parent skill, Root skill.
