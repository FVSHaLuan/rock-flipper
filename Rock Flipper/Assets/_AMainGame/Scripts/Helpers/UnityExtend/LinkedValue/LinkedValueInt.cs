using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinkedValueInt : LinkedValue<int>
{
    [SerializeField, Weight(2)]
    private LinkedValueIntAsset asset;
    [SerializeField, Weight(0.9f)]
    private float factor = 1;
    [SerializeField, Weight(0.9f)]
    private int addition;
    [SerializeField, Weight(0.8f)]
    private FloatToIntPolicy policy = FloatToIntPolicy.Round;

    public LinkedValueIntAsset Asset => asset;
    public float Factor => factor;
    public int Addition => addition;
    public FloatToIntPolicy Policy => policy;

    public enum FloatToIntPolicy
    {
        Round = 0,
        Floor = 1,
        Ceil = 2,
    }

    public static implicit operator int(LinkedValueInt linkedValueInt)
    {
        ///
        if (linkedValueInt == null)
        {
            return 0;
        }

        ///
        return linkedValueInt.Value;
    }

    public LinkedValueInt(LinkedValueIntAsset asset, float factor, int addition, FloatToIntPolicy policy)
    {
        Init(asset, factor, addition, policy);
    }

    public LinkedValueInt()
    {
        Init(0);
    }

    public LinkedValueInt(int addition)
    {
        Init(addition);
    }

    private void Init(int addition)
    {
        Init(null, 1, addition, FloatToIntPolicy.Round);
    }

    private void Init(LinkedValueIntAsset asset, float factor, int addition, FloatToIntPolicy policy)
    {
        this.asset = asset;
        this.factor = factor;
        this.addition = addition;
        this.policy = policy;
    }

    protected override int GetValue()
    {
        ///
        float value = asset != null ? asset.Value : 0;
        value *= factor;
        value += addition;

        ///
        switch (policy)
        {
            case FloatToIntPolicy.Round:
                return Mathf.RoundToInt(value);
            case FloatToIntPolicy.Ceil:
                return Mathf.CeilToInt(value);
            case FloatToIntPolicy.Floor:
                return Mathf.FloorToInt(value);
            default:
                throw new System.NotImplementedException();
        }
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
