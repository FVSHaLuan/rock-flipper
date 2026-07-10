using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomGenerator
{
    public int Range(int minInclusive, int maxExclusive);
    public float Range(float minInclusive, float maxInclusive);
    public bool NextBool();
    public int NextInt();
}
