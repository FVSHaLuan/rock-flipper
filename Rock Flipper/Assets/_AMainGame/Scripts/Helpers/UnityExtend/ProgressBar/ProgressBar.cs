using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    public abstract class ProgressBar : MonoBehaviour, IProgressValueProvider
    {
        [Header("ProgressBar")]
        [SerializeField]
        protected bool inverted;

        public abstract float Value
        {
            get;
            protected set;
        }

        /// <summary>
        /// Same as Value
        /// </summary>
        public float Progress => Value;

        public void SetValue(float value)
        {
            ///
            float effectiveValue;

            ///
            if (inverted)
            {
                effectiveValue = 1 - Mathf.Clamp01(value);
            }
            else
            {
                effectiveValue = Mathf.Clamp01(value);
            }

            ///
            Value = effectiveValue;

            ///
            DisplayValue(effectiveValue);
        }

        protected abstract void DisplayValue(float value);
    }

}