using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FH.Core.HelperComponent;
using UnityEngine.Events;

namespace BT.GameplayHelpers
{
    public class ExecuteOrderedEventsOnMultipleObjects : ExtendedMonoBehaviourWithTime
    {
        [SerializeField]
        private float startDelay = -1;
        [SerializeField]
        private float interval = -1;
        [SerializeField]
        private bool isLooping;

        [Space]
        [SerializeField]
        private bool includeInactive;

        [Space]
        [SerializeField]
        private bool startOnEnable = true;

        [Space]
        [SerializeField]
        private UnityEvent onFinished;

        private List<IEventTriggererWithTime> eventTriggererWithTimes;

        private int currentObjectId;
        private bool isExecuting;

        protected override bool Init()
        {
            ///
            eventTriggererWithTimes = new List<IEventTriggererWithTime>();

            ///
            transform.GetComponentsInChildren(includeInactive, eventTriggererWithTimes);

            ///
            return base.Init();
        }

        protected void OnEnable()
        {
            if (startOnEnable)
            {
                StartExecuting();
            }
        }

        [ContextMenu("StartExecuting")]
        public void StartExecuting()
        {
            ///
            TryInit();

            ///
            if (isExecuting)
            {
                return;
            }

            ///
            isExecuting = true;

            ///
            StartCoroutine(Execute(0, startDelay));
        }

        private IEnumerator Execute(int objectId, float delay)
        {
            ///
            if (delay < 0)
            {
                yield return null;
            }

            ///
            if (!isExecuting)
            {
                yield break;
            }

            ///
            if (delay > 0)
            {
                yield return new WaitForUniversalTime(delay, timeScaleMode);
            }

            ///
            if (!isExecuting)
            {
                yield break;
            }

            ///
            currentObjectId = objectId;

            ///
            (eventTriggererWithTimes[currentObjectId] as Component)?.gameObject.SetActive(true);

            ///
            eventTriggererWithTimes[currentObjectId].Trigger(OnEventFinished);
        }

        private void OnEventFinished(IEventTriggererWithTime eventTriggererWithTime)
        {
            ///
            if (!isExecuting || eventTriggererWithTimes[currentObjectId] != eventTriggererWithTime)
            {
                return;
            }

            ///
            if (currentObjectId + 1 < eventTriggererWithTimes.Count)
            {
                StartCoroutine(Execute(currentObjectId + 1, interval));
            }
            else if (isLooping)
            {
                StartCoroutine(Execute(0, interval));
            }
            else
            {
                ///
                isExecuting = false;

                ///
                onFinished?.Invoke();
            }
        }

        protected void OnDisable()
        {
            ///
            isExecuting = false;

            ///
            StopAllCoroutines();
        }
    }
}
