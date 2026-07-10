using UnityEngine;
using System.Collections;
using System;

namespace Agame.Run
{
    public class CameraShake : ExtendedMonoBehaviour
    {
        public event Action OnShakeStarted;
        public event Action OnShakeEnded;

        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        [SerializeField]
        private Transform camTransform;

        [SerializeField]
        private float defaultShakeTime = 1.0f;
        // Amplitude of the shake. A larger value shakes the camera harder.
        [SerializeField]
        private float shakeAmount = 0.7f;
        [SerializeField]
        private float decreaseFactor = 1.0f;

        private Vector3 originalPos;
        // How long the object should shake for.
        private float shakeDuration = 0f;
        private bool isShaking = false;

        [ContextMenu("Shake")]
        public void Shake()
        {
            Shake(defaultShakeTime);
        }

        public void Shake(float time)
        {
            ///
            var oldValue = shakeDuration;

            ///
            shakeDuration = time;
            isShaking = true;

            ///
            if (Mathf.Approximately(oldValue, 0))
            {
                OnShakeStarted?.Invoke();
            }
        }

        protected override void ExtendedAwake()
        {
            if (camTransform == null)
            {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }
        }

        public void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }

        public void Update()
        {
            ///
            if (!isShaking)
            {
                return;
            }

            ///
            if (entry.timeScaleManager.IsGameplayBeingPaused)
            {
                return;
            }

            ///
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + (Vector3)UnityEngine.Random.insideUnitCircle * shakeAmount * gameSetting.screenShakingEffect;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                ///
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;

                ///
                isShaking = false;
                OnShakeEnded?.Invoke();
            }
        }
    }

}