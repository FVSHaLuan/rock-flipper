using BT.Meta;
using Steamworks;
using UnityEngine;
using static System.Net.WebRequestMethods;

namespace BT.GamePlatform
{
    public class SteamDLCValidator : DLCValidator
    {
        private uint appId;
        private bool isDlc;

        public string StorePageUrl { get; private set; }

        public SteamDLCValidator(DLCEnum dlc, uint appId, bool isDlc) : base(dlc)
        {
            this.appId = appId;
            this.isDlc = isDlc;
            StorePageUrl = $"https://store.steampowered.com/app/{appId}?utm_source=ballatory_getbutton";
        }

        protected override bool GetCurrentAvailability()
        {
#if !DISABLESTEAMWORKS
            if (!SteamManager.Initialized)
            {
                return false;
            }

            ///
            if (isDlc)
            {
                return SteamApps.BIsDlcInstalled(new AppId_t(appId));
            }
            else
            {
                return SteamApps.BIsSubscribedApp(new AppId_t(appId));
            }
#else
            return false;
#endif
        }

        public override void InitiatePurchase()
        {
            MetaUrlLauncher.TryOpenUrlWithSteam(StorePageUrl);
        }
    }

}