#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using UnityEngine;

namespace Agame.GamePlatform
{
    public class StatDisplayer : ValueDisplayerUnified<double>
    {
        [SerializeField]
        private string statId;
        [SerializeField]
        private bool isInt;
        [SerializeField]
        private bool isGlobal;
        [SerializeField]
        private float updateInterval = 60;
        [SerializeField]
        private bool isDevelopmentOnly = false;

        private float lastUpdateTime = -9999999;

        private double lastStatValue;

        protected void OnDisable()
        {
            ///
            StatRetriever.OnGlobalStatsRequested -= StatRetriever_OnGlobalStatsRequested;
        }

        protected override void OnEnable()
        {
            ///
            StatRetriever.OnGlobalStatsRequested += StatRetriever_OnGlobalStatsRequested;

            ///
            base.OnEnable();
        }

        private void StatRetriever_OnGlobalStatsRequested(bool success)
        {
            if (success)
            {
                UpdateValueNow();
            }
        }

        protected void Start()
        {
            if (isDevelopmentOnly
                && !Entry.Instance.GameSetting.enabledTerminal
                && !Application.isEditor)
            {
                gameObject.SetActive(false);
            }
        }

        protected override double GetCurrentValue()
        {
            if (!SteamManager.Initialized
                || Time.realtimeSinceStartup - lastUpdateTime < updateInterval)
            {
                return lastStatValue;
            }

            ///
            if (isGlobal)
            {
                StatRetriever.RequestGlobalStats();
            }

            ///
            UpdateValueNow();

            ///
            return lastStatValue;
        }

        private void UpdateValueNow()
        {
            if (StatRetriever.GetStat(statId, out double result, isInt, isGlobal))
            {
                lastStatValue = result;
            }

            ///
            lastUpdateTime = Time.realtimeSinceStartup;
        }
    }

}