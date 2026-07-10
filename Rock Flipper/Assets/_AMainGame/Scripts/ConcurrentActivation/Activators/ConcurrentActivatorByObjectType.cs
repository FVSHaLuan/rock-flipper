using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConcurrentActivatorByObjectType<T> : ConcurrentActivator where T : Object
{
    protected override ConcurrentActivationManager.IKeyState GetKeyState()
    {
        ///
        var unityObject = GetComponentInParent<T>();

        ///
        if (unityObject == null)
        {
            throw new System.NullReferenceException();
        }

        ///
        return Entry.Instance.concurrentActivationManager.GetKeyStateByUnityObjectInstanceId(unityObject.GetHashCode());
    }

    protected override void UpdateWithActivation()
    {
        ///
        var unityObject = GetComponentInParent<T>();

        ///
        if (unityObject == null)
        {
            throw new System.NullReferenceException();
        }

        ///
        Entry.Instance.concurrentActivationManager.UpdateWithNewActivationByUnityObjectInstanceId(unityObject.GetHashCode());
    }
}
