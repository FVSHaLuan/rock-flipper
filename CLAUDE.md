# Rock Flipper — repo overview

This repo is **two things at once**:
1. A specific game: **Rock Flipper**, a 2D casual/idle "flipping" incremental game shipping on Steam (one-time purchase).
2. A general toolkit: most of the code here is written to be reused across the owner's *future* games, not just this one. When editing, default to asking "does this belong to Rock Flipper specifically, or to the reusable toolkit?" — see the folder-level `CLAUDE.md` files for exactly where that line falls in each area.

## Layout

- **`GD/`** — the game design document: [Rock Flipper - GDD.md](GD/Rock%20Flipper%20-%20GDD.md). Read this for game concept/systems context before working on gameplay. Note: the doc describes several systems (Biomes, Monoliths, The Rift, per-tier unique abilities) that are **not yet implemented in code** — don't assume a design-doc concept exists until you've checked.
- **`Rock Flipper/`** — the Unity project itself (Unity project root: `.sln`, `Assets/`, `ProjectSettings/`, `Packages/`, etc.).

## Inside `Rock Flipper/`

- **`Assets/_AMainGame/`** — all of this game's specific content and scripts. See [Rock Flipper/Assets/_AMainGame/CLAUDE.md](Rock%20Flipper/Assets/_AMainGame/CLAUDE.md).
- **`Assets/_Exp/`** — gitignored scratch/experiment folder. Ignore for any real task; nothing here is shipped or meaningful long-term.
- **`Assets/FHC/`** — the shared in-house Unity framework (`FH.Core.Architecture.*` namespace: pooling, `WritableScriptableObject` save-data base, `MonoBehaviourWithInit`/`ScriptableObjectWithInit`, `Balancer`/`BalancerWithObjects` reference-counted lock pattern). Many `_AMainGame/Scripts` systems build directly on top of this — if you're chasing a base class like `MonoBehaviourWithInit` or `WritableScriptableObject<T>` and it's not in `_AMainGame`, look here.
- Everything else under `Assets/` (Epic Toon FX, EnhancedScroller v2, xNode, TextMesh Pro, Steamworks.NET package, Controller Icons Pack, Shaper2D, CommandTerminal, etc.) is **third-party/vendored** — don't expect project-specific documentation for these; treat them as black-box dependencies unless you find evidence they've been modified in place.
- `Library/`, `Temp/`, `Logs/`, `obj/`, `.vs/`, `UserSettings/` — Unity/IDE-generated, not source.

## Conventions that apply project-wide

- Base-class chain for almost every gameplay/UI script: `MonoBehaviourWithInit` (FHC) → `ExtendedMonoBehaviour` (`Scripts/Common`) → scene-scoped subclass (`ExtendedMonoBehaviourRun`, `ExtendedMonoBehaviourHome`) → concrete class. See [Scripts/Common/CLAUDE.md](Rock%20Flipper/Assets/_AMainGame/Scripts/Common/CLAUDE.md) for the exact lazy-init mechanics.
- Service-locator style, not DI: code reaches other systems through `Entry.Instance`/`entry` (app-wide) or `CommonEntry.CommonInstance` (per-scene common UI), not `FindObjectOfType` or a DI container.
- Reference-counted lock pattern (`BalancerWithObjects`, `Add<X>Lock(obj)`/`Remove<X>Lock(obj)`) recurs everywhere state needs multiple independent holders (pause, input-block, save-lock, screenshot mode) — always pair Add/Remove with the same object reference.
- `#if UNITY_EDITOR`, `#if !DISABLESTEAMWORKS`, and feature-branch defines (`BSB_VER_DEMO`, `BSB_VER_PLAYTEST`, `BSB_F2P`) gate large chunks of code throughout. See `Scripts/FeatureBranching/CLAUDE.md`.
