using UnityEngine;

namespace Agame.Run.Shop
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleRateByMaxUnlockedStage : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private float minRate = 0;
        [SerializeField]
        private float maxRate = 100;

        private new ParticleSystem particleSystem;

        protected void OnDestroy()
        {
        }

        protected void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();

            ///
            Check();
        }

        private void RunData_OnUpStaged(int obj)
        {
            ///
            Check();
        }

        [ContextMenu("Check"), PlayModeOnly]
        public void Check()
        {
            var progress = 0.5f; // Edit this
            var rate = Mathf.Lerp(minRate, maxRate, progress);

            ///
            var emission = particleSystem.emission;
            emission.rateOverTime = rate;
        }
    }

}