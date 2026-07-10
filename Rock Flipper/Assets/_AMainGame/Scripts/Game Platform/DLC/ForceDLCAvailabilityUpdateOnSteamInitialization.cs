using UnityEngine;

namespace BT.GamePlatform
{
    public class ForceDLCAvailabilityUpdateOnSteamInitialization : MonoBehaviour
    {
#if !DISABLESTEAMWORKS
        protected void Awake()
        {
            SteamManager.Instance.DoAfterInited(ForceUpdate);
        }

        private void ForceUpdate()
        {
            DLCManager.UpdateAllDlcAvailability();
        }
#endif
    }

}