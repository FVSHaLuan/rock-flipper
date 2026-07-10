using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI
{
    public abstract class ValueDisplayerUnifiedScreenshotModeSupportedInt : ValueDisplayerUnifiedScreenshotModeSupported<int>
    {
        [Header("ScreenshotModeInt")]
        [SerializeField]
        private int minScreenshotModeValue = 0;
        [SerializeField]
        private int maxScreenshotModeValue = 100;

        protected sealed override int GetCurrentValueScreenshotMode()
        {
            return Random.Range(minScreenshotModeValue, maxScreenshotModeValue);
        }
    }
}
