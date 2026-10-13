using System;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Calculates and caches the Preferred Multiplier, the cash income multiplier for using the Preferred background as
    /// the active background. Cash income sources multiply their income by <see cref="CashIncomeMultiplier"/>.
    /// Recalculated whenever the backgrounds or the build stats change.
    /// </summary>
    public class PreferredBackgroundMultiplier : ExtendedMonoBehaviourRun
    {
        /// <summary>
        /// Invoked after every recalculation, whether or not the values changed.
        /// </summary>
        public event Action OnChanged;

        private RunData subscribedRunData;
        private SkillTreeScreen subscribedSkillTreeScreen;
        private RunStateManager subscribedStateManager;

        /// <summary>
        /// Whether there is a Preferred background, i.e. at least 2 backgrounds are unlocked.
        /// </summary>
        public bool HasPreferredBackground { get; private set; }
        /// <summary>
        /// Whether the active background is the Preferred background, so <see cref="CashIncomeMultiplier"/> applies.
        /// </summary>
        public bool IsUsingPreferredBackground { get; private set; }
        /// <summary>
        /// The multiplier the player gets by using the Preferred background, whether or not they are using it.
        /// 1 when there is no Preferred background.
        /// </summary>
        public double PreferredMultiplier { get; private set; } = 1;
        /// <summary>
        /// The multiplier to apply to cash income right now: <see cref="PreferredMultiplier"/> while using the
        /// Preferred background, 1 otherwise.
        /// </summary>
        public double CashIncomeMultiplier => IsUsingPreferredBackground ? PreferredMultiplier : 1;

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

            // Build stats are rebuilt right before a combat starts (e.g. after prestige).
            subscribedStateManager = StateManager;
            subscribedStateManager.OnCombatStarted += StateManager_OnCombatStarted;

            ///
            Recalculate();
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
            if (subscribedStateManager != null)
            {
                subscribedStateManager.OnCombatStarted -= StateManager_OnCombatStarted;
                subscribedStateManager = null;
            }
        }

        private void RunData_OnBackgroundStateChanged(int backgroundId)
        {
            Recalculate();
        }

        private void SkillTreeScreen_OnClosed()
        {
            Recalculate();
        }

        private void StateManager_OnCombatStarted()
        {
            Recalculate();
        }

        private void Recalculate()
        {
            ///
            var runData = RunData;
            var unlockedCount = runData.UnlockedBackgroundCount;

            // With only 1 background unlocked there is no Preferred background.
            HasPreferredBackground = unlockedCount >= 2;
            IsUsingPreferredBackground = HasPreferredBackground && runData.ActiveBackgroundId == runData.PreferredBackgroundId;
            PreferredMultiplier = BuildStats.GetPreferredMultiplier(unlockedCount);

            ///
            OnChanged?.Invoke();
        }
    }

}
