using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedValueFloatAsset : LinkedValueAsset<float>
{
    [SerializeField, OneLineWithHeader]
    private LinkedValueFloat value = new LinkedValueFloat(0);

    public override float Value => value == null ? 0 : value.Value;

    public static implicit operator float(LinkedValueFloatAsset linkedValueFloatAsset)
    {
        ///
        if (linkedValueFloatAsset == null)
        {
            return 0;
        }

        ///
        return linkedValueFloatAsset.Value;
    }

#if UNITY_EDITOR
    protected override bool CheckForMutualRefExternal(HashSet<ScriptableObject> previousObjects)
    {
        return value == null ? false : value.CheckForCheckForMutualRefExternal(previousObjects);
    }
#endif
}
