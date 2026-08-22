using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    public class RawProgressBar : ProgressBar
    {
        [SerializeField, Range(0, 1)]
        private float value;

        public override float Value { get => value; protected set => this.value = value; }

        protected override void DisplayValue(float value)
        {
            // Intendedly be blank
        }
    }
}
