using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChildTransformCountEvents : ValueDisplayer<bool>
{
    [SerializeField]
    private int minChildCount = 1;
    [SerializeField]
    private bool addMinChildCountAtAwake;

    [Space]
    [SerializeField]
    private UnityEvent onHadChildren;
    [SerializeField]
    private UnityEvent onNoChildren;

    protected void Awake()
    {
        if (addMinChildCountAtAwake)
        {
            minChildCount = transform.childCount + minChildCount;
        }
    }

    protected override void Display(bool isFirst, bool previousValue, bool currentValue)
    {
        if (currentValue)
        {
            onHadChildren?.Invoke();
        }
        else
        {
            onNoChildren?.Invoke();
        }
    }

    protected override bool GetCurrentValue()
    {
        return transform.childCount >= minChildCount;
    }
}
