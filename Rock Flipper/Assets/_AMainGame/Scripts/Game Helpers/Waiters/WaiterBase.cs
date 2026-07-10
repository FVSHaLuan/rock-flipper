using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.GameplayHelpers
{
    public abstract class WaiterBase : ExtendedMonoBehaviour
    {
        [SerializeField]
        private bool startWaitingOnEnable = false;

        [Space]
        [SerializeField]
        private UnityEvent onFinished;

        private bool isWating = false;

        protected abstract void OnStartedWaiting();

        protected virtual void OnFinishedWaiting() { }

        protected void Finish()
        {
            ///
            if (!isWating)
            {
                return;
            }

            ///
            isWating = false;

            ///
            OnFinishedWaiting();

            ///
            onFinished?.Invoke();
        }

        protected void OnEnable()
        {
            if (startWaitingOnEnable)
            {
                StartWaiting();
            }
        }

        protected void OnDisable()
        {
            ///
            isWating = false;

            ///
            OnFinishedWaiting();
        }

        [ContextMenu("Start Waiting")]
        public void StartWaiting()
        {
            ///
            if (!Application.isPlaying)
            {
                return;
            }

            ///
            if (isWating)
            {
                return;
            }

            ///
            isWating = true;

            ///
            OnStartedWaiting();
        }
    }
}