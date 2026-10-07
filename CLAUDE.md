# Rock Flipper — repo overview

This repo is **two things at once**:
1. A specific game: **Rock Flipper**, a 2D casual/idle "flipping" incremental game shipping on Steam (one-time purchase).
2. A general toolkit: most of the code here is written to be reused across the owner's *future* games, not just this one. When editing, default to asking "does this belong to Rock Flipper specifically, or to the reusable toolkit?" — see the folder-level `CLAUDE.md` files for exactly where that line falls in each area.

Two top-level folders: **`GDD/`**, the owner's game design document in Markdown (see [Game design (GDD)](#game-design-gdd)), and **`Rock Flipper/`**, the Unity project root (see [Inside `Rock Flipper/`](#inside-rock-flipper)).

## Implement or investigate? Ask when unclear

The owner often writes a feature request as a bare statement of the desired behavior, with no verb. For example, "first few rocks always spawned near the playfield's center" meant *make the first few rocks spawn near the center*. It was not a bug report asking why they do.

- If a prompt only describes behavior and doesn't say whether to **implement** it, **investigate** it (explain or diagnose current behavior), or **fix** it, ask which one before doing any work. Asking is cheap; a whole investigation in the wrong direction is not.
- Clear verbs settle it, so don't ask then. "Add", "make", "implement" and "change" mean implement. "Why", "how does", "check" and "find out" mean investigate. "X is broken" or "X shouldn't happen" mean fix.
- Reading code to understand the request is fine before asking. Don't edit files or write up a diagnosis until the intent is confirmed.

## Game design (GDD)

Use the GDD to understand what a feature is *supposed* to do and what it's called in design terms. It does not describe the code.

**Where to look.** Start at [GDD/README.md](GDD/README.md), the index of every doc. To resolve a design term the user mentions, use [GDD/Glossary.md](GDD/Glossary.md): one line per term, linking to the doc that owns it.

**What counts as design.** Only a doc's body text is the design. These sections are *not* adopted design, so don't implement from them: `## Brainstorm`, `## Parked Ideas`, `## Creative Guidance`, and `Brainstorm.md`. `## Open Questions` marks undecided rules: ask the user rather than picking an answer in code. `## Decisions` records choices and rejected alternatives: don't implement a rejected one.

**GDD vs. code: expect drift.** The code doesn't necessarily match the GDD, in naming or in rules:
- The **code** is the source of truth for current behavior. The **GDD** is the source of truth for intent.
- GDD terms often have different names in code, or no counterpart yet. Search for the concept, not only the GDD word. Known mappings: Mouse → `Combat/Player Cursor/`; Pure/Purity → `Rock.IsPure`, `RockTier.GetPurityChance()`, `purityCashMultiplier`; tier abilities (Bouncy, Restless) → `Combat/Rocks/Abilities/`; Preferred background → `RunData.PreferredBackgroundId`; Monoliths → `Combat/Monoliths/` (`Monolith`, `MonolithType`; stub only, no behavior yet).
- The reverse also holds: the code has systems the GDD doesn't cover (Shop, Prestige, extra `Currency` enum values, tutorials). Don't remove or reshape them just because the GDD is silent.
- When a task touches something where code and GDD disagree, point out the mismatch and ask which to follow. Don't silently "fix" code to match the GDD, and don't rename code identifiers to GDD terms unless asked.
- The folder `CLAUDE.md` files under `_AMainGame/` describe the code as it is. Some of their remarks about the design may be out of date; trust the GDD for design and the code for behavior.

**Keeping the GDD in sync with design changes.** When the user asks to implement a design change (a new or changed mechanic, rule, stat, system, content instance or game term), update the GDD in the same task. Read [GDD/CLAUDE.md](GDD/CLAUDE.md) first and follow it: document only what the user stated, ask about rule-level choices made during implementation instead of writing them in, and keep the glossary, stat files and content files in sync. Mention the GDD edits in the final summary. Code-only tasks (refactors, bug fixes that restore intended behavior, tooling, UI plumbing) don't touch the GDD. Otherwise, coding tasks don't edit the GDD unless the user asks.

