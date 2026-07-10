using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace FH.Core.Gameplay.HelperComponent
{
    public abstract class Blinker : MonoBehaviourWithInit
    {
        [Header("Blinker")]
        [SerializeField]
        private float showingTime = 0.2f;
        [SerializeField]
        private float hiddenTime = 0.2f;
        [SerializeField]
        private bool defaultState = true;
        [SerializeField]
        private bool startBlinkingState = true;
        [SerializeField]
        private bool randomizedStartBlinkingState = false;

        [Header("Disable control")]
        [SerializeField]
        private bool controlDisabledState;
        [SerializeField, Tooltip("state of visual component when this Blinker disabled")]
        private bool disabledState;

        [Space]
        [SerializeField]
        private float maxAdditionalShowingTime = 0;
        [SerializeField]
        private float maxAdditionalHiddenTime = 0;

        [Space]
        [SerializeField]
        private bool isBlinking;

        [Space]
        [SerializeField]
        private TimeScaleMode timeScaleMode = TimeScaleMode.GameplayUnscaledTime;

        [Space]
        [SerializeField]
        private UnityEvent onBlinkingHide;
        [SerializeField]
        private UnityEvent onBlinkingShow;

        private float timeTracking = 0;
        private float currentInterval;

        public int BlinkCount { get; private set; }
        public bool IsBlinking
        {
            get => isBlinking;

            set
            {
                ///
                if (isBlinking == value)
                {
                    return;
                }

                ///
                isBlinking = value;

                ///
                if (isBlinking)
                {
                    StartBlinking();
                }
                else
                {
                    IsVisible = defaultState;
                }
            }
        }
        public float HiddenTime { get => hiddenTime; set => hiddenTime = value; }
        public float ShowingTime { get => showingTime; set => showingTime = value; }

        protected abstract bool IsVisible { get; set; }

        protected void OnEnable()
        {
            ///
            if (IsBlinking)
            {
                StartBlinking();
            }
            else
            {
                IsVisible = defaultState;
            }
        }

        protected void OnDisable()
        {
            ///
            if (controlDisabledState)
            {
                IsVisible = disabledState;
            }
        }

        private void StartBlinking()
        {
            ///
            var effectiveStartBlinkingState = randomizedStartBlinkingState ? startBlinkingState : (Random.value >= 0.5f);

            ///
            currentInterval = effectiveStartBlinkingState ? GetRandomShowingTime() : GetRandomHiddenTime();
            timeTracking = 0;
            IsVisible = effectiveStartBlinkingState;

            ///
            BlinkCount = 0;
            StopAllCoroutines();
        }

        public void Update()
        {
            ///
            if (!IsBlinking)
            {
                return;
            }

            ///
            timeTracking += Entry.Instance.timeScaleManager.GetDeltaTime(timeScaleMode);

            ///
            if (timeTracking >= currentInterval)
            {
                ///
                timeTracking = 0;
                IsVisible = !IsVisible;

                ///
                currentInterval = IsVisible ? GetRandomShowingTime() : GetRandomHiddenTime();

                ///
                BlinkCount++;

                ///
                if (IsVisible)
                {
                    onBlinkingShow?.Invoke();
                }
                else
                {
                    onBlinkingHide?.Invoke();
                }
            }
        }

        [ContextMenu("ToggleBlinking")]
        public void ToggleBlinking()
        {
            IsBlinking = !IsBlinking;
        }

        public void DoAfter(int minBlinkCount, bool isWhenVisible, System.Action callback)
        {
            StartCoroutine(DoAfterRoutine(minBlinkCount, isWhenVisible, callback));
        }

        private IEnumerator DoAfterRoutine(int minBlinkCount, bool isWhenVisible, System.Action callback)
        {
            ///
            int startBlinkCount = BlinkCount;

            ///
            while ((BlinkCount - startBlinkCount) < minBlinkCount || IsVisible != isWhenVisible)
            {
                yield return null;
            }

            ///
            callback?.Invoke();
        }

        private float GetRandomShowingTime()
        {
            return showingTime + Random.Range(0, maxAdditionalShowingTime);
        }

        private float GetRandomHiddenTime()
        {
            return hiddenTime + Random.Range(0, maxAdditionalHiddenTime);
        }
    }

}