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