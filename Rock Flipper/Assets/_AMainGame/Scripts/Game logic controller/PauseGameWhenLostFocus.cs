using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    [System.Obsolete]
    public class PauseGameWhenLostFocus : ExtendedMonoBehaviour
    {
        private int resumeFrameCount = MaxResumeFrameCount;
        private bool paused = false;

        public static int MaxResumeFrameCount { get; set; } = 3;

        protected void OnApplicationFocus(bool focus)
        {
            if (!focus && !Application.runInBackground)
            {
                entry.timeScaleManager.AddPauseGame(this);
                resumeFrameCount = MaxResumeFrameCount;
                paused = true;

                ///
                // GDebug.LogIfEnabledCheat("Unpaused after lost focus");
            }
        }

        protected void LateUpdate()
        {
            if (paused && Application.isFocused)
            {
                ///
                resumeFrameCount--;

                ///
                if (resumeFrameCount <= 0)
                {
                    ///
                    entry.timeScaleManager.RemovePauseGame(this);
                    paused = false;

                    ///
                    // GDebug.LogIfEnabledCheat("Unpaused after gained focus");
                }
            }
        }
    }
}