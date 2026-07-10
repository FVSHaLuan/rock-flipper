using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Systems.CameraStabilizerEffect
{
    [RequireComponent(typeof(CameraStabilizerEffect))]
    public class CameraStabilizerEffectPositionRandomizer : ExtendedMonoBehaviour
    {
        [SerializeField]
        private float changeInterval = 0.05f;
        [SerializeField]
        private float speed = 1.0f;
        [SerializeField]
        private TimeScaleMode timeScaleMode;

        private CameraStabilizerEffect cameraStabilizerEffect;

        private float lastTimeChangedPosition = -999;
        private Vector2 offset;
        private Vector2 targetOffset;

        protected void Update()
        {
            ///
            if (cameraStabilizerEffect == null)
            {
                cameraStabilizerEffect = GetComponent<CameraStabilizerEffect>();
            }

            ///
            float maxOffset = 1 - cameraStabilizerEffect.sizeScale;

            ///
            var time = GetTime(timeScaleMode);
            var deltaTime = GetDeltaTime(timeScaleMode);
            var timePassed = time - lastTimeChangedPosition;

            // Target offset
            if (timePassed >= changeInterval) // Change target offset
            {
                ///
                targetOffset.x = Random.Range(0, maxOffset);
                targetOffset.y = Random.Range(0, maxOffset);

                ///
                lastTimeChangedPosition = time;
            }
            else // Clamp target offset
            {
                targetOffset = ClampOffset(targetOffset, maxOffset);
            }

            ///
            offset = Vector2.MoveTowards(offset, targetOffset, speed * deltaTime);
            offset = ClampOffset(offset, maxOffset);

            ///
            cameraStabilizerEffect.viewOffsetX = offset.x;
            cameraStabilizerEffect.viewOffsetY = offset.y;
        }

        private Vector2 ClampOffset(Vector2 offset, float maxOffset)
        {
            ///
            offset.x = Mathf.Clamp(offset.x, 0, maxOffset);
            offset.y = Mathf.Clamp(offset.y, 0, maxOffset);

            ///
            return offset;
        }
    }
}