# Rock Flipper — Game Design Documents

This folder holds the game design documents (GDD) for **Rock Flipper**, a 2D casual idle/incremental game for PC (Steam, one-time purchase). The task here is to write, revise and review these docs together with the owner.

Ignore the `Archived/` folder completely.

## Scope: elements and rules, not balancing

These docs define **what exists in the game and how it works**: the elements (rocks, chests, items, currencies, etc.) and the rules connecting them (e.g. "flipping a rock earns Cash; higher-tier rocks earn more").

They **don't set balancing numbers**: how much Cash a flip earns, upgrade costs, drop rates, timings, growth curves and so on. Balancing is done elsewhere. Don't propose or fill in these values; name the tunable value and leave it `TBD`.

A number belongs in the docs only when it's a structural rule rather than a tuning knob (e.g. how many slots a thing has, if that shapes the design). If it's unclear which one a number is, ask.

## Reading order

Read these first. They set the constraints every other doc must respect:
1. [Vibe.md](Vibe.md): pillars, tone, what the game is and isn't.
2. [Core loop.md](Core%20loop.md): the loop and the win condition.
3. [Metadata.md](Metadata.md): platform, genre, monetization.

After that, read only the docs the task touches, plus the docs they link to or mention.

## Folder map

- **Root**: foundation docs (vibe, core loop, metadata).
- **`Systems/`**: *rules*, meaning how a mechanic works (Rocks, Chests, Items, Currencies, Levels, Skill Tree, Mouse, Flipper Bots, Backgrounds, Monoliths, The Rift). One system per file.
- **`Contents/`**: *concrete instances* of those systems, such as the actual rock tiers, items, chests, backgrounds and achievements, plus Story. `X Content.md` pairs with `Systems/X.md`. Most of these files are still empty placeholders.
- **`Layouts/`**: screens and what sits where on them (Combat Screen = the main playfield screen, Skill Tree Screen).

A new doc goes in whichever folder matches that split. If it's unclear which folder something belongs in, ask.

## Design constraints (from Vibe.md; check every proposal against them)

- No reflexes, dexterity or strategic thinking required. The player progresses just by playing along.
- Progress should feel frequent: new upgrades and new mechanics unlock regularly.
- Over time the screen fills with things that work on their own. Early game needs interaction; late game runs itself.
- The game has an ending: it ends when there are no upgrades left to buy.
- Tone is light and casual with a little humor. The setting mixes genres, with sci-fi and modern elements most prominent.

If a proposal breaks one of these constraints, say so explicitly rather than quietly bending it.

## How to collaborate

- **The owner decides.** Existing text is the owner's decision unless it's marked as a proposal or an open question. Don't rewrite or "improve" decided content without being asked.
- **Don't fill gaps silently.** If a task needs something the docs don't define, either ask, or write it as a clearly marked proposal:
  `> **[Proposal]** ...`
  The owner accepts a proposal by removing the marker.
- **Record open questions** in the doc's `## Open questions` section instead of guessing. When a question is answered, move the answer into the body and log it under `## Decisions`.
- **Precise rules, no balance values.** Replace vague words ("some", "harder") with a precise rule: what the value depends on and in which direction it moves (e.g. "Cash per flip increases with rock tier"). Leave the actual values `TBD` (see [Scope](#scope-elements-and-rules-not-balancing)). Structural numbers you suggest must be marked as proposals.
- **Check the ripple effects.** After changing a system, search the other docs for its terms and list any docs that now conflict or need updating. Fix them only if asked.
- **Flag contradictions** between docs rather than choosing a side.
- Keep answers and edits concise. The docs are working specs, not prose.

## Doc conventions

Apply these when creating a doc or making substantial edits to one. Don't mass-reformat existing docs unless asked.

- One H1 per file, naming the topic (e.g. `# Chests`). Use `##` and lower for sections.
- Start with a short header:
  ```
  **Status**: idea | designing | settled | implemented
  **Depends on**: [Rocks](../Systems/Rocks.md), [Currencies](../Systems/Currencies.md)
  ```
- Typical sections, in order: overview, rules/body, `## Parameters` (a table naming the system's tunable values and what each controls; values stay `TBD` since they're set during balancing), `## Open questions`, `## Decisions` (one line each: what was chosen and why).
- Link to other docs with relative markdown links the first time a system is mentioned in a doc.
- Use the canonical term for each concept, consistently. Known inconsistencies to resolve with the owner: "biome" vs "background", and unqualified "exp" vs "chest exp" / "level exp" / item exp.
- Currencies are written `$500` (Cash) and `ST275` (Star).

## Relationship to the code

- The Unity project in `../Rock Flipper/` may differ from these docs. Some systems described here (e.g. Monoliths, The Rift, backgrounds) may not exist in code yet.
- Don't read the code or base design decisions on it unless the owner asks. The docs don't yet track what is implemented; treat everything as open unless its **Status** says `implemented`.

## Git

Don't commit unless asked.
