using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LargeNumberFieldAttribute))]
public class LargeNumberFieldDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                           GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {

        ///
        var tooltip = GetTooltip(property);

        ///
        var newLabel = new GUIContent(label);

        ///
        if (string.IsNullOrEmpty(newLabel.tooltip))
        {
            newLabel.tooltip = tooltip;
        }
        else
        {
            newLabel.tooltip += "\r\n" + tooltip;
        }

        ///
        EditorGUI.PropertyField(position, property, newLabel, true);
    }

    private string GetTooltip(SerializedProperty property)
    {
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            return property.intValue.ToLargeNumberString() + " - " + property.intValue.ToExponentialString();
        }
        else if (property.propertyType == SerializedPropertyType.Float)
        {
            return property.doubleValue.ToLargeNumberString() + " - " + property.doubleValue.ToExponentialString();
        }
        else
        {
            return "<Unsupported type for LargeNumberField>";
        }
    }
}
