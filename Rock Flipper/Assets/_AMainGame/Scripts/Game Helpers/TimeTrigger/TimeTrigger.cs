using UnityEngine;
using System.Collections;
using FH.Core.Architecture;
using UnityEngine.Serialization;
using UnityEngine.Assertions;

namespace FH.Core.Gameplay.HelperComponent
{
    public class TimeTrigger : ExtendedMonoBehaviour
    {
        public event System.Action OnBeforeCountingDown;

        [SerializeField]
        private float countDownTime = 2;
        [SerializeField]
        private float maxAdditionalTime = 0;

        [Space]
        [SerializeField]
        private bool unscaledTime = false;
        [SerializeField]
        private bool useGameplayUnscaledTime = false;

        [Space]
        [SerializeField, ReadOnly]
        private bool ignoreNextCountDown;

        [Space]
        [SerializeField, FormerlySerializedAs("timeOutEvenent")]
        private OrderedEventDispatcher timeOutEvent = new OrderedEventDispatcher();

        private float currentTime;

        public bool IsCounting => isCounting;

        public bool IgnoreNextCountDown
        {
            get => ignoreNextCountDown;
            set
            {
                ///
                ignoreNextCountDown = value;

                ///
                if (ignoreNextCountDown)
                {
                    isCounting = false;
                    enabled = false;
                }
            }
        }

        public float CountDownTime
        {
            get
            {
                return countDownTime;
            }

            set
            {
                countDownTime = value;
            }
        }

        private bool isCounting = false;

        protected override void ExtendedAwake()
        {
            if (!isCounting)
            {
                enabled = false;
            }
        }

        public void RestartCounting()
        {
            ///            
            StartCountDown();
        }

        public void TimeOut()
        {
            if (isActiveAndEnabled && isCounting)
            {
                currentTime = 0;
                Update();
            }
            else
            {
                timeOutEvent?.Dispatch();
            }
        }

        /// <summary>
        /// !!!Shouldn't change this name, it's being called in Editor
        /// </summary>
        public void TimeOutIfCouting()
        {
            if (isActiveAndEnabled && isCounting)
            {
                currentTime = 0;
                Update();
            }
        }

        public void TriggerTimeOutEvent()
        {
            timeOutEvent.Dispatch();
        }

        public void StopCountDown()
        {
            isCounting = false;
            enabled = false;
        }

        protected void Update()
        {
            ///
            if (!isCounting)
            {
                return;
            }

            ///
            var effectiveDeltaTime = unscaledTime ? (useGameplayUnscaledTime ? GameplayUnscaledDeltaTime : Time.unscaledDeltaTime) : Time.deltaTime;

            ///
            currentTime -= effectiveDeltaTime;
            if (currentTime <= 0)
            {
                timeOutEvent.Dispatch();
                enabled = false;
            }
        }

        public void StartCountDown()
        {
            ///
            if (ignoreNextCountDown)
            {
                ignoreNextCountDown = false;
                return;
            }

            ///
            OnBeforeCountingDown?.Invoke();

            ///
            isCounting = true;
            enabled = true;
            currentTime = CountDownTime + Random.Range(0, maxAdditionalTime);
        }

        public void StartCountDownDelay(float delay)
        {
            this.InvokeDelay(StartCountDown, delay);
        }

        public void OnDisable()
        {
            isCounting = false;
        }
    }

}