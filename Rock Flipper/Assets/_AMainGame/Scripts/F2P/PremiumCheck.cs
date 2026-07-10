using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;
using UnityEngine.Events;

namespace BT.F2P
{
    public class PremiumCheck : ValueDisplayer<bool>
    {
        [Header("PremiumCheck")]
        [SerializeField]
        private UnityEvent onPremiumUnlocked;
        [SerializeField]
        private UnityEvent onPremiumLocked;

        protected override void Display(bool isFirst, bool previousValue, bool currentValue)
        {
            if (currentValue)
            {
                onPremiumUnlocked?.Invoke();
            }
            else
            {
                onPremiumLocked?.Invoke();
            }
        }

        protected override bool GetCurrentValue()
        {
            return Entry.Instance.PlayerData.UnlockedPremium;
        }
    }
}
