using Agame.Run.Stats;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class FlipperBotCountStatBuilderButton : StatBuilderButtonSpecificMonoBehaviour, IMaxLevelConfig
    {
        public int MaxLevel => BuildStats.flipperBotMaxCount;

        protected void Start()
        {
            UpdateVisibility();

            ///
            RunEntry.skillTreeScreen.OnClosed += SkillTreeScreen_OnClosed;
        }

        private void SkillTreeScreen_OnClosed()
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(BuildStats.flipperBotsEnabled);
        }

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