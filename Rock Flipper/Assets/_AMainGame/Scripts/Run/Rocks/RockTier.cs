using System.Collections;
using UnityEngine;

namespace Agame.Run.Combat
{
    public enum RockTier
    {
        P0 = 0,
        P1 = 1,
        P2 = 2,
        P3 = 3,
    }

    public static class RockTierExtensions
    {
        private static readonly RockTier[] allTiers;

        static RockTierExtensions()
        {
            // Get all values of the RockTier enum and store them in the array
            allTiers = (RockTier[])System.Enum.GetValues(typeof(RockTier));
        }

        public static RockTier[] AllTiers()
        {
            return allTiers;
        }
    }
}