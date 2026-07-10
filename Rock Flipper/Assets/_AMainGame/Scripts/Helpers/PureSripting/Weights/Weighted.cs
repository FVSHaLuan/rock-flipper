using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    public struct Weighted<T> : IWeighted
    {
        public float weight;
        public T Value { get; set; }
        public float Weight => weight;

        public Weighted(float weight, T value)
        {
            this.weight = weight;
            Value = value;
        }
    }

}