// Edit or Play mode. Sets the balancing fields of one SkillGraphNode in MainSkillTree.asset. Pass "-" to keep a field.
//   NODE         exact node name
//   BUILD_VALUE  buildValue (per level)        LEVELS  levelCount (>= 1)
//   TIER         price.cashTier (0..19)         A, B, C  price.a, price.b, price.c   (cost of level L -> L+1 = round(a + b * tierBase * c^L))
//   ATTENTION    "clear" removes the attention flag, "-" keeps it
// Refuses: unknown or duplicate names, decorative nodes, nodes with a demo limit (demoLimit >= 0).
// Never touches currency, parents, requirements, demoLimit or the agent. The asset is only marked dirty here:
// save it with save-graph.cs once Play mode has stopped. In Play mode, run reload-tree.cs afterwards.
var graph = UnityEditor.AssetDatabase.LoadAssetAtPath<Agame.Run.SkillTreeGraph>("Assets/_AMainGame/Data/Skill Trees/MainSkillTree.asset");
if (graph == null) return "MainSkillTree.asset not found";
var name = "{{NODE}}";
Agame.Run.SkillGraphNode node = null;
foreach (var baseNode in graph.nodes)
{
    var n = baseNode as Agame.Run.SkillGraphNode;
    if (n == null || n.name != name) continue;
    if (node != null) return "REFUSED: more than one node is named '" + name + "'";
    node = n;
}
if (node == null) return "REFUSED: no node named '" + name + "'";
if (node.IsDecorative) return "REFUSED: '" + name + "' is decorative";
if (node.HasDemoLimit) return "REFUSED: '" + name + "' has demo limit " + node.DemoLimit + "; nodes with a demo limit are off-limits";

var inv = System.Globalization.CultureInfo.InvariantCulture;
var so = new UnityEditor.SerializedObject(node);
var price = so.FindProperty("price");
var props = new (string arg, UnityEditor.SerializedProperty prop)[]
{
    ("{{BUILD_VALUE}}", so.FindProperty("buildValue")),
    ("{{LEVELS}}", so.FindProperty("levelCount")),
    ("{{TIER}}", price.FindPropertyRelative("cashTier")),
    ("{{A}}", price.FindPropertyRelative("a")),
    ("{{B}}", price.FindPropertyRelative("b")),
    ("{{C}}", price.FindPropertyRelative("c")),
};
string Show(UnityEditor.SerializedProperty p) => p.propertyType == UnityEditor.SerializedPropertyType.Float ? p.doubleValue.ToString("0.####", inv) : p.intValue.ToString(inv);

var changes = new System.Collections.Generic.List<string>();
foreach (var (arg, prop) in props)
{
    if (arg == "-") continue;
    var before = Show(prop);
    if (prop.propertyType == UnityEditor.SerializedPropertyType.Float)
    {
        if (!double.TryParse(arg, System.Globalization.NumberStyles.Float, inv, out var v)) return "REFUSED: '" + arg + "' isn't a number (" + prop.name + ")";
        prop.doubleValue = v;
    }
    else
    {
        if (!int.TryParse(arg, System.Globalization.NumberStyles.Integer, inv, out var v)) return "REFUSED: '" + arg + "' isn't an integer (" + prop.name + ")";
        if (prop.name == "levelCount" && v < 1) return "REFUSED: levelCount must be >= 1";
        if (prop.name == "cashTier" && (v < 0 || v > 19)) return "REFUSED: cashTier must be 0..19";
        prop.intValue = v;
    }
    changes.Add(prop.name + " " + before + " -> " + Show(prop));
}
var attention = so.FindProperty("attentionFlag");
if ("{{ATTENTION}}" == "clear" && attention.boolValue) { attention.boolValue = false; changes.Add("attentionFlag cleared"); }
else if ("{{ATTENTION}}" != "clear" && "{{ATTENTION}}" != "-") return "REFUSED: ATTENTION must be 'clear' or '-'";

if (changes.Count > 0)
{
    so.ApplyModifiedProperties();
    UnityEditor.EditorUtility.SetDirty(node);
    UnityEditor.EditorUtility.SetDirty(graph);
}

// Same formula as SkillGraphNode.GetNextLevelCost, which needs Entry.Instance (Play mode only)
var tiers = UnityEditor.AssetDatabase.LoadAssetAtPath<Agame.Balancing.CashTiers>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("t:CashTiers")[0]));
var tierBase = tiers.GetConfig((Agame.Balancing.CashTier)price.FindPropertyRelative("cashTier").intValue).cashBase;
var pa = price.FindPropertyRelative("a").doubleValue; var pb = price.FindPropertyRelative("b").doubleValue; var pc = price.FindPropertyRelative("c").doubleValue;
var costs = new System.Collections.Generic.List<string>();
double total = 0;
for (int l = 0; l < node.LevelCount; l++)
{
    var cost = System.Math.Round(pa + pb * tierBase * System.Math.Pow(pc, l));
    total += cost;
    costs.Add(cost.ToString("0", inv));
}
return name + ": " + (changes.Count > 0 ? string.Join(", ", changes) : "no change")
    + (node.AttentionFlag ? " | still ATTENTION" : "")
    + "\ncosts (" + node.Currency + "): " + string.Join(", ", costs) + " | total " + total.ToString("0", inv)
    + "\nbuildValue " + node.BuildValue.ToString("0.####", inv) + " x" + node.LevelCount + " = " + (node.BuildValue * node.LevelCount).ToString("0.####", inv) + " at max";
