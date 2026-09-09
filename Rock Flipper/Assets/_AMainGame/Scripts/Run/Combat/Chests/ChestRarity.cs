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
    }

}
