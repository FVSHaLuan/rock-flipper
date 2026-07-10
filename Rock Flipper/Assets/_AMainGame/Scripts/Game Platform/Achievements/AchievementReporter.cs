#if ENABLE_GOG
using Galaxy.Api;
#endif
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Agame.FeatureBranching;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Agame.GamePlatform
{
    public class AchievementReporter : ExtendedMonoBehaviour
    {
        /// <summary>
        /// Store SteamAchievementIds
        /// </summary>
        private HashSet<string> reportedAchievements = new HashSet<string>();

        private bool HasReported(AchievementConfig config)
        {
            ///
            if (VersionBranchInfo.IsTargetedOrOnMobile)
            {
                return true;
            }

            ///
            return reportedAchievements.Contains(config.SteamAchievementId);
        }

        public bool Report(AchievementConfig achievementConfig)
        {
            ///
            if (HasReported(achievementConfig))
            {
                return false;
            }

            ///
            var steamRs = ReportSteam(achievementConfig.SteamAchievementId);
            var gogRs = ReportGOG(achievementConfig.GOGAchievementId);

            ///
            var rs = steamRs || gogRs;

            ///
            if (rs)
            {
                reportedAchievements.Add(achievementConfig.SteamAchievementId);

                ///
                entry.gamePlatformStatStorer.StoreThisFrame();
            }

            ///
            return rs;
        }

        private bool ReportSteam(string achievementId)
        {
#if !DISABLESTEAMWORKS
            ///
            if (string.IsNullOrWhiteSpace(achievementId))
            {
                ///
                Debug.LogError("Null or empty achievementId");

                ///
                return false;
            }

            ///
            if (!SteamManager.Initialized)
            {
                ///
                Debug.LogWarning($"Steam is not initialized, cannot report achievement: {achievementId}");

                ///
                return false;
            }

            ///
            var rs = SteamUserStats.SetAchievement(achievementId);

            ///
            Debug.LogFormat("Reported Steam achievement: {0}, result: {1}", achievementId, rs);

            ///
            return rs;
#else
            return false;
#endif
        }

        private bool ReportGOG(string achievementId)
        {
#if ENABLE_GOG
            ///
            if (string.IsNullOrWhiteSpace(achievementId))
            {
                ///
                Debug.LogError("Null or empty achievementId");

                ///
                return false;
            }

            ///
            bool rs;

            ///
            if (GalaxyInstance.User().SignedIn())
            {
                try
                {
                    GalaxyInstance.Stats().SetAchievement(achievementId);

                    ///
                    rs = true;
                }
                catch
                {
                    rs = false;
                }
            }
            else
            {
                rs = false;
            }

            ///
            Debug.LogFormat("Reported GOG achievement: {0}, result: {1}", achievementId, rs);

            ///
            return rs;
#else
            return false;
#endif
        }
    }

}