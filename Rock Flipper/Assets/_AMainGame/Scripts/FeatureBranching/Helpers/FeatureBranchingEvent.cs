using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.FeatureBranching
{
    public class FeatureBranchingEvent : FeatureBrancher
    {
        [Space]
        [SerializeField]
        private bool checkOnAwake = true;

        [Space]
        [SerializeField]
        private UnityEvent onActive;
        [SerializeField]
        private UnityEvent onInactive;

        protected void Awake()
        {
            if (checkOnAwake)
            {
                Check();
            }
        }

        public void Check()
        {
            if (ShouldBeActive())
            {
                onActive?.Invoke();
            }
            else
            {
                onInactive?.Invoke();
            }
        }
    }

}