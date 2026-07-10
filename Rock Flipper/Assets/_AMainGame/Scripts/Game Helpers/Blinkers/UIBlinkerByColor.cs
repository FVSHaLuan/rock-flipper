using FH.Core.Gameplay.HelperComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FH.Core.HelperComponent
{
    [RequireComponent(typeof(Graphic))]
    public class UIBlinkerByColor : Blinker
    {
        [Header("UIBlinker")]
        [SerializeField]
        private Graphic targetRenderer;

        [Space]
        [SerializeField]
        private Color onColor;
        [SerializeField]
        private Color offColor;

        private bool isVisible;

        protected override bool Init()
        {
            ///
            UpdateColor();

            ///
            return base.Init();
        }

        protected override bool IsVisible
        {
            get
            {
                ///
                TryInit();

                ///
                return isVisible;
            }

            set
            {
                ///
                TryInit();
                isVisible = value;
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            targetRenderer.color = isVisible ? onColor : offColor;
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            targetRenderer = GetComponent<Graphic>();
            onColor = targetRenderer.color;
            offColor = targetRenderer.color;
            offColor.a = 0;
        }
#endif
    }
}
