using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FH.Core.Architecture.Pool;

public class GeneralPool : MultiPrototypesPool<GeneralPoolMemberSimplified>
{
    public void TryPushPrototype(GeneralPoolMemberSimplified prototype)
    {
        if (!ContainsPrototype(prototype.PrototypeId))
        {
            PushPrototype(prototype);
        }
    }

    /// <summary>
    /// - push prototype if it isn't in the pool yet
    /// - force clone
    /// </summary>
    /// <param name="prototype"></param>
    /// <returns></returns>
    public T TakeInstance<T>(T prototype, object requestedObject) where T : GeneralPoolMemberSimplified
    {
        return TakeInstanceAsGeneralPoolMemberSimplified(prototype, requestedObject) as T;
    }

    /// <summary>
    /// - push prototype if it isn't in the pool yet
    /// - force clone
    /// </summary>
    /// <param name="prototype"></param>
    /// <returns></returns>
    private GeneralPoolMemberSimplified TakeInstanceAsGeneralPoolMemberSimplified(GeneralPoolMemberSimplified prototype, object requestedObject)
    {
        ///
        TryPushPrototype(prototype);

        ///
        return TakeInstance(prototype.PrototypeId,true);
    }

    public GeneralPoolMemberSimplified TakeInstance(int prototypeId, object requestedObject)
    {
        ///
        var instance = base.TakeInstance(prototypeId, true);
        instance.RequestedObject = requestedObject;

        ///
        return instance;
    }

    public override GeneralPoolMemberSimplified TakeInstance(int prototypeId, bool forceCloning = true)
    {
        throw new System.Exception("");
    }
}