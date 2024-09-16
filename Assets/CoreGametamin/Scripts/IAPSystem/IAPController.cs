#if DEBUG_MODE || UNITY_EDITOR
#define DEBUG_IAP
#endif
using System;
using DG.Tweening;
using Gametamin.Core.IAP;
using UnityEngine;
#if PURCHASING_DISABLED
using Purchasing;
#else
using UnityEngine.Purchasing;
#endif

namespace Gametamin.Core.IAP
{
    public static partial class Helper
    {
        public static bool IsFailure(this PurchaseResult purchaseResult) => purchaseResult == PurchaseResult.Failure;
        public static bool IsSuccessful(this PurchaseResult purchaseResult) => purchaseResult == PurchaseResult.Successful;
        public static bool IsSuccessfulOrRestoration(this PurchaseResult purchaseResult) => purchaseResult == PurchaseResult.Successful || purchaseResult == PurchaseResult.Restoration;
        public static bool IsRestoration(this PurchaseResult purchaseResult) => purchaseResult == PurchaseResult.Restoration;
    }
    public static partial class IAPController
    {
        static IAPSystem _system;
        static Action<PurchaseResultData> _onBuyResult;
        static TypeIAP _lastTypeIAP;
        static Tween _delayTime;
        static bool _initialized;
        public static bool Initialized
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return _initialized;
#endif
            }
        }
        static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
        static bool _DebugMode
        {
            get
            {
#if DEBUG_IAP
                return true;
#else
                return false;
#endif
            }
        }
        public static void Init()
        {
            InitializeGamingServices.Initialize(() =>
            {
                if (!IsEditor && !_initialized)
                {
                    //ShopDataConfigs.GetInstance(instance =>
                    //{
                    //    if (instance != null)
                    //    {
                    //        InitInternal();
                    //    }
                    //    else
                    //    {
                    //        if (DefineSymbols.IsDebugIAP || _DebugMode)
                    //        {
                    //            Debug.LogError("Cant load shop data!");
                    //        }
                    //    }
                    //});
                }
            });
        }
        static void InitInternal()
        {
            _system ??= new IAPSystem(OnPurchaseSuccessResult, OnPurchaseFailedResult, OnInitialized, OnInitializeFailed);
        }
        static void OnInitialized()
        {
            _initialized = true;
            _system.GetLocalPrices((typeIAP, localizedPriceString, localizedPrice) =>
            {
                //ShopDataConfigs.SetPrice(typeIAP, localizedPriceString, localizedPrice);
            });
        }
        static void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            if (_DebugMode)
            {
                _initialized = true;
                Debug.LogError($"OnInitializeFailed reason:{error}, message:{message}");
            }
        }
        public static void BuyIAP(TypeIAP typeIAP, Action<PurchaseResultData> onBuyResult = null)
        {
            //var shopdata = ShopDataConfigs.GetShopBundleData(typeIAP);
            //LogStartIAP(shopdata);
            BuyIAPInternal(typeIAP, onBuyResult);
        }
        static void BuyIAPInternal(TypeIAP typeIAP, Action<PurchaseResultData> onBuyResult = null)
        {
            if (_system != null)
            {
                PrepareToBuyIAP();
            }
            else
            {
                onBuyResult?.Invoke(new PurchaseResultData(typeIAP, PurchaseFailureReason.PurchasingUnavailable));
            }
            void PrepareToBuyIAP()
            {
                _lastTypeIAP = typeIAP;
                _onBuyResult = onBuyResult;
                StartBuyIAP(typeIAP);
                //UserInput.Enabled = false;
                //TextReferenceID.Waiting.GetTextById().ShowWaiting(() => StartBuyIAP(typeIAP));
            }
        }
        static void StartBuyIAP(TypeIAP typeIAP)
        {
            //if (DefineSymbols.IsAndroid)
            //{
            //    _delayTime = 60f.DelayCall(() => OnTimeout(typeIAP));
            //}
            _system.BuyProductType(typeIAP);
        }
        static void OnTimeout(TypeIAP typeIAP)
        {
            //var shopdata = ShopDataConfigs.GetShopBundleData(typeIAP);
            //LogBuyIAPTimeOut(shopdata);
        }
        static void OnPurchaseSuccessResult(TypeIAP typeIAP, string orderID)
        {
            if (_DebugMode)
            {
                //Debug.Log($"Purchased typeIAP:{typeIAP}, Path: {GetPathIAPName()}");
            }
            bool hasSender = _lastTypeIAP == typeIAP;
            {
                LogEvent();
                ReturnCallback(PurchaseResult.Successful);
            }
            void LogEvent()
            {
                //var shopData = SaveBoughtData(typeIAP, !hasSender);
                //LogBuyIAPSuceed(shopData, orderID);

                //if (_system != null && _system.GetPurchaseInfo(typeIAP, out string contentId, out decimal localizedPrice, out string currency))
                //{
                //    AnalyticsHelper.LogPurchase(contentId, localizedPrice, currency, orderID);
                //}
            }
            void ReturnCallback(PurchaseResult result)
            {
                _onBuyResult?.Invoke(new PurchaseResultData(typeIAP, result));
                _onBuyResult = null;
            }
        }
        //public static ShopItemData SaveBoughtData(TypeIAP typeIAP, bool broadcast = false)
        //{
        //    var shopdata = ShopDataConfigs.GetShopBundleData(typeIAP);
        //    if (shopdata != null)
        //    {
        //        var rewards = shopdata.GetRewards(true);
        //        LogGetItems(typeIAP, shopdata.Price, rewards);
        //        UserPrefs.SetIAPPurchased(typeIAP, shopdata.Price);
        //        UserPrefs.AddItems(rewards, broadcast);
        //    }
        //    return shopdata;
        //}
        static void OnPurchaseFailedResult(TypeIAP typeIAP, PurchaseFailureReason reason, string orderId)
        {
            if (_DebugMode)
            {
                //Debug.LogError($"PurchaseFailed typeIAP:{typeIAP}, reason:{reason}, Path: {GetPathIAPName()}");
            }
            //var shopdata = ShopDataConfigs.GetShopBundleData(typeIAP);
            //LogBuyIAPFailed(shopdata, reason.ToString(), orderId);
            _onBuyResult?.Invoke(new PurchaseResultData(typeIAP, reason));
            _onBuyResult = null;
        }
    }
}