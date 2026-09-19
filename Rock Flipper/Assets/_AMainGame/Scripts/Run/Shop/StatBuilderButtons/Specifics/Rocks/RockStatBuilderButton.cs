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
            gameObject.SetActive(BuildStats.GetRockTierBuildStats(Tier).unlocked);
        }
    }

}