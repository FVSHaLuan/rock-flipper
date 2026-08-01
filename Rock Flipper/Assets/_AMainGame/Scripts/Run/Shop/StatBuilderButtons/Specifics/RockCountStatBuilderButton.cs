using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RockCountStatBuilderButton : ExtendedMonoBehaviourRun, IStatBuilderButtonSpecific, IMaxLevelConfig
    {
        [SerializeField]
        private RockTier tier;

        public int MaxLevel => BuildStats.GetRockTierBuildStats(tier).maxCount;
    }

}