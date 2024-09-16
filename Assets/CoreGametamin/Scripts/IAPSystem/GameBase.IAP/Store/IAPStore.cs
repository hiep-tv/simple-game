using System;
using System.Collections.Generic;
using UnityEngine;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;
#endif

namespace Gametamin.Core.IAP
{
    public delegate void PurchaseSuccessResult(TypeIAP typeIAP, string value);
    public delegate void OnPurchaseFailedResult(TypeIAP typeIAP, PurchaseFailureReason reason, string order);
    public delegate void PurchaseInitializationFailureReason(InitializationFailureReason error, string message);
    public partial class IAPStore : IDetailedStoreListener
    {
        Action _onInitialized;
        PurchaseInitializationFailureReason _onInitializePurchasingErrorResult;
        PurchaseSuccessResult _onPurchaseSuccessResult;
        OnPurchaseFailedResult _onPurchaseFailedResult;
        public IStoreController storeController;
        private static IExtensionProvider _storeExtensionProvider;
        private List<IAPPack> _products;
        private bool _isInitializing;
        Dictionary<TypeIAP, string> _orderIds;
        Dictionary<TypeIAP, string> _OrderIds => _orderIds ??= new();
        public IAPStore(PurchaseSuccessResult onPurchaseSuccessResult, OnPurchaseFailedResult onPurchaseFailedResult)
        {
            _onPurchaseSuccessResult = onPurchaseSuccessResult;
            _onPurchaseFailedResult = onPurchaseFailedResult;
        }
        public void InitializePurchasing(List<IAPPack> list, Action onInitialized = null, PurchaseInitializationFailureReason onInitializePurchasingErrorResult = null)
        {
            _products = list;
            _onInitialized = onInitialized;
            _onInitializePurchasingErrorResult = onInitializePurchasingErrorResult;
            InitializePurchasing();
        }
        public void InitializePurchasing()
        {
            if (IsInitialized() || _isInitializing)
            {
                return;
            }
            _isInitializing = true;
#if USE_LOCAL_STORE
            InitLocalStore();
#else
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var item in _products)
            {
                IDs ids = new()
                {
                    { item.idProduct, AppleAppStore.Name },
                    { item.idProduct, GooglePlay.Name },
                };
                if (DefineSymbols.IsDebugIAP)
                    Debug.LogError($"ProductID: {item.idProduct}");
                builder.AddProduct(item.idProduct, item.type, ids);
            }
            UnityPurchasing.Initialize(this, builder);
#endif
        }
        public bool IsInitialized()
        {
#if USE_LOCAL_STORE
            return IsLocalStoreInitialized();
#else
            return storeController != null && _storeExtensionProvider != null;
#endif
        }
#if !USE_LOCAL_STORE
        public void BuyProductID(TypeIAP typeIAP)
        {
            if (_products != null)
            {
                var idProduct = GetProductID(typeIAP);
                if (!string.IsNullOrEmpty(idProduct))
                {
                    BuyProductID(idProduct);
                }
                else
                {
                    _onPurchaseFailedResult?.Invoke(GetTypeIAP(idProduct), PurchaseFailureReason.Unknown, string.Empty);
                }
            }
            else
            {
                _onPurchaseFailedResult?.Invoke(GetTypeIAP("Unknown"), PurchaseFailureReason.Unknown, string.Empty);
            }
        }
        void BuyProductID(string productId)
        {
            try
            {
                if (IsInitialized())
                {
                    Product product = storeController.products.WithID(productId);
                    if (product != null && product.availableToPurchase)
                    {
                        if (DefineSymbols.IsDebugIAP)
                            Debug.LogError($"Purchasing product asychronously: '{product.definition.id}'");
                        storeController.InitiatePurchase(product);
                    }
                    else
                    {
                        if (DefineSymbols.IsDebugIAP)
                            Debug.LogError("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                        _onPurchaseFailedResult?.Invoke(GetTypeIAP(productId), PurchaseFailureReason.ProductUnavailable, string.Empty);
                    }
                }
                else
                {
                    InitializePurchasing();
                    if (DefineSymbols.IsDebugIAP)
                        Debug.LogError("BuyProductID FAIL. Not initialized.");
                    _onPurchaseFailedResult?.Invoke(GetTypeIAP(productId), PurchaseFailureReason.Unknown, string.Empty);
                }
            }
            catch (Exception e)
            {
                if (DefineSymbols.IsDebugIAP)
                    Debug.LogError("BuyProductID: FAIL. Exception during purchase. " + e);
                _onPurchaseFailedResult?.Invoke(GetTypeIAP(productId), PurchaseFailureReason.Unknown, string.Empty);
            }
        }
#endif
        string GetProductID(TypeIAP typeIAP)
        {
            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].typeIAP == typeIAP)
                {
                    return _products[i].idProduct;
                }
            }
            return default;
        }
        TypeIAP GetProductType(string id)
        {
            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].idProduct.Equals(id))
                {
                    return _products[i].typeIAP;
                }
            }
            return TypeIAP.Non;
        }
        public void RestorePurchases(Action callback = null)
        {
            if (!IsInitialized())
            {
                if (DefineSymbols.IsDebugIAP)
                    Debug.Log("RestorePurchases FAIL. Not initialized.");
                callback?.Invoke();
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                if (DefineSymbols.IsDebugIAP)
                    Debug.Log("RestorePurchases started ...");
                var apple = _storeExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result, error) =>
                {
                    if (DefineSymbols.IsDebugIAP)
                        Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                    callback?.Invoke();
                });
            }
            else
            {
                if (DefineSymbols.IsDebugIAP)
                    Debug.LogError("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
                callback?.Invoke();
            }
        }
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            if (DefineSymbols.IsDebugIAP)
                Debug.Log("OnInitialized: PASS");

            storeController = controller;
            _storeExtensionProvider = extensions;
            _onInitialized?.Invoke();
        }
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            if (DefineSymbols.IsDebugIAP)
                Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
            _isInitializing = false;
        }
        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            if (DefineSymbols.IsDebugIAP)
                Debug.LogError($"OnInitializeFailed InitializationFailureReason={error}, message={message}");
            _isInitializing = false;
            _onInitializePurchasingErrorResult?.Invoke(error, message);
        }
        private CrossPlatformValidator _validator;
        private CrossPlatformValidator _Validator
        {
            get
            {
                if (_validator == null)
                {
                    //_validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                    //                    AppleTangle.Data(), Application.identifier);
                }
                return _validator;
            }
        }
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            bool validPurchase = true; // Presume valid for platforms with no R.V.
            string transactionID = "";
            string purchaseToken = "";
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
            try
            {
                // On Google Play, result will have a single product Id.
                // On Apple stores receipts contain multiple products.
                var result = _Validator.Validate(args.purchasedProduct.receipt);
                if (result == null || result.Length <= 0)
                {
                    validPurchase = false;
                }
                else
                {
                    foreach (var productReceipt in result)
                    {
#if UNITY_ANDROID
                        if (productReceipt is GooglePlayReceipt google)
                        {
                            // This is Google's Order ID.
                            // Note that it is null when testing in the sandbox
                            // because Google's sandbox does not provide Order IDs.
                            if (string.IsNullOrEmpty(transactionID))
                            {
                                transactionID = google.transactionID;
                                purchaseToken = google.purchaseToken;
                            }
                            else
                            {
                                transactionID = $"{transactionID}|{google.transactionID}";
                                purchaseToken = $"{purchaseToken}|{google.purchaseToken}";
                            }
                            var iap = GetTypeIAP(google.productID);
                            _OrderIds.SafeAdd(iap, google.transactionID);
                        }
#elif UNITY_IOS
                            if (productReceipt is AppleInAppPurchaseReceipt apple)
                            {
                                if (string.IsNullOrEmpty(transactionID))
                                {
                                    transactionID = apple.originalTransactionIdentifier;
                                }
                                else
                                {
                                    transactionID = $"{transactionID}|{apple.originalTransactionIdentifier}";
                                }
                                var iap = GetTypeIAP(apple.productID);
                                _OrderIds.SafeAdd(iap, apple.originalTransactionIdentifier);
                            }
#endif
                    }
                }
            }
            catch (IAPSecurityException)
            {
                validPurchase = false;
            }
