using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.F2P
{
    public class NoInterAds : IInterAdsImplementation
    {
        public bool Show(InterAds.InterAdsCallback callback)
        {
            ///
            callback?.Invoke(false);

            ///
            return false;
        }
    }

}