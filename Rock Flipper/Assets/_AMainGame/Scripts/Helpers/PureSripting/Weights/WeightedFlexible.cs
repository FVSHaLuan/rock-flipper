using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedFlexible<T> : IWeightedFlexible
{
    public T Value { get; set; }
    public float Weight { get; set; }
    public bool IsFlexible { get; set; }

    public WeightedFlexible(T value, float weight, bool isFlexible)
    {
        Value = value;
        Weight = weight;
        IsFlexible = isFlexible;
    }
}
