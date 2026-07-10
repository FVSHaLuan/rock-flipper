using System;
using UnityEngine;

namespace BT
{
    public class LoadingScreenAnimator : ExtendedMonoBehaviour
    {
        private Animator animator;
        private Action midAnimationCallback;
        private Action endAnimationCallback;

        protected override bool Init()
        {
            ///
            animator = GetComponent<Animator>();

            ///
            return base.Init();
        }

        protected void OnDisable()
        {
            entry.completeInputBlocker.RemoveBlockLock(this);
        }

        protected void OnEnable()
        {
            entry.completeInputBlocker.AddBlockLock(this);
        }

        public bool Play(Action midAnimationCallback, Action endAnimationCallback)
        {
            ///
            if (isActiveAndEnabled)
            {
                return false;
            }

            ///
            this.midAnimationCallback = midAnimationCallback;
            this.endAnimationCallback = endAnimationCallback;
            gameObject.SetActive(true);

            ///
            return true;
        }

        public void HandleMidAnimation()
        {
            midAnimationCallback?.Invoke();
        }

        public void HandleEndAnimation()
        {
            gameObject.SetActive(false);

            ///
            endAnimationCallback?.Invoke();
        }
    }

}