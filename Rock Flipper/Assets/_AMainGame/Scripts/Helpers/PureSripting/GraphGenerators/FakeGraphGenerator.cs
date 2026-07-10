using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FakeGraphGenerator : GraphGenerator
{
    [SerializeField]
    private int maxCachedPoint = 50;

    private bool isDirty = false;
    private List<Vector2> points = new List<Vector2>();

    public override bool IsDirty => isDirty;

    protected override List<Vector2> Points => points;

    protected abstract Vector2 UpdateNewPoint(float deltaTime);

    public sealed override void ManualUpdate(float deltaTime)
    {
        ///
        var newPoint = UpdateNewPoint(deltaTime);

        ///
        while (points.Count >= maxCachedPoint)
        {
            points.RemoveAt(0);
        }

        ///
        points.Add(newPoint);

        ///
        isDirty = true;
    }
}
