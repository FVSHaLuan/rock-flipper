using UnityEngine;

namespace Agame.Run
{
    public class AvailableSkillBadge : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject badge;

        protected void OnDisable()
        {
            ///
            RunEntry.skillCostTracker.OnAvailableSkillChanged -= SkillCostTracker_OnAvailableSkillChanged;
        }

        protected void OnEnable()
        {
            badge.SetActive(RunEntry.skillCostTracker.HasAvailableSkill);

            ///
            RunEntry.skillCostTracker.OnAvailableSkillChanged += SkillCostTracker_OnAvailableSkillChanged;
        }

        private void SkillCostTracker_OnAvailableSkillChanged()
        {
            badge.SetActive(RunEntry.skillCostTracker.HasAvailableSkill);
        }
    }

}