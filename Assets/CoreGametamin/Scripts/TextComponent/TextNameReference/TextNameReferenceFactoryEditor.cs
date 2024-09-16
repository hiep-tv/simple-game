#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    [CustomEditor(typeof(TextNameReferenceFactory), true)]
    public class TextReferenceNameFactoryEditor : ReferenceNameFactoryEditor<TextNameReferenceFactory>
    {
        protected override ReferenceNameFactory<TextNameReferenceFactory> InspectedObject => TextNameReferenceFactory.Instance;
        string _textID;
        string _textLanguage;
        bool _clicked;
        //protected override void OnCustomGUI()
        //{
        //    EditorGUIHelper.HorizontalLayout(() =>
        //    {
        //        EditorGUIHelper.GUILabel("Add Text Data");
        //        //_clicked = Helper.GUIStringWithSearch(_textID, _clicked, result => _textID = result);
        //        EditorGUIHelper.GUITextArea(_textLanguage, value => _textLanguage = value);
        //        Event e = Event.current;
        //        if (e.type == EventType.KeyUp && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        //        {
        //            AddData();
        //            Repaint();
        //        }
        //        EditorGUIHelper.GUIButton("Add", AddData);
        //        void AddData()
        //        {
        //            if (_textID != TextReferenceID.Non && !_textLanguage.IsNullOrEmptySafe())
        //            {
        //                //LocalizationFactory.SetText(_textID, _textLanguage);
        //                _textID = TextReferenceID.Non;
        //                _textLanguage = null;
        //            }
        //        }
        //        GUILayout.FlexibleSpace();
        //    });
        //    base.OnCustomGUI();
        //}
    }
}
#endif
