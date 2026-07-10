using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace FH.Core.Gameplay.View
{
    public class ScaleToCameraSize : MonoBehaviour
    {
        [SerializeField]
        private bool autoScaleAtStart = true;
        [SerializeField]
        private Camera targetCamera;

        public void Start()
        {
            if (autoScaleAtStart)
            {
                Scale();
            }
        }

        public void Scale()
        {
            Assert.IsNotNull(targetCamera);
            float height = targetCamera.orthographicSize * 2;
            float width = targetCamera.aspect * height;
            var p = transform.localScale;
            p.x = width;
            p.y = height;
            transform.localScale = p;
        }
    }

}