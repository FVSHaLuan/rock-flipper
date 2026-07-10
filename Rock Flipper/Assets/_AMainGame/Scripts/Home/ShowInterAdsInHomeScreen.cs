using System.Collections;
using System.Collections.Generic;
using BT.F2P;
using UnityEngine;

namespace BT.Home
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