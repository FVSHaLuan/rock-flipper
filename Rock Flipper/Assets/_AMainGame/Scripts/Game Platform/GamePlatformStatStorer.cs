#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using UnityEngine;

namespace Agame.GamePlatform
{
    public class GamePlatformStatStorer : ExtendedMonoBehaviour
    {
        private bool storeFlag;

        public void StoreThisFrame()
        {
            storeFlag = true;
        }

        private void Store()
        {
            StoreSteam();
            StoreGOG();
        }

        private void StoreSteam()
        {
#if !DISABLESTEAMWORKS
            if (SteamManager.Initialized)
            {
                try
                {
                    SteamUserStats.StoreStats();
                }
                catch
                {
                    Debug.LogError("Failed to store achievement state to Steam");
                }
            }
#endif
        }

        private void StoreGOG()
        {
#if ENABLE_GOG
            if (GalaxyInstance.User().SignedIn())
            {
                try
                {
                    GalaxyInstance.Stats().StoreStatsAndAchievements();
                }
                catch
                {
                    Debug.LogError("Failed to store achievement state to GOG");
                }
            }
#endif
        }

        protected void Update()
        {
            if (storeFlag)
            {
                Store();

                ///
                storeFlag = false;
            }
        }
    }
}