using Agame.Balancing;
using Agame.UI.ToolTips;
using GD;
using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(ToolTipTriggerer))]
    public class LevelProgressView : ExtendedMonoBehaviourRun
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
            var nextLevelRequirement = gameBalance.GetRequiredExpForNextLevel(RunData.Level);
            var currentExp = RunData.LevelExp;
            progressBar.SetValue((float)(currentExp / nextLevelRequirement));
        }

        private void UpdateToolTipContent()
        {
            var nextLevelRequirement = gameBalance.GetRequiredExpForNextLevel(RunData.Level);
            var stars = new CurrencyAmount(Currency.STAR, GameBalance.StarsPerLevelUp).GetFormattedString();
            var content = $"<b>Level {RunData.Level}</b>\n";
            content += $"Landing rocks fills this bar. When it's full, you level up and earn {stars}.\n";
            content += $"Exp: {RunData.LevelExp.ToLargeNumberString()}/{nextLevelRequirement.ToLargeNumberString()}";

            ///
            if (content != toolTipTriggerer.MainText)
            {
                toolTipTriggerer.MainText = content;
            }
        }
    }

}
