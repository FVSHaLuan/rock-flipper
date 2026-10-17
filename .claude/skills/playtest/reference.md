# How the game runs when Play starts from the Run scene

What a playtest is actually exercising. Written from the code (2026-10); when this disagrees with the code, the code wins, so re-check before relying on a detail. Design intent lives in the GDD, not here.

## Boot sequence

1. **`EntryLoader`** (scene root) finds no `Entry`, so it instantiates `Resources/OV Entry` (`DontDestroyOnLoad`). `Entry.Awake` loads `PlayerData` and `GameSetting` from disk and builds the pools. On the Demo branch, `Entry.PlayerDataObject` is `PlayerData_Demo`, not `PlayerData`.
2. **Which save slot opens**: `RunDataManager` starts on `PlayerData.lastSlotId` (0 on a fresh install). Nothing in the Run scene chooses a slot; the Home scene's slot picker is skipped entirely.
3. **`RunEntry.Init`**: if the slot was never started (`initedRun` false) it calls `InitRun` and logs `Inited runData slot N`. Then `StartRun`, stores `lastSlotId`, and schedules a save for this frame. **The save files change as soon as Play starts.**
4. **Base build stats**, in order: `Assets/_Exp/BuildStatsTest.asset` if it exists and is enabled (editor only, the "Test Build Stats Mark" label shows on screen); otherwise `BuildStats - Default` if `RunData.FinishedTutorial`, else `BuildStats - Tutorial`. Then Shop levels, skill-tree nodes and items are applied on top (`ApplyAllToBuildStats`).
   - The tutorial task UI (`TutorialTaskManager`) isn't in the Run scene, and nothing calls `RunEntry.FinishTutorial()`. A slot that starts in the Run scene therefore keeps `finishedTutorial: false`, and plays on `BuildStats - Tutorial` for good. `state.cs` prints which one is in use. Mention it in reports when it matters.
5. **`InitRunState.Start`** → `RunStateManager.StartCombat()` → state `Combat` (or straight to `OnPrestige` if the slot is mid-prestige). `OnCombatStarted` spawns the rocks for each tier's count (`RocksSpawner`), the saved chests and the Flipper Bots, and resets the dev time-scale slider to 1x.
6. **Demo and Playtest branches only**: `ShowDemoNoticePopupInRun` opens the **Demo notice screen** one frame after the combat screen activates. It holds a `GamePauser`, so **gameplay is paused (`timeScale 0`) until it's closed** with `Demo notice screen/Wrapper/Back Button UI`.

Expected log lines on every start: "Did not init Steam due to editor setting", "Failed to load Steam discount: ConnectionError", "UnityPurchaser: DontDestroyOnLoad only works for root GameObjects", "Forcing mouse and keyboard input", and "Inited runData slot N" on a new slot. As of 2026-10 there's also "The referenced script (Unknown) on this Behaviour is missing!" right at startup (not yet tracked down). Once skills are bought, every purchase logs "TryToReportAchievement for unlocking skill" (a stub).

The Shop tab's content switches on a few frames after combat starts, so a `state` taken right at startup shows the Shop buttons as `(tab hidden)`.

## Saving

- `PlayerDataSaver` writes `PlayerData` plus the **active slot's** `RunData` together, every 10 s in the editor (`RunAutoSaver`), on skill/shop purchases, on opening the System menu, and from `RunEntry.OnDestroy`, so **leaving Play mode saves too**.
- Files: `<persistentDataPath>/WritableData_<standAloneKey>.FH`, UTF-16 JSON, unencrypted. `persistentDataPath` includes the product name, which the version branch changes (e.g. `.../Mini Fun Games/Rock Flipper Demo`). `preflight.cs` prints the path and maps each file to its asset.
- Domain reload is on (Enter Play Mode Options off), so every Play session reloads the data from disk. Restoring files between sessions therefore really resets the game.
- The terminal's `DisableSave` (`playerDataSaver.AddUnsavableLock`) blocks saving, but by the time a snippet can run, the first save has already been scheduled. Use backup and restore instead.

