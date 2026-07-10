#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using Agame.FeatureBranching;
using UnityEngine;
using static System.Net.WebRequestMethods;

namespace Agame.Meta
{
    public class MetaUrlLauncher : MonoBehaviour
    {
        public static void TryOpenUrlWithSteam(string url)
        {
            if (SteamManager.Initialized)
            {
#if !DISABLESTEAMWORKS
                SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Modal);
#endif
            }
            else
            {
                Application.OpenURL(url);
            }
        }

        [ContextMenu("LaunchDemoFeedback")]
        public void LaunchDemoFeedback()
        {
            var demoUrl = @"https://docs.google.com/forms/d/e/1FAIpQLSeQZwzlQOmOc0rXtCNHwbJECFe1wQKq3OZ7z16li7pM25n1kg/viewform";
            var fullGameUrl = @"https://docs.google.com/forms/d/e/1FAIpQLSfFVGbGf7-Epiv0F5zdzsfLVWCf2-F15Rw-JKAX4aJhHMfchw/viewform";

            ///
            var url = VersionBranchInfo.IsFullGame ? fullGameUrl : demoUrl;

            ///
            TryOpenUrlWithSteam(url);
        }

        [ContextMenu("LaunchSteamPage")]
        public void LaunchSteamPage()
        {
            LaunchSteamPageStatic();
        }

        public static void LaunchSteamPageStatic()
        {
            TryOpenUrlWithSteam(GameConst.SteamPageUrl);
        }

        [ContextMenu("LaunchSteamPageMobile")]
        public void LaunchSteamPageMobile()
        {
            Application.OpenURL(GameConst.SteamPageUrlMobile);
        }

        [ContextMenu("LaunchDiscordServer")]
        public void LaunchDiscordServer()
        {
            Application.OpenURL(@"http://discord.gg/minifungames");
        }

        [ContextMenu("LaunchNewsletter")]
        public void LaunchNewsletter()
        {
            TryOpenUrlWithSteam(GameConst.NewsletterUrl);
        }

        public void LaunchMFGSteamPage_Ending()
        {
            ///
            var url = @"https://store.steampowered.com/publisher/MiniFunGames?utm_source=ballatory_ending";

            ///
            TryOpenUrlWithSteam(url);
        }

        public void LaunchRockCrusherSteamPage_Ending()
        {
            ///
            var url = @"https://store.steampowered.com/app/3456800/Rock_Crusher?utm_source=ballatory_ending";

            ///
            TryOpenUrlWithSteam(url);
        }

        public void LaunchRockCrusherSteamPage()
        {
            ///
            var url = @"https://store.steampowered.com/app/3456800/Rock_Crusher?utm_source=ballatory_main";

            ///
            TryOpenUrlWithSteam(url);
        }

        [ContextMenu("LaunchSoundtrackDLC"), PlayModeOnly]
        public void LaunchSoundtrackDLC()
        {
            ///
            var url = $"https://store.steampowered.com/app/{GameConst.SoundtrackAppId}?utm_source=getsountrackbutton";

            ///
            TryOpenUrlWithSteam(url);
        }
    }

}