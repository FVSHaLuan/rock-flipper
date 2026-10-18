using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(Flippable))]
    public class FlippableByPlayerCursor : MonoBehaviour
    {
        /// <summary>
        /// Fired on the frame the cursor starts hovering over this, whether or not hovering can flip it
        /// </summary>
        public event System.Action OnStartedHovering;

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

        public void StartHovering()
        {
            OnStartedHovering?.Invoke();
        }
    }
}