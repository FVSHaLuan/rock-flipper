using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Agame.F2P
{
    public class PurchasedProductsHandler : ExtendedMonoBehaviour
    {
        protected void Start()
        {
            IAPHub.OnPurchasedProduct += IAPHub_OnPurchasedProduct;
        }

        private void IAPHub_OnPurchasedProduct(string productId)
        {
            if (productId == GameConst.PremiumProductId)
            {
                HandlePurchasedPremium();
            }
        }

        private void HandlePurchasedPremium()
        {
            throw new System.NotImplementedException();
        }
    }
}
