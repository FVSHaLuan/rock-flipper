using BT.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BT.FeatureBranching
{
    public class FeatureBranchingObject : FeatureBrancher, IGameObjectBuildStateSetter
    {
        [Space]
        [SerializeField]
        private GameObject targetObject;
        [SerializeField]
        private bool defaultBuildActiveState = true;

        bool IGameObjectBuildStateSetter.IncludedInConfiguration => false;

        protected void Awake()
        {
            if (targetObject != null)
            {
                targetObject.SetActive(ShouldBeActive());
            }
            else if (!ShouldBeActive())
            {
                gameObject.SetActive(false);
            }
        }

        bool IGameObjectBuildStateSetter.SetBuildState()
        {
            ///
            gameObject.SetActive(defaultBuildActiveState);

            ///
            if (targetObject != null && targetObject != gameObject)
            {
                targetObject.SetActive(false);
            }

            ///
            return true;
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            Assert.IsTrue(targetObject == null || targetObject.transform.IsChildOf(transform));
        }

        protected void Reset()
        {
            defaultBuildActiveState = gameObject.activeSelf;
        }
#endif
    }

}