using Agame.GamePlatform;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run
{
    [DisallowMultipleComponent]
    public partial class SkillTree : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private SkillTreeGraph mainSkillTreeGraph;
        [SerializeField]
        private SkillTreeGraph laserSkillTreeGraph;
        [SerializeField]
        private SkillTreeGraph lightningSkillTreeGraph;

        [Space]
        [SerializeField]
        private Transform mainTreeRoot;
        [SerializeField]
        private Transform laserTreeRoot;
        [SerializeField]
        private Transform lightningTreeRoot;

        [Space]
        [SerializeField, ReadOnly]
        private SkillNode mainRootNode;
        [SerializeField, ReadOnly]
        private SkillNode specialEntryNode;
        [SerializeField, ReadOnly]
        private SkillNode laserRootNode;
        [SerializeField, ReadOnly]
        private SkillNode lightningRootNode;
        [SerializeField, ReadOnly]
        private Vector2 minPosition;
        [SerializeField, ReadOnly]
        private Vector2 maxPosition;

        [Space]
        [SerializeField]
        private AchievementConfig maxedSkillTreeAchievementConfig;

        [Space]
        [SerializeField, ReadOnly]
        private List<SkillNode> mainSkillNodes;
        [SerializeField, ReadOnly]
        private List<SkillNode> laserSkillNodes;
        [SerializeField, ReadOnly]
        private List<SkillNode> lightningSkillNodes;

        private HashSet<SkillNode> maxableSkills = new HashSet<SkillNode>();
        private HashSet<SkillNode> upgradableSkills = new HashSet<SkillNode>();
        private SpecialCrusherId activeSpecialTree;

        protected override bool Init()
        {
            ///
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return base.Init();
            }
#endif
            ///
            SetActiveSpecialTree(SpecialCrusherId.None);

            ///
            AssignParents();

            ///
            DrawTree(true);

            ///
            return base.Init();
        }

        [ContextMenu("DrawTreeWithoutApplyingToBuildStats"), PlayModeOnly]
        public void DrawTreeWithoutApplyingToBuildStats()
        {
            DrawTree(false);
        }

        private void DrawTree(bool applyToBuildStats)
        {
            HideAllNodes();

            ///
            GetUpgradableSkills();
            GetMaxableSkills();

            ///
            mainRootNode.Activate(applyToBuildStats, false);

            ///
            TryToTriggerMaxedSkillTreeAchievement();
        }

        private void HideAllNodes()
        {
            HideNodeRecursively(mainRootNode);
        }

        private void HideNodeRecursively(SkillNode skillNode)
        {
            skillNode.Deactivate();
            for (int i = 0; i < skillNode.OutputNodeCount; i++)
            {
                HideNodeRecursively(skillNode.GetOutputNode(i));
            }
        }

        private void AssignParents()
        {
            AssignParentsRecursively(mainRootNode);
        }

        private void AssignParentsRecursively(SkillNode skillNode)
        {
            ///
            if (skillNode == mainRootNode)
            {
                skillNode.AddParentNode(null);
            }

            ///
            for (int i = 0; i < skillNode.OutputNodeCount; i++)
            {
                var node = skillNode.GetOutputNode(i);
                node.AddParentNode(skillNode);
            }

            ///
            for (int i = 0; i < skillNode.OutputNodeCount; i++)
            {
                var node = skillNode.GetOutputNode(i);
                if (node.ParentNodeCount == 1)
                {
                    AssignParentsRecursively(node);
                }
            }
        }

        //        protected void Update()
        //        {
        //#if UNITY_EDITOR
        //            if (!Application.isPlaying)
        //            {
        //                ///
        //                if (mainRootNode != null)
        //                {
        //                    Editor_Outline();
        //                }
        //            }
        //#endif
        //        }

        public void ApplyToBuildStats()
        {
            ApplyToBuildStatsRecursive(mainRootNode, true);
        }

        private void ApplyToBuildStatsRecursive(SkillNode skillNode, bool applySelf)
        {
            ///
            if (applySelf)
            {
                skillNode.ApplyToBuildStats();
            }

            ///
            for (int i = 0; i < skillNode.OutputNodeCount; i++)
            {
                var childNode = skillNode.GetOutputNode(i);
                ApplyToBuildStatsRecursive(childNode, childNode.GetParentNode(0) == skillNode);
            }
        }

        public SkillNode GetActiveSpecialEntry()
        {
            switch (activeSpecialTree)
            {
                case SpecialCrusherId.Demo:
                    throw new System.Exception("Demo has no tree");
                case SpecialCrusherId.Laser:
                    return laserRootNode;
                case SpecialCrusherId.Lightning:
                    return lightningRootNode;
                default:
                    throw new System.Exception("Unknown special tree: " + activeSpecialTree);
            }
        }

        private List<SkillNode> GetActiveSpecialSkillNodes()
        {
            switch (activeSpecialTree)
            {
                case SpecialCrusherId.Demo:
                    throw new System.Exception("Demo has no tree");
                case SpecialCrusherId.Laser:
                    return laserSkillNodes;
                case SpecialCrusherId.Lightning:
                    return lightningSkillNodes;
                default:
                    throw new System.Exception("Unknown special tree: " + activeSpecialTree);
            }
        }

        private void SetActiveSpecialTree(SpecialCrusherId specialCrusherId)
        {
            ///
            activeSpecialTree = specialCrusherId;
            DisplayActiveSpecialTree();
        }

        private void DisplayActiveSpecialTree()
        {
            laserTreeRoot.gameObject.SetActive(activeSpecialTree == SpecialCrusherId.Laser);
            lightningTreeRoot.gameObject.SetActive(activeSpecialTree == SpecialCrusherId.Lightning);
        }

        private void GetMaxableSkills()
        {
            ///
            maxableSkills.Clear();

            ///
            foreach (var skillNode in upgradableSkills)
            {
                if (skillNode.IsMaxable)
                {
                    maxableSkills.Add(skillNode);
                }
            }
        }

        private void GetUpgradableSkills()
        {
            ///
            upgradableSkills.Clear();

            ///
            foreach (var skillNode in mainSkillNodes)
            {
                if (skillNode.IsUpgradable)
                {
                    upgradableSkills.Add(skillNode);
                }
            }

            ///
            foreach (var skillNode in GetActiveSpecialSkillNodes())
            {
                if (skillNode.IsUpgradable)
                {
                    upgradableSkills.Add(skillNode);
                }
            }
        }

        public void RegisterMaxedSkill(SkillNode skillNode)
        {
            maxableSkills.Remove(skillNode);
            upgradableSkills.Remove(skillNode);
        }

        public void TryToTriggerMaxedSkillTreeAchievement()
        {
            if (maxableSkills.Count == 0)
            {
                entry.achievementReporter.Report(maxedSkillTreeAchievementConfig);
            }
        }

        public void GetUpgradableSkillNodes(List<SkillNode> skillNodes)
        {
            ///
            skillNodes.Clear();

            ///
            foreach (var skillNode in upgradableSkills)
            {
                if (!skillNode.IsMaxed && skillNode.IsEnoughCurrencyToLevelUp())
                {
                    skillNodes.Add(skillNode);
                }
            }
        }

        public void GetUnmaxedUnupgradableSkillNodes(List<SkillNode> skillNodes)
        {
            ///
            skillNodes.Clear();

            ///
            foreach (var skillNode in maxableSkills)
            {
                if (!skillNode.IsMaxed && !skillNode.IsEnoughCurrencyToLevelUp())
                {
                    skillNodes.Add(skillNode);
                }
            }
        }
    }

}