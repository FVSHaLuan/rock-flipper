using FH.Core.HelperComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FH.Core.HelperComponent
{
    [DisallowMultipleComponent]
    public class OrderedEventsTriggererWithTime : MonoBehaviour, IEventTriggererWithTime
    {
        [SerializeField]
        private TimeScaleMode timeScaleMode = TimeScaleMode.ScaledTime;
        [SerializeField]
        private bool triggerOnEnable;

        [Space]
        [SerializeField]
        private List<UnityEventWithTime> unityEventWithTimes;

        private bool isExecuting;
        private IEventTriggererWithTimeCallback finishCallback;

        [System.Serializable]
        private class UnityEventWithTime
        {
            public string name;
            public float minTime = -1;
            public bool waitAtLeastOneFrame = false;
            public UnityEvent unityEvent;

            public bool ShouldIgnore
            {
                get
                {
                    ///
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return false;
                    }

                    ///
                    return name[0] == '_';
                }
            }
        }

        protected void OnEnable()
        {
            if (triggerOnEnable)
            {
                TriggerInCoroutine();
            }
        }

        protected void OnDisable()
        {
            isExecuting = false;
            StopAllCoroutines();
        }

        [ContextMenu("TriggerInCoroutine")]
        public void TriggerInCoroutine()
        {
            Trigger(null);
        }

        private IEnumerator ExecuteEvents()
        {
            ///
            isExecuting = true;

            ///
            for (int i = 0; i < unityEventWithTimes.Count; i++)
            {
                ///
                var unityEventWithTime = unityEventWithTimes[i];

                ///
                if (unityEventWithTime == null)
                {
                    continue;
                }

                ///
                if (unityEventWithTime.ShouldIgnore)
                {
                    ///
                    Debug.LogWarningFormat("OrderedEventsTriggererWithTime, Ignored {0}", unityEventWithTime.name);

                    ///
                    continue;
                }

                ///
                unityEventWithTime.unityEvent?.Invoke();

                ///
                if (unityEventWithTime.minTime > 0)
                {
                    yield return new WaitForUniversalTime(unityEventWithTime.minTime, timeScaleMode);
                }
                else if (unityEventWithTime.waitAtLeastOneFrame)
                {
                    yield return null;
                }
            }

            ///
            isExecuting = false;

            ///
            finishCallback?.Invoke(this);
        }

        public void Trigger(IEventTriggererWithTimeCallback finishCallback)
        {
            ///
            if (isExecuting)
            {
                return;
            }

            ///
            this.finishCallback = finishCallback;

            ///
            StartCoroutine(ExecuteEvents());
        }

        public void Trigger()
        {
            TriggerInCoroutine();
        }
    }
}
