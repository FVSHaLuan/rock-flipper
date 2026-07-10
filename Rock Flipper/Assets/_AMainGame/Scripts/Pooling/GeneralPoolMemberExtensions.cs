using Agame;
using FH.Core.Architecture.Pool;
using UnityEngine;

public static class GeneralPoolMemberExtensions
{
    private static object GeneralPoolMemberExtensionsRepresentation = new object();

    public static T SpawnFromEntryPool<T>(this T prototype, Vector3 position, Transform parent = null) where T : GeneralPoolMemberSimplified
    {
        return Spawn(prototype, Entry.Instance.GeneralPool, position, parent);
    }

    public static T SpawnFromCurrentScenePool<T>(this T prototype, Vector3 position, Transform parent = null) where T : GeneralPoolMemberSimplified
    {
        return Spawn(prototype, CommonEntry.CommonInstance.GeneralPool, position, parent);
    }

    public static T Spawn<T>(this T prototype, GeneralPool generalPool, Vector3 position, Transform parent = null) where T : GeneralPoolMemberSimplified
    {
        ///
        var instance = generalPool.TakeInstance(prototype, GeneralPoolMemberExtensionsRepresentation);

        ///
        instance.transform.position = position;
        instance.transform.SetParent(parent);
        instance.transform.localScale = Vector3.one;

        ///
        instance.gameObject.SetActive(true);

        ///
        return instance;
    }
}
