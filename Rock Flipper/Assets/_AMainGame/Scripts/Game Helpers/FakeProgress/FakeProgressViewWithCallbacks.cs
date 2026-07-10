using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.GameplayHelpers
{
    public class FakeProgressViewWithCallbacks : ExtendedMonoBehaviourWithTime
    {
        [SerializeField]
        private FakeProgress fakeProgress = new FakeProgress();

        [Space]
        [SerializeField]
        private float regularCallbackInterval = 0.01f;
        [SerializeField]
        private int maxRegularCallbackCountInOneFrame = 1;

        [Space]
        [SerializeField]
        private ProgressBar progressBar;

        [Space]
        [SerializeField]
        private UnityEvent regularCallback;

        [Space]
        [SerializeField]
        private List<CallbackPoint> callbackPoints = new List<CallbackPoint>();

        [System.Serializable]
        private struct CallbackPoint
        {
            public string name;
            public float minProgress;

            [Space]
            [SerializeField]
            private UnityEvent onCalledBack;

            public void TriggerCallback()
            {
                onCalledBack.Invoke();
            }
        }

        private float savedProgress;
        private int nextCallbackPointId = 0;

        [ContextMenu("ResetProgress")]
        public void ResetProgress()
        {
            savedProgress = 0;
            nextCallbackPointId = 0;
            fakeProgress.Reset();
        }

        protected void Update()
        {
            ///
            fakeProgress.Update(GameplayDeltaTime);

            ///
            UpdateView();

            ///
            TryToInvokeCallbacks();

            ///
            TryToInvokeRegularCallback();
        }

        private void TryToInvokeRegularCallback()
        {
            ///
            int callcount = 0;

            ///
            while (callcount < maxRegularCallbackCountInOneFrame && (savedProgress + regularCallbackInterval) <= fakeProgress.Progress)
            {
                ///
                savedProgress += regularCallbackInterval;

                ///
                regularCallback?.Invoke();
            }
        }

        private void TryToInvokeCallbacks()
        {
            while (nextCallbackPointId < callbackPoints.Count)
            {
                ///
                var callbackPoint = callbackPoints[nextCallbackPointId];

                ///
                if (callbackPoint.minProgress <= fakeProgress.Progress)
                {
                    ///
                    callbackPoint.TriggerCallback();

                    ///
                    nextCallbackPointId++;
                }
                else
                {
                    break;
                }
            }
        }

        private void UpdateView()
        {
            progressBar?.SetValue(fakeProgress.Progress);
        }
    }

}