using UnityEngine;
using System.Collections;
using FH.Core.Architecture;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using FHC.Core.Architecture;

namespace FH.Core.HelperComponent
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private int sceneId = -1;
        [SerializeField]
        private string sceneName = "";
        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        [Header("Async loading")]
        [SerializeField]
        private bool async = false;
        [SerializeField]
        private float mininumSyncLoadingTime = 0;

        [Header("Events")]
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("onFinishedAddictiveLoading")]
        private UnityEvent onFinishedAdditiveLoading;

        private BalancerWithObjects sceneActivationBalancer = new BalancerWithObjects();

        public void AddPauseActivationLock(Object @object)
        {
            sceneActivationBalancer.AddObject(@object);
        }

        public void RemovePauseActivationLock(Object @object)
        {
            sceneActivationBalancer.RemoveObject(@object);
        }

        public void Load()
        {
            if (async)
            {
                StartCoroutine(LoadSceneAsync());
            }
            else
            {
                ///
                if (sceneId >= 0)
                {
                    ///
                    SceneManager.LoadScene(sceneId, loadSceneMode);
                }
                else
                {
                    SceneManager.LoadScene(sceneName, loadSceneMode);
                }

                ///
                if (loadSceneMode == LoadSceneMode.Additive)
                {
                    onFinishedAdditiveLoading?.Invoke();
                }
            }
        }

        private IEnumerator LoadSceneAsync()
        {
            ///
            var asyncOperation = sceneId >= 0 ? SceneManager.LoadSceneAsync(sceneId, loadSceneMode) : SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            if (mininumSyncLoadingTime == 0)
            {
                asyncOperation.allowSceneActivation = true;
            }
            else
            {
                asyncOperation.allowSceneActivation = false;
                float timeTracking = 0;
                while (timeTracking < mininumSyncLoadingTime)
                {
                    timeTracking += Time.unscaledDeltaTime;
                    yield return null;
                }
                asyncOperation.allowSceneActivation = true;
            }

            ///
            if (loadSceneMode == LoadSceneMode.Additive)
            {
                ///
                while (!asyncOperation.isDone || !sceneActivationBalancer.IsBalanced)
                {
                    yield return null;
                }

                ///
                onFinishedAdditiveLoading?.Invoke();
            }

            ///
            yield return null;
        }
    }

}