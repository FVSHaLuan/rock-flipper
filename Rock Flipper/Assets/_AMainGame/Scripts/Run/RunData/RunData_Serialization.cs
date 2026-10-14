using Agame.Run;
using UnityEngine;

namespace Agame
{
    public partial class RunData
    {
        public void UpdateBeforeSave()
        {
            if (RunEntry.Instance != null
                && RunEntry.Instance.RunData == this)
            {
                UpdatePlayTime();
            }
        }

        /// <summary>
        /// Commits the play time elapsed since <see cref="LastSaveTime"/> (plus <see cref="PlayTimeAdjustment"/>) into
        /// the serialized <c>playTime</c>/<c>prestigePlayTime</c>. The only place those fields grow; runs on every save
        /// of the active run, so <see cref="PlayTimeNow"/> stays continuous across it.
        /// </summary>
        private void UpdatePlayTime()
        {
            ///
            var delta = Time.realtimeSinceStartup - lastSaveTime;

            ///
            if (delta <= 0)
            {
                return;
            }

            ///
            playTime += delta + PlayTimeAdjustment;
            prestigePlayTime += delta + PlayTimeAdjustment;
            lastSaveTime = Time.realtimeSinceStartup;
            PlayTimeAdjustment = 0;
        }
    }
}