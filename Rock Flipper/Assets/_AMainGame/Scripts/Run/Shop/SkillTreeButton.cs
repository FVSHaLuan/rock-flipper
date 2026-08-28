using Agame.Run.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    public class SkillTreeButton : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private Button button;

        protected void OnDisable()
        {
            BuildStatsObject.OnUnlockedSkillTree -= BuildStatsObject_OnUnlockedSkillTree;
        }

        protected void OnEnable()
        {
            ///
            button.interactable = BuildStats.UnlockedSkillTree;

            ///
            BuildStatsObject.OnUnlockedSkillTree += BuildStatsObject_OnUnlockedSkillTree;
        }

        private void BuildStatsObject_OnUnlockedSkillTree()
        {
            button.interactable = BuildStats.UnlockedSkillTree;
        }
    }

}