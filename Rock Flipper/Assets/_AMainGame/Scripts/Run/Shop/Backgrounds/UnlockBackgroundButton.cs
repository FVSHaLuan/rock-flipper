using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Unlocks a random locked background for stars. The first unlock (the player's first background
    /// besides background 0) costs <see cref="firstUnlockCost"/>, every later one costs <see cref="unlockCost"/>.
    /// </summary>
    public class UnlockBackgroundButton : ExtendedMonoBehaviourRun
    {
        private const Currency CostCurrency = Currency.STAR;

        [SerializeField]
        private Button button;
        [SerializeField]
        private UnifiedText costText;

        [Space]
        [SerializeField, Min(0)]
        private double firstUnlockCost = 0;
        [SerializeField, Min(0)]
        private double unlockCost = 1;

        private readonly List<int> lockedBackgroundIds = new List<int>();
        private RunData subscribedRunData;

        protected override void ExtendedAwake()
        {
            base.ExtendedAwake();
            button.onClick.AddListener(Button_OnClick);
        }

        protected void OnEnable()
        {
            ///
            subscribedRunData = RunData;
            subscribedRunData.OnCurrencyValueModifiedThisFrame += RunData_OnCurrencyValueModifiedThisFrame;
            subscribedRunData.OnBackgroundUnlocked += RunData_OnBackgroundUnlocked;

            ///
            UpdateView();
        }

        protected void OnDisable()
        {
            // RunData outlives this scene, so drop the handlers even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnCurrencyValueModifiedThisFrame -= RunData_OnCurrencyValueModifiedThisFrame;
                subscribedRunData.OnBackgroundUnlocked -= RunData_OnBackgroundUnlocked;
                subscribedRunData = null;
            }
        }

        private void RunData_OnCurrencyValueModifiedThisFrame(Currency currency)
        {
            if (currency == CostCurrency)
            {
                UpdateView();
            }
        }

        private void RunData_OnBackgroundUnlocked(int backgroundId)
        {
            UpdateView();
        }

        private CurrencyAmount GetNextUnlockCost()
        {
            // Background 0 is always unlocked, so a count of 1 means no background has been bought yet.
            var amount = RunData.UnlockedBackgroundCount <= 1 ? firstUnlockCost : unlockCost;
            return new CurrencyAmount(CostCurrency, amount);
        }

        private void CollectLockedBackgroundIds()
        {
            lockedBackgroundIds.Clear();
            var backgroundManager = RunEntry.combatBackgroundManager;
            for (int id = 1; id < backgroundManager.BackgroundCount; id++)
            {
                if (backgroundManager.HasBackground(id) && !RunData.IsBackgroundUnlocked(id))
                {
                    lockedBackgroundIds.Add(id);
                }
            }
        }

        private void UpdateView()
        {
            ///
            CollectLockedBackgroundIds();
            if (lockedBackgroundIds.Count == 0)
            {
                costText.SetText("ALL UNLOCKED");
                button.interactable = false;
                return;
            }

            ///
            var cost = GetNextUnlockCost();
            costText.SetText(cost.amount > 0 ? cost.GetFormattedString() : "FREE");
            button.interactable = RunData.IsEnough(cost);
        }

        private void Button_OnClick()
        {
            ///
            CollectLockedBackgroundIds();
            if (lockedBackgroundIds.Count == 0)
            {
                return;
            }

            ///
            if (!RunData.SpendCurrency(GetNextUnlockCost()))
            {
                return;
            }

            ///
            var backgroundId = lockedBackgroundIds[Random.Range(0, lockedBackgroundIds.Count)];
            RunData.UnlockBackground(backgroundId);
        }
    }

}
