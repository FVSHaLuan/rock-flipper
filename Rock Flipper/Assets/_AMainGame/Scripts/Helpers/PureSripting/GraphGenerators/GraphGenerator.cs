using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GraphGenerator : MonoBehaviour
{
    public abstract bool IsDirty { get; }
    protected abstract List<Vector2> Points { get; }

    public abstract void ManualUpdate(float deltaTime);

    public void GetPoints(List<Vector2> points)
    {
        ///
        points.Clear();
        points.AddRange(Points);
    }
}
