using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame
{
    public class GameLogoAnimation : MonoBehaviour
    {
        [SerializeField]
        private Transform titleTransform;
        [SerializeField]
        private Transform subtitleTransform;

        [Space]
        [SerializeField]
        private float maxSwingAmplitude = 10;
        [SerializeField]
        private float swingDuration = 3;
        [SerializeField]
        private int swingCircleCount = 3;

        [Space]
        [SerializeField]
        private bool isEndless = false;
        [SerializeField]
        private bool swingOnEnable = false;

        [Space]
        [SerializeField]
        private UnityEvent onFinishedSwinging;

        protected void OnDisable()
        {
            StopAllCoroutines();
            ResetRotations();
        }

        protected void OnEnable()
        {
            if (swingOnEnable)
            {
                Swing();
            }
        }

        [ContextMenu("Swing")]
        public void Swing()
        {
            StartCoroutine(SwingCoroutine());
        }

        private IEnumerator SwingCoroutine()
        {
            ///
            ResetRotations();

            ///            
            float timeElapsed = 0;

            ///
            while (timeElapsed <= swingDuration)
            {
                ///
                timeElapsed += Time.unscaledDeltaTime;

                ///
                var t = timeElapsed / swingDuration;
                t = Mathf.Clamp01(t);

                ///
                var amplitude = isEndless ? maxSwingAmplitude : Mathf.Lerp(maxSwingAmplitude, 0, t);

                ///
                var angleRad = t * swingCircleCount * 360 * Mathf.Deg2Rad;
                var displacement = Mathf.Sin(angleRad) * amplitude;

                ///
                var titleAngle = displacement;
                var subtitleAngle = -displacement;

                ///
                titleTransform.localRotation = Quaternion.Euler(0, titleAngle, 0);
                subtitleTransform.localRotation = Quaternion.Euler(0, subtitleAngle, 0);

                ///
                yield return null;
            }

            ///
            ResetRotations();

            ///
            if (!isEndless)
            {
                onFinishedSwinging?.Invoke();
            }
            else
            {
                StartCoroutine(SwingCoroutine());
            }
        }

        private void ResetRotations()
        {
            titleTransform.localRotation = Quaternion.Euler(0, 0, 0);
            subtitleTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
