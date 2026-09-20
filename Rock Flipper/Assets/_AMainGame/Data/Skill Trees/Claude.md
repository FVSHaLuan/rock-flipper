# Skill Trees

This folder holds the **skill tree graph data**: `MainSkillTree.asset`, an [xNode](https://github.com/Siccity/xNode) `NodeGraph` asset (`Agame.Run.SkillTreeGraph`). **Rock Flipper has exactly one skill tree.** Any future mention of "the skill tree" / "skill tree graph" in this codebase refers to `MainSkillTree.asset`.

There's also `SkillTree` (`Agame.Run.SkillTree`), a `[DisallowMultipleComponent]` MonoBehaviour placed on a GameObject in `Scenes/Run.unity` (not a prefab in this repo) that references `mainSkillTreeGraph` — this is the runtime driver. It also has unused `laserSkillTreeGraph`/`lightningSkillTreeGraph` fields and matching `Special Crusher Configs/` (see sibling `SpecialCrusherId`) plumbing for special/challenge trees — **these are not populated/shipped**; don't assume they exist as content, only as dead code paths guarded by a `SpecialEntry`-named node that doesn't currently exist in `MainSkillTree.asset`.

## The two representations of a node

Every skill is represented **twice**, and both must exist and agree:

1. **`SkillGraphNode`** (`Scripts/Run/Skill tree/Graph/SkillGraphNode.cs`) — the xNode `Node` subclass, one instance per node inside `MainSkillTree.asset`. This is the **source of truth**: name, position, connections, unlock rules, costs, and the `buildAgent` reference all live here.
2. **`SkillNode`** (`Scripts/Run/Skill tree/SkillNode.cs`) — a runtime uGUI prefab instance living under the `SkillTree` component's `mainTreeRoot` transform in `Scenes/Run.unity`. This is **generated from #1** by an editor tool (see "Regenerating the runtime tree" below) — you never hand-author or hand-edit a `SkillNode` GameObject's graph-derived fields (icon, costs, output list, position, grade text, name). It only carries UI wiring (images, tooltip anchors, connector references) that isn't graph data.

Editing the graph and forgetting to regenerate #2 is the most common way to silently break the skill tree — the Run scene will still show the old layout/costs until re-imported.

## Anatomy of one `SkillGraphNode`

Each node in `MainSkillTree.asset` is a YAML `MonoBehaviour` block (`m_Script` guid `1dbf462afc3fc1845a809a21d023fc7f`). Key fields:

- `m_Name` — the node's unique id (`SkillGraphNode.NodeId` returns `name`). Must be unique across the whole graph (editor validates this and logs an error on duplicates during import). **Convention: infer the name from the node's `buildAgent`, kept as short as possible**, mostly in the form `[object] [property]` — e.g. `Mouse Radius`, `FlipBot Max Count`, `P0 Landing Cash`. Object abbreviations: rock tiers as `P0`/`P1`/`P2`/`P3` (not "P0 Rock"/"Tier 0 Rock"), Flipper Bots as `FlipBot`. Drop words the `[object] [property]` shape doesn't need (e.g. "Unlock" prefixes, articles) unless they're required to disambiguate from another node.
- `position` — xNode graph-space position (editor-only, used for layout in the graph window). The graph's `gridSnapStep: 250` (`XNode.NodeGraph.GridSnapStep`, `Assets/xNode/Scripts/NodeGraph.cs`) is applied by the graph window itself whenever you drag a node with grid-snap on (default on; hold Ctrl to invert) — dragged nodes' `position.x`/`.y` get rounded to the nearest multiple of 250 automatically. **Every node must land on this 250 grid, with no two nodes overlapping**:
  - **No overlap**: xNode's own `NodeGraph.Editor_CheckOverlapNodes()` (right-click the graph asset in the Inspector → "Check overlap", `OverlappingDistanceSqr = 20*20`) flags any two nodes whose positions are within 20 units of each other into `overlappedNodes`. Run this after manual position edits or pasting nodes to catch stragglers — two nodes stacked on the same grid cell hide each other and make the graph unreadable/unclickable.
  - **Stay grid-aligned**: don't manually type a `position` that isn't a multiple of 250, and don't leave a node at whatever pixel it landed on if you dragged with snap disabled — re-drag it (with snap on) or hand-correct it to a multiple of 250 before committing.
  - Connector-direction snapping in `SkillNode_Editor.cs` (runtime side) additionally uses a `SnapThreshold` of 100 on each axis — **keep new nodes axis-aligned or diagonal to their parent** on top of being grid-snapped, or the auto-connector-direction logic (`Editor_GetConnector`) won't find a matching 8-directional connector sprite and the link will render with no connector arrow.
- `ports` (`input`/`output`, both `System.Int32`, `ConnectionType.Multiple`, `TypeConstraint.Strict`) — these are xNode's actual graph edges. A node can have multiple parents (multiple `input` connections) and multiple children (multiple `output` connections). The port **values themselves are meaningless** (always `0`) — only the *existence of a connection* matters. Don't touch `input`/`output` numeric fields; only the `connections` lists matter.
- `unlockingRequirement` (`Min(0)`) — total summed **level** of activated parents required to unlock this node (see `SkillNode.IsUnlocked()`: sums `item.Level` across all activated parents, node unlocks when sum ≥ this value).
- `minParentLevelEach` — if > 0, **every** parent must individually be at least this level before this node can unlock at all (checked before the sum in `IsUnlocked()`).
- `demoLimit` — level cap in playtest/demo builds (`-1` = no cap); gated by `VersionBranchInfo.IsPlaytestOrDemo`. See `Scripts/FeatureBranching/CLAUDE.md`.
- `buildAgent` — **the link to gameplay effect**. See next section.
- `buildValue` — the per-level magnitude passed to `BuildAgent.Apply(currentLevel, addingLevel, buildValuePerLevel)`.
- `cashTier` — a `CashTier` enum; if the first cost's currency is `CASH`, its amount gets multiplied by `Entry.Instance.cashTiers.GetConfig(cashTier).cashBase` (see `SkillGraphNode.ApplyCashTier`). Also drives the icon tint in the graph editor (`Editor_GetCashTierColor`).
- `costs_1` / `costs_2` / `costs_3` — each a list of `CurrencyAmount` (currency + amount), **one entry per level** (`costs_1.Count` == `LevelCount`, i.e. how many times the node can be leveled up). `costs_2`/`costs_3` are optional secondary/tertiary currency costs charged on top of `costs_1` at the same level index.
- `costFormulas` — up to 3 formula strings (editor-only), one per cost list, evaluated by `Editor_FillCosts` (context menu `Editor_FillCosts` on the node) to auto-fill `costs_1..3` from a base cost. Formula vars: `i` (level index), `b` (base cost, i.e. `costs_N[0].amount`), `p` (previous level's computed cost). Only fills levels beyond index 0.
- `skillNodePrototype` — optional override `SkillNode` prefab to instantiate for this node instead of the tree's default `nodePrototype` (rarely set; leave `{fileID: 0}`/null unless this node needs bespoke UI).
- `attentionFlag` — marks a node as "needs attention" (blocks clicking in-game with an error + auto-opens the graph and selects the node; shown in the graph editor as an error). Used as a WIP marker for incomplete nodes — **never ship a node with this true**.

The graph asset itself (`SkillTreeGraph`, id `11400000` in the YAML) additionally holds:
- `nodes` — the flat list of every node fileID in the graph (must include every `SkillGraphNode`/`TrueBoolNode` you add — xNode usually keeps this in sync automatically when you add nodes via the graph window, but double check after manual YAML edits).
- `rootNode` — the single entry-point node (drawn with `rootNodeHeaderColor`). There must be exactly one; `Editor_SpawnNodes` logs an error if more than one node claims to be `graph.RootNode` — but that field is a single reference, so this only matters if you reassign it.

## The `buildAgent` link — where node effects actually live

`buildAgent` is a reference to a **component on a prefab**, not the prefab itself: `{fileID: <component>, guid: <prefab guid>, type: 3}`. Every "build agent" prefab lives under `Prefabs/Run/BuildAgents/`, organized by subsystem:

- `Prefabs/Run/BuildAgents/Rock/<Effect Name>/<Tier-specific prefab>.prefab` — e.g. `Rock/Reduce Landing Cooldown/Reduce P0 Rock Landing Cooldown.prefab`. Effects are grouped by folder (one folder per `RockBuildAgent` subclass), with one prefab per rock tier (P0/P1/P2/P3) since `RockBuildAgent` (`Scripts/Run/Stats/Agents/Implementations/Rocks/RockBuildAgent.cs`) targets a specific `rockTier` field.
- `Prefabs/Run/BuildAgents/Mouse/*.prefab` — mouse-hover/radius related agents.
- `Prefabs/Run/BuildAgents/Flipper Bots/*.prefab` — Flipper Bot related agents (active/idle time, movement speed, flip interval, max count).
- `Prefabs/Run/BuildAgents/DumbBuildAgent.prefab` — a no-op agent (`DumbBuildAgent.Apply` just logs) for placeholder/testing nodes.
- `Prefabs/Run/BuildAgents/SKill Build Agent.prefab` — the **template** for creating a brand-new build agent (see workflow below).

Each build agent prefab is a single flat GameObject with (typically) 3 components:
1. **The `BuildAgent` subclass** (e.g. `ReduceRockLandingCooldown`, `EnableMouseRadius`, `IncreaseFlipperBotActiveTime`) — implements `Apply(int currentLevel, int addingLevel, double buildValuePerLevel)`, which mutates the relevant `BuildStats` sub-struct (e.g. `RockTierBuildStats`) each time the node levels up, and is also replayed on load via `SkillNode.ApplyToBuildStats()`. This is where the actual gameplay number gets changed. Class hierarchy: `BuildAgent` → subsystem base (e.g. `RockBuildAgent`, which adds a `rockTier` field and re-dispatches `Apply`) → concrete leaf class.
2. **`SkillMetaData`** — `icon`, `subIcon`, `titleGroup`, `title` (and localized variants). Read by `SkillGraphNode.SkillMetaData` (via `buildAgent.GetComponent<SkillMetaData>()`) to drive the node's UI icon/title in the skill tree screen. **A node's icon/title is not on the `SkillGraphNode` itself — it lives on the build agent prefab.**
3. **`SkillDescriptor`** — `descriptionFormat` (a `string.Format` template where `{0}` is `buildValue * buildValueMultiplier`), `extraDescription`, `buildValueMultiplier`. Drives the tooltip description text (`SkillNode.Description`/`ExtraDescription`).

So to find "what does node X actually do in gameplay": open `MainSkillTree.asset` in the xNode graph window (or read the YAML), find node X's `buildAgent` guid+fileID, then open the prefab at that guid under `Prefabs/Run/BuildAgents/` and read its `BuildAgent` subclass component.

### Creating a new build agent (new gameplay effect)

1. Duplicate `Prefabs/Run/BuildAgents/SKill Build Agent.prefab` into the appropriate subfolder (create one if it's a new effect category) and rename it to describe the concrete node (e.g. `Increase P2 Rock Foo.prefab`).
2. It starts with a `BuildAgentScriptPlaceholder` component (a `TypedPlaceholderScript<BuildAgent>`) plus empty `SkillMetaData`/`SkillDescriptor`. Either:
   - Write a new `BuildAgent` subclass under `Scripts/Run/Stats/Agents/Implementations/<Subsystem>/`, then assign it as the placeholder's `scriptReference` and use its "Add Component" context menu to swap the placeholder for the real script in place (preserves the GameObject/other components); or
   - If an existing `BuildAgent` subclass already does what you need (just for a different tier/target), skip the placeholder and just add that existing script component directly, removing the placeholder.
3. Fill in `SkillMetaData` (icon/title/titleGroup) and `SkillDescriptor` (descriptionFormat using `{0}` for the scaled build value).
4. Fill in whatever fields the concrete `BuildAgent` subclass needs (e.g. `rockTier` for `RockBuildAgent`-derived agents).
5. Bring Unity Editor focus so the new prefab (and script, if new) gets a `.meta` file — see root `CLAUDE.md`'s commit checklist.

## Adding / removing / moving a node — end-to-end workflow

1. **Open the graph**: double-click `MainSkillTree.asset` (opens the xNode graph editor window), or select any `SkillNode` in the Run scene and use its `Editor_SelectGraphNode` context menu to jump straight to its graph node.
2. **Add a node**: right-click in the graph window → Create Node → `SkillGraphNode` (there's also a `TrueBoolNode` in this folder — unrelated utility node type, not used for skill effects). Give it a unique `m_Name` immediately (default name shows as an error, "No unique name", in `SkillGraphNodeEditor`). Position it on the 250-unit grid (drag with grid-snap on, the default), axis/diagonally aligned with its intended parent, and not overlapping any existing node.
3. **Wire it up**: drag from a parent's `output` port to the new node's `input` port (or vice versa) in the graph window. A node can have multiple parent connections (all summed toward `unlockingRequirement`) and multiple child connections.
4. **Assign a `buildAgent`**: either reuse an existing prefab under `Prefabs/Run/BuildAgents/` or create a new one (see above). The node editor shows inline errors for "No icon" / "No title group" / "No title" / "No agent" — don't leave the graph with these unresolved.
5. **Set `unlockingRequirement`, `minParentLevelEach`, `buildValue`, `cashTier`, `costs_1` (+ optionally `costs_2`/`costs_3`, `costFormulas`)**.
6. **Removing a node**: delete it in the graph window (right-click → Remove Node) so xNode cleans up dangling connections in other nodes' `connections` lists and removes it from `SkillTreeGraph.nodes`. Don't just delete the YAML block by hand — you'll leave orphaned connection references.
7. **Moving a node** (reparenting / changing its position in the tree): rewire its `input`/`output` connections in the graph window; reposition it (with grid-snap on) to keep it grid-aligned and non-overlapping with its new neighbors.
8. **Regenerate the runtime tree**: open `Scenes/Run.unity`, find the GameObject with the `SkillTree` component, and run its **`Editor_ImportFromGraph (Dirty)`** context menu (defined in `Scripts/Run/Skill tree/SkillTree_EditorLayout.cs`). This:
   - destroys and respawns every `SkillNode` GameObject under `mainTreeRoot` (and the laser/lightning roots, if a `SpecialEntry` node exists) from the current graph contents,
   - copies graph data (icon, costs, output links) onto each spawned `SkillNode` (`Editor_ImportFromGraphNode`),
   - snaps positions/connector sprites to match the graph layout (`Editor_Snap`/`Editor_SnapOutputNodes`/`Editor_MatchOutputNodesToConnectors`),
   - assigns Roman-numeral grade labels (I, II, III…) to nodes sharing the same `buildAgent` (tiered upgrades of the same effect, sorted by depth),
   - checks for duplicate node ids/positions and logs errors if found,
   - resizes the scroll rect to fit the tree and spawns debug overlay nodes.
   **This step is mandatory after any graph edit** — the Run scene's visible skill tree is a snapshot, not a live view of the graph asset.
9. Save the scene. Commit both `MainSkillTree.asset` and the `Run.unity` scene diff together, plus any new/changed build agent prefabs (with their `.meta` files).

## The runtime layout is a direction-only unit grid — separate from the asset's pixel grid

`Editor_CheckOverlapNodes` (above) only protects the **xNode graph asset's** pixel positions. The **live in-game layout** computed by `Editor_ImportFromGraph` uses a completely separate, sign-only integer grid, and it is easy to pass the asset-level check while still breaking the runtime one:

- For every parent→child edge, `SkillNode_Editor.cs`'s `Editor_GetConnector` looks only at the **sign** of the graph-position delta (using a `SnapThreshold` of 100) to bucket the edge into one of 8 `Direction8` values (Up/UpRight/Right/.../UpLeft) — the actual pixel *distance* between parent and child is discarded entirely.
- `Editor_SnapOutputNodes` then walks the tree from `mainRootNode` and gives every `SkillNode` an integer `NodePosition` = parent's `NodePosition` + that direction's unit vector (`Direction8.GetVector`, e.g. `Down = (0,-1)`). This is what actually places the node on screen (`node.transform.position = parent.position + direction * nodeDistanceInUnit`).
- **A big one-hop jump in the graph asset (e.g. straight from a node to a spot 750+ units away) still only ever contributes ±1 to `NodePosition`** — so it can silently land on the exact same integer cell as some unrelated node reached via a completely different chain of normal 250-unit hops. `Editor_CheckForDuplicates` (called at the end of `Editor_ImportFromGraph`) catches this and logs `"Those nodes have the same position"`, but only *after* you've already re-imported — it does not stop you from creating the collision.

**When adding a new branch, don't reason about it from the graph asset's pixel coordinates alone.** Before trusting a layout:
1. Pick a **direction that's actually free from the parent** — check what directions the parent's *existing* children already use (only one child per direction per node).
2. After `Editor_ImportFromGraph`, dump the real picture instead of guessing:
   ```csharp
   var skillTree = UnityEngine.Object.FindFirstObjectByType<Agame.Run.SkillTree>(UnityEngine.FindObjectsInactive.Include);
   foreach (var n in skillTree.GetComponentsInChildren<Agame.Run.SkillNode>(true))
       Debug.Log(n.NodePosition + " " + n.GraphNode.NodeId);
   ```
   (run via `unity command eval_file`, see below) — this lists every node's *actual* runtime unit cell. Free cells are simply integer coordinates that don't appear in the dump; occupied real estate has nothing to do with the asset's 250-unit pixel grid.
3. Check the Console for `"Those nodes have the same position"` / `"Inconsistent nodePosition"` after every `Editor_ImportFromGraph` — a clean `Editor_CheckOverlapNodes` on the asset does **not** imply a clean runtime layout.
4. Prefer chaining new nodes as a single-file line of ordinary 250-unit hops (matching the direction you want on screen) over one large jump — it keeps the asset's visual layout, the unit grid, and the connector-sprite direction all trivially in sync.

## Scripting graph edits via `unity-cli` (`eval`/`eval_file`)

For bulk/structured edits (e.g. "create N nodes for these build agents"), driving the connected Editor with C# is far more reliable than hand-editing the YAML (see root `CLAUDE.md`) or spending many tool calls in the graph window. Gotchas specific to this project's `eval`/`eval_file` sandbox:

- **No `using` directives** — the code is compiled as a script body, not a full source file; `using X;` at the top fails to parse. Fully-qualify every type instead (`UnityEditor.AssetDatabase`, `Agame.Run.SkillGraphNode`, `Agame.Run.Stats.Agents.BuildAgent`, ...), and avoid LINQ extension methods (write plain `foreach` loops) since there's no `using System.Linq;`.
- **Creating a node exactly like the graph window does** (`NodeGraphEditor.CreateNode`, `Assets/xNode/Scripts/Editor/NodeGraphEditor.cs`):
  ```csharp
  XNode.Node.graphHotfix = graph;
  var node = graph.AddNode<Agame.Run.SkillGraphNode>();
  node.name = "New Node Name";
  node.position = new UnityEngine.Vector2(x, y);
  UnityEditor.AssetDatabase.AddObjectToAsset(node, graph); // without this the node isn't saved as a sub-asset of the .asset file
  ```
- **`SkillGraphNode`'s fields are all private `[SerializeField]`s** — set them through `UnityEditor.SerializedObject`/`SerializedProperty`, not direct field access. `buildAgent` takes an `objectReferenceValue` (the `BuildAgent` component on the prefab, found via `prefab.GetComponent<Agame.Run.Stats.Agents.BuildAgent>()`), `costs_1` is an array property (`ClearArray()`/`InsertArrayElementAtIndex`/`GetArrayElementAtIndex(i).FindPropertyRelative("currency"|"amount")`), enum fields (`cashTier`) can be set directly via `.intValue` using the enum's underlying int (e.g. `Currency.CASH == 0`, `CashTier.Tier0 == 0`) without worrying about `enumValueIndex` ordering. Call `so.ApplyModifiedProperties()` at the end.
- **Connect ports** with `child.GetInputPort("input").Connect(parent.GetOutputPort("output"))` (xNode `NodePort.Connect`).
- **`Editor_ImportFromGraph`** (the mandatory step 8 in the workflow above) is `private` and only exposed via `[ContextMenu]` — invoke it through reflection:
  ```csharp
  var skillTree = UnityEngine.Object.FindFirstObjectByType<Agame.Run.SkillTree>(UnityEngine.FindObjectsInactive.Include);
  var method = typeof(Agame.Run.SkillTree).GetMethod("Editor_ImportFromGraph", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
  method.Invoke(skillTree, null);
  UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(skillTree.gameObject.scene);
  UnityEditor.SceneManagement.EditorSceneManager.SaveScene(skillTree.gameObject.scene);
  ```
  This requires `Scenes/Run.unity` to already be open in the connected Editor (check with `unity command list_open_scenes`); open it with `unity command open_scene` first if it isn't.
- Finish with `UnityEditor.AssetDatabase.SaveAssets()` on the graph, then `unity command get_console_logs` to confirm no new errors/warnings before considering the edit done.

## Common pitfalls

- Editing `MainSkillTree.asset` YAML directly (e.g. via search-and-replace) without going through the xNode window risks breaking the `nodes` list / connection fileID references — prefer the `unity-cli` skill or the graph window over hand-editing this YAML.
- Forgetting step 8 (re-import) — the Run scene will keep showing stale nodes/costs/links.
- Two nodes with the same `m_Name` — breaks `NodeId` uniqueness (save-data lookups key on it via `RunData.GetSkillNodeState(NodeId)`), and the importer logs a hard error.
- Placing a new node off the 250-grid, overlapping an existing node, or not axis/diagonally aligned to its parent — grid misalignment/overlap makes nodes hide each other or drift from the grid over successive edits (run the graph's "Check overlap" context action to catch it), and the connector-direction snap (`SnapThreshold = 100`) silently fails to find a matching connector sprite if not axis/diagonally aligned.
- Leaving `attentionFlag` true or icon/title/agent unset on a node you intend to ship — these are surfaced as editor errors, not silent failures, but easy to miss if you don't scroll the graph.
- Connecting a new branch with a large one-hop jump in the graph asset (instead of a chain of normal 250-unit hops) — passes `Editor_CheckOverlapNodes` fine but can silently collide with an unrelated branch on the **runtime unit grid** (see "The runtime layout is a direction-only unit grid" above); always check the Console after `Editor_ImportFromGraph`, not just the asset-level overlap check.
- Assuming laser/lightning "special" trees are real content — they're an unused parallel system gated behind a `SpecialEntry`-named node that doesn't exist in `MainSkillTree.asset` today.
