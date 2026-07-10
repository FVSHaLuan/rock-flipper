using FH.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingObjectsLayout : ObjectsLayout
{
    [SerializeField]
    private int objectCount;
    [SerializeField]
    private PositionProvider centerPoint;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float startAngle;

    private List<ObjectLayoutInfo> calculatedLayouts = null;

    private int effectiveObjectCount;

    public int ObjectCount
    {
        get => objectCount;
        set => objectCount = value;
    }

    public void UpdateLayouts()
    {
        ///
        if (calculatedLayouts == null)
        {
            calculatedLayouts = new List<ObjectLayoutInfo>();
        }
        else
        {
            calculatedLayouts.Clear();
        }

        ///
        if (effectiveObjectCount <= 0)
        {
            return;
        }

        ///
        float currentAngle = startAngle;
        float angularInterval = 360.0f / effectiveObjectCount;
        var centerPos = centerPoint.Position;
        for (int i = 0; i < effectiveObjectCount; i++)
        {
            ///
            var rad = currentAngle * Mathf.Deg2Rad;

            ///
            var x = Mathf.Cos(rad);
            var y = Mathf.Sin(rad);

            ///
            var normalizedTangent = new Vector3(x, y, 0);
            var pos = normalizedTangent * radius + centerPos;

            ///
            var layout = new ObjectLayoutInfo()
            {
                normalizedTangent = normalizedTangent,
                position = pos
            };

            ///
            calculatedLayouts.Add(layout);

            ///
            currentAngle += angularInterval;
        }
    }

    public override IEnumerator<ObjectLayoutInfo> GetEnumerator()
    {
        ///
        effectiveObjectCount = objectCount;

        ///        
        UpdateLayouts();

        ///
        for (int i = 0; i < calculatedLayouts.Count; i++)
        {
            yield return calculatedLayouts[i];
        }
    }
}
