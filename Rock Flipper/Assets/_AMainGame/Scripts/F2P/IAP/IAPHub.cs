using FMod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.F2P
{
    public static class IAPHub
    {
        public static event PurchasedProductCallback OnPurchasedProduct;

        public delegate void PurchasedProductCallback(string productId);

        static IAPHub()
        {
#if UNITY_IOS || UNITY_ANDROID
            UnityPurchaser.Instance.OnPurchaseCompleted += Instance_OnPurchaseCompleted; 
#endif
        }

        private static void Instance_OnPurchaseCompleted(string productId)
        {
            OnPurchasedProduct?.Invoke(productId);
        }

        public static void RestoreIAP()
        {
#if UNITY_IOS || UNITY_ANDROID
            UnityPurchaser.Instance.RestorePurchases(); 
#endif
        }

        public static void BuyProduct(string productId)
        {
#if UNITY_IOS || UNITY_ANDROID
            UnityPurchaser.Instance.BuyProduct(productId); 
#endif
        }
    }
}
