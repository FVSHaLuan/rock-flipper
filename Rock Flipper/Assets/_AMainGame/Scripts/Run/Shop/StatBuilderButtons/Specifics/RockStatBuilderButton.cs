using Agame.Run.Combat;
using Agame.Run.Stats;
using UnityEngine;

namespace Agame.Run.Shop
{
    public abstract class RockStatBuilderButton : StatBuilderButtonSpecificMonoBehaviour
    {
        [SerializeField]
        private RockTier tier;

        protected RockTier Tier => tier;
        protected RockTierBuildStats TierBuildStats => BuildStats.GetRockTierBuildStats(tier);
    }

}