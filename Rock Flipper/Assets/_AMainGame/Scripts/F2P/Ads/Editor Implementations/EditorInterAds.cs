using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BT.F2P
{
    public class EditorInterAds : IInterAdsImplementation
    {
        private InterAds.InterAdsCallback callback;

        public bool Show(InterAds.InterAdsCallback callback)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Inter ads", "Ads!", "OK");
            callback?.Invoke(true);

            ///
            return true;
#else
            TellADev.That("Do NOT use editor inter ads in builds");
            throw new System.NotImplementedException();
#endif
        }
    }

}