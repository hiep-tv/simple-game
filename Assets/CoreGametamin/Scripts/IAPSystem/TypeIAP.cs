namespace Gametamin.Core
{
    public enum TypeIAP
    {
        Non = 0,
        Gem_0,
        Gem_begin,
        Gem_199,
        Gem_299,
        Gem_399,
        Gem_499,
        Gem_799,
        Gem_999,
        Gem_1999,
        Gem_2999,
        Gem_4999,
        Gem_7999,
        Gem_9999,
        Gem_limit = 20,

        Bundle_begin = 21,
        Bundle_299,
        Bundle_399,
        Bundle_499,
        Bundle_799,
        Bundle_999,
        Bundle_1999,
        Bundle_2999,
        Bundle_3999,
        Bundle_7999,
        Bundle_9999,
        Bundle1Free1_799,
        Bundle1Free1_0,
        BundleGrowth_799,
        BundleGrowth_0,
        BundleGrowth_1999,
        Booster_799,
        Retry_399,
        Bundle_limit = 40,

        NoAds_begin = 41,
        NoAds_499,
        NoAds_599,
        NoAds_limit = 50,

        Minigame_begin = 51,
        Minigame_499,
        Minigame_limit = 60,

        Subscription_begin = 61,
        Subscription_limit = 70,

        Weekend_begin = 71,
        Weekend_499,
        Weekend_999,
        Weekend_1999,
        Weekend_2999,
        Weekend_3999,
        Weekend_limit = 100,

        Treasure_begin = 101,
        Treasure_399,
        Treasure_999,
        Treasure_limit = 110,

        SeasonPass_begin = 111,
        SeasonPass_999,
        SeasonPass_limit = 120,

        RollnWin_begin = 121,
        RollnWin_499,
        RollnWin_0,
        RollnWin_999,
        RollnWin_limit = 140,

        FlashSale_begin = 141,
        FlashSale_299,
        FlashSale_999,
        FlashSale_1999,
        FlashSale_limit = 160,

        Special_begin = 161,
        Special_299,
        Special_999,
        Special_1999,
        Special_limit = 180,
        Energy_begin = 181,
        Energy_399,
        Energy_limit = 200,

        OneForAll_begin = 201,
        OneForAll_299,
        OneForAll_399,
        OneForAll_599,
        OneForAll_999,
        OneForAll_limit = 220,

    }
    public static partial class ShopHelper
    {
        public static string ToStoreID(this TypeIAP iapType)
        {
            return $"mg_{iapType.ToString().ToLower()}";
        }
        public static bool IsFreePack(this TypeIAP iapType)
        {
            return iapType == TypeIAP.Bundle1Free1_0 || iapType == TypeIAP.BundleGrowth_0 || iapType == TypeIAP.RollnWin_0;
        }
        public static bool IsFreeVideoGem(this TypeIAP iapType)
        {
            return iapType == TypeIAP.Gem_0;
        }
        public static bool IsEndlessTreasure(this TypeIAP iapType)
        {
            return iapType > TypeIAP.Treasure_begin && iapType < TypeIAP.Treasure_limit;
        }
        public static bool IsFlashSale(this TypeIAP iapType)
        {
            return iapType > TypeIAP.FlashSale_begin && iapType < TypeIAP.FlashSale_limit;
        }
        public static bool IsPickOne(this TypeIAP iapType)
        {
            return iapType > TypeIAP.OneForAll_begin && iapType < TypeIAP.OneForAll_limit;
        }
        public static TypeIAP GetInsteadOfCurrent(this TypeIAP typeIAP)
        {
            if (typeIAP == TypeIAP.Bundle_399)
            {
                return TypeIAP.Bundle_499;
            }
            if (typeIAP == TypeIAP.Bundle_499)
            {
                return TypeIAP.Bundle_999;
            }
            return TypeIAP.Non;
        }
        static bool IOS
        {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }
        public static TypeIAP[] AvailableComsumableProducts =
        {
            TypeIAP.Gem_199,
            TypeIAP.Gem_499,
            TypeIAP.Gem_999,
            TypeIAP.Gem_1999,
            TypeIAP.Gem_4999,
            TypeIAP.Gem_9999,

            TypeIAP.Bundle_299,
            TypeIAP.Bundle_399,
            TypeIAP.Bundle_499,
            TypeIAP.Bundle_999,
            TypeIAP.Bundle_1999,
            TypeIAP.Bundle_2999,
            TypeIAP.Bundle_3999,
            TypeIAP.Bundle_7999,
            TypeIAP.Bundle_9999,

            TypeIAP.Weekend_999,
            TypeIAP.Weekend_1999,
            TypeIAP.Weekend_2999,
            TypeIAP.Weekend_3999,

            TypeIAP.Treasure_399,
            TypeIAP.Treasure_999,

            TypeIAP.FlashSale_299,
            TypeIAP.FlashSale_999,
            TypeIAP.FlashSale_1999,

            TypeIAP.Energy_399,

            TypeIAP. OneForAll_299,
            TypeIAP. OneForAll_399,
            TypeIAP. OneForAll_599,
            TypeIAP. OneForAll_999,
        };
        public static TypeIAP[] AvailableNonComsumableProducts =
        {

        };
        public static TypeIAP[] AvailableSubscriptionProducts =
        {

        };

    }
}