# GDD Review — AI Collaboration Readiness

I read everything in `GD/` except `_Archived.md`. Short version: the ideas are clear and the one-topic-per-file layout is a good start. The weak spot is that almost none of the context an AI needs to extend the design without guessing is written down.

## High impact

**1. [Claude.md](Claude.md) is nearly empty.** It says "develop GDDs, ignore archive" and nothing else. Every session starts cold, so an AI has to rediscover the folder layout, what each folder is for, and how you want to work together. Things it doesn't cover:
- The difference between `Systems/` (rules) and `Contents/` (specific instances?). I'm guessing at this split.
- Whether the AI should edit files directly or propose changes first, and whether it should ask before filling gaps or fill them and flag them.
- How to mark AI-proposed text so it's distinct from decisions you've made.
- A reading order, or which docs are the foundation (Vibe and Core loop, presumably).

**2. The root [CLAUDE.md](../CLAUDE.md) points to a file that doesn't exist.** It links `GD/Rock Flipper - GDD.md`. An AI that follows the link finds nothing and may conclude the docs are missing.

**3. No numbers anywhere.** In an incremental game the economy *is* the design, but every quantity is vague: "some chest exp", "after some time", "harder to fill again", "a multiple of landing cash", "a chance to be pure". An AI can't check pacing, balance costs, or notice that one currency becomes worthless without at least placeholder formulas. Useful additions:
- A target playtime. For a game that ends when the upgrades run out, this is probably the most important missing number.
- Parameter tables per system, with `TBD` cells where you haven't decided. A `TBD` tells the AI the value is open; a vague word doesn't.

**4. Open questions are hidden, not listed.** `[WIP]` is the only marker. Here are gaps I hit on one read, where an AI would silently make something up:
- [Chests.md](Systems/Chests.md): "at least one of them has the same rarity" — the sentence is cut off. The same rarity as the chest?
- Who flips chests: the mouse, bots, or both? Do chests give level exp?
- [Backgrounds.md](Systems/Backgrounds.md): how is the Preferred background chosen? Randomly, on a timer? Does it rotate?
- [Mouse.md](Systems/Mouse.md): once hover is unlocked, does clicking still work?
- Rocks: [Combat Screen.md](Layouts/Combat%20Screen.md) has side-bar "buy rock" buttons, but [Skill Tree.md](Systems/Skill%20Tree.md) says most progress goes through the tree. Which purchases go where?
- [Core loop.md](Core%20loop.md) says the game ends when no upgrades are left. Item levels come from random drops. Are items part of "completion"?
- Monoliths and the Rift are both "at the center of the playfield". How do they share the space?

An `## Open questions` section in each doc would let an AI (or you) work through gaps on purpose instead of papering over them.

**5. There's no record of why decisions were made.** Every commit message is "Updated", and the docs record *what* but never *why*. The AI can't tell a firm decision from a first draft, so it will second-guess settled choices or quietly undo them. A short `Decisions` section per doc ("Chose X over Y because Z") fixes this cheaply.

**6. It isn't clear what's already built.** The root CLAUDE.md says the game is shipping and that the design docs are aspirational. When revising the GDD, the AI doesn't know which parts are already in code and expensive to change, and which are open. A per-doc `Status: implemented / designed / idea` line would fix that.

## Medium impact

**7. Terminology is inconsistent and there's no glossary.**
- "Biome" in Combat Screen vs. "background" in Backgrounds.
- A "Combat Screen" in a game with no combat.
- "exp" vs. "chest exp" vs. "level exp", and "ground rocks".
- "Float" in the art style in [Metadata.md](Metadata.md). Flat? Floaty? It could be read either way, and it affects every visual suggestion.

A `Glossary.md` with one line per term would stop drift, and would stop an AI inventing a third name for the same thing.

**8. Almost nothing is cross-linked.** There are only two links in the whole set. Skill Tree, Star, Items, backgrounds and biomes are all mentioned without links, so an AI working on one doc won't pull in the ones it depends on. When a system changes, it can't find what else is affected.

**9. There are nine empty files in `Contents/`.** Zero bytes, no template. An AI can't tell whether they're placeholders, what format they should follow (a table? one entry per section?), or how they relate to their `Systems/` counterparts. One template, for example a rock tier table with name, HP, landing cash, cost and special ability, would make filling them mostly mechanical.

**10. Design pillars aren't written as checkable rules.** [Vibe.md](Vibe.md) has strong constraints buried in prose. "Players don't need strategic thinking or reflexes" could veto half of all proposals. A numbered pillar list lets you tell the AI "check this proposal against the pillars".

**11. Missing context an AI would lean on heavily:**
- Comparable games ("like X but Y").
- Team size and scope limits (solo dev? how many rock tiers can realistically get art?).
- Who the target player is.

## Low impact

- Heading structure: every file starts with a repeated "Rock Flipper's GDD - X" H1 and then uses more H1s for its sections. The file name already says what it is; use one H1 and `##` for sections.
- Typos that change meaning: "contributed randomly" (should be "distributed") and "dessert" (should be "desert"). The rest ("everythings", "horizonally") are harmless.
- `Claude.md` vs `CLAUDE.md`: it works on Windows, but it would break on a case-sensitive checkout.

## Where to start

In order of value for the effort:
1. Expand the GD `CLAUDE.md` (layout, workflow rules, reading order) and fix the broken root link.
2. Add a glossary and a pillars list.
3. Add a small standard header to each doc (Status, Depends on) plus `Open questions` and `Decisions` sections.
4. Add target playtime and parameter tables with `TBD`s.
5. Add a template for the `Contents/` files.
