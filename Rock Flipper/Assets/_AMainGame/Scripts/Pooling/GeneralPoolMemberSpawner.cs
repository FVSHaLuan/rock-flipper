using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPoolMemberSpawner : MonoBehaviour
{
    [Header("***GeneralPoolMemberSpawner")]
    [SerializeField]
    private GeneralPoolMember prototype;
    [SerializeField]
    private Transform spawningPosition;

    [Header("Transform modifier")]
    [SerializeField]
    private bool syncUpVector;
    [SerializeField]
    private bool syncLocalScale;
    [SerializeField]
    private bool useSourceLossyScale;
    [SerializeField]
    private float scaleFactor = 1;
    [SerializeField]
    private Transform transformParent;
    [SerializeField]
    private bool worldPositionStay = true;
    [SerializeField]
    private bool overrideZ;
    [SerializeField]
    private float overridenZ;

    [Space]
    [SerializeField]
    private bool doNotSpawnIfTransformParentInactive;

    [Space]
    [SerializeField]
    private CustomGeneralPool customGeneralPool;

    private GeneralPool Pool
    {
        get
        {
            if (customGeneralPool != null)
            {
                return customGeneralPool.GeneralPool;
            }
            else
            {
                return Entry.Instance.GeneralPool;
            }
        }
    }

    [ContextMenu("Spawn"), PlayModeOnly]
    public virtual void Spawn()
    {
        ///
        if (doNotSpawnIfTransformParentInactive && transformParent != null)
        {
            if (!transformParent.gameObject.activeInHierarchy)
            {
                return;
            }
        }

        ///
        SpawnWithResult(prototype);
    }

    public GeneralPoolMember SpawnWithResult()
    {
        return SpawnWithResult(prototype);
    }

    public GeneralPoolMember SpawnWithResult(GeneralPoolMember prototype)
    {
        ///
        var instance = Pool.TakeInstance(prototype, this) as GeneralPoolMember;

        ///
        instance.OnSpawned?.Invoke();

        ///
        var parentTransform = (transformParent == null) ? null : transformParent;
        if (!worldPositionStay)
        {
            instance.transform.SetParent(parentTransform, worldPositionStay);
        }
        else
        {
            instance.transform.parent = parentTransform;
        }

        ///
        if (spawningPosition != null)
        {
            ///
            instance.transform.position = GetEffectiveSpawningPosition();

            ///
            if (syncUpVector)
            {
                instance.transform.up = spawningPosition.up;
            }

            ///
            if (syncLocalScale)
            {
                instance.transform.localScale = useSourceLossyScale ? spawningPosition.transform.lossyScale : spawningPosition.localScale;
            }

            ///
            instance.transform.localScale *= scaleFactor;
        }

        ///
        instance.gameObject.SetActive(true);

        ///
        instance.OnAfterSetPosition?.Invoke();

        ///
        return instance;
    }

    protected virtual Vector3 GetEffectiveSpawningPosition()
    {
        ///
        var pos = spawningPosition.position;

        ///
        if (overrideZ)
        {
            pos.z = overridenZ;
        }

        ///
        return pos;
    }
}