using UnityEngine;
using System.Collections;
using FH.Core.Architecture.UI;
using FH.Core.Architecture;

namespace FH.Core.HelperComponent
{
    public class CountDownText : TextContentSetter
    {
        [SerializeField]
        private int startNumber = 3;
        [SerializeField]
        private int endNumber = 1;
        [SerializeField]
        private float interval = 1;
        [SerializeField]
        private bool callTickEventOnLastTick;
        [SerializeField]
        private TimeScaleMode gameplayTimeScaleMode = TimeScaleMode.GameplayUnscaledTime;


        [Space]
        [SerializeField]
        private OrderedEventDispatcher onTick;
        [SerializeField]
        private OrderedEventDispatcher onLastTick;
        [SerializeField]
        private OrderedEventDispatcher onFinish = new OrderedEventDispatcher();

        private int currentNumber = 0;
        private bool counting = false;

        protected override void SetContent()
        {
            Text.text = currentNumber.ToString();
        }

        protected void OnDisable()
        {
            currentNumber = startNumber;
        }

        [ContextMenu("StartCountDown")]
        public void StartCountDown()
        {
            if (!counting)
            {
                StartCoroutine(CountDownAsync());
            }
        }

        public void StopCounting()
        {
            counting = false;
        }

        IEnumerator CountDownAsync()
        {
            counting = true;
            float timeTracking = 0;

            currentNumber = startNumber;
            while (currentNumber != endNumber)
            {
                if (!counting)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
                timeTracking += Entry.Instance.timeScaleManager.GetDeltaTime(gameplayTimeScaleMode);
                if (timeTracking >= interval)
                {
                    ///
                    timeTracking = 0;
                    currentNumber = (int)Mathf.MoveTowards(currentNumber, endNumber, 1);

                    ///
                    if (currentNumber > 0 || callTickEventOnLastTick)
                    {
                        onTick?.Dispatch();
                    }

                    ///
                    if (currentNumber == 0)
                    {
                        onLastTick?.Dispatch();
                    }
                }
            }

            timeTracking = 0;
            while (timeTracking < interval)
            {
                if (!counting)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
                timeTracking += Entry.Instance.timeScaleManager.GetDeltaTime(gameplayTimeScaleMode);
            }

            if (counting)
            {

                onFinish.Dispatch();

                counting = false;
            };
        }

        public void OnValidate()
        {
            setContentAtUpdate = true;
        }
    }

}