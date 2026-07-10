using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharpNeatLib.Maths
{
    public partial class FastRandom : IRandomGenerator
    {
        private const float IntRange = (float)int.MaxValue - (float)int.MinValue;
        private const float IntMinvalue = (float)int.MinValue;

        int IRandomGenerator.Range(int minInclusive, int maxExclusive)
        {
            return Next(minInclusive, maxExclusive);
        }

        float IRandomGenerator.Range(float minInclusive, float maxInclusive)
        {
            return (float)(NextDouble() * (maxInclusive - minInclusive) + minInclusive);
        }
    }

}