using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.GamePlatform
{
    [System.Serializable]
    public class AchievementConfig
    {
        [SerializeField]
        private string commonAchievementId;
        [SerializeField]
        private string steamAchievementId;

        [System.NonSerialized]
        private bool inited;
        [System.NonSerialized]
        private string finalCommonAchievementId;
        [System.NonSerialized]
        private string finalSteamAchievementId;

        public string SteamAchievementId
        {
            get
            {
                ///
                TryInit();

                ///
                return finalSteamAchievementId;
            }
        }

        public string GOGAchievementId => SteamAchievementId;

        private void TryInit()
        {
            if (inited)
            {
                return;
            }

            ///
            inited = true;

            ///
            finalCommonAchievementId = commonAchievementId != null ? commonAchievementId.Trim() : null;
            finalSteamAchievementId = string.IsNullOrWhiteSpace(steamAchievementId) ? finalCommonAchievementId : steamAchievementId.Trim();
        }
    }

    public static class AchievementConfigExtensions
    {
        public static bool IsValid(this AchievementConfig config)
        {
            ///
            if (config == null)
            {
                return false;
            }

            ///
            if (string.IsNullOrWhiteSpace(config.SteamAchievementId)
                && string.IsNullOrWhiteSpace(config.GOGAchievementId))
            {
                return false;
            }

            ///
            return true;
        }

        public static bool Report(this AchievementConfig config)
        {
            if (Entry.Instance?.achievementReporter == null
                || !config.IsValid())
            {
                return false;
            }

            ///
            return Entry.Instance.achievementReporter.Report(config);
        }
    }

}