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
        [SerializeField]
        private SkillNode skillNodePrototype;

        [Space]
        [SerializeField]
        private bool attentionFlag;

        [Space]
        [SerializeField]
        private Sprite icon;

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
        [SerializeField]
        private CashTiers.CashTier cashTier = CashTiers.CashTier.NotSet;
        [OneLineWithHeader]
        public List<CurrencyAmount> costs_1 = new List<CurrencyAmount>() { new CurrencyAmount() { currency = Currency.CASH, amount = 1 } };
        [OneLineWithHeader]
        public List<CurrencyAmount> costs_2 = new List<CurrencyAmount>();
        [OneLineWithHeader]
        public List<CurrencyAmount> costs_3 = new List<CurrencyAmount>();
        [SerializeField]
        private List<string> costFormulas = new List<string>();

        public bool IsRoot => GetInputValue("isRoot", false);
        public string NodeId => name;
        public SkillNode SkillNodePrototype => skillNodePrototype;
        public Sprite Icon { get => icon; }
        public bool HasDemoLimit => demoLimit >= 0;
        public int DemoLimit { get => demoLimit; }
        public int UnlockingRequirement { get => unlockingRequirement; }
        public int MinParentLevelEach { get => minParentLevelEach; }
        public CashTiers.CashTier CashTier { get => cashTier; }
        public BuildAgent BuildAgent { get => buildAgent; }
        public double BuildValue { get => buildValue; }
        public int LevelCount => costs_1 == null ? 0 : costs_1.Count;
        public Currency SecondaryCurrency => (costs_2 == null || costs_2.Count == 0) ? Currency.INVALID : costs_2[0].currency;
        public Currency ThirdCurrency => (costs_3 == null || costs_3.Count == 0) ? Currency.INVALID : costs_3[0].currency;
        public bool AttentionFlag { get => attentionFlag; }

        public override object GetValue(NodePort port)
        {
            return null;
        }

        public void CopyCosts(List<CurrencyAmount> costList_1, List<CurrencyAmount> costList_2, List<CurrencyAmount> costList_3)
        {
            costList_1.Clear();
            costList_2.Clear();
            costList_3.Clear();

            ///
            costList_1.AddRange(costs_1);
            costList_2.AddRange(costs_2);
            costList_3.AddRange(costs_3);

            ///
            var castTierConfig = Entry.Instance.cashTiers.GetConfig(cashTier);
            ApplyCashTier(costList_1, castTierConfig);
            ApplyCashTier(costList_2, castTierConfig);
            ApplyCashTier(costList_3, castTierConfig);
        }

        private void ApplyCashTier(List<CurrencyAmount> costList, CashTiers.CashTierConfig config)
        {
            for (int i = 0; i < costList.Count; i++)
            {
                var cm = costList[i];
                if (cm.currency != Currency.CASH)
                {
                    continue;
                }

                ///
                cm.amount *= config.cashBase;
                cm.amount = System.Math.Round(cm.amount, 0);
                costList[i] = cm;
            }
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
            if (costs_1 == null
                || costs_1.Count == 0
                || (costs_1[0].currency != Currency.CASH && costs_1[0].currency != Currency.BOSS && costs_1[0].currency != Currency.P7)
                )
            {
                return Color.white;
            }

            ///
            var firstCurrency = costs_1[0].currency;
            if (firstCurrency == Currency.BOSS
                || firstCurrency == Currency.P7)
            {
                return DevEntry.Instance.currencyConfigManager.Editor_GetConfig(firstCurrency).Color;
            }


            ///
            if (DevEntry.Instance?.cashTiers == null)
            {
                return Color.white;
            }

            ///
            return DevEntry.Instance.cashTiers.GetColor(cashTier, (float)costs_1[0].amount);
        }

        [ContextMenu("Editor_FillCosts")]
        public void Editor_FillCosts()
        {
            ///
            if (costFormulas == null)
            {
                return;
            }

            ///
            if (costFormulas.Count >= 1)
            {
                Editor_FillCosts(costFormulas[0], costs_1, costs_1.Count);
            }
            if (costFormulas.Count >= 2)
            {
                Editor_FillCosts(costFormulas[1], costs_2, costs_1.Count);
            }
            if (costFormulas.Count >= 3)
            {
                Editor_FillCosts(costFormulas[2], costs_3, costs_1.Count);
            }
        }

        private void Editor_FillCosts(string formula, List<CurrencyAmount> costList, int count)
        {
            ///
            if (string.IsNullOrWhiteSpace(formula)
                || costList == null
                || costList.Count == 0)
            {
                return;
            }

            ///
            UnityEditor.Undo.RecordObject(this, "Fill Costs");

            ///
            var baseCost = costList[0];

            ///
            costList.Clear();
            double previousCost = 0;
            for (int i = 0; i < count; i++)
            {
                ///
                if (i == 0)
                {
                    costList.Add(baseCost);
                    previousCost = baseCost.amount;
                    continue;
                }

                ///
                var cost = Editor_GetCost(formula, baseCost, i, previousCost);
                costList.Add(cost);

                ///
                previousCost = cost.amount;
            }
        }

        private CurrencyAmount Editor_GetCost(string formula, CurrencyAmount baseCost, int index, double previousCost)
        {
            double amount;
            var finalFormula = formula.Replace("i", index.ToString())
                                .Replace("b", baseCost.amount.ToString())
                                .Replace("p", previousCost.ToString());
            var success = ExpressionEvaluator.Evaluate(finalFormula, out amount);
            amount = baseCost.currency != Currency.CASH ? Math.Floor(amount) : amount;

            ///
            if (!success)
            {
                throw new System.InvalidOperationException(string.Format("Could not evaluate formula: {0}"));
            }

            ///
            return new CurrencyAmount()
            {
                amount = amount,
                currency = baseCost.currency,
            };
        }
#endif    

    }
}