## The core loop (what to exercise)

- **Rocks** (`RockInstanceManager.ActiveRocks`): tiers `P0` to `P3` (GDD Tier I to IV), some **Pure** (rolled on spawn and re-roll). A flip sends the rock up and back down. On landing: HP −1, chest exp and level exp, **landing cash** (× pure multiplier × temporary multiplier). At 0 HP it earns **breaking cash** and re-rolls into a new rock. After it lands, the cursor can't flip that rock again until its landing cooldown has passed (`FlippableByPlayerCursor.CooledDown`).
- **Mouse** (`PlayerCursor`, GDD "Mouse"): left-click flips what's under the cursor. Once the `Mouse Hover` skill node is bought, hovering flips instead. `Enable Mouse Radius` swaps the point test for a circle of `mouseRadius`.
- **Shop tab** (`Side bar/Tab Contents/Shop`): `Rock Count P0`…`P3`, `Flipper Bot Count`, `Unlock Skill Tree`. Each is a `StatBuilderButton`, saved by `buttonId` (`rockCountP0`, …, `flipperBotCount`, `unlockSkillTree`), and it's interactable only when affordable and not maxed. Buying rock count spawns the rock right away.
- **Skill tree**: `Side bar/Skill tree button` is interactable only after `Unlock Skill Tree` is bought. It opens `SkillTree Screen`, and nodes are `Skill Tree/Main/<n>_g<grade>_<Name>` (e.g. `0_g0_P0 Landing Cash`, `1_g0_Mouse Hover`). Only activated (visible) nodes can be bought, and only when affordable; `upgrades.cs` lists them with costs. Each node's cost has one currency, **CASH or STAR**: e.g. `Unlock L VIII` and `Backgrounds` cost 1 STAR. Node buttons are always interactable: `SkillNode.HandleClick` returns silently when the node is maxed or unaffordable, so a click that "worked" may have bought nothing.
  - **Needs attention**: a node whose `SkillGraphNode.AttentionFlag` is set hasn't been given balancing values yet, and can't be bought. Clicking it logs `This node needs attention` and selects the graph node in the Editor. As of 2026-10, 10 reachable nodes were flagged, including `Unlock P1 Rock`, `Unlock P2 Rock`, `FlipBot Enable`, `Mouse Radius` and `P0 Max Count`. That's why the early game runs out of new things to buy. Setting their values and clearing the flag is balancing work (the `balance-demo` skill), not a playtest fix: report it. The scene's nodes read the flag, costs, levels and build values from the graph asset live, so such changes reach a running game through `SkillTreeReloader` (the dev Reload Skill Tree button) without a re-import. **Rocks and monoliths only re-read their stats when the screen closes** (`SkillTreeScreen.OnClosed`), so close it (`SkillTree Screen/Wrapper/Back Button UI Variant`) before judging an upgrade's effect. The demo-limited nodes open the Steam page instead of upgrading, and `click.cs` refuses them.
- **Level bar / Chest bar**: fed by landings (1 exp each at the start; the first level needs 2000 exp, every chest level 1000). Each chest level-up drops a chest (`NewChestSpawner`). Flipping a chest down to 0 HP opens it, with an animation, and it finds an item (the Items tab gets a badge). Each player level-up grants `GameBalance.StarsPerLevelUp` (1) **STAR**, the second currency, which some skill nodes cost. The exp needed grows: 2000 for level 1, 4000 for level 2.
- **Backgrounds tab**: unlock and select backgrounds. The preferred background gives a multiplier. Demo/playtest builds skip the backgrounds marked as not in the demo.
- **Flipper Bots**: wander and flip rocks in range on an interval. Bought in the Shop.
- **Monoliths** (`Monolith Circle`): each is hidden until its skill-tree unlock. Only the Prism (L VIII) does anything: it beams airborne rocks, raising their temporary multiplier.
- **Rock abilities**: Bouncy (P1), Restless (P2), Shockwave (P3), tuned by `BuildStatsObject` fields.
- **Fresh save, early numbers** (Demo branch, `BuildStats - Tutorial`, 2026-10): 1 P0 rock and $0. Each landing gives $1. Shop prices: P0 rock $5, $10, $18, $29, $46…; Unlock Skill Tree $25. Buying the cheapest thing with AutoMouse, a purchase came every 13-70 gameplay seconds through about 33 gameplay minutes (≈62 purchases). Then the next one needed over a minute: P0 Rock L16 at $6563, with income around $60/s. Re-measure rather than trusting these; they're here to spot big changes.
- **Prestige** is scaffolded only: its data resets are commented out. Don't expect a reset to reset anything.

