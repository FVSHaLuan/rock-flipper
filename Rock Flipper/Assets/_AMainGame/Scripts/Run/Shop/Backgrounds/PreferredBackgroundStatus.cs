using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows the status of the Preferred Multiplier: whether the Preferred feature is available yet, whether the
    /// player is using the Preferred background as the active background, and how to increase the multiplier.
    /// </summary>
    public class PreferredBackgroundStatus : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private UnifiedText statusText;

        private RunData subscribedRunData;
        private SkillTreeScreen subscribedSkillTreeScreen;

        protected void OnEnable()
        {
            ///
            subscribedRunData = RunData;
            subscribedRunData.OnActiveBackgroundIdChanged += RunData_OnBackgroundStateChanged;
            subscribedRunData.OnPreferredBackgroundIdChanged += RunData_OnBackgroundStateChanged;
            subscribedRunData.OnBackgroundUnlocked += RunData_OnBackgroundStateChanged;

            // Skill tree upgrades to the Preferred Multiplier reach the player once the screen closes.
            subscribedSkillTreeScreen = RunEntry.skillTreeScreen;
            subscribedSkillTreeScreen.OnClosed += SkillTreeScreen_OnClosed;

            ///
            UpdateView();
        }

        protected void OnDisable()
        {
            // RunData outlives this scene, so drop the handlers even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnActiveBackgroundIdChanged -= RunData_OnBackgroundStateChanged;
                subscribedRunData.OnPreferredBackgroundIdChanged -= RunData_OnBackgroundStateChanged;
                subscribedRunData.OnBackgroundUnlocked -= RunData_OnBackgroundStateChanged;
                subscribedRunData = null;
            }
            if (subscribedSkillTreeScreen != null)
            {
                subscribedSkillTreeScreen.OnClosed -= SkillTreeScreen_OnClosed;
                subscribedSkillTreeScreen = null;
            }
        }

        private void RunData_OnBackgroundStateChanged(int backgroundId)
        {
            UpdateView();
        }

        private void SkillTreeScreen_OnClosed()
        {
            UpdateView();
        }

        private static string FormatMultiplier(double multiplier)
        {
            return $"x{multiplier:0.##}";
        }

        private void UpdateView()
        {
            ///
            var unlockedCount = RunData.UnlockedBackgroundCount;

            // With only 1 background unlocked there is no Preferred background.
            if (unlockedCount < 2)
            {
                statusText.SetText("Unlock more backgrounds to unlock the Preferred background.");
                return;
            }

            ///
            var multiplier = FormatMultiplier(BuildStats.GetPreferredMultiplier(unlockedCount));
            if (RunData.ActiveBackgroundId != RunData.PreferredBackgroundId)
            {
                statusText.SetText($"Select the Preferred background to get <b>{multiplier}</b> cash income.");
                return;
            }

            ///
            var text = $"Preferred background: <b>{multiplier}</b> cash income.";
            if (unlockedCount < GameConst.BackgroundCount)
            {
                text += "\nUnlock more backgrounds to increase it.";
            }
            statusText.SetText(text);
        }
    }

}
