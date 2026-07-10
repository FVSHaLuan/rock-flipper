using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourWithInit : MonoBehaviourWithLogger
{
    [field: System.NonSerialized]
    public bool Inited { get; private set; } = false;
    [field: System.NonSerialized]
    protected int InitedFrame { get; private set; } = -1;
    [field: System.NonSerialized]
    protected bool InitedRepeatable { get; set; } = false;

    protected bool InitedThisFrame => InitedFrame == Time.frameCount;

    protected virtual void ExtendedAwake() { }
    protected virtual bool Init() { return true; }
    protected virtual void InitRepeatable() { }

    protected void Awake()
    {
        ///
        TryInit();

        ///
        ExtendedAwake();
    }

    public virtual bool TryInit()
    {
        ///
        if (Inited)
        {
            return true;
        }

        ///
        Inited = Init();

        ///
        if (Inited)
        {
            InitedFrame = Time.frameCount;
        }

        ///
        return Inited;
    }

    protected void TryInitRepeatable()
    {
        if (InitedRepeatable)
        {
            return;
        }

        ///
        InitedRepeatable = true;

        ///
        InitRepeatable();
    }
}
