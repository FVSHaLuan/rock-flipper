using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Architecture
{
    public struct TransformPositionProviderStruct : IPositionProvider
    {
        private Transform transform;

        public Vector3 Position => transform.position;

        public TransformPositionProviderStruct(Transform transform)
        {
            ///
            if (transform == null)
            {
                throw new System.NullReferenceException();
            }

            ///
            this.transform = transform;
        }
    }

    public static class TransformPositionProviderStructExtension
    {
        public static TransformPositionProviderStruct GetPositionProvider(this Transform transform)
        {
            ///
            return new TransformPositionProviderStruct(transform);
        }
    }

}