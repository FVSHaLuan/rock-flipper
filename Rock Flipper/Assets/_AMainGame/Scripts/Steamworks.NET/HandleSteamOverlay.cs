#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Steamworks
{
    public class HandleSteamOverlay : ExtendedMonoBehaviour
    {
        public event System.Action<bool> OnSteamOverlayActivation;

#if !DISABLESTEAMWORKS
        private Callback<GameOverlayActivated_t> gameOverlayActivatedCallback;

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            gameOverlayActivatedCallback = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }

        private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
        {
            if (pCallback.m_bActive != 0)
            {
                // Debug.Log("Steam Overlay has been activated");
                entry.completeInputBlocker.AddBlockLock(this);

                ///
                OnSteamOverlayActivation?.Invoke(true);
            }
            else
            {
                // Debug.Log("Steam Overlay has been closed");
                entry.completeInputBlocker.RemoveBlockLock(this);

                ///
                OnSteamOverlayActivation?.Invoke(false);
            }
        }
#endif
    }
}