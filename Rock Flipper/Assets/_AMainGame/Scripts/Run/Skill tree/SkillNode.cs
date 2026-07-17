using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GD;
using Agame.Run.Stats;
using OneLine;
using XNode;
using Agame.Balancing;
using Agame.FeatureBranching;
using Agame.Meta;
using I2.Loc;

namespace Agame.Run
{
    [DisallowMultipleComponent]
    public partial class SkillNode : ExtendedMonoBehaviourRun
    {
        private const float SnapThreshold = 100f;

        public event System.Action OnStateChanged;

        #region From the graph
        [Header("--------From graph")]
        [SerializeField, ReadOnly]
        private SkillGraphNode graphNode;
        [SerializeField, ReadOnly]
        private Vector2Int nodePosition;

        [Space]
        private List<CurrencyAmount> costs_1;
        private List<CurrencyAmount> costs_2;
        private List<CurrencyAmount> costs_3;

        [Space]
        [SerializeField, ReadOnly]
        private List<SkillNode> outputNodes = new List<SkillNode>();
        #endregion From the graph

        [Space]
        [Header("--------Set at import")]
        [SerializeField, ReadOnly]
        private SkillTree skillTree;
        [SerializeField, ReadOnly]
        private bool isSpecialEntry;

        [Header("Overrider")]
        [SerializeField]
        private SkillNodeOverrider overrider;

        [Space]
        [Header("--------Prefab config")]
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private UnifiedText gradeText;

        [Space]
        [SerializeField]
        private SkillNodeToolTip toolTipPrototype;
        [SerializeField]
        private Transform toolTipTopRightAnchor;
        [SerializeField]
        private Transform toolTipBottomRightAnchor;
        [SerializeField]
        private Transform toolTipTopLeftAnchor;
        [SerializeField]
        private Transform toolTipBottomLeftAnchor;

        [Space]
        [SerializeField]
        private Image borderImage;
        [SerializeField]
        private Color notEnoughColor;
        [SerializeField]
        private Color enoughColor;
        [SerializeField]
        private Color maxedColor;
        [SerializeField]
        private float notEnoughIconAlpha = 0.5f;
        [SerializeField]
        private GameObject upgradeFxObject;
        [SerializeField]
        private Image upgradeFxImage;
        [SerializeField]
        private GameObject maxedObject;
        [SerializeField]
        private GameObject demoLimitedObject;

        [Space]
        [SerializeField]
        private GameObject editor_AttentionObject;

        [Space]
        [SerializeField, ReadOnly]
        private List<SkillNodeConnector> connectors = new List<SkillNodeConnector>();

        [Space]
        [SerializeField, Tooltip("From the top, clockwise")]
        private List<SkillNodeConnector> allConnectors = new List<SkillNodeConnector>();

        private List<SkillNode> parents = new List<SkillNode>();
        private SkillNodeToolTip activeToolTip;
        private SkillNodeState state;
        private string description;
        private int currencyCount;
        private float lastTimeHandledClick;

