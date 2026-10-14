using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    /// <summary>
    /// Total play time each background has spent as the active background, added up across visits.
    /// Nothing is counted until <see cref="StartBackgroundActiveTime"/> is called (when the background feature unlocks).
    /// </summary>
    public partial class RunData
    {
        [SerializeField]
        private bool startedBackgroundActiveTime;
        /// <summary>Play time (<see cref="PlayTimeNow"/>) since which the active background's time isn't committed yet.</summary>
        [SerializeField]
        private float activeBackgroundCommitPlayTime;
        /// <summary>Committed active time per background, indexed by background id.</summary>
        [SerializeField]
        private List<float> backgroundActiveTimes = new List<float>();

        public bool StartedBackgroundActiveTime => startedBackgroundActiveTime;

        /// <summary>
        /// Starts counting active time, from now on. Does nothing if it already started.
        /// <paramref name="activeBackgroundOffset"/> is added to the current active background's time right away.
        /// </summary>
        public void StartBackgroundActiveTime(float activeBackgroundOffset)
        {
            ///
            if (startedBackgroundActiveTime)
            {
                return;
            }

            ///
            startedBackgroundActiveTime = true;
            activeBackgroundCommitPlayTime = PlayTimeNow;
            AddBackgroundActiveTime(activeBackgroundId, activeBackgroundOffset);
        }

        /// <summary>
        /// Total play time <paramref name="backgroundId"/> has been the active background, including the current visit.
        /// </summary>
        public float GetBackgroundActiveTime(int backgroundId)
        {
            ///
            var time = 0f;
            if (backgroundActiveTimes != null && backgroundId >= 0 && backgroundId < backgroundActiveTimes.Count)
            {
                time = backgroundActiveTimes[backgroundId];
            }

            ///
            if (startedBackgroundActiveTime && backgroundId == activeBackgroundId)
            {
                time += PlayTimeNow - activeBackgroundCommitPlayTime;
            }
            return time;
        }

        /// <summary>
        /// Adds the current visit's time to the active background's total. Called right before the active background changes.
        /// </summary>
        private void CommitActiveBackgroundTime()
        {
            ///
            if (!startedBackgroundActiveTime)
            {
                return;
            }

            ///
            var now = PlayTimeNow;
            AddBackgroundActiveTime(activeBackgroundId, now - activeBackgroundCommitPlayTime);
            activeBackgroundCommitPlayTime = now;
        }

        private void AddBackgroundActiveTime(int backgroundId, float time)
        {
            ///
            if (backgroundId < 0)
            {
                Debug.LogWarning($"Can't add active time to background {backgroundId}.");
                return;
            }

            ///
            if (backgroundActiveTimes == null)
            {
                backgroundActiveTimes = new List<float>();
            }
            while (backgroundActiveTimes.Count <= backgroundId)
            {
                backgroundActiveTimes.Add(0);
            }

            ///
            backgroundActiveTimes[backgroundId] += time;
        }
    }

}
