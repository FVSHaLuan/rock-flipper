// Sets one serialized value on a component in the open Run scene and prints a SCENE line for the balancing log.
//   GO         full hierarchy path of the GameObject, as shop.cs prints it ("Gameplay Canvas/.../Shop/Rock Count P1")
//   COMPONENT  component type name, as shop.cs prints it (RockCountCostConfig, DumbCostConfig, ...)
//   PROP       property path, as shop.cs prints it (prices.Array.data[0].b, amount.a, maxLevel, ...)
//   VALUE      the new value (number, true/false, or an enum name such as Tier1)
//   MODE       "play": Play mode only, the change is lost when Play stops (write the SCENE line down).
//              "apply": Edit mode only, re-applies a logged change for real (undoable, marks the scene dirty,
//              doesn't save it). Only with the user's go-ahead: it's a scene edit.
var mode = "{{MODE}}";
var playing = UnityEditor.EditorApplication.isPlaying;
if (mode == "play" && !playing) return "REFUSED: MODE=play needs Play mode";
if (mode == "apply" && UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return "REFUSED: MODE=apply needs Edit mode (stop Play first)";
if (mode != "play" && mode != "apply") return "REFUSED: MODE must be play or apply";

var path = "{{GO}}";
UnityEngine.GameObject go = null;
foreach (var t in UnityEngine.Object.FindObjectsByType<UnityEngine.Transform>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var p = t.name; var x = t;
    while (x.parent != null) { x = x.parent; p = x.name + "/" + p; }
    if (p != path) continue;
    if (go != null) return "REFUSED: more than one GameObject at '" + path + "'";
    go = t.gameObject;
}
if (go == null) return "REFUSED: no GameObject at '" + path + "'";
UnityEngine.Component comp = null;
foreach (var c in go.GetComponents<UnityEngine.Component>()) if (c != null && c.GetType().Name == "{{COMPONENT}}") comp = c;
if (comp == null) return "REFUSED: no {{COMPONENT}} on '" + path + "'";

var so = new UnityEditor.SerializedObject(comp);
var prop = so.FindProperty("{{PROP}}");
if (prop == null) return "REFUSED: no property '{{PROP}}' on {{COMPONENT}}";
var inv = System.Globalization.CultureInfo.InvariantCulture;
var value = "{{VALUE}}";
string Show(UnityEditor.SerializedProperty q)
{
    switch (q.propertyType)
    {
        case UnityEditor.SerializedPropertyType.Float: return q.doubleValue.ToString("0.####", inv);
        case UnityEditor.SerializedPropertyType.Integer: return q.longValue.ToString(inv);
        case UnityEditor.SerializedPropertyType.Boolean: return q.boolValue.ToString();
        case UnityEditor.SerializedPropertyType.Enum: return q.enumNames[q.enumValueIndex];
        default: return "(" + q.propertyType + ")";
    }
}
var before = Show(prop);
switch (prop.propertyType)
{
    case UnityEditor.SerializedPropertyType.Float:
        if (!double.TryParse(value, System.Globalization.NumberStyles.Float, inv, out var d)) return "REFUSED: '" + value + "' isn't a number";
        prop.doubleValue = d; break;
    case UnityEditor.SerializedPropertyType.Integer:
        if (!long.TryParse(value, System.Globalization.NumberStyles.Integer, inv, out var l)) return "REFUSED: '" + value + "' isn't an integer";
        prop.longValue = l; break;
    case UnityEditor.SerializedPropertyType.Boolean:
        if (!bool.TryParse(value, out var bo)) return "REFUSED: '" + value + "' isn't true/false";
        prop.boolValue = bo; break;
    case UnityEditor.SerializedPropertyType.Enum:
        var idx = System.Array.IndexOf(prop.enumNames, value);
        if (idx < 0) return "REFUSED: '" + value + "' isn't one of " + string.Join(", ", prop.enumNames);
        prop.enumValueIndex = idx; break;
    default:
        return "REFUSED: can't set a " + prop.propertyType + " property";
}
if (mode == "apply")
{
    UnityEditor.Undo.RecordObject(comp, "Balance: " + path + " {{PROP}}");
    so.ApplyModifiedProperties();
    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
}
else
{
    so.ApplyModifiedPropertiesWithoutUndo();
}
return "SCENE " + path + " | {{COMPONENT}} | {{PROP}}: " + before + " -> " + Show(prop) + (mode == "apply" ? " (applied, scene dirty, not saved)" : " (Play mode only)");
