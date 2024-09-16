using UnityEngine;
namespace Gametamin.Core.Localization
{
    public static partial class LocalizationHelper
    {
        static bool _initialized;
        public static readonly LanguageType DefaultLanguage = LanguageType.English;
        static readonly string _OldKey = "ga";
        static readonly string _Key = "cl01";
        static LanguageType _currentLanguage = DefaultLanguage;
        public static LanguageType CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    PlayerPrefs.SetInt(_Key, (int)value);
                }
            }
        }
        public static void Init()
        {
            if (_initialized) return;
            _initialized = true;
            if (PlayerPrefs.HasKey(_Key))
            {
                _currentLanguage = (LanguageType)PlayerPrefs.GetInt(_Key, (int)DefaultLanguage);
            }
            else if (PlayerPrefs.HasKey(_OldKey))
            {
                _currentLanguage = (LanguageType)PlayerPrefs.GetInt(_OldKey, (int)DefaultLanguage);
            }
            else
            {
                _currentLanguage = Application.systemLanguage.ToLanguageType();
                PlayerPrefs.SetInt(_Key, (int)_currentLanguage);
            }
        }
        public static LanguageType ToLanguageType(this SystemLanguage systemLanguage)
        {
            if (systemLanguage == SystemLanguage.English) return LanguageType.English;
            if (systemLanguage == SystemLanguage.German) return LanguageType.German;
            if (systemLanguage == SystemLanguage.French) return LanguageType.French;
            if (systemLanguage == SystemLanguage.Russian) return LanguageType.Russian;
            if (systemLanguage == SystemLanguage.Japanese) return LanguageType.Japanese;
            if (systemLanguage == SystemLanguage.Korean) return LanguageType.Korean;
            if (systemLanguage == SystemLanguage.Chinese) return LanguageType.Chinese;
            if (systemLanguage == SystemLanguage.Spanish) return LanguageType.Spanish;
            if (systemLanguage == SystemLanguage.Portuguese) return LanguageType.Portuguese;
            if (systemLanguage == SystemLanguage.Italian) return LanguageType.Italian;
            //if (systemLanguage == SystemLanguage.Arabic) return LanguageType.Arabic;
            if (systemLanguage == SystemLanguage.Indonesian) return LanguageType.Indonesian;
            //if (systemLanguage == SystemLanguage.Thai) return LanguageType.Thai;
            if (systemLanguage == SystemLanguage.Turkish) return LanguageType.Turkish;
            if (systemLanguage == SystemLanguage.Vietnamese) return LanguageType.Vietnamese;

            return DefaultLanguage;
        }
        public static string ToLanguage(this LanguageType languageType)
        => languageType switch
        {
            LanguageType.English => "English",
            LanguageType.German => "Deutsch",
            LanguageType.French => "Français",
            LanguageType.Russian => "Русский",
            LanguageType.Japanese => "日本語",
            LanguageType.Korean => "한국인",
            LanguageType.Chinese => "简体中文",
            LanguageType.Spanish => "Español",
            LanguageType.Portuguese => "Português",
            LanguageType.Hindi => "हिंदी",
            LanguageType.Italian => "Italiano",
            LanguageType.Arabic => "اللغة العربية",
            LanguageType.Indonesian => "Bahasa Indonesia",
            LanguageType.Thai => "แบบไทย",
            LanguageType.Turkish => "Türkçe",
            LanguageType.Vietnamese => "Tiếng Việt",
            _ => "English",
        };
    }
}
