#if USE_LOCAL_STORE
using Medrick.Payment.Application;
using System;
namespace Gametamin.Core.IAP
{
    public partial class IAPStore
    {
        public void GetLocalPriceStrings(Action<TypeIAP, string, decimal> callback = null)
        {
            if (IsInitialized())
            {
                _productIds.For(productId =>
                {
                    var product = PurchaseHandler.GetProductInfo(productId);
                    if (product != null)
                    {
                        var price = (decimal)product.Price / 10;
                        var formatPrice = price.GetFormatStorePrice();
                        callback?.Invoke(GetProductType(productId), $"{"تومان".GetFixedTextRLT(false)} {formatPrice}", price);
                    }
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
                    var product = PurchaseHandler.GetProductInfo(productId);
                    if (product != null)
                    {
                        return product.Price.ToString();
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
                    var product = PurchaseHandler.GetProductInfo(productId);
                    if (product != null)
                    {
                        contentId = product.ProductId;
                        localizedPrice = (decimal)product.Price;
                        currency = product.IsoCurrencyCode;
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