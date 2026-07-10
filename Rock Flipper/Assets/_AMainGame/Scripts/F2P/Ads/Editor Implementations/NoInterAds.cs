using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.F2P
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