using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.LoadingScreen
{
    public class LoadingScreenHandle : MonoBehaviourWithInit
    {
        public event System.Action OnFinished;

        public static LoadingScreenHandle Instance { get; private set; }

        public bool IsFinished { get; private set; } = false;

        public static bool IsNullOrFinished => Instance == null || Instance.IsFinished;

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            Instance = this;

            ///
            DontDestroyOnLoad(gameObject);

            ///
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void SceneManager_sceneUnloaded(UnityEngine.SceneManagement.Scene arg0)
        {
            ///
            IsFinished = true;

            ///
            OnFinished?.Invoke();
        }
    }
}
