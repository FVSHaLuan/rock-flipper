# _AMainGame

All of Rock Flipper's own content lives here. See the root [CLAUDE.md](../../../CLAUDE.md) for the repo-wide picture and [GD/Rock Flipper - GDD.md](../../../GD/Rock%20Flipper%20-%20GDD.md) for the design doc.

## Game in one paragraph

Idle/incremental: rocks spawn on screen; the player clicks/hovers to "flip" them (early game) or autonomous **Flipper Bots** do it (mid/late game). A rock goes up, falls, lands, earns Cash, loses HP. Rocks have **Tiers** and a rarer **Pure Rock** variant (multiplied cash). Progression is via a node-based **Skill Tree** and a **Shop** (buy rock counts/upgrades). Design doc also describes Biomes, Monoliths, The Rift, and Prestige as major systems — **check [Scripts/Run/CLAUDE.md](Scripts/Run/CLAUDE.md)'s maturity section before assuming any of these exist in code; most don't yet.**

## Folders

| Folder | Contents |
|---|---|
| `Scripts/` | All code. See [Scripts/CLAUDE.md](Scripts/CLAUDE.md) for the full map — most subfolders are reusable systems, `Scripts/Run` is gameplay-specific, `Scripts/Home` is mostly-specific. |
| `Scenes/` | Unity scenes (Home, Run, etc.) |
| `Data/` | ScriptableObject asset instances (configs, balance data, dev config assets like `Data/Dev/DevEntry.asset`) |
| `Prefabs/` | Prefabs |
| `Resources/` | Resources-loaded assets, incl. the `Entry` prefab (`GameConst.EntryResourcePath`, currently `"OV Entry"`) |
| `Images/`, `Materials/`, `Shaders/`, `Fonts/`, `Sounds/`, `Animation/`, `Render Textures/`, `Physics Materials/` | Standard asset content folders |
| `InputActions/` | Unity Input System action assets |
| `Texts/` | Text content/localization-adjacent data |

## Where to start for common tasks

- **New gameplay feature / rock mechanic / skill tree node / balance change** → [Scripts/Run/CLAUDE.md](Scripts/Run/CLAUDE.md) — this is the single most important doc for gameplay work.
- **Home screen / save-slot flow** → [Scripts/Home/CLAUDE.md](Scripts/Home/CLAUDE.md)
- **New UI screen/menu/tooltip/gamepad prompt** → [Scripts/UI Systems/CLAUDE.md](Scripts/UI%20Systems/CLAUDE.md)
- **Looking for an existing utility (math, random, formatting, pooling, animation helper)** before writing a new one → [Scripts/Helpers/CLAUDE.md](Scripts/Helpers/CLAUDE.md)
- **Save data / persistence** → [Scripts/PlayerData/CLAUDE.md](Scripts/PlayerData/CLAUDE.md) (cross-run) and `Scripts/Run/RunData/` (per-run, documented in Run's CLAUDE.md)
- **Pause / slow-motion** → [Scripts/TimeScaleManager/CLAUDE.md](Scripts/TimeScaleManager/CLAUDE.md)
- **Ads / IAP / premium gating** → [Scripts/F2P/CLAUDE.md](Scripts/F2P/CLAUDE.md)
- **Demo/playtest/platform-specific behavior** → [Scripts/FeatureBranching/CLAUDE.md](Scripts/FeatureBranching/CLAUDE.md)
- **`Entry`/`ExtendedMonoBehaviour`/app-wide wiring** → [Scripts/Common/CLAUDE.md](Scripts/Common/CLAUDE.md)
- **Dev tools / terminal commands / cheats** → [Scripts/Dev/CLAUDE.md](Scripts/Dev/CLAUDE.md)
