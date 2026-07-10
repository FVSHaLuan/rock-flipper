using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Gameplay.HelperComponent
{
    public class BlinkerActivation : Blinker
    {
        [Space]
        [SerializeField]
        private GameObject targetGameObject;

        protected override bool IsVisible { get => targetGameObject.activeSelf; set => targetGameObject.SetActive(value); }
    }

}