**Editing the GDD.** Any GDD work follows [GDD/CLAUDE.md](GDD/CLAUDE.md). Its `idea-critic` subagent lives in `GDD/.claude/agents/`, so it's only available when the session is started from `GDD/`.

## Inside `Rock Flipper/`

- **`Assets/_AMainGame/`** — all of this game's specific content and scripts. See [its CLAUDE.md](Rock%20Flipper/Assets/_AMainGame/CLAUDE.md).
- **`Assets/_Exp/`** — gitignored scratch/experiment folder. Ignore for any real task; nothing here is shipped or meaningful long-term.
- **`Assets/FHC/`** — the shared in-house Unity framework (`FH.Core.Architecture.*` namespace: pooling, `WritableScriptableObject` save-data base, `MonoBehaviourWithInit`/`ScriptableObjectWithInit`, `Balancer`/`BalancerWithObjects` reference-counted lock pattern). Many `_AMainGame/Scripts` systems build directly on top of this — if you're chasing a base class like `MonoBehaviourWithInit` or `WritableScriptableObject<T>` and it's not in `_AMainGame`, look here.
- Everything else under `Assets/` (Epic Toon FX, EnhancedScroller v2, xNode, TextMesh Pro, Steamworks.NET package, Controller Icons Pack, Shaper2D, CommandTerminal, etc.) is **third-party/vendored** — don't expect project-specific documentation for these; treat them as black-box dependencies unless you find evidence they've been modified in place.
- `Library/`, `Temp/`, `Logs/`, `obj/`, `.vs/`, `UserSettings/` — Unity/IDE-generated, not source.

## Committing changes

- Before committing, check every newly created asset file (scripts, prefabs, scenes, assets, folders, etc. under `Rock Flipper/Assets/`) for a matching `.meta` file. Unity only generates `.meta` files when the Editor has focus and detects the new file on disk — if a file was created (e.g. by Claude) while Unity wasn't focused/running, its `.meta` may not exist yet.
- If a `.meta` file is missing for a new asset, do not commit the asset without it: bring Unity Editor focus to the project (or use the `unity-cli` skill) so Unity generates the `.meta`, then verify it exists before staging/committing. Committing an asset without its `.meta` breaks Unity's GUID references for anyone else who pulls the change.
- This check applies to new files only — modified/deleted existing files already have (or correctly lack) `.meta` files.

## Tooling

- Use the `unity-cli` skill for any interaction with the Unity Editor or this Unity project: inspecting/editing the scene hierarchy, creating or modifying GameObjects, editing prefabs/assets, running C# in a live connected Editor, or building/testing the project. Prefer it over hand-editing `.unity`/`.prefab`/`.asset` YAML files directly whenever a live or CLI-driven Editor operation can do the job.

## Conventions that apply project-wide

- Base-class chain for almost every gameplay/UI script: `MonoBehaviourWithInit` (FHC) → `ExtendedMonoBehaviour` (`Scripts/Common`) → scene-scoped subclass (`ExtendedMonoBehaviourRun`, `ExtendedMonoBehaviourHome`) → concrete class. See [Scripts/Common/CLAUDE.md](Rock%20Flipper/Assets/_AMainGame/Scripts/Common/CLAUDE.md) for the exact lazy-init mechanics.
- Service-locator style, not DI: code reaches other systems through `Entry.Instance`/`entry` (app-wide) or `CommonEntry.CommonInstance` (per-scene common UI), not `FindObjectOfType` or a DI container.
- Reference-counted lock pattern (`BalancerWithObjects`, `Add<X>Lock(obj)`/`Remove<X>Lock(obj)`) recurs everywhere state needs multiple independent holders (pause, input-block, save-lock, screenshot mode) — always pair Add/Remove with the same object reference.
- `#if UNITY_EDITOR`, `#if !DISABLESTEAMWORKS`, and feature-branch defines (`BSB_VER_DEMO`, `BSB_VER_PLAYTEST`, `BSB_F2P`) gate large chunks of code throughout. See `Scripts/FeatureBranching/CLAUDE.md`.
