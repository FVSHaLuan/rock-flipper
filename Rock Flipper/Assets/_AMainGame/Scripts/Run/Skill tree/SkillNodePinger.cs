using FH.Core.Architecture.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class SkillNodePinger : ExtendedMonoBehaviourRun
    {
        private static List<SkillNode> skillNodes = new List<SkillNode>();

        [SerializeField]
        private GeneralPoolMemberSimplified pingVfx;

        public void PingAvailableSkill()
        {
            RunEntry.skillTree.GetUpgradableSkillNodes(skillNodes);
            if (skillNodes.Count == 0)
            {
                return;
            }

            ///
            var skillNode = skillNodes[Random.Range(0, skillNodes.Count)];
            pingVfx.SpawnFromEntryPool(skillNode.transform.position);
        }

        public void PingUnmaxedSkill()
        {
            RunEntry.skillTree.GetUnmaxedUnupgradableSkillNodes(skillNodes);
            if (skillNodes.Count == 0)
            {
                return;
            }

            ///
            var skillNode = skillNodes[Random.Range(0, skillNodes.Count)];
            pingVfx.SpawnFromEntryPool(skillNode.transform.position);
        }
    }

}