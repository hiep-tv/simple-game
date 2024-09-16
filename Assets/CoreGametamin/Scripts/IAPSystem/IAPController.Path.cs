//using Gametamin;
//namespace Gametamin.Core.IAP
//{
//    public static partial class IAPController
//    {
//        //public enum PathIAPType
//        //{
//        //    Non = -1,
//        //    Shop,
//        //    MainMenu,
//        //    MainGame,
//        //    Season,
//        //    OnetimeOffer,
//        //    TwoOffer,
//        //    WeekendOffer,
//        //    SeasonOffer,
//        //    Treasure,
//        //    AddEnergy,
//        //    ShopEnergy,
//        //    ShopSellItem,
//        //    ItemInfo
//        //}
//        //public static PathIAPType[] menuPaths = { PathIAPType.MainMenu, PathIAPType.Shop };

//        //public static PathIAPType[] shopEnergyPaths = { PathIAPType.ShopEnergy, PathIAPType.Shop };
//        //public static PathIAPType[] addEnergyPaths = { PathIAPType.AddEnergy, PathIAPType.Shop };
//        //public static PathIAPType[] shopSellItemPaths = { PathIAPType.ShopSellItem, PathIAPType.Shop };
//        //public static PathIAPType[] itemInfoPaths = { PathIAPType.ItemInfo, PathIAPType.Shop };

//        //public static PathIAPType[] oneTimeOffer = { PathIAPType.MainMenu, PathIAPType.OnetimeOffer };
//        //public static PathIAPType[] twoOffer = { PathIAPType.MainMenu, PathIAPType.TwoOffer };
//        //public static PathIAPType[] weekendOffer = { PathIAPType.MainMenu, PathIAPType.WeekendOffer };
//        //public static PathIAPType[] treasureOffer = { PathIAPType.MainMenu, PathIAPType.Treasure };
//        //public static PathIAPType[] seasonOffer = { PathIAPType.MainMenu, PathIAPType.SeasonOffer };
//        //public static PathIAPType[] seasonOffer2 = { PathIAPType.MainMenu, PathIAPType.Season, PathIAPType.SeasonOffer };


//        //public static PathIAPType[] currentPaths;

//        public static string GetPathIAP()
//        {
//            //var paths = "";// JsonHelper.JoinItems(currentPaths, item => item.ToString(), '_');
//            return MergeGame.UI.ShopIAP.ShopIAPUI.GetPathIAP();
//        }
//        public static string GetPathIAPName()
//        {
//            //var paths = "";//JsonHelper.JoinItems(currentPaths, item => item.ToString(), '>');
//            return MergeGame.UI.ShopIAP.ShopIAPUI.GetPathIAPName();
//        }
//        //public static void SetMenuPath()
//        //{
//        //    currentPaths = menuPaths;
//        //}
//        //public static void SetOnetimePath()
//        //{
//        //    currentPaths = oneTimeOffer;
//        //}
//        //public static void Set2OfferPath()
//        //{
//        //    currentPaths = twoOffer;
//        //}
//        //public static void SetWeekendPath()
//        //{
//        //    currentPaths = weekendOffer;
//        //}
//        //public static void SetTreasurePath()
//        //{
//        //    currentPaths = treasureOffer;
//        //}
//        ///// <summary>
//        ///// Menu -> Season Offer
//        ///// </summary>
//        ///// <param name="menu"></param>
//        //public static void SetSeasonMenuPath()
//        //{
//        //    currentPaths = seasonOffer;
//        //}
//        ///// <summary>
//        ///// Menu -> Season -> Season Offer
//        ///// </summary>
//        ///// <param name="menu"></param>
//        //public static void SetSeasonMenuPath2()
//        //{
//        //    currentPaths = seasonOffer2;
//        //}

//        //public static void SetShopEnergyPath()
//        //{
//        //    currentPaths = shopEnergyPaths;
//        //}
//        //public static void SetAddEnergyPath()
//        //{
//        //    currentPaths = addEnergyPaths;
//        //}
//        //public static void SetShopSellItemPath()
//        //{
//        //    currentPaths = shopSellItemPaths;
//        //}
//        //public static void SetItemInfoPath()
//        //{
//        //    currentPaths = itemInfoPaths;
//        //}
//    }
//}