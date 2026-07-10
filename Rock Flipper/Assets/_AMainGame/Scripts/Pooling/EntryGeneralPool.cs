using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryGeneralPool : GeneralPool
{
    private Transform pooledObjectsRoot;

    public EntryGeneralPool(Transform pooledObjectsRoot)
    {
        this.pooledObjectsRoot = pooledObjectsRoot;
    }

    public override void PushInstance(GeneralPoolMemberSimplified memberInstance)
    {
        ///
        base.PushInstance(memberInstance);

        ///
        memberInstance.transform.SetParent(pooledObjectsRoot);
    }
}
