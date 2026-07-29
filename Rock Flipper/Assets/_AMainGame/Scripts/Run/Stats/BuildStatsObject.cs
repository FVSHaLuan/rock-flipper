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
    }
}