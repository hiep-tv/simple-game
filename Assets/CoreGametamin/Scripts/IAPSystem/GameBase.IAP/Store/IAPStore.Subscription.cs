using System;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
#endif

namespace Gametamin.Core.IAP
{
    public partial class IAPStore
    {
        IAppleExtensions _appleExtensions;
        IAppleExtensions _AppleExtensions
        {
            get
            {
                if (_appleExtensions == null)
                {
                    _appleExtensions = _storeExtensionProvider.GetExtension<IAppleExtensions>();
                }
                return _appleExtensions;
            }
        }
        IGooglePlayStoreExtensions _googlePlayStoreExtensions;
        IGooglePlayStoreExtensions _GooglePlayStoreExtensions
        {
            get
            {
                if (_googlePlayStoreExtensions == null)
                {
                    _googlePlayStoreExtensions = _storeExtensionProvider.GetExtension<IGooglePlayStoreExtensions>();
                }
                return _googlePlayStoreExtensions;
            }
        }
        public bool IsSubscribedTo(string subscriptionId)
        {
            if (!IsInitialized())
            {
                return false;
            }
            var subscriptionProduct = storeController.products.WithID(subscriptionId);
            return IsSubscribedTo(subscriptionProduct);
        }
        public bool IsSubscribedTo(Product subscription)
        {
            if (subscription == null || !subscription.hasReceipt)
            {
                return false;
            }
            var subscriptionManager = new SubscriptionManager(subscription, null);
            var info = subscriptionManager.getSubscriptionInfo();
            return info.isSubscribed() == Result.True;
        }
        public SubscriptionInfo GetSubscriptionInfo(string subscriptionId)
        {
            var subscriptionProduct = storeController.products.WithID(subscriptionId);
            if (subscriptionProduct == null || !subscriptionProduct.hasReceipt)
            {
                return null;
            }
            var subscriptionManager = new SubscriptionManager(subscriptionProduct, null);
            var info = subscriptionManager.getSubscriptionInfo();
            return info;
        }
        public void RestorePurchase(Action<bool, string> callback = null)
        {
            bool canRestore = false;
            if (!DefineSymbols.IsEditor)
            {
                if (IsInitialized())
                {
                    if (DefineSymbols.IsIOS)
                    {
                        canRestore = true;
                        _AppleExtensions.RestoreTransactions(callback);
                    }
                    else if (DefineSymbols.IsAndroid)
                    {
                        canRestore = true;
                        _GooglePlayStoreExtensions.RestoreTransactions(callback);
                    }
                }
            }
            if (!canRestore)
            {
                callback?.Invoke(false, null);
            }
        }
    }
}