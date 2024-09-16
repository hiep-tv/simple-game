
#if UNITY_EDITOR
using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Gametamin.Core.Localization
{
    public partial class LocalizationFactory
    {
        [SerializeField] LanguageData[] _languages;
        LanguageData GetLanguageData(LanguageType languageType)
        {
            LanguageData result = default;
            _languages.ForBreakable(item =>
            {
                if (item.LanguageType == languageType)
                {
                    result = item;
                    return true;
                }
                return false;
            });
            return result;
        }
        [ContextMenu("LogUnique")]
        void LogUnique()
        {
            var st1 = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ¡£¥º¿ÀÁÂÃÄÇÈÉÊÌÍÎÑÒÓÔÕÖÙÚÛÜÝßàáâãäçèéêìíîñòóôõöùúûüýĂăĐĞğĨĩİıŞşƠơƯửẠạẢảẤấẦầẨẩẪẫẬậẮắẰằẲẳẴẵẶặẸẹẺẻẼẽẾếỀềỂểỄễỆệỈỉỊịỌọỎỏỐốỒồỔổỖỗỘộỚớỜờỞởỠỡỢợỤụỦủỨứỪừỬửỮữỰựỲỳ​…₣₫€₱₲₵₹₺₼₽";
            var st2 = "$€£¥₣₹د.كد.إ﷼‎₻₽₾₺₼₸₴₷฿원₫₮₯₱₳₵₲₪₰";
            var temp = new string(st2.Where(c => !st1.Contains(c)).ToArray());
            Debug.Log($"temp={temp}");
        }
        [ContextMenu("LogUniqueCharacters")]
        public void LogUniqueCharacters()
        {
            var textIds = TextReferenceID.Values;
            var languages = Enum.GetNames(typeof(LanguageType));
            var builder = new StringBuilder();
            var num = "\n 0123456789";
            var result = string.Empty;
            var commonTexts = string.Empty;
            char[] english = default;
            languages.For(languageName =>
            {
                var languageType = languageName.ToEnum<LanguageType>();
                if (languageType != LanguageType.Limit)
                {
                    textIds.For(textId =>
                    {
                        var text = _textDatas.GetText(languageType, textId);
                        builder.Append(text);
                        if (!text.IsNullOrEmptySafe())
                        {
                            builder.Append(text.ToUpper());
                        }
                    });
                    if (languageType == LanguageType.English)
                    {
                        english = builder.ToString().ToArray();
                        builder.Append(num);
                        var uniques = builder.ToString().Distinct().ToArray();
                        builder.Clear();
                        uniques.For(@char => builder.Append(@char));
                        english = builder.ToString().ToArray();
                        result += $"\n\n{languageName}: {builder}";
                    }
                    else if (IsCommon(languageType))
                    {
                        var temp = builder.ToString();
                        temp = new string(temp.Where(c => !english.Contains(c)).ToArray());
                        var uniques = temp.Distinct().ToArray();
                        builder.Clear();
                        uniques.For(@char => builder.Append(@char));
                        commonTexts += builder.ToString();
                    }
                    else if (languageType == LanguageType.Korean)
                    {
                        builder.Append("원");
                        var temp = builder.ToString();
                        temp = new string(temp.Where(c => !english.Contains(c)).ToArray());
                        var uniques = temp.Distinct().ToArray();
                        builder.Clear();
                        uniques.For(@char => builder.Append(@char));
                        result += $"\n\n{languageName}: {builder}";
                    }
                    else if (languageType == LanguageType.Chinese || languageType == LanguageType.Japanese)
                    {
                        builder.Append("¥");
                        var temp = builder.ToString();
                        temp = new string(temp.Where(c => !english.Contains(c)).ToArray());
                        var uniques = temp.Distinct().ToArray();
                        builder.Clear();
                        uniques.For(@char => builder.Append(@char));
                        result += $"\n\n{languageName}: {builder}";
                    }
                    else
                    {
                        var temp = builder.ToString();
                        temp = new string(temp.Where(c => !english.Contains(c)).ToArray());
                        var uniques = temp.Distinct().ToArray();
                        builder.Clear();
                        uniques.For(@char => builder.Append(@char));
                        result += $"\n\n{languageName}: {builder}";
                    }
                    builder.Clear();
                }
            });
            var uniques = commonTexts.Distinct().ToArray();
            builder.Clear();
            uniques.For(@char => builder.Append(@char));
            commonTexts = builder.ToString();
            commonTexts = new string(commonTexts.Where(c => !english.Contains(c)).ToArray());
            result += $"\n\nMainFont: {commonTexts}";
            //Gametamin.FileHelper.SaveText(result, "Editor Default Resources/unique_chars.txt");
            bool IsCommon(LanguageType languageType)
                => languageType == LanguageType.Portuguese
                || languageType == LanguageType.French
                || languageType == LanguageType.Spanish
                || languageType == LanguageType.Vietnamese
                || languageType == LanguageType.Italian
                || languageType == LanguageType.Indonesian
                || languageType == LanguageType.Turkish
                || languageType == LanguageType.German;
        }
        public static void SetText(string id, string text = null)
        {
            Instance.SetTextInternal(id, text);
        }
        void SetTextInternal(string id, string text = null)
        {
            _textDatas.SetText(id, text);
            EditorUtility.SetDirty(this);
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
        public void ExportToCSV()
        {
            //Gametamin.FileHelper.WriteFile("Editor Default Resources/AnimalFarm/Resources/CSVs/localization_csv.csv", writter =>
            //{
            //    writter.WriteLine("TextID,Value");
            //    _textDatas.GetTexts(LanguageType.English, (id, value) =>
            //    {
            //        writter.WriteLine($"{id},{value.Replace("\n", "\\n")}");
            //    });
            //});
        }
        public void ImportFromCSV()
        {
            var languages = Enum.GetNames(typeof(LanguageType));
            languages.For(languageName =>
            {
                var language = languageName.ToEnum<LanguageType>();
                var languageData = GetLanguageData(language);
                if (languageData != null)
                {
                    languageData.Clear();
                }
                _textDatas.Reset(language);
            });
            string Key = "TextID";
            this.LoadCSV3("localization_csv", rawDatas =>
            {
                for (int i = 0; i < rawDatas.Count; i++)
                {
                    var rawData = rawDatas[i];
                    if (rawData.ContainsKey(Key))
                    {
                        languages.For(languageName =>
                        {
                            if (rawData.ContainsKey(languageName))
                            {
                                LanguageType language = languageName.ToEnum<LanguageType>();
                                var textID = rawData[Key].ToString().Trim();
                                var value = rawData[languageName].ToString().Trim().Replace("\"\"", "\"");
                                var languageData = GetLanguageData(language);
                                if (languageData != null)
                                {
                                    languageData.SetText(textID, value);
                                    EditorUtility.SetDirty(languageData);
                                }
                                _textDatas.SetText(textID, value, language);
                                if (language == LanguageType.English)
                                {
                                    TextNameReferenceFactory.Instance.AddName(textID);
                                }
                            }
                        });
                    }
                }
            });
        }
    }
}
#endif