using System.Collections;
using System.Collections.Generic;
using Agame.F2P;
using UnityEngine;

namespace Agame.Home
{
    public class ShowInterAdsInHomeScreen : ExtendedMonoBehaviourHome
    {
        private static int count;

        protected void Start()
        {
            count++;

            ///
            if (count == 1)
            {
                return;
            }

            ///
            InterAds.Show(null);
        }
    }

}