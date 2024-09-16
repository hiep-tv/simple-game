#if DEBUG_MODE||UNITY_EDITOR
//#define DEBUG_SET_TEXT_VALUE
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public enum TextStyle
    {
        Non = -1,
        UpperCase,
        LowerCase
    }
    [DisallowMultipleComponent]
    public abstract partial class BaseTextComponent : MonoBehaviour, ITextComponent
    {
        [SerializeField]
        TextStyle _textStyle = TextStyle.Non;
        public abstract string Text { get; set; }

        public void OnSetText(object value)
        {
            //Assert.IsNotNull(value, "set text value is null");
            if (value != default)
            {
                SetTextValueCheckStyle(value.ToString());
            }
        }
        public void OnSetText(string value)
        {
            SetTextValueCheckStyle(value);
        }
        public void OnSetText(string value, TextStyle style)
        {
            OnSetTextStyle(style);
            SetTextValueCheckStyle(value);
        }
        public void OnSetText(object value, TextStyle style)
        {
            OnSetTextStyle(style);
            OnSetText(value);
        }
        public void OnSetTextStyle(TextStyle style)
        {
            _textStyle = style;
        }
        void SetTextValueCheckStyle(string value)
        {
            switch (_textStyle)
            {
                case TextStyle.Non:
                    SetText(value);
                    break;
                case TextStyle.UpperCase:
                    SetText(value.ToUpper());
                    break;
                case TextStyle.LowerCase:
                    SetText(value.ToLower());
                    break;
            }
        }
        void SetText(string value)
        {
            var empty = string.IsNullOrEmpty(value);
            gameObject.SetActiveSafe(!empty);
            if (!empty)
            {
                Text = value;
            }
            else
            {
                Logger("Bug: Trying to set an empty string!");
            }
        }
        [System.Diagnostics.Conditional("DEBUG_SET_TEXT_VALUE")]
        protected void Logger(string log)
        {
            Debug.LogWarning(log);
        }
    }
}
