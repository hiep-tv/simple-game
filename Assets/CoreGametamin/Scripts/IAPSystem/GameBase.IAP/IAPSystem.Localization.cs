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
        public void GetLocalPrices(Action<TypeIAP, string, decimal> callback = null)
        {
            if (_iapStore != null)
            {
                _iapStore.GetLocalPriceStrings(callback);
            }
        }
        public string GetLocalPriceString(TypeIAP typeIAP)
        {
            if (_iapStore != null)
            {
                return _iapStore.GetLocalPriceString(typeIAP);
            }
            return default;
        }
        public decimal GetLocalPriceNumber(TypeIAP typeIAP)
        {
            if (_iapStore != null)
            {
#if !USE_LOCAL_STORE
                return _iapStore.GetLocalPriceNumber(typeIAP);
#endif
            }
            return decimal.MinusOne;
        }
    }
}