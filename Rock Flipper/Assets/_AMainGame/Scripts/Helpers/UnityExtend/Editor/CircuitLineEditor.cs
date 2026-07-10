using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CircuitLine))]
public class CircuitLineEditor : Editor
{
    private CircuitLine circuitLine;
    private List<CircuitLine.Segment> segments = new List<CircuitLine.Segment>();

    protected void OnSceneGUI()
    {
        circuitLine = target as CircuitLine;
        circuitLine.Editor_GetSegments(segments);

        ///
        bool isDirty = false;
        var startPoint = Vector2.zero;
        for (int i = 0; i < segments.Count; i++)
        {
            isDirty = isDirty || DrawHandleForSegment(i, startPoint);
            startPoint = segments[i].GetEndPoint(startPoint);
        }

        ///
        if (isDirty)
        {
            circuitLine.Editor_SetSegments(segments);
        }
    }

    private bool DrawHandleForSegment(int index, Vector2 startPoint)
    {
        ///
        var segment = segments[index];

        ///
        var endPointWorld = circuitLine.transform.TransformPoint(segment.GetEndPoint(startPoint));
        var newEndPointWorld = Handles.PositionHandle(endPointWorld, Quaternion.identity);

        ///
        if (newEndPointWorld == endPointWorld)
        {
            ///
            return false;
        }

        ///
        var newEndPoint = circuitLine.transform.InverseTransformPoint(newEndPointWorld);
        var newLength = Vector2.Distance(startPoint, newEndPoint);

        ///
        segment.length = newLength;
        segments[index] = segment;

        ///
        return true;
    }
}
