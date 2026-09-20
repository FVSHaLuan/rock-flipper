using Agame.Run.Stats;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class FlipperBotCountStatBuilderButton : StatBuilderButtonSpecificMonoBehaviour, IMaxLevelConfig
    {
        public int MaxLevel => BuildStats.flipperBotMaxCount;

        //public override void HandleLeveledUp(int levelCount)
        //{
        //    base.HandleLeveledUp(levelCount);

        //    ///
        //    for (int i = 0; i < levelCount; i++)
        //    {
        //        // Spawn a new Flipper Bot
        //    }
        //}
    }

}