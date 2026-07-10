using UnityEngine;

namespace BT.Run.Combat.Tutorials
{
    public abstract class TutorialTaskController : ExtendedMonoBehaviourRun
    {
        [SerializeField, TextArea]
        private string taskDescription;
        [SerializeField, TextArea]
        private string taskHint;
        [SerializeField]
        private bool hideTaskProgressBar;

        public string TaskDescription => taskDescription;
        public string TaskHint => taskHint;
        public bool HideTaskProgressBar => hideTaskProgressBar;

        protected TutorialTaskManager TutorialTaskManager { get; private set; }

        protected virtual void OnStart() { }

        public void StartWith(TutorialTaskManager tutorialTaskManager)
        {
            TutorialTaskManager = tutorialTaskManager;

            ///
            gameObject.SetActive(true);

            ///
            OnStart();
        }

        protected void Finish()
        {
            ///
            gameObject.SetActive(false);

            ///
            TutorialTaskManager.NextTask();
        }
    }
}