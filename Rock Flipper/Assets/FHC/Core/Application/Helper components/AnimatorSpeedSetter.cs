using UnityEngine;
using System.Collections;

namespace FH.Core.HelperComponent
{
    public class AnimatorSpeedSetter : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private float speed = 1;
        [SerializeField]
        private float maxAdditionalSpeed = 0;
        [SerializeField]
        private bool setOnEnabled = false;

        protected void OnEnable()
        {
            if (setOnEnabled)
            {
                Set();
            }
        }

        [ContextMenu("Set")]
        public void Set()
        {
            animator.speed = speed + Random.Range(0, maxAdditionalSpeed);
        }

        public void Reset()
        {
            animator = GetComponent<Animator>();
        }
    }

}