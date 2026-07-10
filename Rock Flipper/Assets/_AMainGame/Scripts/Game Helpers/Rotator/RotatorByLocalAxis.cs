using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Gameplay.HelperComponent
{
    public class RotatorByLocalAxis : OutsiteTargetTransform
    {
        [SerializeField]
        private float angularSpeed;
        [SerializeField]
        private TransformVector transformVector;
        [SerializeField]
        private bool unscaledTime = false;

        protected void Update()
        {
            ///
            var angularDelta = angularSpeed * (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);

            ///
            Vector3 axis;

            ///
            switch (transformVector)
            {
                case TransformVector.Up:
                    axis = TargetTransform.up;
                    break;
                case TransformVector.Right:
                    axis = TargetTransform.right;
                    break;
                case TransformVector.Forward:
                    axis = TargetTransform.forward;
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            ///
            TargetTransform.RotateAround(TargetTransform.position, axis, angularDelta);
        }
    }

}