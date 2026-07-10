using BT.FeatureBranching;
using System;
using System.Collections.Generic;
using UnityEngine;
using BT.Run;
using BT.Balancing;
using I2.Loc;
using BT.Demo;
using BT.Localization;

namespace BT.Dev
{
    [CreateAssetMenu(fileName = "DevEntry", menuName = "BSB/Dev/DevEntry")]
    [ExecuteInEditMode]
    public class DevEntry : ScriptableObject
    {
        private const string AssetPath = "Assets/_AMainGame/Data/Dev/DevEntry.asset";
        private static DevEntry instance;

        [Header("Refs")]
        public UniqueIntManager uniqueIntManager;
        public PlayerDataObject playerDataObject;
        public PlayerDataObject playerDataObjectDemo;
        public GameBalance gameBalance;
        public CashTiers cashTiers;
        public CurrencyConfigManager currencyConfigManager;
        public Font monoSpaceFont;
        public LanguageSourceAsset languageSourceAsset;
        public VisualDefinitions visualDefinitions;
        public DemoHub demoHub;
        public LocalizedStrings localizedStrings;

#if UNITY_EDITOR
        [NonSerialized]
        private bool editor_inited = false;
#endif       

        public PlayerDataObject CurrentPlayerDataObject => VersionBranchInfo.Current == VersionBranch.Demo ? playerDataObjectDemo : playerDataObject;

        public static DevEntry Instance
        {
            get
            {
#if UNITY_EDITOR
                ///
                if (instance == null)
                {
                    instance = UnityEditor.AssetDatabase.LoadAssetAtPath<DevEntry>(AssetPath);
                }

                ///
                return instance;
#else
                throw new System.NotImplementedException();
#endif
            }
        }

#if UNITY_EDITOR
        public void Editor_TryInit()
        {
            ///
            if (editor_inited)
            {
                return;
            }

            ///
            editor_inited = true;
        }

        protected void OnValidate()
        {

        }
#endif
    }
}