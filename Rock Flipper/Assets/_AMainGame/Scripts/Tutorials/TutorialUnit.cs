using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Tutorials
{
    public class TutorialUnit : ExtendedMonoBehaviour
    {
        [SerializeField]
        protected Tutorial tutorial;

        [Space]
        [SerializeField]
        protected GameObject viewWrapper;
        [SerializeField]
        protected bool tryShowingOnEnable;
        [SerializeField]
        private bool disableWhenPassed;

        protected virtual void OnEnable()
        {
            ///
            if (tutorial.HasPassed
                && disableWhenPassed)
            {
                enabled = false;
            }

            if (tryShowingOnEnable)
            {
                TryShowing();
            }
        }

        public void TryShowing()
        {
            if (!tutorial.HasPassed)
            {
                Show();
            }
            else
            {
                viewWrapper.SetActive(false);
            }
        }

        public void SetAsPassed()
        {
            tutorial.SetAsPassed();

            ///
            enabled = false;
        }

        public void CloseAndSetAsPassed()
        {
            viewWrapper.SetActive(false);

            ///
            tutorial.SetAsPassed();

            ///
            enabled = false;
        }

        protected virtual void Show()
        {
            viewWrapper.SetActive(true);
        }

        protected void Hide()
        {
            viewWrapper.SetActive(false);
        }
    }

}