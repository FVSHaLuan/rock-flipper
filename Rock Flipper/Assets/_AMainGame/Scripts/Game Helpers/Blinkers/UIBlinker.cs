using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FH.Core.Gameplay.HelperComponent;

namespace FH.Core.HelperComponent
{
    [RequireComponent(typeof(Graphic))]
    public class UIBlinker : Blinker
    {
        [Header("UIBlinker")]
        [SerializeField]
        private Graphic targetRenderer;

        protected override bool IsVisible { get => targetRenderer.enabled; set => targetRenderer.enabled = value; }

#if UNITY_EDITOR
        protected void Reset()
        {
            targetRenderer = GetComponent<Graphic>();
        }
#endif
    }

}