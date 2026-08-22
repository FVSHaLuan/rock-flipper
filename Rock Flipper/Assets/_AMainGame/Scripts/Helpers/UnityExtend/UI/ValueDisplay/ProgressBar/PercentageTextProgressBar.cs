using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    public class PercentageTextProgressBar : ProgressBar
    {
        [Header("PercentageTextProgressBar")]
        [SerializeField]
        private UnifiedText unifiedText;
        [SerializeField]
        private string format = "{0:0.000}%";
        [SerializeField]
        private float valueScaleFactor = 100;

        private float value;
        private float viewingValue;
        private bool viewedOnce = false;

        public override float Value { get => value; protected set => this.value = value; }

        protected override void DisplayValue(float value)
        {
            if (!viewedOnce || !Mathf.Approximately(viewingValue, value))
            {
                ///
                viewedOnce = true;

                ///
                viewingValue = value;

                ///
                unifiedText.Text = string.Format(format, value * valueScaleFactor);
            }
        }
    }

}