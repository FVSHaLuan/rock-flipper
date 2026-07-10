using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLocalScaleTo : PropertyMover<Vector3>
{
    [Header("MoveScaleTo")]
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private bool useX;
    [SerializeField]
    private bool useY;
    [SerializeField]
    private bool useZ;

    protected override bool MoveToTarget(Vector3 targetValue, float deltaTime)
    {
        ////
        var effectiveTargetValue = targetValue;
        var currentScale = targetTransform.localScale;

        ///
        if (!useX)
        {
            effectiveTargetValue.x = currentScale.x;
        }
        if (!useY)
        {
            effectiveTargetValue.y = currentScale.y;
        }
        if (!useZ)
        {
            effectiveTargetValue.z = currentScale.z;
        }

        ///
        currentScale = Vector3.MoveTowards(currentScale, effectiveTargetValue, Speed * deltaTime);

        ///
        targetTransform.localScale = currentScale;

        ///
        return Mathf.Approximately(Vector3.SqrMagnitude(currentScale - effectiveTargetValue), 0);
    }

    public void SetImmediately(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
