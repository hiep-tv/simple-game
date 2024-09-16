#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    [ExecuteInEditMode]
    public abstract partial class BaseTextComponent
    {
        [TextReferenceID]
        [SerializeField] string _textID = TextReferenceID.Empty;
        public string TextID { get => _textID; set => _textID = value; }
        string _previous = TextReferenceID.Empty;
        TextStyle _previousStyle = TextStyle.Non;
        protected virtual void Start()
        {
            if (!Application.isPlaying)
            {
                if (_textID != TextReferenceID.Empty)
                {
                    LoadText(GetText(_textID));
                }
                else if (_textStyle != TextStyle.Non)
                {
                    LoadText(Text);
                }
            }
        }
        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (_textID != _previous)
                {
                    _previous = _textID;
                    LoadText(GetText(_textID));
                }
                else if (_previousStyle != _textStyle)
                {
                    _previousStyle = _textStyle;
                    if (_textStyle == TextStyle.Non)
                    {
                        LoadText(GetText(_textID));
                    }
                    else
                    {
                        LoadText(Text);
                    }
                }
            }
        }
        protected string GetText(string textId)
        {
            //LoadText(Gametamin.Core.Localization.LocalizationFactory.GetText(_textID));
            return textId;
        }
        protected virtual void LoadText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                OnSetText(text);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
            }
        }
    }
}
#endif