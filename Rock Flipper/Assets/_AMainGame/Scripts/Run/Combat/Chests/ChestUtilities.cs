using System.Collections.Generic;
using UnityEngine;
using GD;

namespace Agame.Run.Combat
{
    public static class ChestUtilities
    {
        private static readonly ChestRarity[] allRarities = (ChestRarity[])System.Enum.GetValues(typeof(ChestRarity));
        private static readonly List<WeightedChestRarity> weightedRarities = new List<WeightedChestRarity>(allRarities.Length);

        private readonly struct WeightedChestRarity : IWeighted
        {
            public ChestRarity Rarity { get; }
            public float Weight { get; }

            public WeightedChestRarity(ChestRarity rarity, float weight)
            {
                Rarity = rarity;
                Weight = weight;
            }
        }

        /// <summary>
        /// Picks the rarity of a chest dropping right now, by weight. A rarity whose chest would drop no item
        /// (<see cref="Shop.ItemManager.GetDroppableItemCount"/> is 0) is skipped.
        /// Returns false if no rarity can drop.
        /// </summary>
        public static bool TryPickRarity(out ChestRarity rarity, IRandomGenerator random = null)
        {
            ///
            random ??= UnityRandom.Default;
            var runEntry = RunEntry.Instance;

            ///
            weightedRarities.Clear();
            foreach (var candidate in allRarities)
            {
                // Unique drop rules already give 0 when no Unique item is left.
                float weight;
                if (candidate == ChestRarity.Unique)
                {
                    weight = runEntry.uniqueChestDropRules.GetUniqueChestWeight();
                }
                else
                {
                    weight = runEntry.itemManager.GetDroppableItemCount(candidate, runEntry.RunData) > 0
                        ? runEntry.BuildStats.GetChestRarityBuildStats(candidate).weight
                        : 0;
                }

                if (weight > 0)
                {
                    weightedRarities.Add(new WeightedChestRarity(candidate, weight));
                }
            }

            ///
            if (weightedRarities.Count == 0)
            {
                rarity = default;
                return false;
            }
            rarity = weightedRarities.PickOne(random).Rarity;
            weightedRarities.Clear();
            return true;
        }

    }

}
