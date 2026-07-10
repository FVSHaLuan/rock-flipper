using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OneTimeView : MonoBehaviourWithInit
{
    [SerializeField]
    private bool updateViewOnEnable = true;

    [ContextMenu("UpdateView")]
    public abstract void UpdateView();

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        if (!updateViewOnEnable)
        {
            UpdateView();
        }
    }

    protected void OnEnable()
    {
        if (updateViewOnEnable)
        {
            UpdateView();
        }
    }
}
