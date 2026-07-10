using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run
{
    public class CameraShakeHelper : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private UnityEvent onShakeStarted;
        [SerializeField]
        private UnityEvent onShakeEnded;

        public void OnEnable()
        {
            RunEntry.cameraShake.OnShakeStarted += CameraShake_OnShakeStarted;
            RunEntry.cameraShake.OnShakeEnded += CameraShake_OnShakeEnded;
        }

        public void OnDisable()
        {
            RunEntry.cameraShake.OnShakeStarted -= CameraShake_OnShakeStarted;
            RunEntry.cameraShake.OnShakeEnded -= CameraShake_OnShakeEnded;
        }

        private void CameraShake_OnShakeStarted()
        {
            onShakeStarted?.Invoke();
        }

        private void CameraShake_OnShakeEnded()
        {
            onShakeEnded?.Invoke();
        }

        public void Shake()
        {
            RunEntry.cameraShake.Shake();
        }
    } 
}
