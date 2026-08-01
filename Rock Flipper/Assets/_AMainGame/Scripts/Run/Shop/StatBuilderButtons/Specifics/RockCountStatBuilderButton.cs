using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RockCountStatBuilderButton : RockStatBuilderButton, IMaxLevelConfig
    {
        public int MaxLevel => TierBuildStats.maxCount;
    }

}