        public SkillTree SkillTree { get => skillTree; }
        public bool IsSpecialEntry { get => isSpecialEntry; }
        public SkillGraphNode GraphNode => graphNode;
        public bool IsActivated { get; private set; }
        public string NodeId => graphNode.NodeId;
        public Sprite Icon => graphNode.Icon;
        public int OutputNodeCount => outputNodes.Count;
        public int ParentNodeCount => parents.Count;
        /// <summary>
        /// how far it is from the root node
        /// </summary>
        public int DepthMax { get; private set; } = -1;
        public int Level
        {
            get
            {
                ///
                TryInit();

                ///
                return Mathf.Min(state.level, LevelCount);
            }
        }
        public int LevelCount
        {
            get
            {
                ///
                if (overrider != null && overrider.GetLevelCount(out var rs))
                {
                    return rs;
                }

                ///
                return costs_1 == null ? 0 : costs_1.Count;
            }
        }
        public bool IsMaxed => Level >= LevelCount;
        public string Description
        {
            get
            {
                ///
                bool forceRebuild = false;

                ///
#if UNITY_EDITOR
                forceRebuild = true;
#endif

                ///
                if (description == null
                    || forceRebuild)
                {
                    ///
                    var buildAgent = graphNode.BuildAgent;
                    var buildValue = graphNode.BuildValue;

                    ///
                    if (buildAgent != null)
                    {
                        description = buildAgent.GetDescriptionText(buildValue);
                    }
                    else
                    {
                        description = "<No BuildAgent>";
                    }
                }

                ///
                return description;
            }
        }
        public string ExtraDescription
        {
            get
            {
                ///
                var buildAgent = graphNode.BuildAgent;

                ///
                if (buildAgent != null)
                {
                    return buildAgent.ExtraDescription;
                }
                else
                {
                    return "<No BuildAgent>";
                }
            }
        }
        public Transform ToolTipTopRightAnchor => toolTipTopRightAnchor;
        public Transform ToolTipBottomRightAnchor => toolTipBottomRightAnchor;
        public Transform ToolTipTopLeftAnchor => toolTipTopLeftAnchor;
        public Transform ToolTipBottomLeftAnchor => toolTipBottomLeftAnchor;
        public Vector2Int NodePosition => nodePosition;
        public CashTiers.CashTier CashTier => graphNode.CashTier;
        public bool IsMaxable
        {
            get
            {
                ///
                if (overrider != null && overrider.GetIsMaxable(out var result))
                {
                    return result;
                }

                ///
                return !IsSpecialEntry;
            }
        }

        public bool IsUpgradable
        {
            get
            {
                return !IsSpecialEntry;
            }
        }

        public bool IsDemoLimited
        {
            get
            {
                ///
                if (!VersionBranchInfo.IsPlaytestOrDemo)
                {
                    return false;
                }

                if (graphNode.HasDemoLimit
                    && Level >= graphNode.DemoLimit)
                {
                    return true;
                }

                ///
                return false;
            }
        }

        protected override bool Init()
        {
            ///
            if (overrider != null)
            {
                ///
                overrider.SkillNode = this;

                ///
                overrider.OnStateChanged += Overrider_OnStateChanged;
            }

            ///
            return base.Init();
        }

        private void Overrider_OnStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        private void Init(bool resetState)
        {
            ///
            costs_1 = new List<CurrencyAmount>();
            costs_2 = new List<CurrencyAmount>();
            costs_3 = new List<CurrencyAmount>();
            graphNode.CopyCosts(costs_1, costs_2, costs_3);

            ///
            state = resetState ? new SkillNodeState() : RunData.GetSkillNodeState(NodeId);

            ///
            CountCurrencies();

            ///
#if UNITY_EDITOR
            iconImage.sprite = graphNode.Icon;
            iconImage.color = graphNode.Editor_GetCashTierColor();
#endif

            ///
            upgradeFxImage.color = iconImage.color;
        }

        private void CountCurrencies()
        {
            currencyCount = 1;
            if (costs_2 != null && costs_2.Count > 0)
            {
                currencyCount++;
            }
            if (costs_3 != null && costs_3.Count > 0)
            {
                currencyCount++;
            }
        }

        protected void OnDisable()
        {
            ///
            upgradeFxObject.SetActive(false);

            ///
            RunData.OnCurrencyValueModified -= RunData_OnCurrencyValueModified;
        }

        protected void OnEnable()
        {
            UpdateLevelRelatedVisuals();

            ///
            RunData.OnCurrencyValueModified += RunData_OnCurrencyValueModified;
        }

        private bool CostsContain(Currency currency)
        {
            if (costs_1[0].currency == currency)
            {
                return true;

            }
            if (costs_2 != null && costs_2.Count > 0 && costs_2[0].currency == currency)
            {
                return true;
            }
            if (costs_3 != null && costs_3.Count > 0 && costs_3[0].currency == currency)
            {
                return true;
            }

            ///
            return false;
        }

        private void RunData_OnCurrencyValueModified(Currency currency)
        {
            ///
            if (IsMaxed)
            {
                return;
            }

            ///
            if (CostsContain(currency))
            {
                UpdateLevelRelatedVisuals();
            }
        }

        public SkillNode GetParentNode(int index)
        {
            return parents[index];
        }

