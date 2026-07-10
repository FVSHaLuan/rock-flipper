using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FHC.Helpers.UnityExtend
{
    public class SizableParticleLine : ParticleManipulator
    {
        [SerializeField]
        private bool isLocalRotation = false;

        [Space]
        [SerializeField]
        private float visibleLength = 5;
        [SerializeField]
        private float startFadeLength = 0.5f;
        [SerializeField]
        private float endFadeLength = 0.5f;

        private float currentAngle;

        public float VisibleLength
        {
            get => visibleLength;
            set => visibleLength = value;
        }

        protected override void OnStartManipulatingParticles()
        {
            ///
            base.OnStartManipulatingParticles();

            ///
            currentAngle = isLocalRotation ? -transform.localEulerAngles.z : -transform.eulerAngles.z;
        }

        protected override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
        {
            ///
            particle.rotation = currentAngle;

            ///
            particle = UpdateParticleAlpha(particle);

            ///
            return particle;
        }

        private ParticleSystem.Particle UpdateParticleAlpha(ParticleSystem.Particle particle)
        {
            ///
            var c = particle.startColor;

            ///
            var posY = particle.position.y;

            ///
            if (posY > visibleLength)
            {
                c.a = 0;
            }
            else
            {
                if (posY >= (visibleLength - endFadeLength))
                {
                    c.a = (byte)Mathf.Lerp(255, 0, (posY - visibleLength + endFadeLength) / endFadeLength);
                }
                else if (posY < startFadeLength)
                {
                    c.a = (byte)Mathf.Lerp(0, 255, posY / startFadeLength);
                }
                else
                {
                    c.a = byte.MaxValue;
                }
            }

            ///
            particle.startColor = c;

            ///
            return particle;
        }
    }

}