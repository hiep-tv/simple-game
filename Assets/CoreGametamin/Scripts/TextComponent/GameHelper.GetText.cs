#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using Gametamin.Core.Localization;
namespace Gametamin.Core
{
    public static partial class TextHelper
    {
        public static string GetTextFormatById(this string textId, params object[] @param)
        {
            var text = GetTextById(textId);
            return string.Format(text, @param);
        }
        public static string GetTextById(this string textId)
        {
            return LocalizationHelper.GetText(textId);
        }
    }
}
