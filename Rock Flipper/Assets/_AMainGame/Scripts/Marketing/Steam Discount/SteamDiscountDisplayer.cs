using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Marketing
{
    public class SteamDiscountDisplayer : ExtendedMonoBehaviour
    {
        [SerializeField]
        private GameObject viewWrapper;
        [SerializeField]
        private UnifiedText percentageText;

        protected void OnDisable()
        {
            ///
            entry.steamStoreStateDetector.OnStoreStateLoaded -= SteamDiscountDetector_OnDiscountPercentageChanged;
        }

        protected void OnEnable()
        {
            ViewDiscount(entry.steamStoreStateDetector.DiscountPercentage);

            ///
            entry.steamStoreStateDetector.OnStoreStateLoaded += SteamDiscountDetector_OnDiscountPercentageChanged;
        }

        private void SteamDiscountDetector_OnDiscountPercentageChanged()
        {
            ViewDiscount(entry.steamStoreStateDetector.DiscountPercentage);
        }

        private void ViewDiscount(int percentage)
        {
            if (percentage <= 0)
            {
                viewWrapper.SetActive(false);

                ///
                return;
            }

            ///
            viewWrapper.SetActive(true);
            percentageText.Text = string.Format("-{0}%", percentage);
        }
    }

}