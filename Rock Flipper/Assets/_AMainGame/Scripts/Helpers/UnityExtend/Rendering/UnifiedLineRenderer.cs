using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnifiedLineRenderer : MonoBehaviourWithInit
{
    /// <summary>
    /// replace points starting from index. Expand the list if needed.
    /// </summary>
    /// <param name="points">new points</param>
    /// <param name="index">starting index</param>
    public abstract void UpdatePoints(ICollection<Vector2> points, int index);
    public abstract void SetMaxPointCount(int maxPointCount);
    public abstract void Clear();

    public void SetPoints(ICollection<Vector2> points)
    {
        UpdatePoints(points, 0);
        SetMaxPointCount(points.Count);
    }
}
