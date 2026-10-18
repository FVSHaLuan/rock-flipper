using Agame.Localization;
using System.Collections;
using UnityEngine;

namespace Agame.Run.Combat
{
    public enum ChestRarity
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Unique = 4,
    }

    public static class ChestRarityExtensions
    {
        public static Color GetForegroundColor(this ChestRarity rarity)
        {
            switch (rarity)
            {
                case ChestRarity.Common:
                    return VisualDefinitions.Instance.commonRarityForegroundColor;
                case ChestRarity.Uncommon:
                    return VisualDefinitions.Instance.uncommonRarityForegroundColor;
                case ChestRarity.Rare:
                    return VisualDefinitions.Instance.rareRarityForegroundColor;
                case ChestRarity.Epic:
                    return VisualDefinitions.Instance.epicRarityForegroundColor;
                case ChestRarity.Unique:
                    return VisualDefinitions.Instance.uniqueRarityForegroundColor;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public static string GetLocalizedName(this ChestRarity rarity)
        {
            var strings = SpecificLocalizedStrings.Instance;
            switch (rarity)
            {
                case ChestRarity.Common:
                    return strings.commonRarity.GetLocalizedStringOr(rarity.ToString());
                case ChestRarity.Uncommon:
                    return strings.uncommonRarity.GetLocalizedStringOr(rarity.ToString());
                case ChestRarity.Rare:
                    return strings.rareRarity.GetLocalizedStringOr(rarity.ToString());
                case ChestRarity.Epic:
                    return strings.epicRarity.GetLocalizedStringOr(rarity.ToString());
                case ChestRarity.Unique:
                    return strings.uniqueRarity.GetLocalizedStringOr(rarity.ToString());
                default:
                    throw new System.NotImplementedException();
            }
        }
    }

}
