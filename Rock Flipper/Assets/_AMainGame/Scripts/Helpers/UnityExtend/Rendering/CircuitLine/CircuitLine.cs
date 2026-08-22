using OneLine;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)), ExecuteInEditMode]
public class CircuitLine : MonoBehaviour
{
    public const float Angle = 30;

    [SerializeField, OneLineWithHeader]
    private List<Segment> segments = new List<Segment>();

    [System.Serializable]
    public struct Segment
    {
        public Direction8 direction;
        public float length;

        public Vector2 GetEndPoint(Vector2 startPoint)
        {
            return startPoint + direction.GetNormalizedVector() * length;
        }
    }

#if UNITY_EDITOR
    protected void Update()
    {
        if (Application.isPlaying)
        {
            enabled = false;
            return;
        }

        ///
        if (UnityEditor.Selection.activeGameObject == gameObject)
        {
            Editor_UpdateLineRendererDirty();
        }
    }

    public void Editor_GetSegments(List<Segment> outSegments)
    {
        outSegments.Clear();
        if (segments != null)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                outSegments.Add(segments[i]);
            }
        }
    }

    public void Editor_SetSegments(List<Segment> inSegments)
    {
        if (segments == null)
        {
            segments = new List<Segment>();
        }
        segments.Clear();
        if (inSegments != null)
        {
            for (int i = 0; i < inSegments.Count; i++)
            {
                segments.Add(inSegments[i]);
            }
        }

        ///
        Editor_UpdateLineRendererDirty();
    }

    [ContextMenu("Editor_UpdateLineRenderer (Dirty)"), EditorModeOnly]
    private void Editor_UpdateLineRendererDirty()
    {
        var lineRenderer = GetComponent<LineRenderer>();
        UnityEditor.Undo.RecordObject(lineRenderer, "Update CircuitLine LineRenderer");
        Editor_UpdateLineRenderer();
    }

    private void Editor_UpdateLineRenderer()
    {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        List<Vector2> points = new List<Vector2>();

        ///
        if (segments == null || segments.Count == 0)
        {
            Editor_SetLinePoints(lineRenderer, points);
            return;
        }

        ///
        points.Add(Vector2.zero);

        ///
        try
        {
            for (int i = 0; i < segments.Count; i++)
            {
                ///
                var segment = segments[i];

                ///
                Vector2 lastPoint = points[points.Count - 1];
                Vector2 newPoint = segment.GetEndPoint(lastPoint);
                points.Add(newPoint);
            }
        }
        catch (System.Exception)
        {
            Debug.LogError("Error while calculating line points. Please check the segments configuration.", this);
            throw;
        }

        ///
        Editor_SetLinePoints(lineRenderer, points);
    }

    private void Editor_SetLinePoints(LineRenderer lineRenderer, List<Vector2> points)
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }
#endif
}
