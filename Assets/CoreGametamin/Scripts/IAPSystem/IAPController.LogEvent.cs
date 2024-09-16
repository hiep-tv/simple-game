//using System.Collections.Generic;
//namespace Gametamin.Core.IAP
//{
//    public static partial class IAPController
//    {
//        static void LogStartIAP(ShopItemData data)
//        {
//            AnalyticsHelper.LogStartIAP(GetPathIAP(), data);
//        }
//        static void LogBuyIAPTimeOut(ShopItemData data)
//        {
//            AnalyticsHelper.LogBuyIAP(GetPathIAP(), data, null, "Timeout");
//        }
//        static void LogBuyIAPSuceed(ShopItemData data, string orderID)
//        {
//            AnalyticsHelper.LogBuyIAP(GetPathIAP(), data, orderID, null);
//        }
//        static void LogBuyIAPFailed(ShopItemData data, string reason, string orderId)
//        {
//            AnalyticsHelper.LogBuyIAP(GetPathIAP(), data, null, reason);
//            AnalyticsHelper.LogPurchaseFail(data.TypeIAP.ToStoreID());
//        }
//        public static void LogGetItems(TypeIAP typeIAP, int price, List<ItemData> rewards)
//        {
//            AnalyticsHelper.LogGetItems(MergeGame.GetItemFromType.Shop, rewards, dict =>
//            {
//                dict.Add(AnalyticsHelper.Keys.Param1, typeIAP);
//            });
//        }
//    }
//}
