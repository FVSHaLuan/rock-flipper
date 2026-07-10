using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityCustomArrayElementHeaderAttribute))]
public class ArrayElementTitleDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Parse index
        int index = -1;
        try
        {
            index = int.Parse(label.text.Split(' ')[1]);
        }
        catch (Exception) { }

        ///
        string newlabel = GetTitle(property, index);

        ///
        if (string.IsNullOrEmpty(newlabel))
            newlabel = label.text;

        ///
        EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
    }

    private string GetTitle(SerializedProperty property, int index)
    {
        ///
        var unityCustomArrayElementHeader = property.boxedValue as IUnityCustomArrayElementHeader;

        ///
        if (unityCustomArrayElementHeader == null)
        {
            return null;
        }

        ///
        return unityCustomArrayElementHeader.GetHeader(index);
    }
}