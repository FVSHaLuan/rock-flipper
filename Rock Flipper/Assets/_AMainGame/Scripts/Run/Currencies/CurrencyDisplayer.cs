using UnityEngine;
using UnityEngine.UI;

namespace BT.Run
{
    public class CurrencyDisplayer : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject wrapperView;
        [SerializeField]
        private LayoutElement layoutElement;
        [SerializeField]
        private Currency currency;
        [SerializeField]
        private UnifiedText text;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private bool disableCountingEffect = true;
        [SerializeField]
        private float countingDuration = 0.5f;
        [SerializeField]
        private bool forceAlwaysDisplayed;
        [SerializeField]
        private bool exponentialStyle;

        private double targetValue;
        private double currentValue;
        private double startCountingValue;
        private bool isCounting;
        private float countingTime;

        public Currency Currency
        {
            get => currency;
            set
            {
                currency = value;
                UpdateIcon();
                DisplayImmediately();
            }
        }

        public bool ForceAlwaysDisplayed
        {
            get => forceAlwaysDisplayed;
            set
            {
                forceAlwaysDisplayed = value;

                ///
                DisplayImmediately();
            }
        }

        protected void Start()
        {
            ///
            UpdateIcon();
        }

        protected void OnDisable()
        {
            RunData.OnCurrencyValueModifiedThisFrame -= RunData_OnCurrencyValueModified;
            StateManager.OnBeforeCombatFromPrestige -= StateManager_OnBeforeCombatFromPrestige;
        }

        protected void OnEnable()
        {
            ///
            DisplayImmediately();

            ///
            RunData.OnCurrencyValueModifiedThisFrame += RunData_OnCurrencyValueModified;
            StateManager.OnBeforeCombatFromPrestige += StateManager_OnBeforeCombatFromPrestige;
        }

        private void StateManager_OnBeforeCombatFromPrestige()
        {
            DisplayImmediately();
        }

        protected void LateUpdate()
        {
            if (!isCounting)
            {
                return;
            }

            ///
            countingTime += Time.deltaTime;
            if (countingTime >= countingDuration)
            {
                isCounting = false;
                currentValue = targetValue;
            }
            else
            {
                var progress = countingTime / countingDuration;
                currentValue = System.Math.Floor(Mathg.Lerp(startCountingValue, targetValue, progress));
            }

            ///
            DisplayCurrentValue();
        }

        private void RunData_OnCurrencyValueModified(Currency obj)
        {
            ///
            if (obj != currency)
            {
                return;
            }

            ///
            targetValue = GetTargetValue();

            ///
            if (disableCountingEffect || Mathg.Approximately(targetValue, currentValue))
            {
                DisplayImmediately();
                return;
            }

            ///
            startCountingValue = currentValue;
            isCounting = true;
            countingTime = 0;
        }

        private void UpdateIcon()
        {
            ///
            if (icon == null)
            {
                return;
            }

            ///
            var config = Entry.Instance.currencyConfigManager.GetConfig(currency);
            icon.sprite = config.Icon;
            icon.color = config.Color;
        }

        protected string GetString(double value)
        {
            return exponentialStyle ? value.ToExponentialString() : value.ToLargeNumberString();
        }

        protected double GetTargetValue()
        {
            return RunEntry.Instance.RunData.GetCurrencyValue(currency);
        }

        public void DisplayImmediately()
        {
            currentValue = targetValue = GetTargetValue();
            isCounting = false;
            DisplayCurrentValue();
        }

        private void DisplayCurrentValue()
        {
            ///
            if (currentValue < 0.5f && !ForceAlwaysDisplayed)
            {
                wrapperView.SetActive(false);
                if (layoutElement != null)
                {
                    layoutElement.ignoreLayout = true;
                }
                return;
            }

            ///
            wrapperView.SetActive(true);
            if (layoutElement != null)
            {
                layoutElement.ignoreLayout = false;
            }

            ///
            text.Text = GetString(currentValue);
        }
    }

}