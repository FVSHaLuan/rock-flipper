using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.UI.LoadingScreen
{
    public class LoadingScreenHandleEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onLoadingScreenNotFoundOrFinished;
        [SerializeField]
        private UnityEvent onFoundLoadingScreen;
        [SerializeField]
        private UnityEvent onLoadingScreenFinished;

        protected void OnEnable()
        {
            ///
            var handle = LoadingScreenHandle.Instance;

            ///
            if (handle != null && !handle.IsFinished)
            {
                ///
                onFoundLoadingScreen?.Invoke();

                ///
                handle.OnFinished += Handle_OnFinished;
            }
            else
            {
                onLoadingScreenNotFoundOrFinished?.Invoke();
            }
        }

        protected void OnDisable()
        {
            if (LoadingScreenHandle.Instance != null)
            {
                LoadingScreenHandle.Instance.OnFinished -= Handle_OnFinished;
            }
        }

        private void Handle_OnFinished()
        {
            onLoadingScreenFinished?.Invoke();
        }
    }
}
