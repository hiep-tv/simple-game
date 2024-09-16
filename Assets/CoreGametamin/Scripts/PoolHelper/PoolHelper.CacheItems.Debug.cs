#if DEBUG_MODE||UNITY_EDITOR
#define CACHE_ITEM_TRACKING
#endif
using System.Diagnostics;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        [Conditional("CACHE_ITEM_TRACKING")]
        public static void AddTracker(this Component target, ICacher cacher)
        {
            target.gameObject.AddTracker(cacher);
        }
        [Conditional("CACHE_ITEM_TRACKING")]
        public static void AddTracker(this GameObject target, ICacher cacher)
        {
#if CACHE_ITEM_TRACKING
            var debugger = target.GetOrAddComponentSafe<CacheItemDebugger>();
            debugger.AddItem(cacher.ItemObject, cacher.ItemId);
#endif
        }
        [Conditional("CACHE_ITEM_TRACKING")]
        public static void ClearTracker(this Component target)
        {
            target.gameObject.ClearTracker();
        }
        [Conditional("CACHE_ITEM_TRACKING")]
        public static void ClearTracker(this GameObject target)
        {
#if CACHE_ITEM_TRACKING
            var debugger = target.GetComponentSafe<CacheItemDebugger>();
            if (!debugger.IsNullSafe())
            {
                debugger.Clear();
            }
#endif
        }
        [Conditional("CACHE_ITEM_TRACKING")]
        public static void ClearTracker(this Component target, ICacher cacher)
        {
            ClearTracker(target.gameObject, cacher);
        }
        [Conditional("CACHE_ITEM_TRACKING")]
        public static void ClearTracker(this GameObject target, ICacher cacher)
        {
#if CACHE_ITEM_TRACKING
            var debugger = target.GetComponentSafe<CacheItemDebugger>();
            if (!debugger.IsNullSafe())
            {
                debugger.Clear(cacher.ItemObject);
            }
#endif
        }
    }
}
