using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnifiedColoredObject))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UnifiedColoredTween : ValueDisplayer<float>
{
    [SerializeField]
    private Color startColor = Color.white;
    [SerializeField]
    private Color endColor = Color.red;
    [SerializeField, Range(0, 1)]
    private float value;

    private UnifiedColoredObject unifiedColoredObject;

    protected override void Display(bool isFirst, float previousValue, float currentValue)
    {
        ///
        if (unifiedColoredObject == null)
        {
            unifiedColoredObject = GetComponent<UnifiedColoredObject>();
        }

        ///
        unifiedColoredObject.Color = Color.Lerp(startColor, endColor, currentValue);
    }

    protected override float GetCurrentValue()
    {
        return value;
    }

#if UNITY_EDITOR
    protected override void Update()
    {
        base.Update();
    }
#endif
}
