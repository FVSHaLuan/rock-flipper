using Agame.Dev;
using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Localization
{
    public class LocalizedStrings : ScriptableObjectWithInit
    {
        public LocalizedString stage;
        public LocalizedString slot;
        public LocalizedString on;
        public LocalizedString off;
        public LocalizedString speed;
        public LocalizedString damage;
        public LocalizedString cash;
        public LocalizedString maxed;
        public LocalizedString cost;
        public LocalizedString recommended;
        public LocalizedString level;
        public LocalizedString lvl;
        public LocalizedString exp;
        public LocalizedString naInDemo;
        public LocalizedString moreInFullGame;
        public LocalizedString allUnlocked;
        public LocalizedString perSecondSuffix;
        public LocalizedString costWillBeFullyRefunded;
        public LocalizedString unsupportedSaveVersion;

        public static LocalizedStrings Instance
        {
            get
            {
                ///
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return DevEntry.Instance.localizedStrings;
                }
#endif

                ///
                return Entry.Instance.localizedStrings;
            }
        }
    }

}