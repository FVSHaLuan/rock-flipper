using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectWithInit : ScriptableObject
{
    [System.NonSerialized]
    private bool inited = false;

    protected virtual void Init() { }

    public void TryInit()
    {
        ///
        if (inited)
        {
            return;
        }

        ///
        inited = true;

        ///
        Init();
    }
}
