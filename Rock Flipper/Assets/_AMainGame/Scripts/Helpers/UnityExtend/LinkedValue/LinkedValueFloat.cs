using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinkedValueFloat : LinkedValue<float>
{
    [SerializeField]
    private LinkedValueFloatAsset asset;
    [SerializeField]
    private float factor = 1;
    [SerializeField]
    private float addition;

    public static implicit operator float(LinkedValueFloat linkedValueFloat)
    {
        ///
        if (linkedValueFloat == null)
        {
            return 0;
        }

        ///
        return linkedValueFloat.Value;
    }

    public LinkedValueFloat()
    {
        Init(0);
    }

    public LinkedValueFloat(float addition)
    {
        Init(addition);
    }

    private void Init(float addition)
    {
        factor = 1;
        this.addition = addition;
    }

    protected override float GetValue()
    {
        ///
        float value = asset != null ? asset.Value : 0;
        value *= factor;
        value += addition;

        ///
        return value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

#if UNITY_EDITOR
    public override bool CheckForCheckForMutualRefExternal(HashSet<ScriptableObject> previousObjects)
    {
        if (asset == null)
        {
            return false;
        }
        else
        {
            return asset.CheckForMutualRef(previousObjects);
        }
    } 
#endif
}
