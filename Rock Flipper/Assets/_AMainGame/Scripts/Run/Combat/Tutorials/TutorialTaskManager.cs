using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat.Tutorials
{
    public class TutorialTaskManager : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private UnifiedText taskText;
        [SerializeField]
        private GameObject taskProgressBarView;
        [SerializeField]
        private ProgressBar taskProgress;
        [SerializeField]
        private UnifiedText hintText;

        [Space]
        [SerializeField]
        private CanvasGroup completeTaskCheckMark;
        [SerializeField]
        private GameAudioController completeTaskSfx;

        [Space]
        [SerializeField]
        private float nextTaskDelay;
        [SerializeField]
        private float completeCheckMarkFadingDuration = 0.5f;

        [Space]
        [SerializeField]
        private UIScreen finishedTutorialScreen;

        [Space]
        [SerializeField]
        private Transform taskControllerRoot;

        private List<TutorialTaskController> taskControllers = new List<TutorialTaskController>();

        private int currentTaskIndex = 0;

        protected void Start()
        {
            if (!IsTutorial)
            {
                return;
            }

            ///
            completeTaskCheckMark.gameObject.SetActive(false);

            ///
            taskControllerRoot.GetComponentsInChildren(true, taskControllers);
            foreach (var item in taskControllers)
            {
                item.gameObject.SetActive(false);
            }

            ///
            StartTask(taskControllers[0]);
        }

        private void StartTask(TutorialTaskController taskController)
        {
            ///
            taskText.Text = $"<b>Tutorial Task [{currentTaskIndex + 1}/{taskControllers.Count}]:</b> {taskController.TaskDescription}";
            hintText.Text = $"<b>Hint:</b> {taskController.TaskHint}";
            taskProgressBarView.SetActive(!taskController.HideTaskProgressBar);

            ///
            taskProgress.SetValue(0);

            ///
            taskController.StartWith(this);
        }

        public void NextTask()
        {
            currentTaskIndex++;

            ///
            StopAllCoroutines();

            ///
            if (currentTaskIndex < taskControllers.Count)
            {
                StartCoroutine(NextTaskDelay());
            }
            else
            {
                // Finish tutorial
                StartCoroutine(FinishTutorialDelay());
            }
        }

        public void SetTaskProgress(float progress)
        {
            taskProgress.SetValue(progress);
        }

        private IEnumerator PlayTaskCompletionEffect()
        {
            completeTaskCheckMark.gameObject.SetActive(true);
            completeTaskCheckMark.alpha = 1f;
            completeTaskSfx.Play();

            ///
            yield return new WaitForSecondsRealtime(nextTaskDelay);
        }

        private IEnumerator NextTaskDelay()
        {
            ///
            yield return StartCoroutine(PlayTaskCompletionEffect());

            ///            
            StartTask(taskControllers[currentTaskIndex]);

            ///
            yield return StartCoroutine(FadeCompleteTaskCheckMark());
        }

        private IEnumerator FinishTutorialDelay()
        {
            ///
            yield return StartCoroutine(PlayTaskCompletionEffect());

            ///
            finishedTutorialScreen.gameObject.SetActive(true);
        }

        private IEnumerator FadeCompleteTaskCheckMark()
        {
            float elapsedTime = 0f;
            float startAlpha = completeTaskCheckMark.alpha;
            float targetAlpha = 0f; // Fade out to fully transparent

            while (elapsedTime < completeCheckMarkFadingDuration)
            {
                elapsedTime += Time.deltaTime;
                completeTaskCheckMark.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / completeCheckMarkFadingDuration);
                yield return null;
            }

            completeTaskCheckMark.alpha = targetAlpha; // Ensure it ends at the target alpha
            completeTaskCheckMark.gameObject.SetActive(false); // Optionally disable the GameObject after fading
        }

    }

}