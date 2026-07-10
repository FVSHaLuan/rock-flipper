using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.F2P
{
    public class IAPHelper : MonoBehaviour
    {
        public void RestoreIAP()
        {
            IAPHub.RestoreIAP();
        }

        public void BuyPremium()
        {
            IAPHub.BuyProduct(GameConst.PremiumProductId);
        }
    }

}