using Agame.Balancing;
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
            var content = "<b>Chest Bar</b>\n";
            content += "Landing rocks fills this bar. When it's full, a chest drops.\n";
            content += $"Exp: {RunData.ChestLevelExp.ToLargeNumberString()}/{nextLevelRequirement.ToLargeNumberString()}";

            ///
            if (content != toolTipTriggerer.MainText)
            {
                toolTipTriggerer.MainText = content;
            }
        }
    }

}