        public SkillNode GetOutputNode(int index)
        {
            var node = outputNodes[index];

            ///
            if (node.isSpecialEntry)
            {
                return skillTree.GetActiveSpecialEntry();
            }
            else
            {
                return node;
            }
        }

        public void AddParentNode(SkillNode parentNode)
        {
            ///
            if (parentNode == null)
            {
                if (parents.Count > 0)
                {
                    throw new System.InvalidOperationException("Can't set current node as root!");
                }

                ///
                DepthMax = 0;

                ///
                return;
            }

            ///
            if (parents.Contains(parentNode))
            {
                throw new System.InvalidCastException("Can't assign a parent node twice");
            }

            ///
            if (parentNode.DepthMax < 0)
            {
                Debug.LogError("Can't assign node with Depth<0 as parent.", gameObject);
                throw new System.Exception();
            }

            ///
            parents.Add(parentNode);
            if (DepthMax < (parentNode.DepthMax + 1))
            {
                DepthMax = parentNode.DepthMax + 1;
            }
        }

        private SkillNodeConnector GetConnector(Direction8 direction8)
        {
            return allConnectors[(int)direction8];
        }

        private SkillNodeConnector GetConnector(int x, int y)
        {
            if (x == 0)
            {
                if (y == 0)
                {
                    return null;
                }
                else if (y < 0)
                {
                    return GetConnector(Direction8.Down);
                }
                else
                {
                    return GetConnector(Direction8.Up);
                }
            }
            else if (x < 0)
            {
                if (y == 0)
                {
                    return GetConnector(Direction8.Left);
                }
                else if (y < 0)
                {
                    return GetConnector(Direction8.DownLeft);
                }
                else
                {
                    return GetConnector(Direction8.UpLeft);
                }
            }
            else
            {
                if (y == 0)
                {
                    return GetConnector(Direction8.Right);
                }
                else if (y < 0)
                {
                    return GetConnector(Direction8.DownRight);
                }
                else
                {
                    return GetConnector(Direction8.UpRight);
                }
            }
        }

        [ContextMenu("ShowToolTip"), PlayModeOnly]
        public void ShowToolTip()
        {
            if (activeToolTip != null)
            {
                return;
            }

            ///
            activeToolTip = generalPool.TakeInstance(toolTipPrototype, this);
            activeToolTip.transform.SetParent(CommonEntry.tooltipTransformParent);
            activeToolTip.transform.localScale = Vector3.one;
            activeToolTip.ShowFor(this);
        }

        [ContextMenu("ReleaseToolTip"), PlayModeOnly]
        public void ReleaseToolTip()
        {
            if (activeToolTip == null)
            {
                return;
            }

            ///
            activeToolTip.TryReturnToPoolAndDeactivate();
            activeToolTip = null;
        }

        public void HandleClick()
        {
            ///
            TryInit();

            ///
            var timeSinceLastClick = Time.realtimeSinceStartup - lastTimeHandledClick;
            lastTimeHandledClick = Time.realtimeSinceStartup;

            ///
            if (IsDemoLimited)
            {
                if (timeSinceLastClick >= 2)
                {
                    MetaUrlLauncher.LaunchSteamPageStatic();
                }

                ///
                return;
            }

            ///
            if (graphNode.AttentionFlag)
            {
                Debug.LogError("This node needs attention");

                ///
#if UNITY_EDITOR
                Editor_SelectGraphNode();
#endif

                ///
                return;
            }

            ///
            if (overrider != null && overrider.HandleClick())
            {
                return;
            }

            ///
            if (IsMaxed
                || !IsEnoughCurrencyToLevelUp())
            {
                return;
            }

            ///
            if (!graphNode.BuildAgent.RefundCost && !Spend())
            {
                return;
            }

            ///
            LevelUp();

            ///
            if (IsMaxed)
            {
                ///
                RunEntry.skillTree.TryToTriggerMaxedSkillTreeAchievement();
            }

            ///
            PlayClickSoundAndUpgradeFx();
        }

        public void PlayClickSoundAndUpgradeFx()
        {
            upgradeFxObject.SetActive(false);
            upgradeFxObject.SetActive(true);

            ///
            entry.uiSoundManager.PlayPressSound();
            entry.clickParticleManager.Play();
        }

