using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FH.Core.HelperComponent
{
    public class WaitForMonoBehaviourToDisabled : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviorEvent monoBehaviorEvent;
        [SerializeField]
        private bool checkOnEnable = true;
        [SerializeField]
        private bool ignoreStateOnCheck;

        [Space]
        [SerializeField]
        private UnityEvent onTargetDisabled;

        private bool checkedTarget = false;

        public void Check()
        {
            ///
            if (checkedTarget)
            {
                return;
            }

            ///
            checkedTarget = true;

            ///
            if (!monoBehaviorEvent.isActiveAndEnabled && !ignoreStateOnCheck)
            {
                ///
                onTargetDisabled?.Invoke();

                ///
                return;
            }

            ///           
            monoBehaviorEvent.OnDisabled += MonoBehaviorEvent_OnDisabled;
        }

        protected void OnEnable()
        {
            if (checkOnEnable)
            {
                Check();
            }
        }

        protected void OnDisable()
        {
            checkedTarget = false;
            monoBehaviorEvent.OnDisabled -= MonoBehaviorEvent_OnDisabled;
        }

        private void MonoBehaviorEvent_OnDisabled()
        {
            onTargetDisabled?.Invoke();
        }
    }
}