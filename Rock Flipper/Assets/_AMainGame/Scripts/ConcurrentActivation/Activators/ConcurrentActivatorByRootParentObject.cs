using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcurrentActivatorByRootParentObject : ConcurrentActivator
{
    private Object GetUnityObject()
    {
        return transform.root.gameObject;
    }

    protected override ConcurrentActivationManager.IKeyState GetKeyState()
    {
        ///
        return Entry.Instance.concurrentActivationManager.GetKeyStateByUnityObjectInstanceId(GetUnityObject().GetHashCode());
    }

    protected override void UpdateWithActivation()
    {
        Entry.Instance.concurrentActivationManager.UpdateWithNewActivationByUnityObjectInstanceId(GetUnityObject().GetHashCode());
    }
}
