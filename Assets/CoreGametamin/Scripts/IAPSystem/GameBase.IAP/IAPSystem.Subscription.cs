using System;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
#endif

namespace Gametamin.Core.IAP
{
    public partial class IAPSystem
    {
        public bool IsSubscribedTo(TypeIAP subscriptionType)
        {
            if (_iapStore != null)
            {
                var subscriptionId = ToProductID(subscriptionType);
                return _iapStore.IsSubscribedTo(subscriptionId);
            }
            return false;
        }
        public void RestorePurchases(Action<bool, string> callback = null)
        {
            if (_iapStore != null)
            {
                _iapStore.RestorePurchase(callback);
            }
            else
            {
                callback?.Invoke(false, null);
            }
        }
        public bool IsInitialized
        {
            get
            {
                if (_iapStore != null)
                {
                    return _iapStore.IsInitialized();
                }
                return false;
            }
        }
        public SubscriptionInfo GetSubscriptionInfo(TypeIAP subscriptionType)
        {
            if (_iapStore != null)
            {
                var subscriptionId = ToProductID(subscriptionType);
                return _iapStore.GetSubscriptionInfo(subscriptionId);
            }
            return default;
        }
    }
}