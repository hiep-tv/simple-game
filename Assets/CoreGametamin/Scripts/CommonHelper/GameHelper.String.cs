#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Gametamin.Core
{
    public static partial class StringHelper
    {
        const char _space = ' ';
        const char _EmptyChar = '\0';
        static StringBuilder _stringBuilder;
        static StringBuilder _StringBuilder => _stringBuilder ??= new StringBuilder();
        public static string DefaultValue => string.Empty;
        public static StringBuilder GetBuilder(this string value)
        {
            _StringBuilder.Clear();
            _StringBuilder.Append(value);
            return _StringBuilder;
        }
        public static string GetValueInBuilder(this StringBuilder builder)
        {
            var value = builder.ToString();
            builder.Clear();
            return value;
        }
        public static string ToUpperSafe(this string value)
        {
            if (!value.IsNullOrEmptySafe())
            {
                return value.ToUpper();
            }
            return DefaultValue;
        }
        public static string ToLowerSafe(this string value)
        {
            if (!value.IsNullOrEmptySafe())
            {
                return value.ToLower();
            }
            return DefaultValue;
        }
        public static string ToUpperChar(this string value, int index = 0)
        {
            string result = DefaultValue;
            StringBuilder(value, builder =>
            {
                _StringBuilder[0] = char.ToUpper(value[index]);
                result = _StringBuilder.ToString();
            });
            return result;
        }
        static void StringBuilder(string value, Action<StringBuilder> callback = null)
        {
            _StringBuilder.Append(value);
            callback?.Invoke(_StringBuilder);
            _StringBuilder.Clear();
        }
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string RemoveSpaceSafe(this string value)
        {
            if (!value.IsNullOrEmptySafe())
            {
                return sWhitespace.Replace(value.Trim(), string.Empty);
            }
            return value;
        }
        public static string ToCamelCase(this string newName)
        {
            if (!newName.IsNullOrEmptySafe())
            {
                var arr = newName.Trim().Split(_space);
                arr.For(item =>
                {
                    item = item.Trim();
                    if (!string.IsNullOrEmpty(item))
                    {
                        _StringBuilder.Append(char.ToUpperInvariant(item[0]) + item[1..]);
                    }
                });
                newName = _StringBuilder.ToString();
                _StringBuilder.Clear();
            }
            return newName;
        }
        public static string ToSnakeCase(this string newName)
        {
            if (!newName.IsNullOrEmptySafe())
            {
                newName = newName.ToLower().Replace(_space, '_');
            }
            return newName;
        }
        public static string UpperFirstCase(this string newName)
        {
            if (!newName.IsNullOrEmptySafe())
            {
                var arr = newName.ToCharArray();
                arr[0] = char.ToUpper(arr[0]);
                newName = new string(arr);
            }
            return newName;
        }
        public static char GetCharAt(this string text, int indexChar)
        {
            if (!text.IsNullOrEmptySafe())
            {
                if (indexChar >= 0 && indexChar <= text.Length)
                {
                    return text[indexChar];
                }
            }
            return _EmptyChar;
        }
        public static bool EqualsSafe(this string text1, string text2)
        {
            return !text1.IsNullOrEmptySafe() && text1.Equals(text2);
        }
        public static bool EqualsSafe(this string text1, string text2, System.StringComparison comparison)
        {
            return !text1.IsNullOrEmptySafe() && text1.Equals(text2, comparison);
        }
        public static bool ContainsSafe(this string text1, string text2)
        {
            return !text1.IsNullOrEmptySafe() && text1.Contains(text2);
        }
        public static bool ContainsSafe(this string text1, string text2, System.StringComparison comparison)
        {
            return !text1.IsNullOrEmptySafe() && text1.Contains(text2, comparison);
        }
        public static bool IsNullOrEmptySafe(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
        public static bool IsItemExist(this List<string> items, string newName, System.StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (items.GetCountSafe() <= 0) return false;
            var equals = false;
            items.ForBreakable(item =>
            {
                equals = item.EqualsSafe(newName, comparison);
                return equals;
            });
            return equals;
        }
        public static bool IsItemExist(this List<ReferenceNameData> items, string newName, System.StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (items.GetCountSafe() <= 0) return false;
            var equals = false;
            items.ForBreakable(item =>
            {
                equals = item.Name.EqualsSafe(newName, comparison);
                return equals;
            });
            return equals;
        }
    }
}
