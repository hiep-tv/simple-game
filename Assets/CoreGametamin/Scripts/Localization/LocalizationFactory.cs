using System;
using UnityEngine;
namespace Gametamin.Core.Localization
{
    public partial class LocalizationFactory : ScriptableObject
    {

        private static LocalizationFactory _instance;
        public static LocalizationFactory Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying && !Active)
                {
                    string[] SearchSpriteAtlasInFolders = { "Assets/Gametamin.Core/" };
                    SearchSpriteAtlasInFolders.FindAssetDatabase<LocalizationFactory>(resutlt => _instance = resutlt);
                }
#endif
                return _instance;
            }
        }
        public static bool Active => _instance != null;
        public static void GetInstance(Action<LocalizationFactory> callback = null)
        {
            if (_instance == null)
            {
                AddressableLabels.Localization_data.LoadAddressableAssetAsync<LocalizationFactory>(handler =>
                {
                    if (handler.Status.OperationHandleSucceeded())
                    {
                        _instance = handler.Result;
                        callback?.Invoke(_instance);
                    }
                    else
                    {
                        handler.ReleaseAddressablesAsset();
                        callback?.Invoke(null);
                    }
                });
            }
            else
            {
                callback?.Invoke(_instance);
            }
        }
        [SerializeField] LocalizationAssetData _textDatas;
        public static string GetText(string textReferenceID, LanguageType languageType = LanguageType.English)
        {
            return Instance.GetTextInternal(textReferenceID, languageType);
        }
        string GetTextInternal(string textReferenceID, LanguageType languageType = LanguageType.English)
        {
            return _textDatas.GetText(languageType, textReferenceID);
        }
    }
}