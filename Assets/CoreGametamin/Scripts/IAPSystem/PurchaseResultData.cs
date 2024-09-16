#if !PURCHASING_DISABLED
using UnityEngine.Purchasing;
#endif
namespace Gametamin.Core
{
    public enum PurchaseResult
    {
        Failure,
        Successful,
        Restoration
    }
#if PURCHASING_DISABLED
    public enum PurchaseFailureReason
    {
        PurchasingUnavailable,
        ExistingPurchasePending,
        ProductUnavailable,
        SignatureInvalid,
        UserCancelled,
        PaymentDeclined,
        DuplicateTransaction,
        Unknown
    }
#endif
    public struct PurchaseResultData
    {
        public TypeIAP typeIAP;
        public PurchaseResult purchaseResult;
        public PurchaseFailureReason purchaseFailureReason;
        public PurchaseResultData(TypeIAP typeIAP, PurchaseFailureReason purchaseFailureReason)
        {
            this.typeIAP = typeIAP;
            this.purchaseResult = PurchaseResult.Failure;
            this.purchaseFailureReason = purchaseFailureReason;
        }
        public PurchaseResultData(TypeIAP typeIAP, PurchaseResult purchaseResult)
        {
            this.typeIAP = typeIAP;
            this.purchaseResult = purchaseResult;
            this.purchaseFailureReason = PurchaseFailureReason.Unknown;
        }
    }
}
