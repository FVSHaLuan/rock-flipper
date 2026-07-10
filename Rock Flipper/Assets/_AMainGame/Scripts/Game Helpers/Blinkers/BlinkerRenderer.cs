using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Gameplay.HelperComponent
{
    public class BlinkerRenderer : Blinker
    {
        [Header("BlinkerRenderer")]
        [SerializeField]
        private Renderer targetRenderer;

        protected override bool IsVisible { get => targetRenderer.enabled; set => targetRenderer.enabled = value; }
    }
}
