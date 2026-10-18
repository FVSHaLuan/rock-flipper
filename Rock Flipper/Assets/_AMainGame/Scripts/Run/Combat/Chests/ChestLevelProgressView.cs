using Agame.Balancing;
using Agame.Localization;
using Agame.UI.ToolTips;
using GD;
using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(ToolTipTriggerer))]
    public class ChestLevelProgressView : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private ProgressBar progressBar;

        private ToolTipTriggerer toolTipTriggerer;

        protected override void ExtendedAwake()
        {
            base.ExtendedAwake();

            ///
            toolTipTriggerer = GetComponent<ToolTipTriggerer>();
            toolTipTriggerer.OnBeforeShow += UpdateToolTipContent;
        }

        protected void Start()
        {
            UpdateProgressBar();
        }

        protected void Update()
        {
            UpdateProgressBar();

            ///
            if (toolTipTriggerer.IsShowing)
            {
                UpdateToolTipContent();
            }
        }

        private void UpdateProgressBar()
        {
            var nextLevelRequirement = GameBalance.GetRequiredExpForNextChestLevel(RunData.ChestLevel);
            var currentExp = RunData.ChestLevelExp;
            progressBar.SetValue((float)(currentExp / nextLevelRequirement));
        }

        private void UpdateToolTipContent()
        {
            var nextLevelRequirement = GameBalance.GetRequiredExpForNextChestLevel(RunData.ChestLevel);
            var specificStrings = SpecificLocalizedStrings.Instance;
            var content = $"<b>{specificStrings.chestBar.GetLocalizedString()}</b>\n";
            content += $"{specificStrings.chestBarDescription.GetLocalizedString()}\n";
            content += $"{LocalizedStrings.Instance.exp.GetLocalizedString()}: {RunData.ChestLevelExp.ToLargeNumberString()}/{nextLevelRequirement.ToLargeNumberString()}";

            ///
            if (content != toolTipTriggerer.MainText)
            {
                toolTipTriggerer.MainText = content;
            }
        }
    }

}
