using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NoOverrideAttribute))]
public class NoOverrideDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        bool wasEnabled = GUI.enabled;
        if (property.prefabOverride)
            PrefabUtility.RevertPropertyOverride(property, InteractionMode.AutomatedAction);
        if (PrefabUtility.IsPartOfVariantPrefab(property.serializedObject.targetObject)
            && !PrefabUtility.IsAddedComponentOverride(property.serializedObject.targetObject))
            GUI.enabled = false;

        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = wasEnabled;
    }
}
