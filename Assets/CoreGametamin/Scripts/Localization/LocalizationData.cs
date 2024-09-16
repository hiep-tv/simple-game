
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Gametamin.Core.Localization
{
    [Serializable]
    public class LocalizationAssetData
    {
        [SerializeField] LanguageType _defaultLanguage = LanguageType.English;
        [SerializeField] LanguageAssetData[] _textDatas;
        public void SetText(string textReferenceID, string text = default, LanguageType languageType = LanguageType.English)
        {
            var data = GetLanguageAssetData(languageType);
            data.SetText(textReferenceID, text);
        }
        public string GetText(LanguageType languageType, string textReferenceID)
        {
            if (textReferenceID == TextReferenceID.Empty) return default;
            var data = GetLanguageAssetData(languageType);
            var result = data.GetText(textReferenceID);
            if (result.IsNullOrEmptySafe() && languageType != _defaultLanguage)
            {
                data = GetLanguageAssetData(_defaultLanguage);
                result = data.GetText(textReferenceID);
            }
            return result;
        }
        public void GetTexts(LanguageType languageType, Action<string, string> callback = null)
        {
            var data = GetLanguageAssetData(languageType);
            data.Texts.For(item =>
            {
                callback?.Invoke(item.TextID.ToString(), item.Text);
            });
        }
        public void Reset(LanguageType languageType)
        {
            GetLanguageAssetData(languageType).Texts.Clear();
        }
        LanguageAssetData GetLanguageAssetData(LanguageType languageType)
        {
            LanguageAssetData result = default, defaultData = default;
            _textDatas.ForBreakable(data =>
            {
                if (data.LanguageType == languageType)
                {
                    result = data;
                    return true;
                }
                else if (data.LanguageType == _defaultLanguage)
                {
                    defaultData = data;
                }
                return false;
            });
            return result ?? defaultData;
        }
    }
    [Serializable]
    public class LanguageAssetData
    {
        [SerializeField] LanguageType _languageType;
        [SerializeField] List<TextAssetData> _texts;
        public LanguageType LanguageType => _languageType;
        public List<TextAssetData> Texts => _texts;
        public LanguageAssetData(Dictionary<string, object> rawData)
        {
            _languageType = rawData["LanguageType"].ToString().ToEnum<LanguageType>();
            _texts = new();
            var index = 0;
            foreach (var key in rawData.Keys)
            {
                if (!key.Equals("LanguageType"))
                {
                    _texts[index++] = new TextAssetData(key, rawData[key].ToString());
                }
            }
        }
        public void SetText(string textID, string text)
        {
            var exist = false;
            _texts.ForBreakable(data =>
            {
                exist = textID == data.TextID;
                if (exist)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        data.Text = text;
                    }
                }
                return exist;
            });
            if (!exist)
            {
                _texts.Add(new TextAssetData(textID, text));
            }
        }
        public string GetText(string textID)
        {
            var result = string.Empty;

            if (!textID.IsNullOrEmptySafe())
            {
                _texts.ForBreakable(data =>
                {
                    if (textID == data.TextID)
                    {
                        result = data.Text;
                        return true;
                    }
                    return false;
                });
            }
            return result;
        }
    }
    [Serializable]
    public class TextAssetData
    {
        [SerializeField] string _textID;
        [SerializeField] string _text;
        public TextAssetData(string textID, string text)
        {
            _textID = textID;
            _text = text;
        }
        public string TextID => _textID;
        public string Text
        {
            get => _text;
            set => _text = value;
        }
        public override string ToString()
        {
            return $"(Id:{TextID}, Text:{Text})";
        }
    }
}