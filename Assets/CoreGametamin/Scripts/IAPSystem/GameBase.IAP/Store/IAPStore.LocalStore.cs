#if USE_LOCAL_STORE

#if PAYMENT_PACKAGE_MYKET
using Medrick.Payment.Main.PaymentMethods.Myket;
#endif

#if PAYMENT_PACKAGE_CAFEBAZAAR
using Medrick.Payment.Main.PaymentMethods.CafeBazaar;
#endif

using Medrick.Payment.Application;
using Medrick.Payment.Application.PaymentBuilder;
using Medrick.Payment.Application.Purchase;
using Medrick.Payment.Application.QuerySku;
using Medrick.Payment.Verification;
using System.Threading.Tasks;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
#endif
using System.Collections.Generic;

namespace Gametamin.Core.IAP
{
    public partial class IAPStore : IPostQueryBehavior, IVerifier//, IPostVerificationBehavior
    {
        TypeIAP _purchasingTypeIAP;
        List<string> _productIds;
        string _purchaseToken;
        public PurchaseHandler PurchaseHandler;
        bool IsLocalStoreInitialized()
        {
            return PurchaseHandler != null && PurchaseHandler.IsInitialized();
        }
        void InitLocalStore()
        {
            PaymentConfigurationBuilder builder = default;
            _productIds = GetProductIds();
            var queryInitData = new QueryAfterInitialization
            {
                HasQueryAfterInitialization = true,
                InitialQueryProductIds = _productIds
            };
#if PAYMENT_PACKAGE_CAFEBAZAAR
            builder = new CafeBazaarPaymentBuilder()
                .SetQueryAfterInitialization(queryInitData)
                .SetPostQueryBehavior(this)
                .SetVerifier(this);
#endif
#if PAYMENT_PACKAGE_MYKET
            builder = new MyketPaymentBuilder()
                .SetQueryAfterInitialization(queryInitData)
                .SetPostQueryBehavior(this)
                .SetVerifier(this);
#endif
            PurchaseHandler = new PurchaseHandler(builder);
        }
        public void BuyProductID(TypeIAP typeIAP)
        {
            var idProduct = GetProductID(typeIAP);
            var model = new PurchaseModel()
            {
                ProductId = idProduct,
                PriceInToman = 0
            };
            _purchasingTypeIAP = typeIAP;
            PurchaseHandler.Purchase(model, OnPurchased, OnPurchaseFailed);
            var infor = PurchaseHandler.GetProductInfo(idProduct);
        }
        void OnPurchased()
        {
            _onPurchaseSuccessResult?.Invoke(_purchasingTypeIAP, _purchaseToken);
        }
        void OnPurchaseFailed(PurchaseError error)
        {
            _onPurchaseFailedResult?.Invoke(_purchasingTypeIAP, GetPurchaseFailureReason(error), string.Empty);
        }
        PurchaseFailureReason GetPurchaseFailureReason(PurchaseError error) => error switch
        {
            PurchaseError.DontSupportPurchasing => PurchaseFailureReason.PurchasingUnavailable,
            PurchaseError.InternetFailed => PurchaseFailureReason.PaymentDeclined,
            PurchaseError.PurchaseFailed => PurchaseFailureReason.UserCancelled,
            PurchaseError.VerificationFailed => PurchaseFailureReason.SignatureInvalid,
            PurchaseError.ConsumeFailed => PurchaseFailureReason.ProductUnavailable,
            _ => PurchaseFailureReason.Unknown,
        };
        List<string> GetProductIds()
        {
            var ids = new List<string>();
            _products.For(item => ids.Add(item.idProduct));
            return ids;
        }

        public void OnQuerySuccess(List<ProductInfo> productInfos)
        {
            _onInitialized?.Invoke();
        }

        public void OnQueryFail(string error)
        {

        }

        public Task<VerificationStatus> Verify(string serializedPayload)
        {
#if PAYMENT_PACKAGE_CAFEBAZAAR
            var verificationModel = JsonUtil.DeserializeModel<CafeBazaarVerificationModel>(serializedPayload);
#elif PAYMENT_PACKAGE_MYKET
            var verificationModel = JsonUtil.DeserializeModel<MyketVerificationModel>(serializedPayload);
#endif
            var sku = verificationModel.ProductId;
            _purchaseToken = verificationModel.PurchaseToken;
            AnalyticsHelper.PurchaseToken = _purchaseToken;
            return Task.Run(() =>
            {
                return VerificationStatus.VerificationSuccessFull;
            });
        }
    }
}
#endif