using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityRandom : IRandomGenerator
{
    public static UnityRandom Default { get; private set; } = new UnityRandom();

    public bool NextBool()
    {
        return Random.value >= 0.5f;
    }

    public int Range(int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }

    public float Range(float minInclusive, float maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive);
    }

    public int NextInt()
    {
        return (int)Random.Range((float)int.MinValue, (float)int.MaxValue);
    }
}
