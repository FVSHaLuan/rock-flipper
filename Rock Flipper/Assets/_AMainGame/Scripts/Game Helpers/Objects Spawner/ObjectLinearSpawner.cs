using FH.Core.Architecture.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLinearSpawner : ObjectsSpawner
{
    [Header("OjectLinearSpawner")]
    [SerializeField]
    private int maxObjectCount;
    [SerializeField]
    private Transform startTransform;
    [SerializeField]
    private Vector3 positionIncrement;
    [SerializeField]
    private Vector3 rotationIncrement;
    [SerializeField]
    private Vector3 scaleIncrement;

    [Header("OjectLinearSpawner -- Gizmos")]
    [SerializeField]
    private Vector3 gizmoSize = Vector3.one;

    protected override IEnumerable<GeneralPoolMemberSimplified> GenerateCells(float cellCountPortion, int prototypeId)
    {
        ///
        int objectCount = (int)(cellCountPortion * maxObjectCount);

        ///
        Vector3 pos = startTransform.localPosition;
        Vector3 rotation = startTransform.localEulerAngles;
        Vector3 scale = startTransform.localScale;

        ///
        for (int i = 0; i < objectCount; i++)
        {
            ///
            var newObject = generalPool.TakeInstance(prototypeId, this);
            newObject.transform.SetParent(objectsParent);

            ///
            newObject.transform.localPosition = pos;
            newObject.transform.localEulerAngles = rotation;
            newObject.transform.localScale = scale;

            ///
            pos += positionIncrement;
            rotation += rotationIncrement;
            scale += scaleIncrement;

            ///
            yield return newObject;
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        ///
        if (startTransform == null)
        {
            return;
        }

        ///
        SetGizmosMatrix();

        ///
        Vector3 pos = startTransform.localPosition;
        Vector3 rotation = startTransform.localEulerAngles;
        Vector3 scale = startTransform.localScale;

        ///
        Gizmos.color = Color.red;

        ///
        for (int i = 0; i < maxObjectCount; i++)
        {
            ///
            Gizmos.DrawWireCube(pos, new Vector3(scale.x * gizmoSize.x, scale.y * gizmoSize.y, scale.z * gizmoSize.z));

            ///
            pos += positionIncrement;
            rotation += rotationIncrement;
            scale += scaleIncrement;
        }
    }

    public void Reset()
    {
        objectsParent = transform;
        startTransform = transform;
    }
#endif
}
