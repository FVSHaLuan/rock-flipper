using OneLine;
using Agame.Balancing;
using Agame.Dev;
using Agame.Run.Stats.Agents;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Agame.Run
{
    [NodeWidth(250)]
    public class SkillGraphNode : Node
    {
        [SerializeField, FormerlySerializedAs("parentsInput"), ReadOnly, Input(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Multiple)]
        private int input;
        [SerializeField, FormerlySerializedAs("childrenOutput"), ReadOnly, Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Multiple)]
        private int output;

        [Space]
        [SerializeField, Tooltip("If null, use default")]
        private SkillNode skillNodePrototype;

        [Space]
        [SerializeField]
        private bool attentionFlag;

        [Header("-- Requirements")]
        [SerializeField, FormerlySerializedAs("unlockingRequirement"), Min(0), Tooltip("Total parents' depth required to unlock this node")]
        private int unlockingRequirement = 1;
        [SerializeField]
        private int minParentLevelEach = 0;

        [Header("-- Demo limit")]
        [SerializeField]
        private int demoLimit = -1;

        [Header("-- Build")]
        [SerializeField]
        private BuildAgent buildAgent;
        [SerializeField, LargeNumberField]
        private double buildValue;

        [Header("-- Cost")]
        [SerializeField, Min(1)]
        private int levelCount = 1;
        [SerializeField]
        private Currency currency = Currency.CASH;
        [SerializeField, OneLineWithHeader, Tooltip("Cost of leveling up from level L uses GetPrice(L)")]
        private TieredExponentialPrice price = new TieredExponentialPrice() { cashTier = CashTier.Tier0, a = 0, b = 1, c = 1 };

        [System.NonSerialized]
        private SkillMetaData skillMetaData;
        [System.NonSerialized]
        private SkillDescriptor skillDescriptor;

        public SkillMetaData SkillMetaData
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    skillMetaData = GetSkillMetaData();
                }
#endif
                ///
                if (skillMetaData == null)
                {
                    skillMetaData = GetSkillMetaData();
                }

                ///
                return skillMetaData;
            }
        }
        public SkillDescriptor SkillDescriptor
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    skillDescriptor = GetSkillDescriptor();
                }
#endif
                ///
                if (skillDescriptor == null)
                {
                    skillDescriptor = GetSkillDescriptor();
                }

                ///
                return skillDescriptor;
            }
        }
        public bool IsRoot => GetInputValue("isRoot", false);
        public string NodeId => name;
        public SkillNode SkillNodePrototype => skillNodePrototype;
        /// <summary>
        /// Uses a decorative SkillNode prototype: no build agent, no demo limit, always 1 level
        /// </summary>
        public bool IsDecorative => skillNodePrototype != null && skillNodePrototype.IsDecorative;
        public Sprite Icon { get => SkillMetaData?.Icon; }
        public Sprite SubIcon => SkillMetaData?.SubIcon;
        public string TitleGroup { get => SkillMetaData?.TitleGroup; }
        public string Title { get => SkillMetaData?.Title; }
        public bool HasDemoLimit => !IsDecorative && demoLimit >= 0;
        public int DemoLimit { get => demoLimit; }
        public int UnlockingRequirement { get => unlockingRequirement; }
        public int MinParentLevelEach { get => minParentLevelEach; }
        public CashTier CashTier { get => price.cashTier; }
        public BuildAgent BuildAgent { get => IsDecorative ? null : buildAgent; }
        public double BuildValue { get => buildValue; }
        public int LevelCount => IsDecorative ? 1 : levelCount;
        public Currency Currency => currency;
        public bool AttentionFlag { get => attentionFlag; }
        public override string RecommendedName => IsDecorative ? "Decorative" : (buildAgent != null ? buildAgent.name : base.RecommendedName);

        public override object GetValue(NodePort port)
        {
            return null;
        }

        /// <summary>
        /// Cost to level up from <paramref name="currentLevel"/> to the next level
        /// </summary>
        public CurrencyAmount GetNextLevelCost(int currentLevel)
        {
            return new CurrencyAmount(currency, System.Math.Round(price.GetPrice(currentLevel)));
        }

        private SkillMetaData GetSkillMetaData()
        {
            if (BuildAgent == null)
            {
                return null;
            }

            ///
            return BuildAgent.GetComponent<SkillMetaData>();
        }

        private SkillDescriptor GetSkillDescriptor()
        {
            if (BuildAgent == null)
            {
                return null;
            }

            ///
            return BuildAgent.GetComponent<SkillDescriptor>();
        }

#if UNITY_EDITOR
        public void Editor_GetOutputNodes(List<SkillNode> outputNodes, SkillTree skillTree)
        {
            outputNodes.Clear();

            ///
            foreach (var item in Ports)
            {
                ///
                if (item.IsInput
                    || item.ConnectionCount == 0)
                {
                    continue;
                }

                ///
                for (int i = 0; i < item.ConnectionCount; i++)
                {
                    var node = (item.GetConnection(i)?.node) as SkillGraphNode;

                    ///
                    if (node != null)
                    {
                        outputNodes.Add(skillTree.Editor_GetSkillNode(node));
                    }
                }
            }
        }

        public Color Editor_GetCashTierColor()
        {
            ///
            if (IsDecorative
                || (currency != Currency.CASH && currency != Currency.BOSS && currency != Currency.P7)
                )
            {
                return Color.white;
            }

            ///
            if (currency == Currency.BOSS
                || currency == Currency.P7)
            {
                return DevEntry.Instance.currencyConfigManager.Editor_GetConfig(currency).Color;
            }

            ///
            if (DevEntry.Instance?.cashTiers == null)
            {
                return Color.white;
            }

            ///
            return DevEntry.Instance.cashTiers.GetColor(price.cashTier, (float)price.b);
        }

        [ContextMenu("Editor_LogCosts")]
        public void Editor_LogCosts()
        {
            for (int i = 0; i < LevelCount; i++)
            {
                Debug.Log($"{name} - Level {i} -> {i + 1}: {GetNextLevelCost(i).amount} {currency}");
            }
        }
#endif    

    }
}