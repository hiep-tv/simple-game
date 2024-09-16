using System;
using System.Globalization;
namespace Gametamin.Core
{
    public static partial class GameHelper
    {
        public static bool ToBoolSafe(this string s, bool defaultValue = false)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Length == 1)
                {
                    return s[0] == '0' ? false : true;
                }

                if (bool.TryParse(s, out bool value))
                {
                    return value;
                }
            }
            //Assert.Bug($"cannot parse {s} to bool!");
            return defaultValue;
        }

        public static int ToIntSafe(this string s, int defaultValue = 0)
        {
            if (int.TryParse(s, out int value))
            {
                return value;
            }
            //Assert.Bug($"cannot parse {s} to int!");
            return defaultValue;
        }

        public static long ToLongSafe(this string s, long defaultValue = 0)
        {
            if (long.TryParse(s, out long value))
            {
                return value;
            }
            //Assert.Bug($"cannot parse {s} to long!");
            return defaultValue;
        }

        public static float ToFloatSafe(this string s, float defaultValue = 0)
        {
            if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }
            //Assert.Bug($"cannot parse {s} to float!");
            return defaultValue;
        }

        /// <summary>
        /// From string of date time's binary.
        /// </summary>
        public static DateTime? ToDateTimeSafe(this string s)
        {
            if (long.TryParse(s, out long dateData))
            {
                if (dateData != 0)
                {
                    return DateTime.FromBinary(dateData);
                }
            }
            //Assert.Bug($"cannot parse {s} to DateTime!");
            return null;
        }
    }
}
