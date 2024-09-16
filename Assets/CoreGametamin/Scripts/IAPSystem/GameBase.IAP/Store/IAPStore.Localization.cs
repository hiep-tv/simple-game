#if !USE_LOCAL_STORE
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
        public void GetLocalPriceStrings(Action<TypeIAP, string, decimal> callback = null)
        {
            if (IsInitialized())
            {
                var products = storeController.products.all;
                products.For(product =>
                {
                    var productId = product.definition.id;
                    var meta = product.metadata;
                    callback?.Invoke(GetProductType(productId), meta.localizedPriceString, meta.localizedPrice);
                });
            }
        }
        public string GetLocalPriceString(TypeIAP typeIAP)
        {
            if (IsInitialized())
            {
                var productId = GetProductID(typeIAP);
                if (!string.IsNullOrEmpty(productId))
                {
                    Product product = storeController.products.WithID(productId);
                    if (product != null)
                    {
                        return product.metadata.localizedPriceString;
                    }
                }
            }
            return default;
        }
        public decimal GetLocalPriceNumber(TypeIAP typeIAP)
        {
            if (IsInitialized())
            {
                var productId = GetProductID(typeIAP);
                if (!string.IsNullOrEmpty(productId))
                {
                    Product product = storeController.products.WithID(productId);
                    if (product != null)
                    {
                        return product.metadata.localizedPrice;
                    }
                }
            }
            return default;
        }
        public bool GetPurchaseInfo(TypeIAP typeIAP, out string contentId, out decimal localizedPrice, out string currency)
        {
            if (IsInitialized())
            {
                var productId = GetProductID(typeIAP);
                if (!string.IsNullOrEmpty(productId))
                {
                    Product product = storeController.products.WithID(productId);
                    if (product != null)
                    {
                        contentId = product.definition.id;
                        localizedPrice = product.metadata.localizedPrice;
                        currency = product.metadata.isoCurrencyCode;
                        return true;
                    }
                }
            }

            contentId = "";
            localizedPrice = 0;
            currency = "";
            return false;
        }
    }
}
#endif