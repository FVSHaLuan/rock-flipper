using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT
{
    public class TightScreenDetector : MonoBehaviourWithInit
    {
        public event System.Action OnTightStateChanged;

        [SerializeField]
        private Camera uiCamera;

        private bool isTight;

        public bool IsTight
        {
            get
            {
                TryInit();

                ///
                return isTight;
            }
        }

        protected override bool Init()
        {
            ///
            CheckTight();

            ///
            return base.Init();
        }

        private void CheckTight()
        {
            isTight = uiCamera.aspect < ScreenSizeConfig.MinWidthRatio;
        }

        protected void Update()
        {
            var savedIsTight = isTight;
            CheckTight();

            ///
            if (savedIsTight != isTight)
            {
                OnTightStateChanged?.Invoke();
            }
        }
    }

}