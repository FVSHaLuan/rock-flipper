using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Localization
{
    public class SpecificLocalizedStrings : ScriptableObjectWithInit
    {
        [Header("Backgrounds")]
        public LocalizedString unlockRandom;
        public LocalizedString unlockMoreBackgrounds;
        public LocalizedString selectPreferredBackground;
        [Tooltip("{0}: the multiplier, e.g. x2")]
        public LocalizedString preferredBackgroundCashIncome;
        public LocalizedString unlockMoreBackgroundsToIncreaseIt;

        [Header("Items")]
        public LocalizedString notFoundYet;
        public LocalizedString cashEarned;
        public LocalizedString effectsScaleWithItemLevel;
        public LocalizedString commonRarity;
        public LocalizedString uncommonRarity;
        public LocalizedString rareRarity;
        public LocalizedString epicRarity;
        public LocalizedString uniqueRarity;

        [Header("Level & Chest bars")]
        [Tooltip("{0}: the stars earned on level up")]
        public LocalizedString levelBarDescription;
        public LocalizedString chestBar;
        public LocalizedString chestBarDescription;

        public static SpecificLocalizedStrings Instance
        {
            get
            {
                return Entry.Instance.specificLocalizedStrings;
            }
        }


    }

}
