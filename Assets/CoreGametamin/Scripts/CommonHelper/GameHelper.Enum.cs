using System;
namespace Gametamin.Core
{
    public static partial class EnumHelper
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            //Assert.IsInArray(Enum.GetNames(typeof(T)), value, $"{typeof(T).Name} not contain {value}");
            if (Enum.TryParse(value, out T result))
            {
                return result;
            }
            else
            {
                var values = Enum.GetValues(typeof(T));
                //Assert.IsPositive(values.Length, "Enum empty");
                return (T)values.GetValue(0);
            }
        }
        public static bool ToEnum<T>(this string value, out T result, bool assert = true) where T : struct
        {
            if (assert)
            {
                //Assert.IsInArray(Enum.GetNames(typeof(T)), value, $"{typeof(T).Name} not contain {value}");
            }
            if (Enum.TryParse(value, out result))
            {
                return true;
            }
            else
            {
                var values = Enum.GetValues(typeof(T));
                //Assert.IsPositive(values.Length, "Enum empty");
                return false;
            }
        }
        public static bool ToEnum<T>(this string value, bool ignoreCase, out T result) where T : struct
        {
            //Assert.IsInArray(Enum.GetNames(typeof(T)), value.ToLower(), (item) =>
            //{
            //    return value.ToLower().SafeEquals(item.ToLower());
            //}, $"{typeof(T).Name} not contain {value}");
            if (Enum.TryParse(value, ignoreCase, out result))
            {
                return true;
            }
            else
            {
                var values = Enum.GetValues(typeof(T));
                //Assert.IsPositive(values.Length, "Enum empty");
                return false;
            }
        }
    }
}
