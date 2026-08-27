using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(Flippable))]
    public class FlippableByPlayerCursor : MonoBehaviour
    {
        private Flippable flippable;
        private float lastTimeLanded;
        private float landingCooldown;

        public Flippable Flippable
        {
            get
            {
                if (flippable == null)
                {
                    flippable = GetComponent<Flippable>();
                }
                return flippable;
            }
        }

        public float LastTimeLanded { get => lastTimeLanded; set => lastTimeLanded = value; }
        public float LandingCooldownTime { get => landingCooldown; set => landingCooldown = value; }
        public bool CooledDown => (Time.time - lastTimeLanded) >= LandingCooldownTime;
    }
}