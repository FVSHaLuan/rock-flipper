using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcurrentActivatorByUnityObjectInstance : ConcurrentActivator
{
    [SerializeField]
    private Object unityObject;

    protected override ConcurrentActivationManager.IKeyState GetKeyState()
    {
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
        if (unityObject == null)
        {
            throw new System.NullReferenceException();
        }

        ///
        Entry.Instance.concurrentActivationManager.UpdateWithNewActivationByUnityObjectInstanceId(unityObject.GetHashCode());
    }
}
