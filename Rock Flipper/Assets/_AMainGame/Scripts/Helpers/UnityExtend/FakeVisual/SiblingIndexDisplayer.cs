using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblingIndexDisplayer : OneTimeTextDisplayer<int>
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private int multiplier = 1;
    [SerializeField]
    private int additioner = 0;

    protected override int GetValue()
    {
        ///
        var effectiveTransform = targetTransform == null ? transform : targetTransform;

        ///
        return effectiveTransform.GetSiblingIndex() * multiplier + additioner;
    }

    protected void Reset()
    {
        targetTransform = transform;
    }
}
