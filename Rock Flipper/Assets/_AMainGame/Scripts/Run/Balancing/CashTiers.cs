using System.Collections.Generic;
using UnityEngine;

namespace Agame.Balancing
{
    public class CashTiers : ScriptableObjectWithInit
    {
        [SerializeField]
        private Color svColor = Color.white;
        [SerializeField, OneLine.OneLineWithHeader]
        private List<CashTierConfig> cashTierConfigs = new List<CashTierConfig>();

        public enum CashTier
        {
            NotSet = -1,
            Tier0 = 0,
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3,
            Tier4 = 4,
            Tier5 = 5,
            Tier6 = 6,
            Tier7 = 7,
            Tier8 = 8,
            Tier9 = 9,
            Tier10 = 10,
            Tier11 = 11,
            Tier12 = 12,
            Tier13 = 13,
            Tier14 = 14,
            Tier15 = 15,
            Tier16 = 16,
            Tier17 = 17,
            Tier18 = 18,
            Tier19 = 19
        }

        [System.Serializable]
        public struct CashTierConfig
        {
            [LargeNumberField]
            public double cashBase;
            public Color colorCode;
        }

        public Color GetColor(CashTier cashTier, float multiplier)
        {
            ///
            if (cashTier == CashTier.NotSet)
            {
                return Color.black;
            }

            ///
            var cashValue = GetConfig(cashTier).cashBase * multiplier;
            return GetColor(cashValue);
        }

        private Color GetColor(double cashValue)
        {
            return GetConfig(cashValue).colorCode;
        }

        public CashTierConfig GetConfig(CashTier cashTier)
        {
            ///
            var index = (int)cashTier;

            ///
            if (index < 0 || index >= cashTierConfigs.Count)
            {
                return new CashTierConfig()
                {
                    cashBase = 0,
                    colorCode = Color.black
                };
            }

            /// 
            return cashTierConfigs[(int)cashTier];
        }

        private CashTierConfig GetConfig(double cashValue)
        {
            ///
            for (int i = 1; i < cashTierConfigs.Count - 1; i++)
            {
                var config = cashTierConfigs[i];
                if (cashValue < config.cashBase)
                {
                    return cashTierConfigs[i - 1];
                }
            }

            ///
            return cashTierConfigs[cashTierConfigs.Count - 1];
        }

#if UNITY_EDITOR
        [ContextMenu("Auto Colors")]
        private void Editor_AutoColors()
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Auto Colors");

            /// Start hue from blue (0.666) and distribute colors evenly counter-clockwise
            float startHue = 0.666f;
            Color.RGBToHSV(svColor, out _, out var colorS, out var colorV);
            for (int i = 0; i < cashTierConfigs.Count; i++)
            {
                var config = cashTierConfigs[i];
                float hue = (startHue - (float)i / cashTierConfigs.Count) % 1.0f; // Decrement hue for counter-clockwise
                if (hue < 0) hue += 1.0f; // Ensure hue stays within [0, 1]
                config.colorCode = Color.HSVToRGB(hue, colorS, colorV);
                cashTierConfigs[i] = config;
            }

            /// Mark the object as dirty to save changes
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Large Numbers")]
        private void Editor_LargeNumbers()
        {
            for (int i = 0; i < cashTierConfigs.Count; i++)
            {
                Debug.Log($"{i} - {cashTierConfigs[i].cashBase.ToLargeNumberString()} - {cashTierConfigs[i].cashBase.ToExponentialString()}");
            }
        }
#endif
    }

}