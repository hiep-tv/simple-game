#if DEBUG_MODE||UNITY_EDITOR
#define CACHE_VALUE_TRACKING
#endif

using System.Diagnostics;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        [Conditional("CACHE_VALUE_TRACKING")]
        public static void SetName<T>(this GenericValueCacher<T> valueCacher, string name)
        {
#if CACHE_VALUE_TRACKING
            valueCacher.Name = name;
#endif
        }
        [Conditional("CACHE_VALUE_TRACKING")]
        public static void Validate<T>(this GenericValueCacher<T> valueCacher, string name)
        {
#if CACHE_VALUE_TRACKING
            if (!valueCacher.Name.EqualsSafe(name, System.StringComparison.OrdinalIgnoreCase))
            {
                UnityEngine.Debug.LogError($"Invalid Value Cacher {name} => {valueCacher.Name}");
            }
#endif
        }
    }
}

