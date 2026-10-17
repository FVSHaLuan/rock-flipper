---
name: playtest
description: Playtest Rock Flipper in the connected Unity Editor, starting Play mode from the Run scene. Drives the game through C# snippets (flip rocks, buy Shop items and skill nodes, open screens, speed up time, AutoMouse, cheats), watches state, console and screenshots, backs up and restores the save files, and reports what happened. Use when asked to playtest, play the game, try out or verify a feature or balance change in-game, reproduce a gameplay bug, or check how an early/mid run feels.
---

# Playtest Rock Flipper

You play the game through the live Editor: enter Play mode on the Run scene, act through snippets that call the same code real clicks call, look at state, logs and screenshots, then put the saves back and report.

Read [reference.md](reference.md) first. It covers what happens when Play starts from the Run scene (which slot loads, which base stats, the demo popup that pauses the game), the core loop, the screens, and what snippets can't reach.

Helpers, all run from the repo root (`D:/UP/rock-flipper`) **with the Bash tool**. In the PowerShell tool, `bash` starts WSL, not Git Bash, and fails. Use PowerShell only for `Start-Sleep`.

| Command | What it does |
|---|---|
| `bash .claude/skills/playtest/scripts/run.sh preflight` | Edit or Play mode: scene, branch, `persistentDataPath`, test build stats, save files |
| `bash .claude/skills/playtest/scripts/run.sh state` | Snapshot: run state, pause, screen stack, cash/level/chest, rocks by tier, chests, bots, Shop levels and costs, affordable skill nodes |
| `bash .claude/skills/playtest/scripts/run.sh flip COUNT=10 TARGET=any` | Mouse-flips up to N random flippable rocks/chests (`any`/`rocks`/`chests`) |
| `bash .claude/skills/playtest/scripts/run.sh click PATH="<hierarchy path suffix>" FORCE=false` | Clicks a UI button as a player would; refuses hidden/non-interactable ones unless `FORCE=true` |
| `bash .claude/skills/playtest/scripts/run.sh automouse ON=true` | Dev AutoMouse: plays the mouse continuously |
| `bash .claude/skills/playtest/scripts/run.sh timescale SCALE=5` | Dev gameplay time scale (range printed; 0.2 to 20 as of 2026-10) |
| `bash .claude/skills/playtest/scripts/shot.sh "<scratchpad>/playtest-01.png"` | Screenshot with UI, written outside the project |
| `bash .claude/skills/playtest/scripts/logs.sh <cursor> [log\|warn\|error]` | Console entries since a cursor, plus the next cursor |
| `bash .claude/skills/playtest/scripts/saves.sh backup\|restore\|fresh\|list <persistentDataPath> [backup]` | Save-file safety net |

`run.sh <file.cs>` also runs any snippet you write. Put one-off snippets in the session scratchpad, not in this folder. Snippet rules: fully qualified type names (`Agame.Run.RunEntry`, `Agame.Run.Currency.CASH`), `return` a string, and never loop or wait inside one.

## 0. Agree on the test (before touching Play mode)

The root `CLAUDE.md` says to ask before entering Play mode. A user who asked for a playtest has already said yes. If you reached this skill on your own (e.g. to verify your change), ask first: say what you want to verify and roughly how long it'll take.

Settle these, asking only for what the request leaves open:
- **Goal**: what to verify or explore, and what counts as pass/fail ("buying P1 rock count spawns a P1 rock and costs what the Shop showed").
- **Starting state**: the current save (default), a fresh save (`saves.sh fresh`, i.e. a first launch), or the current save plus cheats to reach a point (e.g. add cash, buy the skill-tree unlock).
- **Keep or discard progress**: discard by default (restore the backup at the end).
- **Organic or fast-forward**: real flips at 1x for feel and pacing questions; `timescale`, AutoMouse and cheats to get somewhere quickly. Say in the report which you used, because cheats invalidate pacing conclusions.

## 1. Preflight (Edit mode)

1. `unity status` must show the Editor `ready`. If it can't connect, see the `unity-cli` skill (Safe Mode = compile errors).
2. `run.sh preflight`, then check:
   - Active scene is `Assets/_AMainGame/Scenes/Run.unity`. If not, or if it has unsaved changes, stop and ask: opening another scene would discard or prompt about those changes.
   - `isCompiling: False`. Compile errors show up in `logs.sh 0 error`.
   - `testBuildStats` is off. If it's enabled, the run doesn't start from the real stats. Tell the user, and don't change it yourself.
   - `versionBranch`: on Demo or Playtest, expect the demo notice popup and demo limits (`VersionBranchInfo.IsPlaytestOrDemo`).
3. Back up the saves: `saves.sh backup "<persistentDataPath>"`. Keep the printed backup path for step 5. If the start is a fresh save, run `saves.sh fresh "<persistentDataPath>"` after the backup.
4. Note the console cursor: `logs.sh 0 error` prints `next cursor: N` on its last line.
5. Keep the Editor ticking while it isn't focused: `unity command set_autotick --enable true --no-banner`. If it answers "already enabled", the owner had it on; note that and leave it on when you stop (section 5).

## 2. Enter Play mode and reach combat

