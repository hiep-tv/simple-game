#if PURCHASING_DISABLED
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gametamin.Core;
namespace Purchasing
{
    public enum InitializationFailureReason
    {
        PurchasingUnavailable,
        NoProductsAvailable,
        AppNotKnown
    }

    //public enum PurchaseFailureReason
    //{
    //    PurchasingUnavailable,
    //    ExistingPurchasePending,
    //    ProductUnavailable,
    //    SignatureInvalid,
    //    UserCancelled,
    //    PaymentDeclined,
    //    DuplicateTransaction,
    //    Unknown
    //}

    public enum PurchaseProcessingResult
    {
        Complete,
        Pending
    }

    public enum ProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }

    public class ProductDefinition
    {
        public string id { get; set; }
        public string storeSpecificId { get; set; }
        public ProductType type { get; set; }
        public bool enabled { get; set; }
    }

    public class ProductMetadata
    {
        public string localizedPriceString { get; set; }
        public string localizedTitle { get; set; }
        public string localizedDescription { get; set; }
        public string isoCurrencyCode { get; set; }
        public decimal localizedPrice { get; set; }
    }

    public class Product
    {
        public ProductDefinition definition { get; set; }
        public ProductMetadata metadata { get; set; }
        public bool availableToPurchase { get; set; }
        public string transactionID { get; set; }
        public string appleOriginalTransactionID { get; set; }
        public bool appleProductIsRestored { get; set; }
        public bool hasReceipt => !string.IsNullOrEmpty(receipt);
        public string receipt { get; set; }
    }

    public class PurchaseEventArgs
    {
        public Product purchasedProduct { get; set; }
    }

    public class ProductCollection
    {
        public HashSet<Product> set { get; set; }
        public Product[] all { get; set; }
        public Product WithID(string id) => default;
        public Product WithStoreSpecificID(string id) => default;
    }

    public interface IStoreController
    {
        ProductCollection products { get; }
        void InitiatePurchase(Product product, string payload);
        void InitiatePurchase(string productId, string payload);
        void InitiatePurchase(Product product);
        void InitiatePurchase(string productId);

        [Obsolete]
        void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason> failCallback);
        void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason, string> failCallback);
        void ConfirmPendingPurchase(Product product);
    }

    public interface IStoreExtension
    {
    }

    public interface IExtensionProvider
    {
        T GetExtension<T>() where T : IStoreExtension;
    }

    public interface IDetailedStoreListener : IStoreListener
    {
        void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription);
    }

    public enum Result
    {
        True,
        False,
        Unsupported
    }

    public class AppleAppStore
    {
        public const string Name = "AppleAppStore";
    }

    public class GooglePlay
    {
        public const string Name = "GooglePlay";
    }

    public class AppleInAppPurchaseReceipt : IPurchaseReceipt
    {
        public int quantity { get; }
        public string productID { get; }
        public string transactionID { get; }
        public string originalTransactionIdentifier { get; }
        public DateTime purchaseDate { get; }
        public DateTime originalPurchaseDate { get; }
        public DateTime subscriptionExpirationDate { get; }
        public DateTime cancellationDate { get; }
        public int isFreeTrial { get; }
        public int productType { get; }
        public int isIntroductoryPricePeriod { get; }
    }

    public class SubscriptionInfo
    {
        public SubscriptionInfo(AppleInAppPurchaseReceipt r, string intro_json) { }

        public SubscriptionInfo(string skuDetails, bool isAutoRenewing, DateTime purchaseDate, bool isFreeTrial, bool hasIntroductoryPriceTrial, bool purchaseHistorySupported, string updateMetadata) { }

        public SubscriptionInfo(string productId) { }

        public string getProductId() => default;

        public DateTime getPurchaseDate() => default;

        public Result isSubscribed() => default;

        public Result isExpired() => default;

        public Result isCancelled() => default;

        public Result isFreeTrial() => default;

        public Result isAutoRenewing() => default;

        public TimeSpan getRemainingTime() => default;

        public Result isIntroductoryPricePeriod() => default;

        public TimeSpan getIntroductoryPricePeriod() => default;

        public string getIntroductoryPrice() => default;

        public long getIntroductoryPricePeriodCycles() => default;

        public DateTime getExpireDate() => default;

        public DateTime getCancelDate() => default;

        public TimeSpan getFreeTrialPeriod() => default;

        public TimeSpan getSubscriptionPeriod() => default;

        public string getFreeTrialPeriodString() => default;

        public string getSkuDetails() => default;

        public string getSubscriptionInfoJsonString() => default;
    }

    public interface IStoreListener
    {
        [Obsolete]
        void OnInitializeFailed(InitializationFailureReason error);
        void OnInitializeFailed(InitializationFailureReason error, string message);

        PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent);

        [Obsolete("Use IDetailedStoreListener.OnPurchaseFailed for more detailed callback.", false)]
        void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason);
        void OnInitialized(IStoreController controller, IExtensionProvider extensions);
    }

    public interface IPurchaseReceipt
    {
        string transactionID { get; }
        string productID { get; }
        DateTime purchaseDate { get; }
    }

    public class CrossPlatformValidator
    {
        public CrossPlatformValidator(byte[] googlePublicKey, byte[] appleRootCert, string appBundleId) { }
        public CrossPlatformValidator(byte[] googlePublicKey, byte[] appleRootCert, string googleBundleId, string appleBundleId) { }
        public IPurchaseReceipt[] Validate(string unityIAPReceipt) => null;
    }

    public enum AppStore
    {
        NotSpecified,
        GooglePlay,
        AmazonAppStore,
        UDP,
        MacAppStore,
        AppleAppStore,
        WinRT,
        fake
    }

    public enum FakeStoreUIMode
    {
        Default,
        StandardUser,
        DeveloperUser
    }

    public abstract class AbstractPurchasingModule : IPurchasingModule
    {
        protected IPurchasingBinder m_Binder;
        public void Configure(IPurchasingBinder binder) { }
        protected void RegisterStore(string name, IStore store) { }
        protected void BindExtension<T>(T instance) where T : IStoreExtension { }
        protected void BindConfiguration<T>(T instance) where T : IStoreConfiguration { }
        public abstract void Configure();
    }

    public interface IAndroidStoreSelection : IStoreConfiguration
    {
        AppStore appStore { get; }
    }

    public class StandardPurchasingModule : AbstractPurchasingModule, IAndroidStoreSelection, IStoreConfiguration
    {
        [Obsolete("Not accurate. Use Version instead.", false)]
        public const string k_PackageVersion = "3.0.1";

        public AppStore appStore { get; set; }
        public FakeStoreUIMode useFakeStoreUIMode { get; set; }
        public bool useFakeStoreAlways { get; set; }

        static StandardPurchasingModule _instance = new StandardPurchasingModule();
        public static StandardPurchasingModule Instance() => _instance;
        public static StandardPurchasingModule Instance(AppStore androidStore) => _instance;
        public override void Configure() { }
    }

    public interface IStoreConfiguration
    {
    }

    public class ProductDescription
    {
        public ProductType type;
        public string storeSpecificId { get; }
        public ProductMetadata metadata { get; }
        public string receipt { get; }
        public string transactionId { get; set; }
    }

    public class PurchaseFailureDescription
    {
        public string productId { get; }
        public PurchaseFailureReason reason { get; }
        public string message { get; }
    }

    public interface IStoreCallback
    {
        ProductCollection products { get; }
        bool useTransactionLog { get; set; }

        [Obsolete]
        void OnSetupFailed(InitializationFailureReason reason);
        void OnSetupFailed(InitializationFailureReason reason, string message);
        void OnProductsRetrieved(List<ProductDescription> products);
        void OnPurchaseSucceeded(string storeSpecificId, string receipt, string transactionIdentifier);
        void OnAllPurchasesRetrieved(List<Product> purchasedProducts);
        void OnPurchaseFailed(PurchaseFailureDescription desc);
    }

    public interface IStore
    {
        void Initialize(IStoreCallback callback);
        void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products);
        void Purchase(ProductDefinition product, string developerPayload);
        void FinishTransaction(ProductDefinition product, string transactionId);
    }

    public interface ICatalogProvider
    {
        void FetchProducts(Action<HashSet<ProductDefinition>> callback);
    }

    public interface IPurchasingBinder
    {
        void RegisterStore(string name, IStore store);
        void RegisterExtension<T>(T instance) where T : IStoreExtension;
        void RegisterConfiguration<T>(T instance) where T : IStoreConfiguration;
        void SetCatalogProvider(ICatalogProvider provider);
        void SetCatalogProviderFunction(Action<Action<HashSet<ProductDefinition>>> func);
    }

    public interface IPurchasingModule
    {
        void Configure(IPurchasingBinder binder);
    }

    public class IDs : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        public void Add(string id, params string[] stores) { }
        public void Add(string id, params object[] stores) { }
        IEnumerator IEnumerable.GetEnumerator() => default;
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => default;
    }

    public enum PayoutType
    {
        Other,
        Currency,
        Item,
        Resource
    }

    public class PayoutDefinition
    {
        public const int MaxSubtypeLength = 64;
        public const int MaxDataLength = 1024;
        public PayoutType type { get; set; }
        public string typeString => default;
        public string subtype { get; }
        public double quantity { get; }
        public string data { get; }
    }

    public class ConfigurationBuilder
    {
        public bool useCatalogProvider { get; set; }
        public HashSet<ProductDefinition> products { get; }
        public T Configure<T>() where T : IStoreConfiguration => default;
        static ConfigurationBuilder _instance = new ConfigurationBuilder();
        public static ConfigurationBuilder Instance(IPurchasingModule first, params IPurchasingModule[] rest) => _instance;
        public ConfigurationBuilder AddProduct(string id, ProductType type) => _instance;
        public ConfigurationBuilder AddProduct(string id, ProductType type, IDs storeIDs) => _instance;
        public ConfigurationBuilder AddProduct(string id, ProductType type, IDs storeIDs, PayoutDefinition payout) => _instance;
        public ConfigurationBuilder AddProduct(string id, ProductType type, IDs storeIDs, IEnumerable<PayoutDefinition> payouts) => _instance;
        public ConfigurationBuilder AddProducts(IEnumerable<ProductDefinition> products) => _instance;
    }

    public enum TranslationLocale
    {
        zh_TW,
        cs_CZ,
        da_DK,
        nl_NL,
        en_US,
        fr_FR,
        fi_FI,
        de_DE,
        iw_IL,
        hi_IN,
        it_IT,
        ja_JP,
        ko_KR,
        no_NO,
        pl_PL,
        pt_PT,
        ru_RU,
        es_ES,
        sv_SE,
        zh_CN,
        en_AU,
        en_CA,
        en_GB,
        fr_CA,
        el_GR,
        id_ID,
        ms_MY,
        pt_BR,
        es_MX,
        th_TH,
        tr_TR,
        vi_VN
    }

    public class LocalizedProductDescription
    {
        public TranslationLocale googleLocale = TranslationLocale.en_US;
        public string Title { get; set; }
        public string Description { get; set; }
        public LocalizedProductDescription Clone() => default;
        private static string EncodeNonLatinCharacters(string s) => s;
        private static string DecodeNonLatinCharacters(string s) => s;
    }

    public class Price
    {
        public decimal value;
        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() { }
    }

    public class ProductCatalogPayout
    {
        public enum ProductCatalogPayoutType
        {
            Other,
            Currency,
            Item,
            Resource
        }

        public const int MaxSubtypeLength = 64;
        public const int MaxDataLength = 1024;
        public ProductCatalogPayoutType type { get; set; }
        public string typeString => default;
        public string subtype { get; set; }
        public double quantity { get; set; }
        public string data { get; set; }
    }

    public class StoreID
    {
        public string store;
        public string id;
        public StoreID(string store_, string id_) { }
    }

    public class ProductCatalogItem
    {
        public string id;
        public ProductType type;
        public LocalizedProductDescription defaultDescription = new LocalizedProductDescription();
        public string screenshotPath;
        public int applePriceTier;
        public Price googlePrice = new Price();
        public string pricingTemplateID;
        public Price udpPrice = new Price();
        public IList<ProductCatalogPayout> Payouts => default;
        public ICollection<StoreID> allStoreIDs => default;
        public bool HasAvailableLocale => false;
        public TranslationLocale NextAvailableLocale { get; }
        public ICollection<LocalizedProductDescription> translatedDescriptions => default;
        public void AddPayout() { }
        public void RemovePayout(ProductCatalogPayout payout) { }
        public ProductCatalogItem Clone() => default;
        public void SetStoreID(string aStore, string aId) { }
        public string GetStoreID(string store) => store;
        public void SetStoreIDs(ICollection<StoreID> storeIds) { }
        public LocalizedProductDescription GetDescription(TranslationLocale locale) => default;
        public LocalizedProductDescription GetOrCreateDescription(TranslationLocale locale) => default;
        public LocalizedProductDescription AddDescription(TranslationLocale locale) => default;
        public void RemoveDescription(TranslationLocale locale) { }
    }

    public interface IProductCatalogImpl
    {
        ProductCatalog LoadDefaultCatalog();
    }

    public class ProductCatalog
    {
        public string appleSKU;
        public string appleTeamID;
        public bool enableCodelessAutoInitialization = true;
        public bool enableUnityGamingServicesAutoInitialization;
        public const string kCatalogPath = "Assets/Resources/IAPProductCatalog.json";
        public const string kPrevCatalogPath = "Assets/Plugins/UnityPurchasing/Resources/IAPProductCatalog.json";
        public ICollection<ProductCatalogItem> allProducts => default;
        public ICollection<ProductCatalogItem> allValidProducts => default;
        public static void Initialize(IProductCatalogImpl productCatalogImpl) { }
        public void Add(ProductCatalogItem item) { }
        public void Remove(ProductCatalogItem item) { }
        public bool IsEmpty() => true;
        public static string Serialize(ProductCatalog catalog) => default;
        public static ProductCatalog Deserialize(string catalogJSON) => default;
        public static ProductCatalog FromTextAsset(TextAsset asset) => default;
        public static ProductCatalog LoadDefaultCatalog() => default;
    }

    public static class IAPConfigurationHelper
    {
        public static void PopulateConfigurationBuilder(ref ConfigurationBuilder builder, ProductCatalog catalog) { }
    }

    public static class UnityPurchasing
    {
        [Obsolete("Use Initialize(IDetailedStoreListener, ConfigurationBuilder)", false)]
        public static void Initialize(IStoreListener listener, ConfigurationBuilder builder) { }
    }

    public interface IGooglePlayConfiguration : IStoreConfiguration
    {
        void SetServiceDisconnectAtInitializeListener(Action action);
        void SetQueryProductDetailsFailedListener(Action<int> action);
        void SetDeferredPurchaseListener(Action<Product> action);
        void SetDeferredProrationUpgradeDowngradeSubscriptionListener(Action<Product> action);
        void SetObfuscatedAccountId(string accountId);
        void SetObfuscatedProfileId(string profileId);
        void SetFetchPurchasesAtInitialize(bool enable);
        void SetFetchPurchasesExcludeDeferred(bool exclude);
    }

    public enum AppleStorePromotionVisibility
    {
        Default,
        Hide,
        Show
    }

    public interface IAppleExtensions : IStoreExtension
    {
        bool simulateAskToBuy { get; set; }
        void RefreshAppReceipt(Action<string> successCallback, Action<string> errorCallback);
        [Obsolete("RefreshAppReceipt(Action<string> successCallback, Action errorCallback) is deprecated, please use RefreshAppReceipt(Action<string> successCallback, Action<string> errorCallback) instead.")]
        void RefreshAppReceipt(Action<string> successCallback, Action errorCallback);
        string GetTransactionReceiptForProduct(Product product);
        [Obsolete("RestoreTransactions(Action<bool> callback) is deprecated, please use RestoreTransactions(Action<bool, string> callback) instead.")]
        void RestoreTransactions(Action<bool> callback);
        void RestoreTransactions(Action<bool, string> callback);
        void RegisterPurchaseDeferredListener(Action<Product> callback);
        void SetApplicationUsername(string applicationUsername);
        void FetchStorePromotionOrder(Action<List<Product>> successCallback, Action errorCallback);
        void SetStorePromotionOrder(List<Product> products);
        void FetchStorePromotionVisibility(Product product, Action<string, AppleStorePromotionVisibility> successCallback, Action errorCallback);
        void SetStorePromotionVisibility(Product product, AppleStorePromotionVisibility visible);
        void ContinuePromotionalPurchases();
        Dictionary<string, string> GetIntroductoryPriceDictionary();
        Dictionary<string, string> GetProductDetails();
        void PresentCodeRedemptionSheet();
    }

    public enum GooglePlayProrationMode
    {
        UnknownSubscriptionUpgradeDowngradePolicy,
        ImmediateWithTimeProration,
        ImmediateAndChargeProratedPrice,
        ImmediateWithoutProration,
        Deferred,
        ImmediateAndChargeFullPrice
    }

    public enum GooglePurchaseState
    {
        Purchased = 0,
        Cancelled = 1,
        Refunded = 2,
        Deferred = 4
    }

    public interface IGooglePlayStoreExtensions : IStoreExtension
    {
        void UpgradeDowngradeSubscription(string oldSku, string newSku);
        void UpgradeDowngradeSubscription(string oldSku, string newSku, int desiredProrationMode);
        void UpgradeDowngradeSubscription(string oldSku, string newSku, GooglePlayProrationMode desiredProrationMode);
        [Obsolete("RestoreTransactions(Action<bool> callback) is deprecated, please use RestoreTransactions(Action<bool, string> callback) instead.")]
        void RestoreTransactions(Action<bool> callback);
        void RestoreTransactions(Action<bool, string> callback);
        void ConfirmSubscriptionPriceChange(string productId, Action<bool> callback);
        bool IsPurchasedProductDeferred(Product product);
        GooglePurchaseState GetPurchaseState(Product product);
    }

    public class IAPSecurityException : Exception
    {
        public IAPSecurityException() { }
        public IAPSecurityException(string message) : base(message) { }
    }

    public class GooglePlayTangle
    {
        public static readonly bool IsPopulated = true;
        public static byte[] Data() => default;
    }

    public class AppleTangle
    {
        public static readonly bool IsPopulated = true;
        public static byte[] Data() => default;
    }

    public class GooglePlayReceipt : IPurchaseReceipt
    {
        public string productID { get; }
        public string transactionID => orderID;
        public string orderID { get; }
        public string packageName { get; }
        public string purchaseToken { get; }
        public DateTime purchaseDate { get; }
        public GooglePurchaseState purchaseState { get; }
        public GooglePlayReceipt(string productID, string orderID, string packageName, string purchaseToken, DateTime purchaseTime, GooglePurchaseState purchaseState) { }
    }

    public class SubscriptionManager
    {
        public static void UpdateSubscription(Product newProduct, Product oldProduct, string developerPayload, Action<Product, string> appleStore, Action<string, string> googleStore) { }
        public static void UpdateSubscriptionInGooglePlayStore(Product oldProduct, Product newProduct, Action<string, string> googlePlayUpdateCallback) { }
        public static void UpdateSubscriptionInAppleStore(Product newProduct, string developerPayload, Action<Product, string> appleStoreUpdateCallback) { }
        public SubscriptionManager(Product product, string intro_json) { }
        public SubscriptionManager(string receipt, string id, string intro_json) { }
        public SubscriptionInfo getSubscriptionInfo() => default;
    }
}
#endif