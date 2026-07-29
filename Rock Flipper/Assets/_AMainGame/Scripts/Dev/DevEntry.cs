using Agame.FeatureBranching;
using System;
using UnityEngine;
using Agame.Run;
using Agame.Balancing;
using Agame.Demo;

namespace Agame.Dev
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
        public VisualDefinitions visualDefinitions;
        public DemoHub demoHub;

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