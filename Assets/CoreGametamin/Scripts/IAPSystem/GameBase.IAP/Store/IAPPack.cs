using System;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
#endif

namespace Gametamin.Core.IAP
{
    //[Serializable]
    public class IAPPack
    {
        //[Header("Important: This is the key to buy")]
        public string idProduct;
        public TypeIAP typeIAP;
        public ProductType type;

        public IAPPack(string idproduct, TypeIAP typeIAP, ProductType type)
        {
            idProduct = idproduct;
            this.type = type;
            this.typeIAP = typeIAP;
        }
    }
}