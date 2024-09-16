using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Gametamin.Core.Localization
{
    public static partial class LocalizationHelper
    {
        //static List<TMP_Dropdown.OptionData> _options;
        //static List<TMP_Dropdown.OptionData> GetLanguages()
        //{
        //    if (_options == null)
        //    {
        //        _options = new();
        //        var limit = (int)LanguageType.Limit;
        //        for (int i = 0; i < limit; i++)
        //        {
        //            var language = (LanguageType)i;
        //            _options.Add(new TMP_Dropdown.OptionData(language.ToLanguage()));
        //        }
        //    }
        //    return _options;
        //}
        static List<Dropdown.OptionData> _options;
        static List<Dropdown.OptionData> GetLanguages()
        {
            if (_options == null)
            {
                _options = new();
                var limit = (int)LanguageType.Limit;
                for (int i = 0; i < limit; i++)
                {
                    var language = (LanguageType)i;
                    _options.Add(new Dropdown.OptionData(language.ToLanguage().ToUpper()));
                }
            }
            return _options;
        }
        public static void ShowLanguageSelector(this GameObjectReference iref, Action onReload)
        {
            //iref.SetTextReference(TextReferenceID.LanguageLabel.GetText(), GameObjectReferenceID.Label);
            //var under = iref.GetComponentReference<Image>(Gametamin.Core.GameObjectReferenceID.Image);
            //under.SetImageAlpha(0f);
            CustomDropdownUGUI dropdown = default;
            var currentIndex = (int)CurrentLanguage;
            var datas = GetLanguages();
            dropdown = iref.SetLanguage(datas, currentIndex, value =>
            {
                if (currentIndex != value)
                {
                    var newLanguage = (LanguageType)value;
                    UserInput.Enabled = false;
                    //Helper.ShowWaiting(TextReferenceID.Waiting.GetText(), () =>
                    //{
                    newLanguage.LoadFontAsset(result =>
                    {
                        UserInput.Enabled = true;
                        //Helper.HideWaiting(() =>
                        //{
                        if (result)
                        {
                            CurrentLanguage = newLanguage;
                            onReload?.Invoke();
                        }
                        else
                        {
                            dropdown.value = currentIndex;
                        }
                        //});
                    });
                    //});
                }
            });
            dropdown.OnShow = () => OnActiveUnderBg(true);
            dropdown.OnHide = () => OnActiveUnderBg(false);
            void OnActiveUnderBg(bool active)
            {
                //under.DOKill();
                //under.DOFade(active ? 1f : 0f, 0.18f);
            }
        }
        //static CustomDropdown SetLanguage(this IGetReference ilanguageRef, List<TMP_Dropdown.OptionData> optionDatas, int currentSelected, Action<int> onSelected = null)
        //{
        //    var dropdown = ilanguageRef.GetComponentReference<CustomDropdown>(Gametamin.Core.GameObjectReferenceID.Dropdown);
        //    dropdown.options = optionDatas;
        //    dropdown.value = currentSelected;
        //    dropdown.onValueChanged.AddListener(value => onSelected?.Invoke(value));
        //    return dropdown;
        //}
        static CustomDropdownUGUI SetLanguage(this GameObjectReference ilanguageRef, List<Dropdown.OptionData> optionDatas, int currentSelected, Action<int> onSelected = null)
        {
            var dropdown = ilanguageRef.GetComponentReference<CustomDropdownUGUI>(GameObjectReferenceID.Dropdown);
            dropdown.options = optionDatas;
            dropdown.value = currentSelected;
            dropdown.onValueChanged.AddListener(value => onSelected?.Invoke(value));
            return dropdown;
        }
    }
}
