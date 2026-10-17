---
name: balance-demo
description: Balance Rock Flipper's demo (and playtest) build for pacing. Sets skill-node values in MainSkillTree.asset (buildValue, levelCount, price, attention flag) and Shop values in the Run scene, while playtesting progression runs to hit the targets (about 1 hour until nothing is left to buy, a purchase every few tens of seconds, faster early and slower late, demo stats at 20-30% of the full game). Use when asked to balance or tune the demo, fix demo pacing, set values for unbalanced or "needs attention" skill nodes, or make the demo last about an hour.
---

# Balance the demo

You tune numbers, then play the game to check the pace, and repeat. The playing is the `playtest` skill: read its [SKILL.md](../playtest/SKILL.md) and [reference.md](../playtest/reference.md) first. This skill adds the targets, the rules on what may change, and the helpers that change values.

## Targets

- **Length**: about **1 hour of gameplay** until there's nothing left to buy: every skill node and Shop upgrade the demo allows is maxed (demo-limited nodes excluded).
- **Pace**: on average, something to spend cash on **every few tens of seconds**. Mix in **major upgrades** regularly: unlocking something, or a first level of an upgrade (level 0 → 1).
- **Curve**: the early game runs **2-3x faster** than that average, and the late game can run **2-3x slower**. `pace.py` checks median gaps (gameplay seconds) of early (0-15 min) 8-20 s, mid 20-60 s, late (45 min+) 40-150 s.
- **Potential**: it's a demo, so upgrades shouldn't reach their full potential. Each stat's total gain in the demo (`buildValue × levelCount` for a node, or a Shop max) should be about **20-30% of what the full game would reach**. The graph is shared with the full game, so these values become the full game's too until the owner extends them; that's expected. When the full-game figure isn't obvious, assume one, and put it in the report so the owner can correct it.

## What may change

**Skill tree (`MainSkillTree.asset`)**, the main lever:
- Change only these fields of a `SkillGraphNode`: `buildValue`, `levelCount`, the price amount (`price.cashTier`, `a`, `b`, `c`; cost of level L → L+1 = `round(a + b × cashTierBase × c^L)`), and the **attention flag**. Never `currency`, parents, `unlockingRequirement`, `minParentLevelEach`, `demoLimit` or the build agent. `node-set.cs` only touches the allowed fields.
- **Don't touch nodes with `demoLimit >= 0`.** `node-set.cs` refuses them. Decorative nodes have nothing to balance.
- **Attention flag = not balanced yet.** A flagged node can't be bought. Once you've set its values, clear the flag (`ATTENTION=clear`) so the node can be used. Nodes still on the default price (`a 0, b 1, c 1`: 5 cash a level) are unbalanced too even without the flag; `graph.cs` marks them `PLACEHOLDER-PRICE`.
- Other nodes already carry the owner's values. Change them when the pace needs it, and list each change in the report.
- Edits to the graph **persist after Play stops**, because it's an asset. Save it once Play has stopped (`save-graph.cs`).
- **Reload the skill tree** after edits made during Play (`reload-tree.cs`, the same as the dev Reload Skill Tree button). It applies `buildValue`, `levelCount`, price and flag changes to the running game, with no re-import and no restart.
- `Skill Repo` is the parking spot for new nodes (no parents), not reachable in game.

**Run scene (Shop prices and max levels, other scene components)**:
- **Values changed in Play mode are lost when Play stops.** Every scene change made while playing goes into the balancing log (below) at once, so it can be applied after Play stops.
- Applying them for real is a scene edit: ask the user whether to apply them with `scene-set.cs MODE=apply` or to do it by hand. Never save the scene without their go-ahead.
- Re-entering Play mode starts from the saved scene, so re-apply the logged `MODE=play` changes in each new session.

**Anything else** (`BuildStats - Default`/`- Tutorial`, `GameBalance` exp curve, `CashTiers`, chest and item values): ask before changing it. If the pace can't be reached with the levers above, say which value is in the way.

## Helpers

Run from the repo root (`D:/UP/rock-flipper`) with the Bash tool, through the playtest skill's runner. `R=.claude/skills/playtest/scripts/run.sh`, `S=.claude/skills/balance-demo/scripts`:

