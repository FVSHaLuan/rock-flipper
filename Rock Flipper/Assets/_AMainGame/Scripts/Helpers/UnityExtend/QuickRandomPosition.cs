using GD;
using OneLine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuickRandomPosition : MonoBehaviour
{
    [SerializeField, OneLineWithHeader]
    private List<Circle> circles;

    private bool inited = false;

    [System.Serializable]
    public struct Circle : IWeighted
    {
        public float Weight { get; private set; }
        public Vector2 center;
        public float radius;

        public Circle UpdateWeight()
        {
            var rs = this;
            rs.Weight = radius * radius * Mathf.PI;
            return rs;
        }
    }

    private void TryInit()
    {
        if (inited)
        {
            return;
        }

        ///
        if (Application.isPlaying)
        {
            inited = true;
        }

        ///
        if (circles == null)
        {
            return;
        }

        ///
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i] = circles[i].UpdateWeight();
        }
    }

    public Vector2 GetRandomPosition()
    {
        ///
        TryInit();

        ///
        var circle = circles.PickOne(UnityRandom.Default);
        var p = Random.insideUnitCircle * circle.radius + circle.center;
        return transform.TransformPoint(p);
    }

#if UNITY_EDITOR
    public void Editor_GetCircles(List<Circle> circles)
    {
        circles.Clear();
        if (this.circles != null)
        {
            circles.AddRange(this.circles);
        }
    }

    public void Editor_SetCircles(List<Circle> circles)
    {
        ///
        UnityEditor.Undo.RecordObject(this, "Set Circles");

        ///
        if (this.circles == null)
        {
            this.circles = new List<Circle>();
        }
        this.circles.Clear();
        this.circles.AddRange(circles);
    }

    protected void OnDrawGizmosSelected()
    {
        if (circles == null)
        {
            return;
        }

        ///
        for (int i = 0; i < circles.Count; i++)
        {
            Editor_DrawGizmosFor(i);
        }
    }

    private void Editor_DrawGizmosFor(int index)
    {
        ///
        var circle = circles[index];
        var center = transform.TransformPoint(circle.center);
        var radius = circle.radius * transform.lossyScale.x;

        ///
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(center, radius);

        ///
        Handles.color = Color.limeGreen;
        Handles.Label(center + new Vector3(0.1f, -0.1f, 0), $"Circle {index}");
    }
#endif
}
