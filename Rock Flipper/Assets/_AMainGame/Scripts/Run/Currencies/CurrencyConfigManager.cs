using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace BT.Run
{
    public class CurrencyConfigManager : ScriptableObjectWithInit
    {
        private const string ConfigAssetPath = "Assets/_AMainGame/Data/Currency Configs";

        [Space]
        [SerializeField, UnityCustomArrayElementHeader]
        private List<CurrencyConfigAsset> configAssets = new List<CurrencyConfigAsset>();

        [Space]
        [SerializeField]
        private List<Currency> upgradableCurrencies;

        [System.NonSerialized]
        private Dictionary<Currency, CurrencyConfigAsset> configDict;

        protected override void Init()
        {
            ///
            base.Init();

            ///
            configDict = new Dictionary<Currency, CurrencyConfigAsset>();

            ///
            foreach (var item in configAssets)
            {
                configDict.Add(item.Currency, item);
            }
        }

        public CurrencyConfigAsset GetConfig(Currency currency)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                foreach (var item in configAssets)
                {
                    if (item.Currency == currency)
                    {
                        return item;
                    }
                }

                ///
                throw new System.ArgumentOutOfRangeException();
            }
#endif

            ///
            TryInit();

            ///
            return configDict[currency];
        }

        public void GetUpgradableCurrencies(List<Currency> currencies)
        {
            currencies.Clear();
            currencies.AddRange(upgradableCurrencies);
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Sync")]
        private void Editor_Sync()
        {
            ///
            Editor_LoadConfigAssets();

            ///
            List<CurrencyConfigAsset> duplicatedStats = new List<CurrencyConfigAsset>();
            List<CurrencyConfigAsset> invalidStats = new List<CurrencyConfigAsset>();

            ///
            var allStats = System.Enum.GetValues(typeof(Currency)).OfType<Currency>();

            ///
            var allStatIntValues = new HashSet<int>();
            foreach (var item in allStats)
            {
                ///
                var intValue = (int)item;

                ///
                if (allStatIntValues.Contains(intValue))
                {
                    Debug.LogErrorFormat("Enum value {0} is duplicated", intValue);

                    ///
                    return;
                }

                ///
                allStatIntValues.Add(intValue);
            }

            ///
            var allStatHashSet = new HashSet<Currency>(allStats);

            // Check duplication
            foreach (var item in configAssets)
            {
                ///
                var isDuplicated = !allStatHashSet.Remove(item.Currency);

                ///
                item.Editor_UpdateHeader(isDuplicated);

                ///
                if (isDuplicated)
                {
                    duplicatedStats.Add(item);
                }

                ///
                if (!item.IsValid)
                {
                    invalidStats.Add(item);
                }
            }

            // Create new
            foreach (var item in allStatHashSet)
            {
                ///
                if (item == Currency.INVALID)
                {
                    continue;
                }

                ///
                var statConfig = CreateConfigAsset(item);

                ///
                statConfig.Editor_UpdateHeader(false);
                invalidStats.Add(statConfig);

                ///
                configAssets.Add(statConfig);
            }

            // Log
            Debug.LogFormat("------Duplicated: {0}", duplicatedStats.Count);
            foreach (var item in duplicatedStats)
            {
                Debug.Log(item.name, item);
            }
            Debug.LogFormat("------Invalid: {0}", invalidStats.Count);
            foreach (var item in invalidStats)
            {
                Debug.Log(item.name, item);
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);

            ///
            UnityEditor.AssetDatabase.Refresh();
        }

        private CurrencyConfigAsset CreateConfigAsset(Currency buffStat)
        {
            ///
            var configAsset = CreateInstance<CurrencyConfigAsset>();
            configAsset.Editor_SetCurrency(buffStat);

            ///
            var path = ConfigAssetPath + "/" + buffStat.ToString() + ".asset";

            ///
            UnityEditor.AssetDatabase.CreateAsset(configAsset, path);
            UnityEditor.AssetDatabase.SaveAssets();

            ///
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CurrencyConfigAsset>(path);
        }

        private void Editor_LoadConfigAssets()
        {
            ///
            configAssets = new List<CurrencyConfigAsset>();

            ///
            EditorHelper.GetAllObjetsFromPath<CurrencyConfigAsset>(ConfigAssetPath, configAssets);

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public CurrencyConfigAsset Editor_GetConfig(Currency currency)
        {
            ///
            foreach (var item in configAssets)
            {
                if (item.Currency == currency)
                {
                    return item;
                }
            }

            ///
            return null;
        }
#endif
    }

}