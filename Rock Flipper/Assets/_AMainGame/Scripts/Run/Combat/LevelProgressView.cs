using Agame.Balancing;
using Agame.Localization;
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
            var strings = LocalizedStrings.Instance;
            var content = $"<b>{strings.level.GetLocalizedString()} {RunData.Level}</b>\n";
            content += $"{string.Format(SpecificLocalizedStrings.Instance.levelBarDescription.GetLocalizedString(), stars)}\n";
            content += $"{strings.exp.GetLocalizedString()}: {RunData.LevelExp.ToLargeNumberString()}/{nextLevelRequirement.ToLargeNumberString()}";

            ///
            if (content != toolTipTriggerer.MainText)
            {
                toolTipTriggerer.MainText = content;
            }
        }
    }

}
