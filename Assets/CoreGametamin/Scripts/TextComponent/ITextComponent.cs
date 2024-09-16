namespace Gametamin.Core
{
    public interface ITextComponent
    {
        string Text
        {
            get;
            set;
        }
        void OnSetText(object value);
        void OnSetText(string value);
        void OnSetText(string value, TextStyle style);
        void OnSetText(object value, TextStyle style);
        void OnSetTextStyle(TextStyle style);
    }
}