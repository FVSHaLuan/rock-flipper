using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedValueIntAsset : LinkedValueAsset<int>
{
    [SerializeField, OneLineWithHeader]
    private LinkedValueInt value = new LinkedValueInt(0);

    public override int Value => value == null ? 0 : value.Value;

#if UNITY_EDITOR
    protected override bool CheckForMutualRefExternal(HashSet<ScriptableObject> previousObjects)
    {
        return value == null ? false : value.CheckForCheckForMutualRefExternal(previousObjects);
    }
#endif
}