| Command | What it does |
|---|---|
| `bash $R $S/graph.cs FILTER=all COSTS=short` | Every graph node: `EDITABLE` / `DEMO-LIMIT n` / `DECORATIVE`, plus `ATTENTION` / `PLACEHOLDER-PRICE`; agent, buildValue × levels, price, costs (first … last, total), unlock requirement, parents. In Play mode also the owned level. `FILTER=p1` narrows by name, `COSTS=all` lists every level's cost |
| `bash $R $S/node-set.cs NODE="P1 Purity" BUILD_VALUE=0.02 LEVELS=5 TIER=- A=0 B=40 C=1.6 ATTENTION=clear` | Sets one node's fields (`-` keeps one) and prints before → after and the new costs. Marks the asset dirty, doesn't save |
| `bash $R $S/reload-tree.cs` | Play mode: reloads the scene's tree from the graph and rebuilds BuildStats |
| `bash $R $S/save-graph.cs` | Edit mode: saves `MainSkillTree.asset` |
| `bash $R $S/shop.cs LEVELS=5` | Every Shop button's config components with their property paths and values; in Play mode also owned level and next costs |
| `bash $R $S/scene-set.cs GO="<path>" COMPONENT=<type> PROP=<path> VALUE=<v> MODE=play` | Sets one scene value during Play and prints a `SCENE …` line for the log. `MODE=apply` (Edit mode, user's go-ahead only) re-applies a logged line for real |
| `python $S/pace.py <log>` | Pacing report from `buy-cheapest.sh` output: every purchase with its gap, phase medians against the targets, gaps between major upgrades |

Rock-count prices (`RockCountCostConfig.prices`) are a list: one price set per max-count cap, and the level restarts at 0 after each cap increase (`MaxCountCapBreakpoints`). The caps come from the `P<n> Max Count` nodes.

## Balancing log

Before the first change, create `<scratchpad>/balance-log.md` and append to it as you go: every `node-set.cs` result (node, before → after), every `SCENE …` line, and each `pace.py` summary with its setup (fresh or continued save, time scale). It's the record for the report, and the only copy of the scene changes once Play stops.

## Workflow

### 1. Survey (Edit mode)

1. `graph.cs FILTER=all COSTS=short` and `shop.cs LEVELS=5`. List what the demo can buy: editable nodes, their parents (what unlocks what), what's still flagged or on placeholder prices, and which demo-limited nodes cut a branch short.
2. Read the GDD for what each stat does ([GDD/README.md](../../../GDD/README.md), `GDD/Stats/`). The GDD has no numbers; balance values aren't design, so don't write them into the GDD.
3. Check `versionBranch` with the playtest `preflight`: it should be Demo or Playtest, or demo limits and the demo item/background filters don't apply. If it isn't, ask before going on.

### 2. Draft on paper

Work out values before playing, from the income at each point of the run. A fresh demo save earns about $1 per landing at first; the playtest reference has a recent measured income curve. Lay the purchasable things along the hour: what should be bought around 5, 15, 30, 45 and 60 minutes, then set each node's first cost near the income the player has when it unlocks, and its growth (`c`) and `levelCount` so it stays relevant through its window without dominating. Keep gains at 20-30% of the full game.

Present the plan to the user briefly (what you'll change, what you'll leave, any assumed full-game figures) before editing, unless they asked you to just go ahead.

### 3. Apply, then measure with a progression run

1. Apply the draft with `node-set.cs` (Edit mode is fine: edits are in memory until saved). Log each one.
2. Start the playtest from a **fresh save** (`saves.sh backup`, then `saves.sh fresh`), enter Play mode, close the demo notice, and re-apply any logged `MODE=play` scene changes.
3. `automouse ON=true`, `timescale SCALE=4`, then `buy-cheapest.sh 300 400 | tee -a <scratchpad>/run-1.log`. Keep the time scale at 3-5: each poll takes a few real seconds, so a higher scale blurs purchase times. AutoMouse clicks 5 times a second, which is a busy player; mention it with the numbers.
   - It stops at the first stall (next buy over 300 s away, nothing buyable, or a failed buy). Note why, fix it or let it run, and call it again: the log keeps gameplay time across calls.
4. `python $S/pace.py <scratchpad>/run-1.log`.

### 4. Adjust while playing

- **Too fast / too slow in a phase**: raise or lower first costs (`b`, `a`) or growth (`c`) of what's bought there. **Long stall**: something cheaper should unlock there (a cheaper first level, an earlier branch) or income needs a lift from an earlier node. **Run too short / too long**: change `levelCount` on the long tracks, staying within the 20-30% potential.
- After `node-set.cs`, run `reload-tree.cs`, then go on with `buy-cheapest.sh`. A tweak only affects what's bought after it, so judge its phase on the next run.
- After big changes, measure again from a fresh save: stop Play, `saves.sh fresh`, play again (scene changes re-applied). Repeat until `pace.py` shows the three phases in range and the run ends near 60 minutes with nothing left to buy.

### 5. Stop and finish

1. Read the logs (`logs.sh`) before stopping. Then stop Play, confirm `playMode: stopped`, and restore the saves (playtest section 5).
2. `save-graph.cs`, then `graph.cs FILTER=all COSTS=short` to confirm the saved values (and that no flag you meant to clear is still set), and `git diff --stat` to check only `MainSkillTree.asset` changed.
3. Scene changes: show the logged `SCENE` lines and ask the user whether to apply them (`scene-set.cs MODE=apply`, then they save the scene) or do it by hand.
4. Don't commit unless asked.

## Report

Lead with the result: the last `pace.py` summary (length, phase medians against targets, longest gaps) and whether the targets are met. Then:
- **Graph changes**: a table of node → field before → after, flags cleared, and each changed node's total cost and max gain (with the assumed full-game figure when you used one).
- **Scene changes**: the `SCENE` lines, and whether they're applied.
- **Setup**: branch, base build stats (Tutorial vs Default; see the playtest reference), fresh save or not, time scale, AutoMouse, cheats.
- **Not reached**: phases still off target, and what's in the way (a demo-limited node, a non-graph value you didn't change, a missing feature).
- The path to `balance-log.md`.
