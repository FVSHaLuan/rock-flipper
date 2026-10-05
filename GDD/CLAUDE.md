# Game design document for the game Rock Flipper.

## Overview
You, the AI, help me document the game, also propose ideas when I ask to.

## Documenting vs. Proposing
When I ask you to document something, write down only what I said. Rewording, fixing typos and restructuring for readability are fine, but do not add new facts, rules or assumptions of your own (e.g. "the background is purely decorative").
- If something is unclear, missing or worth deciding, put it under `## Open Questions` instead of guessing.
- Only add your own ideas when I ask you to propose them.

When do tasks required inventing new things, propose in the chat first before making any changes.

## Relation to the Code
The GDD lives in the game's repo, at `GDD/`, next to the Unity project (`Rock Flipper/`). It describes the intended design. The code doesn't necessarily match it in naming or rules.
- The code is not a source for the GDD. Don't document code behavior or code names into the GDD unless I say so. If you notice a mismatch, mention it in chat; don't edit the GDD to "fix" it.
- When I ask to implement a design change in code, update the GDD in the same task, following every rule in this file:
  - The change I asked for is my instruction to replace the affected body text. Everything else in the body text stays protected by [Body Text](#body-text).
  - Document only what I stated. If implementing it required a rule-level choice I didn't state, don't write it into the GDD. List it in chat and ask whether to add it to the design or to `## Open Questions`.
  - Keep the glossary, stat files and content files in sync, as for any other GDD edit.

### Creative Tasks
For any creative task (proposing ideas, inventing content, brainstorming, naming, etc.), **use ALL the info available in the GDD**, not just the doc being worked on:
- Body text of every related doc, plus the glossary, so ideas fit the existing design.
- `Brainstorm.md` and every `## Brainstorm` section.
- Every `## Parked Ideas` section: reuse, combine or build on them, and point out when a proposal relates to a parked idea.
- Every `## Creative Guidance` section: follow the guidance of the system(s) the task concerns; use other systems' guidance as extra direction.
- `## Open Questions` and `## Decisions`: proposals may answer open questions, and must not contradict decisions (including rejected alternatives).

Brainstorm, Parked Ideas and Creative Guidance are inspiration only; they still don't become part of the design until I adopt them.

#### Mandatory Critique
**Every non-trivial proposal must be critiqued by the `idea-critic` subagent before I see it.**
1. Draft the proposals.
2. Run `idea-critic`, passing the full text of the proposals and the task they answer.
3. Revise or drop proposals based on the critique. If you revise substantially, run the critic again on the revised version.
4. Present the final proposals to me, with a short note per proposal on what the critic flagged and how you addressed it (or why you disagreed). Don't silently hide dropped ideas; list them in one line each with the reason.

Never present a non-trivial proposal that skipped this step. If the subagent can't be run, tell me instead of presenting un-critiqued proposals.

**Trivial proposals skip the critique.** A proposal is trivial only if it adds no new rule, mechanic, stat or system and changes no existing one. For example:
- Naming or rewording (a term, a content piece, a section title).
- A small tweak to a proposal I already reviewed, when the tweak doesn't touch a rule.
- Answering a yes/no or either/or question I asked, using only what's already in the GDD.
- I explicitly say "no critique", "quick" or similar.

When you skip, say so in one line (e.g. "Skipped critique: naming only"), so I can ask for one. When in doubt, run the critic.

## File Structure
Create new files, organize in folders,...how you see fit so that both I can read easily, and you can perform future tasks efficiently and well. Here are some tips: including `## Open Question`, `## Decisions` and `## Parked Ideas` to keep track of state of the GDD (see [Writing Style](#writing-style) for what goes in each).

As the GDD grows, at time you feel the need to re-structure, do it, but make sure no information losses.

### Body Text
**Body text** is all content of a GDD doc outside its `## Decisions`, `## Open Questions`, `## Parked Ideas`, `## Creative Guidance` and `## Brainstorm` sections: the facts, rules, examples, tables and diagrams that describe the game.
- When restructuring or applying any rule in this file, you may move, reword, merge and reformat body text, but **never remove or weaken any of its content** (facts, details, examples, wording that carries meaning), even if it violates a rule here or seems redundant / derivable.
- If you think body text content should be removed, always ask me explicitly first, listing each piece, and wait for my answer.

## Writing Style
Keep the docs concise:
- **Every system doc starts with `## Purpose`.** It is the first section, right after the one-line intro, and tells how the system contributes to the player's experience of the game.
- **Every system doc has all common sections, even when empty.** After `## Purpose` and the body sections, it always ends with, in this order: `## Decisions`, `## Open Questions`, `## Parked Ideas`, `## Creative Guidance`, `## Brainstorm`. An empty section is just its header; never omit or merge it away.
- **Decisions record choices, not a body recap.** Only list a Decision when it captures something the body doesn't make obvious: a rejected alternative, a naming choice, or a scope boundary (e.g. "Item terms are item level and item exp, to keep them distinct from the player's level").
- **Each open question has one owning doc.** Put it in the doc that owns the concept; other docs link to it or say nothing. Merge questions that share the same topic across docs into one bullet (e.g. all "do rock rules also apply to chests?" questions live in Chests).
- **Parked Ideas hold ideas I haven't decided to use.** They are not part of the design yet, so other docs and the glossary don't rely on them.
  - Before referencing a term from another doc, confirm it appears in that doc's body text, not in Parked Ideas, Brainstorm or Creative Guidance. Glossary terms are safe, since parked ideas never enter the glossary.
  - Only add an idea when I ask you to park it; never park your own proposals on your own.
  - Each idea is one line (or one table row) max.
  - Sub-headers inside the section are allowed to organize the ideas when needed.
  - Put it in the doc that owns the concept, like an open question.
  - When I adopt an idea, move it into the body text and remove it from Parked Ideas. Don't drop a parked idea for any other reason without asking me first.
- **Stats live in the `Stats/` folder.** Every system with stats has its own stat file (e.g. `Systems/Rocks.md` → `Stats/Rock Stats.md`) listing all its stats in a table with columns `Stat | Scope | Upgrade Direction` (scope: global, per tier, per rarity, per individual,...; upgrade direction: `(+)` Increase, `(-)` Decrease or `(*)` Unlock, written as the symbol only). The system doc links to its stat file instead of holding stat tables.
  - **Infer Scope and Upgrade Direction.** As an exception to [Documenting vs. Proposing](#documenting-vs-proposing), fill these cells with your best inference from the docs and the stat's nature (e.g. a stat that makes the game easier or more rewarding as it grows → (+); a stat defined per rock tier → per tier). Write inferred values in italic (e.g. `_(+)_`, `_Global_`; escape Unlock as `_(\*)_`) so I can tell them apart from values I stated; once I confirm one, write it in plain text.
  - Write `?` only when you can't reasonably infer a cell; a `?` cell doesn't need its own Open Question.
- **Content lives in the `Content/` folder.** A system with distinct instances (e.g. rock tiers, monoliths, items, backgrounds) has its own content file (e.g. `Systems/Rocks.md` → `Content/Rock Tiers.md`) listing them. The system doc keeps the shared rules and links to its content file.
  - Only list an instance for what makes it differ in rules (e.g. a unique ability). Instances that differ only in numbers or presentation get no entry.
  - Open questions about a specific instance go in the content file. Questions about the system as a whole stay in the system doc.
  - Create a content file only once a system has instance-specific content.
- **Bold only marks a definition.** Bold a glossary term once, where it's defined in its owning doc. Don't bold anything else.
- **Link a term once per doc**, on its first mention.
- **Minimal scaffolding.**
  - The intro is one line and isn't repeated by the sections below it.
  - No `---` separators between sections.
  - Merge single-bullet sections into a neighboring section, unless the glossary links to that section's anchor or it is a required common section.

## Brainstorm
`Brainstorm.md` is my unorganized idea dump. A GDD doc may also end with a `## Brainstorm` section (always present in system docs): the same kind of idea dump, but only for that doc's topic. Both follow these rules:
- Read all of them as inspiration for every creative task (see [Creative Tasks](#creative-tasks)).
- They are not part of the design: other docs and the glossary never rely on them, and the Writing Style, Glossary and Scope rules don't apply to them.
- Don't edit, organize or clean them up unless I ask. When restructuring a doc, keep its `## Brainstorm` section as-is and as the last section of the file.

## Creative Guidance
Every system doc has a `## Creative Guidance` section, placed right before its `## Brainstorm` section (or last, if the doc has no Brainstorm). It holds my guidance on how to be creative within that system: the "how" and the rules to follow when inventing things for it. Unlike Brainstorm, which can hold anything (e.g. a specific raw idea), Creative Guidance holds direction and rules, not concrete ideas.
- Read it for every creative task (see [Creative Tasks](#creative-tasks)); skip it for non-creative tasks.
- Like Brainstorm, it is not part of the design: other docs and the glossary never rely on it, and the Writing Style, Glossary and Scope rules don't apply to it.
- Don't edit, organize or clean it up unless I ask. When restructuring a doc, keep its `## Creative Guidance` section as-is and in its place.

## Glossary
`Glossary.md` lists every game-specific term. Keep it in sync with these rules:
- **One line per term + a link to the doc that owns it.** The owning doc is the source of truth; a glossary entry never contains details that aren't in the owning doc.
- **No guessed definitions.** If a term is only mentioned and has no owning doc yet, say only what the docs state and mark it *Not yet documented*.
- **Update in the same change.** Whenever a term is added, renamed, removed, or gets its own doc, update the glossary (entry and link) in the same edit.
- **Use glossary terms exactly** in all docs; don't introduce synonyms. If I use a different word for an existing term, ask whether it's a rename or a new concept.
- **Entries define, they don't explain.** One clause saying what the term *is*; rules and details stay in the owning doc behind the link.

## Git History
Do not look at git commit history (e.g. `git log`, `git show`, `git blame`, `git diff` against past commits) of GDD files. The current files are the single source of truth for the GDD. This rule doesn't restrict using git for code work in the rest of the repo.

## Scope
This GDD focuses on the concepts and rules, so
- Do not write exact number values if it doesn't matter to the concepts and rules. For example:
  - "This upgrade decreases Max HP", "This upgrade increases critical landing chance" are enough and correct, do not specify how much it decreases or increases.
  - "When HP reaches 0, the rock breaks and earn a multiplied income", the exact number here is needed because it's part of the rule.
- Do not propose or ask about sound, and graphic or anything else that doesn't matter to the concepts and rules.
- Do not open questions about content quantity, e.g. "How many backgrounds/items are there?". How much content the game has doesn't affect the concepts or rules.
- General test before documenting anything or adding an Open Question: **would the answer change how a concept or rule works?** If not (content amount, exact tuning values, naming of individual content pieces, presentation, etc.), leave it out.
- This test also applies to what's already written: when editing a doc, remove existing Open Questions that fail it. For [body text](#body-text) that fails it, ask me first.
