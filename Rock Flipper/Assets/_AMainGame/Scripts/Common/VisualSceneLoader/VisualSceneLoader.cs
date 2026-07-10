using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Agame
{
    public class VisualSceneLoader : ExtendedMonoBehaviour
    {
        [SerializeField]
        private GameObject viewWrapper;
        [SerializeField]
        private MoveAlphaTo moveAlphaTo;
        [SerializeField]
        private float fadeInDuration;
        [SerializeField]
        private float fadeOutDuration;
        [SerializeField]
        private float waitingDuration;
        [SerializeField]
        private Image sceneIconImage;
        [SerializeField]
        private Sprite sceneHomeIcon;
        [SerializeField]
        private Sprite sceneRunIcon;

        [Space]
        [SerializeField]
        private GameObject currentWaveText;
        [SerializeField]
        private GameObject homeText;
        [SerializeField]
        private GameObject storyText;
        [SerializeField]
        private UnifiedText customText;

        private bool isLoading;
        private string loadingScene;
        private GameScene loadingGameScene;

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            if (!isLoading)
            {
                viewWrapper.SetActive(false);
            }
        }

        public static string GetGameSceneName(GameScene scene)
        {
            switch (scene)
            {
                case GameScene.Home:
                    return GameConst.SceneHomeName;
                case GameScene.Run:
                    return GameConst.SceneRunName;
                case GameScene.FakeOS:
                    return GameConst.SceneFakeOSName;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public void Load(GameScene scene, string customTextStr = null)
        {
            ///
            loadingGameScene = scene;

            ///
            Load(GetGameSceneName(scene), customTextStr);
        }

        private void Load(string sceneName, string customTextStr)
        {
            ///
            if (isLoading)
            {
                throw new System.InvalidOperationException(string.Format("Can not load scene ({0}) because there is another scene being loaded ({1}).", sceneName, loadingScene));
            }

            ///
            StartCoroutine(LoadSceneRoutine(sceneName, customTextStr));
        }

        private void ViewContent(string customTextStr)
        {
            ///
            bool notHasCustomText = string.IsNullOrWhiteSpace(customTextStr);

            ///
            currentWaveText.SetActive(false);
            homeText.SetActive(false);
            storyText.SetActive(false);

            ///
            if (!notHasCustomText)
            {
                customText.gameObject.SetActive(true);
                customText.Text = customTextStr;
            }
            else
            {
                customText.gameObject.SetActive(false);
            }

            ///
            switch (loadingGameScene)
            {
                case GameScene.Other:
                    throw new System.InvalidOperationException();
                case GameScene.Home:
                    homeText.SetActive(notHasCustomText);
                    sceneIconImage.sprite = sceneHomeIcon;
                    break;
                case GameScene.Run:
                    storyText.SetActive(notHasCustomText);
                    sceneIconImage.sprite = sceneRunIcon;
                    break;
                default:
                    break;
            }
        }

        private IEnumerator LoadSceneRoutine(string sceneName, string customTextStr)
        {
            ///
            entry.completeInputBlocker.AddBlockLock(this);

            ///
            loadingScene = sceneName;
            isLoading = true;
            viewWrapper.SetActive(true);

            ///
            ViewContent(customTextStr);

            // Fade in
            moveAlphaTo.Speed = 1.0f / fadeInDuration;
            moveAlphaTo.SetAlphaImmediately(0);
            moveAlphaTo.MoveTo(1);
            while (moveAlphaTo.IsMoving)
            {
                yield return null;
            }

            ///
            yield return new WaitForSecondsRealtime(waitingDuration);

            ///
            GC.Collect();

            ///
            SceneManager.LoadScene(sceneName);

            ///
            yield return new WaitForSecondsRealtime(0.2f);

            // Fade out
            moveAlphaTo.Speed = 1.0f / fadeOutDuration;
            moveAlphaTo.SetAlphaImmediately(1);
            moveAlphaTo.MoveTo(0);
            while (moveAlphaTo.IsMoving)
            {
                yield return null;
            }

            ///
            isLoading = false;
            viewWrapper.SetActive(false);

            ///
            entry.completeInputBlocker.RemoveBlockLock(this);
        }
    }
}
