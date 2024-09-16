using System;
using System.Collections.Generic;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
#endif

namespace Gametamin.Core.IAP
{
    public partial class IAPSystem
    {
        private IAPStore _iapStore;
        public IAPSystem(PurchaseSuccessResult onPurchaseSuccessResult, OnPurchaseFailedResult onPurchaseFailedResult, Action onInitialized = null, PurchaseInitializationFailureReason onInitializationFailed = null)
        {
            if (_iapStore == null)
            {
                _iapStore = new IAPStore(onPurchaseSuccessResult, onPurchaseFailedResult);
                Init(onInitialized, onInitializationFailed);
            }
        }
        void Init(Action onInitialized = null, PurchaseInitializationFailureReason onInitializationFailed = null)
        {
            var products = new List<IAPPack>();
            LoadConsumableProducts(products);
            LoadNonConsumableProducts(products);
            LoadSubscriptionProducts(products);
            _iapStore.InitializePurchasing(products, onInitialized, onInitializationFailed);
        }
        public void BuyProductType(TypeIAP typeIAP)
        {
            if (_iapStore != null)
            {
                _iapStore.BuyProductID(typeIAP);
            }
        }
        public void RestorePurchases()
        {
            _iapStore.RestorePurchases();
        }
        public string GetOrderIdByType(TypeIAP typeIAP)
        {
            return _iapStore.GetOrderIdByType(typeIAP);
        }
        public bool GetPurchaseInfo(TypeIAP typeIAP, out string contentId, out decimal localizedPrice, out string currency)
        {
            if (_iapStore != null)
            {
                return _iapStore.GetPurchaseInfo(typeIAP, out contentId, out localizedPrice, out currency);
            }

            contentId = "";
            localizedPrice = 0;
            currency = "";
            return false;
        }
    }
}