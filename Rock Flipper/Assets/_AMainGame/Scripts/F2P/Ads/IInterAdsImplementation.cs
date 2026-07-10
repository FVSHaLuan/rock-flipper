using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Agame.F2P.InterAds;

namespace Agame.F2P
{
    public interface IInterAdsImplementation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>true: showed, false: not showed</returns>
        public bool Show(InterAdsCallback callback);
    }

}