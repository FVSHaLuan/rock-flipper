using UnityEngine;
using UnityEngine.Events;

namespace BT.Marketing
{
    public class SteamReleaseStateEvents : ExtendedMonoBehaviour
    {
        [SerializeField]
        private UnityEvent onReleased;
        [SerializeField]
        private UnityEvent onUnreleased;

        protected void OnDisable()
        {
            ///
            entry.steamStoreStateDetector.OnStoreStateLoaded -= SteamStoreStateDetector_OnStoreStateLoaded;
        }

        protected void OnEnable()
        {
            Check();

            ///
            entry.steamStoreStateDetector.OnStoreStateLoaded += SteamStoreStateDetector_OnStoreStateLoaded;
        }

        private void SteamStoreStateDetector_OnStoreStateLoaded()
        {
            Check();
        }

        private void Check()
        {
            if (entry.steamStoreStateDetector.IsGameReleased)
            {
                onReleased?.Invoke();
            }
            else
            {
                onUnreleased?.Invoke();
            }
        }
    }

}