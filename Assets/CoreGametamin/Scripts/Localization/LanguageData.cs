using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
namespace Gametamin.Core.Localization
{
    public class LanguageData : ScriptableObject
    {
        [SerializeField] LanguageType _languageType;
        [SerializeField] TMP_FontAsset _fontAsset;
        [SerializeField] List<TextAssetData> _texts;
        public LanguageType LanguageType => _languageType;
        public TMP_FontAsset FontAsset => _fontAsset;
        public List<TextAssetData> Texts => _texts;
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
        public void Clear()
        {
            Texts.SafeClear();
        }
        [NaughtyAttributes.Button]
        void LogAllTexts()
        {
            var builder = new StringBuilder();

            _texts.For(data =>
            {
                builder.Append(data.Text);
            });
            //Gametamin.FileHelper.SaveText(builder.ToString(), $"Editor Default Resources/{LanguageType.ToString().ToLower()}.txt");
        }
    }
}
