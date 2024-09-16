using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        /// <summary>
        /// using 
        /// </summary>
        static Dictionary<int, List<IntValueCacher>> _cacheIntValues;
        static Dictionary<int, List<IntValueCacher>> _CacheIntValues => _cacheIntValues ??= new();
        public static void CacheIntValue(this Component parent, IntValueCacher childData)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            CacheIntValue(instanceID, childData);
        }
        public static IntValueCacher CacheIntValue(this Component parent, int value, int valueId)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return CacheIntValue(instanceID, value, valueId);
        }
        public static void CacheIntValue(this GameObject parent, IntValueCacher childData)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            CacheIntValue(instanceID, childData);
        }
        public static IntValueCacher CacheIntValue(this GameObject parent, int value, int valueId)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return CacheIntValue(instanceID, value, valueId);
        }
        public static IntValueCacher CacheIntValue(this int instanceID, int value, int valueId)
        {
            var icache = GetCacheIntValue(instanceID, valueId);
            if (icache == null)
            {
                icache = new IntValueCacher(value, valueId);
                CacheIntValue(instanceID, icache);
            }
            else
            {
                icache.Value = value;
            }
            return icache;
        }
        public static void CacheIntValue(this int instanceID, IntValueCacher childData)
        {
            if (_CacheIntValues.SafeContainsKey(instanceID))
            {
                var items = _CacheIntValues[instanceID];
                var exist = false;
                items.ForBreakable(item =>
                {
                    exist = item.Id == childData.Id;
                    if (exist)
                    {
                        item.Value = childData.Value;
                    }
                    return exist;
                });
                if (!exist)
                {
                    items.AddSafe(childData);
                }
            }
            else
            {
                var newList = new List<IntValueCacher>()
                {
                    childData
                };
                _CacheIntValues.SafeAdd(instanceID, newList);
            }
        }
        public static IntValueCacher GetCacheIntValue(this Component parent, int id, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return GetCacheIntValue(instanceID, id, force);
        }
        public static IntValueCacher GetCacheIntValue(this GameObject parent, int id, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return GetCacheIntValue(instanceID, id, force);
        }
        static IntValueCacher GetCacheIntValue(this int instanceID, int id, bool force = false)
        {
            IntValueCacher result = default;
            var items = instanceID.GetCacheIntValues(force);
            items.ForBreakable(item =>
            {
                var exist = item.Id == id;
                if (exist)
                {
                    result = item;
                }
                return exist;
            });
            return result;
        }
        public static List<IntValueCacher> GetCacheIntValues(this Component @ref, bool force = false)
        {
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            return instanceID.GetCacheIntValues(force);
        }
        public static List<IntValueCacher> GetCacheIntValues(this GameObject parent, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return instanceID.GetCacheIntValues(force);
        }
        public static List<IntValueCacher> GetCacheIntValues(this int instanceID, bool force = false)
        {
            if (_CacheIntValues.SafeContainsKey(instanceID))
            {
                var items = _CacheIntValues[instanceID];
                return items;
            }
            else if (force)
            {
                var newList = new List<IntValueCacher>();
                _CacheIntValues.SafeAdd(instanceID, newList);
                return newList;
            }
            return default;
        }
        public static void ClearCacheIntValues(this Component @ref, Action<IntValueCacher> callback = null, bool complete = true)
        {
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            instanceID.ClearCacheIntValues(callback, complete);
        }
        public static void ClearCacheIntValues(this GameObject parent, Action<IntValueCacher> callback = null, bool complete = true)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            instanceID.ClearCacheIntValues(callback, complete);
        }
        public static void ClearCacheIntValues(this int instanceID, Action<IntValueCacher> callback = null, bool complete = true)
        {
            if (_CacheIntValues.SafeContainsKey(instanceID))
            {
                var items = _CacheIntValues[instanceID];
                items.For(item => callback?.Invoke(item));
                items.SafeClear();
                if (complete)
                {
                    _CacheIntValues.Remove(instanceID);
                }
            }
        }
        public class IntValueCacher : GenericValueCacher<int>
        {
            public IntValueCacher(int value, int id)
            {
                Value = value;
                Id = id;
            }
        }
    }
}
