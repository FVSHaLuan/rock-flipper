using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run
{
    public class SkillCostTracker : ExtendedMonoBehaviourRun
    {
        public event System.Action OnAvailableSkillChanged;

        private HashSet<SkillNode> trackingSkillNodes = new HashSet<SkillNode>();
        private SkillNode availableSkill;

        public bool HasAvailableSkill => AvailableSkill != null;

        private SkillNode AvailableSkill
        {
            get => availableSkill;
            set
            {
                ///
                if (availableSkill == value)
                {
                    return;
                }

                ///
                availableSkill = value;

                ///
                OnAvailableSkillChanged?.Invoke();
            }
        }

        protected void OnDestroy()
        {
            RunData.OnCurrencyValueModified -= RunData_OnCurrencyValueModified;
        }

        protected void Start()
        {
            RunData.OnCurrencyValueModified += RunData_OnCurrencyValueModified;
        }

        private void RunData_OnCurrencyValueModified(Currency obj)
        {
            UpdateAvailableSkill();
        }

        public void UpdateTrackingSkill(SkillNode skillNode, bool isAdding)
        {
            var rs = trackingSkillNodes.Add(skillNode);

            ///
            if (isAdding)
            {
                if (AvailableSkill == null && rs)
                {
                    AvailableSkill = IsSkillAvailable(skillNode) ? skillNode : null;
                }
            }
            else
            {
                if (AvailableSkill == skillNode)
                {
                    UpdateAvailableSkill();
                }
            }
        }

        public void RemoveTrackingSkill(SkillNode skillNode)
        {
            var rs = trackingSkillNodes.Remove(skillNode);

            ///
            if (!rs)
            {
                return;
            }

            ///
            if (AvailableSkill == skillNode)
            {
                AvailableSkill = null;
                UpdateAvailableSkill();
            }
        }

        private void UpdateAvailableSkill()
        {
            ///
            if (AvailableSkill != null && AvailableSkill.IsEnoughCurrencyToLevelUp())
            {
                return;
            }

            ///
            AvailableSkill = null;

            ///
            foreach (var item in trackingSkillNodes)
            {
                if (IsSkillAvailable(item))
                {
                    AvailableSkill = item;
                    break;
                }
            }
        }

        private bool IsSkillAvailable(SkillNode skillNode)
        {
            ///
            if (skillNode == null || skillNode.IsDemoLimited)
            {
                return false;
            }

            ///
            if (skillNode.IsEnoughCurrencyToLevelUp())
            {
                return true;
            }

            ///
            return false;
        }
    }

}