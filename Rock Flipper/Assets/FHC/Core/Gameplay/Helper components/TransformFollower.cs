using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Gameplay.HelperComponent
{
    public class TransformFollower : MonoBehaviour
    {
        [SerializeField]
        private Transform followedTransform;

        [Space]
        [SerializeField]
        private bool followPosition = true;
        [SerializeField]
        private bool followRotation = true;
        [SerializeField]
        private bool followScale = true;

        [Space]
        [SerializeField]
        private bool isLocalPosition = true;
        [SerializeField]
        private bool isLocalRotation = true;

        protected void LateUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            // Position
            if (followPosition)
            {
                if (isLocalPosition)
                {
                    transform.localPosition = followedTransform.localPosition;
                }
                else
                {
                    transform.position = followedTransform.position;
                }
            }

            // Rotation
            if (followRotation)
            {
                if (isLocalRotation)
                {
                    transform.localRotation = followedTransform.localRotation;
                }
                else
                {
                    transform.rotation = followedTransform.rotation;
                }
            }

            // Scale
            if (followScale)
            {
                transform.localScale = followedTransform.localScale;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Follow")]
        private void Editor_Follow()
        {
            ///
            UnityEditor.Undo.RecordObject(transform, "Editor_Follow");

            ///
            Follow();
        }
#endif
    }
}