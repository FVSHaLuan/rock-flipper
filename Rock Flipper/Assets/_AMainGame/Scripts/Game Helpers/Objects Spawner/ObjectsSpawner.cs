using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ObjectsSpawner : ExtendedMonoBehaviour
{
    [Header("ObjectsSpawner")]
    [SerializeField, FormerlySerializedAs("cellContentParent")]
    protected Transform objectsParent;

    [Header("ObjectsSpawner - Preset")]
    [SerializeField, FormerlySerializedAs("cellContentPrototype"), FormerlySerializedAs("objectContentPrototype")]
    protected GeneralPoolMemberSimplified objectPrototype;
    [SerializeField, Range(0, 1), FormerlySerializedAs("cellCountPortion")]
    protected float objectCountPortion = 0.5f;
    [SerializeField]
    protected float spawnTimeInterval = -1;

    public float SpawnTimeInterval => spawnTimeInterval;

    public IEnumerable<GeneralPoolMemberSimplified> GenerateCells(float cellCountPortion, GeneralPoolMemberSimplified cellPrototype)
    {
        ///
        generalPool.TryPushPrototype(cellPrototype);
        var prototypeId = cellPrototype.PrototypeId;

        ///
        return GenerateCells(cellCountPortion, prototypeId);
    }

    protected abstract IEnumerable<GeneralPoolMemberSimplified> GenerateCells(float cellCountPortion, int prototypeId);

    [ContextMenu("GeneratePreset")]
    public void GeneratePreset()
    {
        StartCoroutine(GeneratePresetCoroutine());
    }

    private IEnumerator GeneratePresetCoroutine()
    {
        foreach (var item in GenerateCells(objectCountPortion, objectPrototype))
        {
            ///
            item.gameObject.SetActive(true);

            ///
            if (spawnTimeInterval <= 0)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(spawnTimeInterval);
            }
        }
    }

#if UNITY_EDITOR
    protected void SetGizmosMatrix()
    {
        if (objectsParent != null)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(objectsParent.position, objectsParent.rotation, objectsParent.lossyScale);
            Gizmos.matrix = rotationMatrix;
        }
    }
#endif
}
