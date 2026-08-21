using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Stats
{
    public class BuildStatsObject : MonoBehaviourWithInit
    {
        [Header("Player Cursor")]
        public bool enabledPlayerCursorRadius = false;
        public float playerCursorRadius = 0.5f;

        [Header("Rocks")]
        [SerializeField]
        private RockTierBuildStats rock_P0;
        [SerializeField]
        private RockTierBuildStats rock_P1;
        [SerializeField]
        private RockTierBuildStats rock_P2;
        [SerializeField]
        private RockTierBuildStats rock_P3;

        [Header("Flipper Bots")]
        public float flipperBotFlippingInterval = 2.0f;


        public RockTierBuildStats GetRockTierBuildStats(RockTier rockTier)
        {
            return rockTier switch
            {
                RockTier.P0 => rock_P0,
                RockTier.P1 => rock_P1,
                RockTier.P2 => rock_P2,
                RockTier.P3 => rock_P3,
                _ => throw new System.NotImplementedException($"RockTier {rockTier} is not implemented in BuildStatsObject."),
            };
        }
    }
}