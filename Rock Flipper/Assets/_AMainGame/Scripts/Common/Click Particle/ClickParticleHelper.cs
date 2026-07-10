using UnityEngine;

namespace BT
{
    public class ClickParticleHelper : ExtendedMonoBehaviour
    {
        public const float ClickParticleInterval = 0.1f;

        private float lastTimeClickParticlePlayed = 0f;

        public void TryPlayClickParticle()
        {
            if (Time.unscaledTime - lastTimeClickParticlePlayed >= ClickParticleInterval)
            {
                entry.clickParticleManager.Play();
                lastTimeClickParticlePlayed = Time.unscaledTime;
            }
        }
    }

}