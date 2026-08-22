using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuickRandomPosition))]
public class QuickRandomPositionEditor : Editor
{
    private List<QuickRandomPosition.Circle> circles = new List<QuickRandomPosition.Circle>();

    private void OnSceneGUI()
    {
        (target as QuickRandomPosition).Editor_GetCircles(circles);

        ///
        if (!(target as MonoBehaviour).enabled)
        {
            return;
        }

        ///
        bool isDirty = false;

        ///
        for (int i = 0; i < circles.Count; i++)
        {
            isDirty = isDirty || DrawHandles(i);
        }

        ///
        if (isDirty)
        {
            (target as QuickRandomPosition).Editor_SetCircles(circles);
        }
    }

    private bool DrawHandles(int index)
    {
        var circle = circles[index];
        var center = (target as QuickRandomPosition).transform.TransformPoint(circle.center);
        center.z = 0;
        var scale = circle.radius * (target as QuickRandomPosition).transform.lossyScale.x;

        ///
        var savedCenter = center;
        var savedScale = scale;

        ///
        center = Handles.PositionHandle(center, Quaternion.identity);
        scale = Handles.ScaleValueHandle(scale, center + Vector3.up * scale, Quaternion.identity, 0.5f, Handles.CylinderHandleCap, 0);

        ///
        if (center == savedCenter && scale == savedScale)
        {
            return false;
        }

        ///
        circle.center = (target as QuickRandomPosition).transform.InverseTransformPoint(center);
        circle.radius = scale / (target as QuickRandomPosition).transform.lossyScale.x;
        circles[index] = circle;

        ///
        return true;
    }
}
