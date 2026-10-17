// Edit or Play mode. Lists the skill graph's nodes (MainSkillTree.asset) with their balancing fields and costs.
// FILTER: "all", or a case-insensitive substring of the node name.  COSTS: "all" = every level's cost, "short" = first, last, total.
// Row: <EDITABLE|DEMO-LIMIT n|DECORATIVE> [ATTENTION] | <name> | owned L<lvl>/<levels> (Play mode) | <agent> val <buildValue>
//      | <currency> <tier>(base) a b c | costs ... total | parents ...
var graph = UnityEditor.AssetDatabase.LoadAssetAtPath<Agame.Run.SkillTreeGraph>("Assets/_AMainGame/Data/Skill Trees/MainSkillTree.asset");
if (graph == null) return "MainSkillTree.asset not found";
var tierGuids = UnityEditor.AssetDatabase.FindAssets("t:CashTiers");
if (tierGuids.Length == 0) return "CashTiers asset not found";
var tiers = UnityEditor.AssetDatabase.LoadAssetAtPath<Agame.Balancing.CashTiers>(UnityEditor.AssetDatabase.GUIDToAssetPath(tierGuids[0]));
var filter = "{{FILTER}}".ToLowerInvariant();
var allCosts = "{{COSTS}}" == "all";
var inv = System.Globalization.CultureInfo.InvariantCulture;
var data = UnityEditor.EditorApplication.isPlaying && Agame.Run.RunEntry.Instance != null ? Agame.Run.RunEntry.Instance.RunData : null;

var sb = new System.Text.StringBuilder();
int editable = 0, attention = 0, demoLimited = 0;
foreach (var baseNode in graph.nodes)
{
    var n = baseNode as Agame.Run.SkillGraphNode;
    if (n == null) continue;
    if (filter != "all" && !n.name.ToLowerInvariant().Contains(filter)) continue;

    var so = new UnityEditor.SerializedObject(n);
    var price = so.FindProperty("price");
    var tier = price.FindPropertyRelative("cashTier").intValue;
    var a = price.FindPropertyRelative("a").doubleValue;
    var b = price.FindPropertyRelative("b").doubleValue;
    var c = price.FindPropertyRelative("c").doubleValue;
    var tierBase = tiers.GetConfig((Agame.Balancing.CashTier)tier).cashBase;

    string kind;
    if (n.IsDecorative) kind = "DECORATIVE";
    else if (n.HasDemoLimit) { kind = "DEMO-LIMIT " + n.DemoLimit; demoLimited++; }
    else { kind = "EDITABLE"; editable++; }
    if (n.AttentionFlag) { kind += " ATTENTION"; attention++; }
    // The default price a new node gets (a 0, b 1, c 1): a flat 5 cash per level, i.e. never balanced
    if (!n.IsDecorative && n.Currency == Agame.Run.Currency.CASH && a == 0 && b == 1 && c == 1) kind += " PLACEHOLDER-PRICE";

    // Same formula as SkillGraphNode.GetNextLevelCost: round(a + b * tierBase * c^level)
    var costs = new System.Collections.Generic.List<string>();
    double total = 0;
    for (int l = 0; l < n.LevelCount; l++)
    {
        var cost = System.Math.Round(a + b * tierBase * System.Math.Pow(c, l));
        total += cost;
        if (allCosts || l == 0 || l == n.LevelCount - 1) costs.Add((l > 0 && !allCosts && n.LevelCount > 2 ? "... " : "") + cost.ToString("0", inv));
    }

    var parents = new System.Collections.Generic.List<string>();
    foreach (var conn in n.GetInputPort("input").GetConnections()) parents.Add(conn.node.name);

    sb.Append(kind).Append(" | ").Append(n.name);
    if (data != null) sb.Append(" | owned L").Append(System.Math.Min(data.GetSkillNodeState(n.NodeId).level, n.LevelCount)).Append("/").Append(n.LevelCount);
    sb.Append(" | ").Append(n.BuildAgent != null ? n.BuildAgent.name : "<no agent>")
      .Append(" val ").Append(n.BuildValue.ToString("0.####", inv)).Append(" x").Append(n.LevelCount)
      .Append(" | ").Append(n.Currency).Append(" T").Append(tier).Append("(").Append(tierBase.ToString("0.##", inv)).Append(")")
      .Append(" a ").Append(a.ToString("0.####", inv)).Append(" b ").Append(b.ToString("0.####", inv)).Append(" c ").Append(c.ToString("0.####", inv))
      .Append(" | costs ").Append(string.Join(", ", costs)).Append(" total ").Append(total.ToString("0", inv))
      .Append(" | req ").Append(n.UnlockingRequirement).Append(n.MinParentLevelEach > 0 ? " each " + n.MinParentLevelEach : "")
      .Append(" | parents ").Append(parents.Count > 0 ? string.Join(", ", parents) : "-")
      .AppendLine();
}
sb.AppendLine("editable " + editable + ", demo-limited " + demoLimited + ", attention " + attention + (UnityEditor.EditorUtility.IsDirty(graph) ? " | graph has UNSAVED changes" : ""));
return sb.ToString();
