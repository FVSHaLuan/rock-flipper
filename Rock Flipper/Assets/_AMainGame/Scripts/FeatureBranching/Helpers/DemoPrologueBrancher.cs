using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.FeatureBranching
{
    public class DemoPrologueBrancher : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onDemo;
        [SerializeField]
        private UnityEvent onPrologue;
        [SerializeField]
        private UnityEvent onPlaytest;
        [SerializeField]
        private UnityEvent onNone;

        [Space]
        [SerializeField]
        private bool doNotCheckOnEditor;

        protected void Start()
        {
            ///
            if (doNotCheckOnEditor
                && Application.isEditor)
            {
                return;
            }

            ///
            if (VersionBranchInfo.IsDemo)
            {
                if (VersionBranchInfo.IsPrologue)
                {
                    onPrologue?.Invoke();
                }
                else
                {
                    onDemo?.Invoke();
                }
            }
            else if (VersionBranchInfo.IsPlaytest)
            {
                onPlaytest?.Invoke();
            }
            else
            {
                onNone?.Invoke();
            }
        }
    }
}
