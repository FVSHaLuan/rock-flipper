using UnityEngine;
using UnityEngine.Localization;

namespace BT.Localization
{
    public class SpecificLocalizedStrings : ScriptableObjectWithInit
    {
        public static SpecificLocalizedStrings Instance
        {
            get
            {
                return Entry.Instance.specificLocalizedStrings;
            }
        }

        public LocalizedString freeTag;
        public LocalizedString jumpy;
        public LocalizedString dark;
        public LocalizedString ball;
        public LocalizedString moduleLevel;

        [Header("Damage sources")]
        public LocalizedString mouseHover;
        public LocalizedString mouseClick;
        public LocalizedString yellowLightningBolt;
        public LocalizedString blueLightningBolt;
        public LocalizedString auraWave;
    }

}