        public void LevelUp()
        {
            ///
            if (IsMaxed)
            {
                return;
            }

            ///
            state.level++;

            ///
            RunData.UpdateSkillNodeState(NodeId, state);

            ///
            var buildAgent = graphNode.BuildAgent;
            var buildValue = graphNode.BuildValue;

            ///
            if (buildAgent != null)
            {
                buildAgent.Apply(1, buildValue);
            }
            else
            {
                Debug.LogWarning("No BuildAgent!", this);
            }

            ///
            OnStateChanged?.Invoke();

            ///
            TryActivateOutputNodes(true, true);

            ///
            UpdateLevelRelatedVisuals();

            ///
            if (IsMaxed)
            {
                RunEntry.skillTree.RegisterMaxedSkill(this);
                RunEntry.skillCostTracker.RemoveTrackingSkill(this);
            }
            else
            {
                RunEntry.skillCostTracker.UpdateTrackingSkill(this, false);
            }

            ///
            entry.playerDataSaver.SetSaveThisFrame();

            ///
            buildAgent.TryToReportAchievement();
            if (IsMaxed)
            {
                buildAgent.TryToReportMaxedAchievement();
            }
        }

        private List<CurrencyAmount> GetCostList(int index)
        {
            switch (index)
            {
                case 0: return costs_1;
                case 1: return costs_2;
                case 2: return costs_3;
                default:
                    throw new System.NotImplementedException();
            }
        }

        private bool Spend()
        {
            var costIndex = GetNextLevelCostIndex();

            ///
            if (!RunData.SpendCurrency(costs_1.GetItemWithClampedIndex(costIndex)))
            {
                return false;
            }
            if (costs_2 != null && costs_2.Count > 0 && !RunData.SpendCurrency(costs_2.GetItemWithClampedIndex(costIndex)))
            {
                return false;
            }
            if (costs_3 != null && costs_3.Count > 0 && !RunData.SpendCurrency(costs_3.GetItemWithClampedIndex(costIndex)))
            {
                return false;
            }

            ///
            return true;
        }

        private void DisplayConnectorsToActivatedOutputNodes()
        {
            for (int i = 0; i < outputNodes.Count; i++)
            {
                var outputNode = GetOutputNode(i);

                ///
                var connector = connectors[i];

                ///
                if (outputNode.IsActivated)
                {
                    ///
                    connector.gameObject.SetActive(true);
                }
            }
        }

        public void Deactivate()
        {
            IsActivated = false;

            ///
            gameObject.SetActive(false);

            ///
            overrider?.OnDeactivate();
        }

        public void ApplyToBuildStats()
        {
            var buildAgent = graphNode.BuildAgent;
            var buildValue = graphNode.BuildValue;

            ///
            if (buildAgent != null)
            {
                buildAgent.Apply(Mathf.Clamp(Level, 0, LevelCount), buildValue);
            }
            else
            {
                Debug.LogWarning("No BuildAgent!", this);
            }
        }

        public void Activate(bool applyToBuildStats, bool resetState)
        {
#if UNITY_EDITOR
            if (editor_AttentionObject.activeSelf != graphNode.AttentionFlag)
            {
                editor_AttentionObject.SetActive(graphNode.AttentionFlag);
                Debug.LogError("The Skill Tree needs to be re-imported");
            }
#endif

            ///
            IsActivated = true;

            ///
            Init(resetState);

            ///
            gameObject.SetActive(true);

            ///
            overrider?.OnActivate(applyToBuildStats, resetState);

            ///
            foreach (var item in parents)
            {
                item.DisplayConnectorsToActivatedOutputNodes();
            }

            ///           
            if (applyToBuildStats)
            {
                ApplyToBuildStats();
            }

            ///
            if (!IsMaxed)
            {
                RunEntry.skillCostTracker.UpdateTrackingSkill(this, true);
            }
            else
            {
                RunEntry.skillTree.RegisterMaxedSkill(this);
            }

            ///
            TryActivateOutputNodes(applyToBuildStats, resetState);
        }

