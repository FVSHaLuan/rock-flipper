#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using System.ComponentModel;
using UnityEngine;

namespace BT.GamePlatform
{
    public class StatRetriever
    {
        public static event System.Action<bool> OnGlobalStatsRequested;

#if !DISABLESTEAMWORKS
        private static CallResult<GlobalStatsReceived_t> requestGlobalStatsResult;

        private static bool inited = false;
        private static bool requestingGlobalStats = false;

        private static void TryInitAfterSteamManagerInitialized()
        {
            ///
            if (inited || !SteamManager.Initialized)
            {
                return;
            }

            ///
            inited = true;

            ///
            requestGlobalStatsResult = CallResult<GlobalStatsReceived_t>.Create(OnRequestGlobalStatsResult);
        }

        private static void OnRequestGlobalStatsResult(GlobalStatsReceived_t result, bool bIOFailure)
        {
            ///
            requestingGlobalStats = false;

            ///
            if (result.m_eResult != EResult.k_EResultOK || bIOFailure)
            {
                Debug.LogWarning($"Failed to request steam global stats. m_eResult: {result.m_eResult}");
                OnGlobalStatsRequested?.Invoke(false);
                return;
            }

            ///
            Debug.Log("Successfully requested steam global stats.");
            OnGlobalStatsRequested?.Invoke(true);
        } 
#endif

        public static void RequestGlobalStats()
        {
#if !DISABLESTEAMWORKS
            ///
            if (!SteamManager.Initialized || requestingGlobalStats)
            {
                return;
            }

            ///
            TryInitAfterSteamManagerInitialized();

            ///
            requestingGlobalStats = true;

            ///
            var handle = SteamUserStats.RequestGlobalStats(1);
            requestGlobalStatsResult.Set(handle); 
#endif
        }

        public static bool GetStat(string statId, out double value, bool isInt, bool isGlobal)
        {
#if !DISABLESTEAMWORKS
            ///
            if (!SteamManager.Initialized)
            {
                value = 0;
                return false;
            }

            ///
            TryInitAfterSteamManagerInitialized();

            ///
            if (isInt)
            {
                if (isGlobal)
                {
                    var rs = SteamUserStats.GetGlobalStat(statId, out long v);
                    value = (double)v;
                    return rs;
                }
                else
                {
                    var rs = SteamUserStats.GetStat(statId, out int v);
                    value = (double)v;
                    return rs;
                }
            }
            else
            {
                if (isGlobal)
                {
                    return SteamUserStats.GetGlobalStat(statId, out value);
                }
                else
                {
                    var rs = SteamUserStats.GetStat(statId, out float v);
                    value = (double)v;
                    return rs;
                }
            } 
#else
            value = 0;
            return false;
#endif
        }
    }

}