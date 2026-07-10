#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using System.Collections;
using UnityEngine;

namespace Agame.Home
{
    public class GetSoundTrackButton : ExtendedMonoBehaviourHome
    {
        [SerializeField]
        private GameObject button;
        [SerializeField]
        private float updateInterval = 10f;

        protected IEnumerator Start()
        {
            ///
            button.SetActive(false);

            ///
            while (!SteamManager.Initialized)
            {
                yield return null;
            }

            ///
            while (true)
            {
                UpdateButton();

                ///
                yield return new WaitForSeconds(updateInterval);
            }
        }

        private void UpdateButton()
        {
#if !DISABLESTEAMWORKS
            button.SetActive(!SteamApps.BIsDlcInstalled(new AppId_t(GameConst.SoundtrackAppId))); 
#else
            button.SetActive(false);
            StopAllCoroutines();
            gameObject.SetActive(false);
#endif
        }
    }

}