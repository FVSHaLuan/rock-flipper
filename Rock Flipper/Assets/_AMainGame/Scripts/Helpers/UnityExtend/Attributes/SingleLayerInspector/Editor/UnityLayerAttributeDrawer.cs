using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityLayerAttribute))]
public class UnityLayerAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.LayerField(position, label, property.intValue);
    }
}