        private void TryActivateOutputNodes(bool applyToBuildStats, bool resetState)
        {
            for (int i = 0; i < outputNodes.Count; i++)
            {
                var outputNode = GetOutputNode(i);

                ///
                var connector = connectors[i];

                ///
                if (outputNode.IsActivated)
                {
                    ///
                    connector.gameObject.SetActive(true);

                    ///
                    continue;
                }

                ///
                if (outputNode.IsUnlocked())
                {
                    ///
                    connector.gameObject.SetActive(true);
                    outputNode.Activate(applyToBuildStats, resetState);
                }
                else
                {
                    connector.gameObject.SetActive(false);
                }
            }
        }

        public bool IsUnlocked()
        {
            ///
            int totalParentLevel = 0;

            ///
            var minParentLevelEach = graphNode.MinParentLevelEach;

            ///
            foreach (var item in parents)
            {
                ///
                if (!item.IsActivated)
                {
                    ///
                    if (minParentLevelEach > 0)
                    {
                        return false;
                    }

                    ///
                    continue;
                }

                ///
                if (minParentLevelEach > item.Level)
                {
                    return false;
                }

                ///
                totalParentLevel += item.Level;
            }

            ///
            return totalParentLevel >= graphNode.UnlockingRequirement;
        }

        private int GetNextLevelCostIndex()
        {
            return state.level;
        }

        public bool IsEnoughCurrencyToLevelUp()
        {
            ///
            if (overrider != null && overrider.IsEnoughCurrencyToLevelUp(out var result))
            {
                return result;
            }

            ///
            if (graphNode.AttentionFlag)
            {
                return false;
            }

            ///
            var costIndex = GetNextLevelCostIndex();

            ///
            if (!RunData.IsEnough(costs_1.GetItemWithClampedIndex(costIndex)))
            {
                return false;
            }
            if (costs_2 != null && costs_2.Count > 0 && !RunData.IsEnough(costs_2.GetItemWithClampedIndex(costIndex)))
            {
                return false;
            }
            if (costs_3 != null && costs_3.Count > 0 && !RunData.IsEnough(costs_3.GetItemWithClampedIndex(costIndex)))
            {
                return false;
            }

            ///
            return true;
        }

        public string GetCostString()
        {
            ///
            TryInit();

            ///
            var s = "";

            ///
            if (overrider != null)
            {
                s = overrider.GetCostString();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    return s;
                }
            }

            ///
            var costIndex = GetNextLevelCostIndex();

            ///
            for (int i = 0; i < currencyCount; i++)
            {
                var costList = GetCostList(i);
                var currencyAmount = costList.GetItemWithClampedIndex(costIndex);
                var currency = currencyAmount.currency;
                var currencyConfig = entry.currencyConfigManager.GetConfig(currency);
                var currencyValue = RunData.GetCurrencyValue(currency);
                var isEnough = currencyValue >= currencyAmount.amount;

                ///
                if (i > 0)
                {
                    s += "\r\n";
                }
                ;

                ///
                if (isEnough)
                {
                    s += string.Format("{0} {1}/{2}", currencyConfig.CurrencyName, currencyValue.ToLargeNumberString(), currencyAmount.amount.ToLargeNumberString());
                }
                else
                {
                    s += string.Format("{0} <color=#{3}>{1}/{2}</color>", currencyConfig.CurrencyName, currencyValue.ToLargeNumberString(), currencyAmount.amount.ToLargeNumberString(), ColorUtility.ToHtmlStringRGB(VisualDefinitions.Instance.notEnoughTextColor));
                }
            }

            ///
            if (graphNode.BuildAgent.RefundCost)
            {
                s += $"\r\n<size=60%><i>*Cost will be fully refunded</i></size>";
            }

            ///
            return s;
        }

        public void UpdateLevelRelatedVisuals()
        {
            Color color;

            ///
            float iconAlpha = 1;
            if (IsMaxed)
            {
                color = maxedColor;
            }
            else if (IsEnoughCurrencyToLevelUp())
            {
                color = enoughColor;
            }
            else
            {
                color = notEnoughColor;
                iconAlpha = notEnoughIconAlpha;
            }

            ///
            borderImage.color = color;
            iconImage.color = iconImage.color.OverrideAlpha(iconAlpha);

            ///
            maxedObject.SetActive(IsMaxed);

            ///
            demoLimitedObject.SetActive(IsDemoLimited);
        }

    }

}