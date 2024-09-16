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
        TypeIAP[] ComsumableTypes => ShopHelper.AvailableComsumableProducts;
        TypeIAP[] NonComsumableTypes => ShopHelper.AvailableNonComsumableProducts;
        TypeIAP[] SubscriptionTypes => ShopHelper.AvailableSubscriptionProducts;
        string ToProductID(TypeIAP typeIAP)
        {
            return typeIAP.ToStoreID();
        }
        void LoadConsumableProducts(List<IAPPack> products)
        {
            if (products != null)
            {
                var typeIAPs = ComsumableTypes;
                for (int i = 0, length = typeIAPs.GetCountSafe(); i < length; i++)
                {
                    var typeIAP = typeIAPs[i];
                    var productId = ToProductID(typeIAP);
                    products.Add(new IAPPack(productId, typeIAP, ProductType.Consumable));
                }
            }
        }
        void LoadNonConsumableProducts(List<IAPPack> products)
        {
            if (products != null)
            {
                var typeIAPs = NonComsumableTypes;
                for (int i = 0, length = typeIAPs.GetCountSafe(); i < length; i++)
                {
                    var typeIAP = typeIAPs[i];
                    var productId = ToProductID(typeIAP);
                    products.Add(new IAPPack(productId, typeIAP, ProductType.NonConsumable));
                }
            }
        }
        void LoadSubscriptionProducts(List<IAPPack> products)
        {
            if (products != null)
            {
                var typeIAPs = SubscriptionTypes;
                for (int i = 0, length = typeIAPs.GetCountSafe(); i < length; i++)
                {
                    var typeIAP = typeIAPs[i];
                    var productId = ToProductID(typeIAP);
                    products.Add(new IAPPack(productId, typeIAP, ProductType.Subscription));
                }
            }
        }
        public static string GetNewPrice(string priceAndCurrency, string newPrice)
        {
            if (!string.IsNullOrEmpty(priceAndCurrency))
            {
                int length = priceAndCurrency.Length;
                int index = 0;
                for (int i = 0; i < length; i++)
                {
                    char c = priceAndCurrency[i];
                    if (c >= '0' && c <= '9')
                    {
                        index = i;
                        break;
                    }
                }
                // Currency at last
                if (index == 0)
                {
                    for (int i = length - 1; i >= 0; i--)
                    {
                        char c = priceAndCurrency[i];
                        if (c >= '0' && c <= '9')
                        {
                            return $"{newPrice}{priceAndCurrency.Substring(i + 1)}";
                        }
                    }
                }
                // Currency at first
                else
                {
                    return $"{priceAndCurrency.Substring(0, index)}{newPrice}";
                }
            }
            return newPrice;
        }
    }
}