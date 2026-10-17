// Edit or Play mode. The Shop's balancing values, which live in the Run scene (not in an asset).
// For each StatBuilderButton: its buttonId, scene path, the serialized fields of its cost / max-level / build-value
// config components (property paths are what scene-set.cs takes), and in Play mode the owned level and next costs.
// LEVELS: how many upcoming costs to print in Play mode.
var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
var inv = System.Globalization.CultureInfo.InvariantCulture;
var playing = UnityEditor.EditorApplication.isPlaying && Agame.Run.RunEntry.Instance != null;
var levels = int.Parse("{{LEVELS}}", inv);
string PathOf(UnityEngine.Transform t) { var p = t.name; while (t.parent != null) { t = t.parent; p = t.name + "/" + p; } return p; }
string Fields(UnityEngine.Component c)
{
    var sb2 = new System.Text.StringBuilder();
    var it = new UnityEditor.SerializedObject(c).GetIterator();
    for (var enter = true; it.NextVisible(enter); enter = false)
    {
        if (it.name == "m_Script" || it.name == "selfLog") continue;
        var p = it.Copy();
        var end = p.GetEndProperty();
        // Leaf values only, with their full property path
        for (var deeper = true; p.NextVisible(deeper) || false; deeper = true)
        {
            if (UnityEditor.SerializedProperty.EqualContents(p, end)) break;
            if (p.hasVisibleChildren || p.propertyType == UnityEditor.SerializedPropertyType.ArraySize || p.propertyPath.StartsWith("selfLog")) continue;
            sb2.Append("      ").Append(p.propertyPath).Append(" = ").Append(Value(p)).AppendLine();
        }
        if (!it.hasVisibleChildren) sb2.Append("      ").Append(it.propertyPath).Append(" = ").Append(Value(it)).AppendLine();
    }
    return sb2.ToString();
}
string Value(UnityEditor.SerializedProperty p)
{
    switch (p.propertyType)
    {
        case UnityEditor.SerializedPropertyType.Float: return p.doubleValue.ToString("0.####", inv);
        case UnityEditor.SerializedPropertyType.Integer: return p.longValue.ToString(inv);
        case UnityEditor.SerializedPropertyType.Boolean: return p.boolValue.ToString();
        case UnityEditor.SerializedPropertyType.Enum: return p.enumValueIndex >= 0 && p.enumValueIndex < p.enumNames.Length ? p.enumNames[p.enumValueIndex] : p.intValue.ToString(inv);
        case UnityEditor.SerializedPropertyType.String: return "\"" + p.stringValue + "\"";
        case UnityEditor.SerializedPropertyType.ObjectReference: return p.objectReferenceValue != null ? p.objectReferenceValue.name : "null";
        default: return "(" + p.propertyType + ")";
    }
}

var sb = new System.Text.StringBuilder();
var t = typeof(Agame.Run.Shop.StatBuilderButton);
foreach (var b in UnityEngine.Object.FindObjectsByType<Agame.Run.Shop.StatBuilderButton>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var id = (string)t.GetField("buttonId", flags).GetValue(b);
    sb.Append(id).Append(" | ").Append(PathOf(b.transform)).AppendLine();
    var seen = new System.Collections.Generic.HashSet<UnityEngine.Component>();
    foreach (var iface in new[] { typeof(Agame.Run.Shop.ICostConfig), typeof(Agame.Run.Shop.IMaxLevelConfig), typeof(Agame.Run.Shop.IBuildValueConfig) })
    {
        var c = b.GetComponentInParent(iface, true);
        if (c == null) { sb.Append("  ").Append(iface.Name).AppendLine(": none"); continue; }
        sb.Append("  ").Append(iface.Name).Append(": ").Append(c.GetType().Name).Append(" on ").Append(PathOf(c.transform)).AppendLine();
        if (seen.Add(c)) sb.Append(Fields(c));
    }
    if (playing)
    {
        var data = Agame.Run.RunEntry.Instance.RunData;
        var level = data.GetBuilderButtonLevel(id);
        var max = ((Agame.Run.Shop.IMaxLevelConfig)t.GetField("maxLevelConfig", flags).GetValue(b)).MaxLevel;
        var cc = (Agame.Run.Shop.ICostConfig)t.GetField("costConfig", flags).GetValue(b);
        var costs = new System.Collections.Generic.List<string>();
        for (int l = level; l < System.Math.Min(max, level + levels); l++)
        {
            try { var cost = cc.GetNextLevelCost(l); costs.Add(cost.amount.ToString("0", inv)); }
            catch (System.Exception e) { costs.Add("ERR " + e.Message); break; }
        }
        sb.Append("  owned L").Append(level).Append("/").Append(max).Append(" | next costs ").Append(costs.Count > 0 ? string.Join(", ", costs) : "-").AppendLine();
    }
}
return sb.ToString();
