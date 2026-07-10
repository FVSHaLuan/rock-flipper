using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIntDisplayer : OneTimeTextDisplayer<int>
{
    [SerializeField]
    private int minInclusiveValue = int.MinValue;
    [SerializeField]
    private int maxExclusiveValue = int.MaxValue;

    protected override int GetValue()
    {
        return Random.Range(minInclusiveValue, maxExclusiveValue);
    }
}
