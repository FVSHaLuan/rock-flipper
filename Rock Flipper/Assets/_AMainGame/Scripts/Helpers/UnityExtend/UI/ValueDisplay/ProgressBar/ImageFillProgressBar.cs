using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GD
{
    public class ImageFillProgressBar : ProgressBar
    {
        [Header("ImageFillProgressBar")]
        [SerializeField]
        Image image;

        public override float Value
        {
            get => image.fillAmount;
            protected set => image.fillAmount = value;
        }

        public void Reset()
        {
            image = GetComponent<Image>();
        }

        protected override void DisplayValue(float value)
        {

        }
    }

}