using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gametamin.Core.Localization
{
    public static partial class LocalizationHelper
    {
        enum InitializeState
        {
            Non,
            Initializing,
            Initialized
        }
        static event Action<LanguageType> _onLanguageChanged;
        static bool _AlwayUpperCase
        {
            get;
            set;
        }
        static List<TextAssetData> _defaultLanguageTexts;
        static List<TextAssetData> _DefaultLanguageTexts
        {
            set => _defaultLanguageTexts = value;
            get => _defaultLanguageTexts;
        }
        static List<TextAssetData> _currentLanguageTexts;
        static List<TextAssetData> _CurrentLanguageTexts
        {
            set => _currentLanguageTexts = value;
            get => _currentLanguageTexts;
        }
        static readonly List<string> _loadLanguageKeys = new List<string>() { AddressableLabels.Text_assets };
        static TMP_FontAsset _mainFont;
        static Action _onInitialized;
        static InitializeState _initializeState;
        public static void Initialize(Action callback = null)
        {
            Init();
            if (_initializeState == InitializeState.Initialized)
            {
                callback?.Invoke();
                return;
            }
            _onInitialized += callback;
            if (_initializeState == InitializeState.Initializing)
            {
                return;
            }
            _initializeState = InitializeState.Initializing;
            var currentLanguage = CurrentLanguage;
            currentLanguage.LoadFontAsset(result =>
            {
                if (!result)
                {
                    LoadLanguage(DefaultLanguage, Callback);
                }
                else
                {
                    Callback();
                }
            });
            void Callback()
            {
                _initializeState = InitializeState.Initialized;
                var temp = _onInitialized;
                _onInitialized = null;
                temp?.Invoke();
            }
        }
        public static void LoadFontAsset(this LanguageType languageType, Action<bool> callback)
        {
            if (_mainFont == null)
            {
                //AddressableLabels.Font_asset.LoadAddressableAssetAsync<TMP_FontAsset>(handler =>
                //{
                //    if (handler.Status.OperationHandleSucceeded())
                //    {
                //        _mainFont = handler.Result;
                //        languageType.LoadFontFallback(_mainFont, callback);
                //    }
                //    else
                //    {
                //        handler.ReleaseAddressablesAsset();
                callback?.Invoke(false);
                //    }
                //});
            }
            else
            {
                languageType.LoadFontFallback(_mainFont, callback);
            }
        }
        static void LoadFontFallback(this LanguageType languageType, TMP_FontAsset mainFont, Action<bool> callback)
        {
            var addressableLabels = string.Empty;
            switch (languageType)
            {
                case LanguageType.English:
                case LanguageType.Portuguese:
                case LanguageType.French:
                case LanguageType.Spanish:
                case LanguageType.Vietnamese:
                case LanguageType.Italian:
                case LanguageType.Turkish:
                case LanguageType.Indonesian:
                case LanguageType.German:
                    LoadLanguage(languageType, () => callback?.Invoke(true));
                    return;
                case LanguageType.Russian:
                case LanguageType.Hindi:
                    //addressableLabels = AddressableLabels.Other_font;
                    break;
                case LanguageType.Japanese:
                    //addressableLabels = AddressableLabels.Japanese_font;
                    break;
                case LanguageType.Korean:
                    //addressableLabels = AddressableLabels.Korean_font;
                    break;
                case LanguageType.Chinese:
                    //addressableLabels = AddressableLabels.Chinese_font;
                    break;
                default:
                    break;
            }
            if (!addressableLabels.IsNullOrEmptySafe())
            {
                CheckFontAssetResources(addressableLabels, hasResource =>
                {
                    if (hasResource)
                    {
                        LoadFontAssets(addressableLabels, mainFont
                            , loaded =>
                            {
                                if (loaded)
                                {
                                    LoadLanguage(languageType, () => callback?.Invoke(true));
                                }
                                else
                                {
                                    callback?.Invoke(false);
                                }
                            });

                    }
                    else
                    {
                        callback?.Invoke(false);
                    }
                });
            }
            else
            {
                callback?.Invoke(false);
            }
        }
        static bool _downloading = false;
        static void CheckFontAssetResources(string key, Action<bool> callback)
        {
            key.GetDownloadSizeAsync(size =>
            {
                var hasResources = size <= 0;
                if (!hasResources)
                {
                    if (!_downloading)
                    {
                        _downloading = true;
                        key.DownloadDependenciesAsync(result =>
                        {
                            _downloading = false;
                        });
                    }
                }
                callback?.Invoke(hasResources);
            });
        }
        static void LoadFontAssets(string addressableLabels, TMP_FontAsset mainFont, Action<bool> callback)
        {
            addressableLabels.LoadAddressableAssetAsync<TMP_FontAsset>(handler =>
            {
                if (handler.Status.OperationHandleSucceeded())
                {
                    var fallback = handler.Result;
                    var fallbackList = mainFont.fallbackFontAssetTable;
                    if (fallbackList != null)
                    {
                        fallbackList.SafeClear();
                        fallbackList.Add(fallback);
                    }
                    else
                    {
                        fallbackList = new()
                        {
                            fallback
                        };
                        mainFont.fallbackFontAssetTable = fallbackList;
                    }
                    callback?.Invoke(true);
                }
                else
                {
                    handler.ReleaseAddressablesAsset();
                    callback?.Invoke(false);
                }
            });
        }
        public static void LoadLanguage(LanguageType languageType, Action callback = null)
        {
            _AlwayUpperCase = languageType == LanguageType.Vietnamese;
            AsyncOperationHandle<IList<LanguageData>> hanlder = default;
            hanlder = _loadLanguageKeys.LoadAddressableAssetsAsync<LanguageData>(data =>
            {
                if (data.LanguageType == LanguageType.English)
                {
                    _DefaultLanguageTexts = data.Texts;
                }
                if (data.LanguageType == languageType)
                {
                    _CurrentLanguageTexts = data.Texts;
                }
            }, () =>
            {
                hanlder.ReleaseAddressablesAsset();
                callback?.Invoke();
            });
            _onLanguageChanged?.Invoke(languageType);
        }
        public static string GetText(string textId)
        {
            var result = string.Empty;
            _CurrentLanguageTexts.ForBreakable(item =>
            {
                var exist = item.TextID.EqualsSafe(textId);
                if (exist)
                {
                    result = item.Text;
                }
                return exist;
            });
            if (result.IsNullOrEmptySafe())
            {
                result = GetDefaultText(textId);
            }
            if (_AlwayUpperCase)
            {
                result = result.ToUpper();
            }
            return result;
        }
        static string GetDefaultText(string textId)
        {
            var result = string.Empty;
            _DefaultLanguageTexts.ForBreakable(item =>
            {
                var exist = item.TextID.EqualsSafe(textId);
                if (exist)
                {
                    result = item.Text;
                }
                return exist;
            });
            return result;
        }
        public static void AddOnLanguageChangeListener(this Action<LanguageType> callback)
        {
            _onLanguageChanged += callback;
        }
        public static void RemoveOnLanguageChangeListener(this Action<LanguageType> callback)
        {
            _onLanguageChanged += callback;
        }
    }
}
