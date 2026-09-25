# Rock Flipper — repo overview

This repo is **two things at once**:
1. A specific game: **Rock Flipper**, a 2D casual/idle "flipping" incremental game shipping on Steam (one-time purchase).
2. A general toolkit: most of the code here is written to be reused across the owner's *future* games, not just this one. When editing, default to asking "does this belong to Rock Flipper specifically, or to the reusable toolkit?" — see the folder-level `CLAUDE.md` files for exactly where that line falls in each area.

## Layout

- **`GD/`** — game design documents for Rock Flipper (e.g. [Rock Flipper - GDD.md](GD/Rock%20Flipper%20-%20GDD.md)). See [Game design documents](#game-design-documents-gd) below for when to load them.
- **`Rock Flipper/`** — the Unity project itself (Unity project root: `.sln`, `Assets/`, `ProjectSettings/`, `Packages/`, etc.).

## Game design documents (`GD/`)

- **Only load `GD/` files when the task is developing the design documents themselves** (writing, revising, or reviewing the GDD or other design docs).
- **When working on code, don't read `GD/` and don't let it shape your decisions** unless the user explicitly tells you to use it. The code and the user's instructions are the source of truth for implementation. The design docs describe planned or aspirational systems (e.g. Biomes, Monoliths, The Rift, per-tier unique abilities) that may not exist in code or may differ from it.

## Inside `Rock Flipper/`

- **`Assets/_AMainGame/`** — all of this game's specific content and scripts. See [Rock Flipper/Assets/_AMainGame/CLAUDE.md](Rock%20Flipper/Assets/_AMainGame/CLAUDE.md).
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
