using FH.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlphaTo : PropertyMover<float>
{
    [Header("MoveAlphaTo")]
    [SerializeField]
    private UnifiedColoredObject unifiedColoredObject;

    protected override bool MoveToTarget(float targetValue, float deltaTime)
    {
        ///
        var color = unifiedColoredObject.Color;

        ///
        color.a = Mathf.MoveTowards(color.a, targetValue, deltaTime * Speed);

        ///
        unifiedColoredObject.Color = color;

        ///
        return Mathf.Approximately(color.a, targetValue);
    }

    public void SetAlphaImmediately(float alpha)
    {
        ///
        var color = unifiedColoredObject.Color;

        ///
        color.a = alpha;

        ///
        unifiedColoredObject.Color = color;
    }
}
