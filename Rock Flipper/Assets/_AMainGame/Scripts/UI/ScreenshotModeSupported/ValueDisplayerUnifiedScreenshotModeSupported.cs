using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI
{
    public abstract class ValueDisplayerUnifiedScreenshotModeSupported<T> : ValueDisplayerUnified<T> where T : System.IComparable<T>
    {
        [Header("ScreenshotMode")]
        [SerializeField]
        private bool ignoreScreenshotMode;
        [SerializeField, Range(0, 1)]
        private float screenshotModeUpdateFrequency = 0.7f;

        private T lastScreenshotModeValue;

        protected bool IsInScreenshotMode => !ignoreScreenshotMode && CommonCheatLib.IsInScreenshotMode;

        protected abstract T GetCurrentValueNormal();
        protected abstract T GetCurrentValueScreenshotMode();

        protected sealed override T GetCurrentValue()
        {
            ///
            if (CommonCheatLib.IsInScreenshotMode)
            {
                if (Random.value <= screenshotModeUpdateFrequency)
                {
                    lastScreenshotModeValue = GetCurrentValueScreenshotMode();
                    return lastScreenshotModeValue;
                }
                else
                {
                    return lastScreenshotModeValue;
                }
            }

            ///
            return GetCurrentValueNormal();
        }
    }

}