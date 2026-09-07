using UnityEngine;
using GD;

namespace Agame.Run.Combat
{
    public static class ChestUtilities
    {
        private static readonly ChestRarity[] allRarities = (ChestRarity[])System.Enum.GetValues(typeof(ChestRarity));
        private static readonly WeightedChestRarity[] weightedRarities = new WeightedChestRarity[allRarities.Length];

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

        public static ChestRarity PickRarity(IRandomGenerator random = null)
        {
            random ??= UnityRandom.Default;

            for (int i = 0; i < allRarities.Length; i++)
            {
                var rarity = allRarities[i];
                var buildStats = RunEntry.Instance.BuildStats.GetChestRarityBuildStats(rarity);
                weightedRarities[i] = new WeightedChestRarity(rarity, buildStats.weight);
            }

            return WeightExtensions.PickOneIn(random, weightedRarities).Rarity;
        }

    }

}