using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run
{
    public class PermanentCurrencyList : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private CurrencyDisplayer firstDisplayer;

        private List<CurrencyDisplayer> displayers;

        protected override bool Init()
        {
            ///
            displayers = new List<CurrencyDisplayer>();

            ///
            var currencyCount = CurrencyExtensions.ValidCount;
            for (int i = 0; i < currencyCount; i++)
            {
                ///
                var currency = CurrencyExtensions.GetValidCurrency(i);
                var config = entry.currencyConfigManager.GetConfig(currency);

                ///
                if (config.IsInCombatOnly)
                {
                    continue;
                }

                ///
                CurrencyDisplayer displayer;
                if (displayers.Count == 0)
                {
                    displayer = firstDisplayer;
                }
                else
                {
                    displayer = Instantiate(firstDisplayer, firstDisplayer.transform.parent);
                    displayer.transform.SetAsLastSibling();
                }

                ///
                displayer.ForceAlwaysDisplayed = currency == Currency.CASH;

                ///
                displayers.Add(displayer);
                displayer.Currency = currency;
            }

            ///
            return base.Init();
        }

        protected void LateUpdate()
        {
            foreach (var item in displayers)
            {
                if (!item.gameObject.activeSelf)
                {
                    item.DisplayImmediately();
                }
            }
        }
    }

}