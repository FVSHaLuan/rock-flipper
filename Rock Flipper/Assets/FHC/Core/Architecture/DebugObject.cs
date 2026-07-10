using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DebugObject
{
    [SerializeField, ReadOnly]
    private string name;
    [SerializeField, ReadOnly]
    private Object unityObject;
    [SerializeField, ReadOnly, SerializeReference]
    private object @object;

    [System.NonSerialized]
    private object originalObject;

    public object OriginalObject => originalObject;

    public static DebugObject FromObject(object originalObject)
    {
        return new DebugObject()
        {
            originalObject = originalObject,
            name = originalObject.ToString(),
            unityObject = (originalObject is Object) ? (Object)originalObject : null,
            @object = (originalObject is Object) ? null : originalObject,
        };
    }
}