1. `unity command editor_play --no-banner`. The domain reload takes a few seconds, and commands can fail while it runs. Wait with PowerShell `Start-Sleep -Seconds 3` (Bash foreground sleep is blocked), then retry.
2. Poll `run.sh state` until `runState: Combat` and rocks are listed.
3. If `screenStack` ends in `Demo notice screen` (or the state says PAUSED), close it: `click PATH="Demo notice screen/Wrapper/Back Button UI" FORCE=false`. Re-run `state` and confirm it isn't paused.
4. Take a first screenshot with `shot.sh "<scratchpad>/playtest-00.png"`, then Read the PNG. **Never pass `--save_path` to `capture_game_view`**: it puts every path, even an absolute one, under `Assets/`, where Unity imports the PNG and adds a `.meta`. If that happens, delete it through the Editor (`AssetDatabase.DeleteAsset`) so the `.meta` goes too.

## 3. Play

Repeat **observe → act → let it run → observe**, keeping it tied to the goal:

- **Observe**: `state` for the numbers (its skill-node list marks nodes the player can't see yet as `hidden`), a screenshot when the look matters (layout, effects, what the player would notice), and `logs.sh <cursor> warn` after every few actions. **Any error or exception is a finding**: note what you did just before it.
- **Act**, the way a player would:
  - Flip: `flip COUNT=…`. A rock has to land and cool down (its tier's `landingCooldown`) before it can be flipped again, so flip in rounds; `flip` reports how many more could flip right now.
  - Shop: the Shop tab must be the one showing. If `state` says `(tab hidden)`, click `Side bar/Tab Buttons/Shop` first, then e.g. `click PATH="Shop/Rock Count P0/Stat Builder Button Variant"`. Other tabs: `Side bar/Tab Buttons/Background`, `Side bar/Tab Buttons/Items`.
  - Skill tree: `click PATH="Side bar/Skill tree button"`, then a node, e.g. `click PATH="Skill Tree/Main/0_g0_P0 Landing Cash"`, then close it with `click PATH="SkillTree Screen/Wrapper/Back Button UI Variant"`. **Upgrades reach rocks only when the screen closes.**
  - Continuous play: `automouse ON=true`, plus `timescale SCALE=5` to cover ground. Turn AutoMouse off before checking real-cursor behavior.
  - Cheats when the goal needs a state rather than the path to it, e.g. `unity command eval --code 'Agame.Run.RunEntry.Instance.RunData.AddCurrency(Agame.Run.Currency.CASH, 1000); return "ok";' --no-banner`. See reference.md for more. Log every cheat for the report.
  - Check guards on purpose too: clicking an unaffordable button with `FORCE=false` should report NOT CLICKABLE; with `FORCE=true` it should still do nothing harmful (no negative cash, no level past max).
- **Let it run**: `Start-Sleep -Seconds N` in PowerShell between observations (a few `run.sh` calls in a row also take a few seconds). Write down the time scale with each observation, so rates (`Cash speed`, cash per minute) are read correctly.
- **Compare against intent**: when a number or rule looks off, check the GDD ([GDD/README.md](../../../GDD/README.md), stats in `GDD/Stats/`) and the code. Report mismatches; **don't fix code during a playtest** unless the user asked for that.

Never during a playtest: save the scene (`save_scene`, `save_all`), edit prefabs or assets, or click Home / Quit / Wishlist / Feedback / Reload Scene (`click.cs` refuses them). Changes snippets make in Play mode (activating dev UIs, editing `BuildStats`) are discarded when Play stops, which is fine.

## 4. Feel and visuals

Snippets can't feel input lag or judge juice, but screenshots can catch a lot: overlapping UI, unreadable numbers, effects that never appear, rocks stuck off the playfield, a screen that didn't close. Capture before and after each notable action (`playtest-01-buy-p1.png`, …) and look at them. For anything that needs real hands (cursor feel, hover precision, gamepad), say so in the report, and suggest the user tries it.

## 5. Stop and restore

1. `automouse ON=false` and `timescale SCALE=1` (tidiness only; stopping resets both).
2. `unity command editor_stop --no-banner`. Leaving Play mode saves the run (`RunEntry.OnDestroy`), so restore **after** the stop. Then run `unity command editor_status --no-banner` to confirm Play mode has ended.
3. Unless the user wants to keep the progress: `saves.sh restore "<persistentDataPath>" "<backup path>"`, then `saves.sh list "<persistentDataPath>"` to confirm.
4. `unity command set_autotick --enable false --no-banner`, only if you turned it on during preflight.
5. Final `logs.sh <cursor> error` for anything logged on exit.

## 6. Report

Lead with the answer to the goal (pass/fail, or the main thing learned), then:
- **Setup**: branch, slot, base build stats (Default / Tutorial / test), starting state, cheats used, time scale(s).
- **What you did**: the sequence of actions, short.
- **Observations**: numbers from `state` (cash before/after, costs, counts, rates) and the screenshots that show them (give their absolute paths).
- **Problems**: errors/exceptions with the triggering action, rule mismatches against the GDD (cite the doc), and visual issues. Mark each as confirmed (reproduced) or seen once.
- **Not covered**: what you couldn't verify (real input, feel, long-session pacing) and why.
- **Saves**: restored or kept, and the backup path. Backups pile up in `.../Mini Fun Games/playtest-backups/`, so mention that they can be deleted.
