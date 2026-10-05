using Agame.Run.Stats;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class RockExtender : MonoBehaviour
    {
        private Rock rock;

        protected Rock Rock
        {
            get
            {
                if (rock == null)
                    rock = GetComponentInParent<Rock>();
                return rock;
            }
        }

        protected BuildStatsObject BuildStats => RunEntry.Instance.BuildStats;
        protected RockTierBuildStats TierStats => BuildStats.GetRockTierBuildStats(Rock.Tier);
    }

}