#endif
            var productId = args.purchasedProduct.definition.id;
            var typeIAP = GetTypeIAP(productId);
            if (validPurchase)
            {
                if (DefineSymbols.IsDebugIAP)
                    Debug.Log("ProcessPurchase finish: " + args.purchasedProduct.definition.id);
                //AnalyticsHelper.PurchaseToken = purchaseToken;
                _onPurchaseSuccessResult?.Invoke(typeIAP, transactionID);
            }
            else
            {
                //AnalyticsHelper.PurchaseToken = "";
                _onPurchaseFailedResult?.Invoke(typeIAP, PurchaseFailureReason.SignatureInvalid, string.Empty);
            }
            return PurchaseProcessingResult.Complete;
        }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            if (DefineSymbols.IsDebugIAP)
                Debug.LogError(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
            string idproduct = product.definition.id;
            _onPurchaseFailedResult?.Invoke(GetTypeIAP(idproduct), failureReason, product.transactionID);
        }
        TypeIAP GetTypeIAP(string productId)
        {
            if (_products != null && !string.IsNullOrEmpty(productId))
            {
                for (int i = 0, length = _products.Count; i < length; i++)
                {
                    if (productId.Equals(_products[i].idProduct))
                    {
                        return _products[i].typeIAP;
                    }
                }
            }
            return TypeIAP.Non;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            if (DefineSymbols.IsDebugIAP)
                Debug.LogError(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureDescription.reason));
            string idproduct = product.definition.id;
            _onPurchaseFailedResult?.Invoke(GetTypeIAP(idproduct), failureDescription.reason, product.transactionID);
        }
        public string GetOrderIdByType(TypeIAP typeIAP)
        {
            if (_orderIds.TryGetValueSafe(typeIAP, out string result))
            {
                return result;
            }
            return default;
        }
    }
}