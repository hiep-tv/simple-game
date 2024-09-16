using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        public static float GetFloat(this IValueCacher cacher, float defaultValue = 0f)
        {
            return cacher != null ? (float)cacher.Value : defaultValue;
        }
        public static int GetInt(this IValueCacher cacher, int defaultValue = 0)
        {
            return cacher != null ? (int)cacher.Value : defaultValue;
        }
        public static bool GetBool(this IValueCacher cacher, bool defaultValue = false)
        {
            return cacher != null ? (bool)cacher.Value : defaultValue;
        }
        public static string GetString(this IValueCacher cacher, string defaultValue = "")
        {
            return cacher != null ? (string)cacher.Value : defaultValue;
        }
        public static Action GetAction(this IValueCacher cacher, Action defaultValue = null)
        {
            return cacher != null ? (Action)cacher.Value : defaultValue;
        }
    }
}
