#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Agame.GamePlatform
{
    public class StatReporter : ExtendedMonoBehaviour
    {
        public bool Report(string statId, float currentValue)
        {
            return Report(statId, currentValue, false);
        }

        public bool Report(string statId, int currentValue)
        {
            return Report(statId, currentValue, true);
        }

        private bool Report(string statId, float currentValue, bool isInt)
        {
#if !DISABLESTEAMWORKS
            if (!SteamManager.Initialized)
            {
                Debug.LogWarning($"Couldn't report stat {statId}, value: {currentValue}, because Steam not inited");
                return false;
            }

            ///
            if (string.IsNullOrWhiteSpace(statId))
            {
                ///
                Debug.LogError("Null or empty statId");

                ///
                return false;
            }

            ///
            var rs = isInt ? SteamUserStats.SetStat(statId, (int)currentValue) : SteamUserStats.SetStat(statId, currentValue);

            ///
            if (rs)
            {
                entry.gamePlatformStatStorer.StoreThisFrame();
            }

            ///
            Debug.Log($"Reported Steam stat: {statId}, isInt: {isInt}, value: {currentValue},  result: {rs}");

            ///
            return rs;
#else
            return false;
#endif
        }
    }

}