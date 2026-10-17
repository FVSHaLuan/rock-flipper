using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Changes the Preferred background to a random other unlocked background every <see cref="preferredInterval"/>
    /// seconds of play time, while the background feature is enabled and at least 2 backgrounds are unlocked.
    /// </summary>
    public class PreferredBackgroundUpdater : ExtendedMonoBehaviourRun
    {
        [SerializeField, Min(1), Tooltip("Seconds of play time between two Preferred background changes.")]
        private float preferredInterval = 300;

        /// <summary>
        /// Seconds of play time between two Preferred background changes.
        /// </summary>
        public float PreferredInterval => preferredInterval;

        protected void Update()
        {
            ///
            if (!BuildStats.BackgroundsEnabled)
            {
                return;
            }

            // With only 1 background unlocked there is no Preferred background.
            var runData = RunData;
            if (runData.UnlockedBackgroundCount < 2)
            {
                return;
            }

            ///
            if (runData.PreferredBackgroundTimeElapsed >= preferredInterval)
            {
                runData.ChangeRandomPreferredBackground();
            }
        }
    }

}