## Screens

UI screens form a stack (`UIScreenManager`). Opening one = `SetActive(true)`, closing = `SetActive(false)`; `state.cs` prints the stack. The ones that matter:

| Screen | Opened by | Pauses? | Close with |
|---|---|---|---|
| Combat Screen | always at the bottom | no | — |
| SkillTree Screen | `Side bar/Skill tree button` | no | `SkillTree Screen/Wrapper/Back Button UI Variant` |
| System Popup | `Side bar/Bottom/System Menu Button` (also an input action, `Open SystemMenu Popup Button`) | yes | `System Popup/Wrapper/Menu/Resume Button` |
| Demo notice screen | automatically on Demo/Playtest | yes | `Demo notice screen/Wrapper/Back Button UI` |
| Setting Popup | System Popup → Setting Button | (under System Popup) | `Setting Popup/Wrapper/Back Button UI` |

Never click (`click.cs` refuses them): Home Button (leaves the Run scene), Quit Button, Reload Scene Button, Wishlist Box / Feedback Button (open the browser or Steam), Reset Tutorial Button.

## Dev tools available in Play mode

- **Dev Gameplay Time Scale Controller** (left edge of the combat screen): range set in the scene (0.2x to 20x as of 2026-10), and keys `1`/`2`/`3` held = 2x/5x/10x. In the editor it also fast-forwards play-time timers. It's `timescale.cs`.
- **Auto Mouse** toggle: see `automouse.cs`. Both are dev UIs, hidden unless `DevEntry.IsShowingDevUIs`. The scripts switch them on for the session.
- **Terminal commands** (`Scripts/Dev/Terminal/TerminalCommands.cs`) are static methods, but the logic is short, so call the same APIs directly from `eval`. Examples:
  - `AddCurrency`: `Agame.Run.RunEntry.Instance.RunData.AddCurrency(Agame.Run.Currency.CASH, 1000);`
  - `AddLevelExp` / `AddChestLevelExp`: `RunData.AddLevelExp(x)` / `RunData.AddChestLevelExp(x)`
  - `AddItemExp`: `RunData.AddItemExp(itemId, n)`
  - `UnlockBackground` / `SetBackground`: `RunData.UnlockBackground(id)`, or `RunData.ActiveBackgroundId = id;` + `RunEntry.Instance.combatBackgroundManager.UpdateBackgroundForCurrentCombat();`
- **Build stats live object**: `Agame.Run.RunEntry.Instance.BuildStats`, a per-run instance, so editing it in Play mode touches no asset. Rocks pick up changes on the next skill-tree close.

## What snippets can and can't reach

- Snippets run on the main thread between frames. Never loop or wait inside one: the game freezes until it returns. Wait between calls instead.
- `flip.cs` calls `Flippable.TryFlipping(FlipSource.Mouse)`, the same call the cursor ends in. It skips the cursor's hit test and its click/hover/radius input checks. For those, use AutoMouse (hover mode really goes through `PlayerCursor.FindHits`), or ask the owner to try the real mouse.
- There's no real mouse or keyboard input. Hotkeys, Esc and gamepad can't be pressed from a snippet. Call what the key would call instead (e.g. open `System Popup` with its button).
- The console buffer that `logs.sh` reads keeps Play-mode entries only until Play stops. Read logs while still playing.
- Right after `editor_play`, Editor commands time out (`Main thread operation timed out after 5000ms`) until the domain reload finishes, about 10 s. The Editor logs those timeouts as errors; they aren't game bugs.
