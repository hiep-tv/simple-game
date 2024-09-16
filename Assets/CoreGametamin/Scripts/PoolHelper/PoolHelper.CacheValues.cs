#if DEBUG_MODE||UNITY_EDITOR
#define CACHE_VALUE_TRACKING
#endif
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
        static Dictionary<int, List<IValueCacher>> _cacheValues;
        static Dictionary<int, List<IValueCacher>> _CacheValues => _cacheValues ??= new();
        public static void CacheValue(this Component parent, IValueCacher childData)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            CacheValue(instanceID, childData);
        }
        public static IValueCacher CacheValue(this Component parent, object value, int valueId)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return CacheValue(instanceID, value, valueId);
        }
        public static void CacheValue(this GameObject parent, IValueCacher childData)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            CacheValue(instanceID, childData);
        }
        public static IValueCacher CacheValue(this GameObject parent, object value, int valueId)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return CacheValue(instanceID, value, valueId);
        }
        public static IValueCacher CacheValue(this int instanceID, object value, int valueId)
        {
            var icache = GetCacheValue(instanceID, valueId);
            if (icache == null)
            {
                icache = new CacheValueData(value, valueId);
                CacheValue(instanceID, icache);
            }
            else
            {
                icache.Value = value;
            }
            return icache;
        }
        public static void CacheValue(this int instanceID, IValueCacher childData)
        {
            if (_CacheValues.SafeContainsKey(instanceID))
            {
                var items = _CacheValues[instanceID];
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
                var newList = new List<IValueCacher>()
                {
                    childData
                };
                _CacheValues.SafeAdd(instanceID, newList);
            }
        }
        public static IValueCacher GetCacheValue(this Component parent, int id, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return GetCacheValue(instanceID, id, force);
        }
        public static IValueCacher GetCacheValue(this GameObject parent, int id, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return GetCacheValue(instanceID, id, force);
        }
        static IValueCacher GetCacheValue(this int instanceID, int id, bool force = false)
        {
            IValueCacher result = default;
            var items = instanceID.GetCacheValues(force);
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
        public static List<IValueCacher> GetCacheValues(this Component @ref, bool force = false)
        {
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            return instanceID.GetCacheValues(force);
        }
        public static List<IValueCacher> GetCacheValues(this GameObject parent, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return instanceID.GetCacheValues(force);
        }
        public static List<IValueCacher> GetCacheValues(this int instanceID, bool force = false)
        {
            if (_CacheValues.SafeContainsKey(instanceID))
            {
                var items = _CacheValues[instanceID];
                return items;
            }
            else if (force)
            {
                var newList = new List<IValueCacher>();
                _CacheValues.SafeAdd(instanceID, newList);
                return newList;
            }
            return default;
        }
        public static void ClearCacheValues(this Component @ref, Action<IValueCacher> callback = null, bool complete = true)
        {
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            instanceID.ClearCacheValues(callback, complete);
        }
        public static void ClearCacheValues(this GameObject parent, Action<IValueCacher> callback = null, bool complete = true)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            instanceID.ClearCacheValues(callback, complete);
        }
        public static void ClearCacheValues(this int instanceID, Action<IValueCacher> callback = null, bool complete = true)
        {
            if (_CacheValues.SafeContainsKey(instanceID))
            {
                var items = _CacheValues[instanceID];
                items.For(item => callback?.Invoke(item));
                items.SafeClear();
                if (complete)
                {
                    _CacheValues.Remove(instanceID);
                }
            }
        }
        public class CacheValueData : IValueCacher
        {
            object _value;
            int _itemId;
            public object Value
            {
                get => _value;
                set => _value = value;
            }
            public int Id
            {
                get => _itemId;
                set => _itemId = value;
            }
            public CacheValueData(object value, int itemId)
            {
                _value = value;
                _itemId = itemId;
            }
        }
        public class GenericValueCacher<T> : IValueCacher<T>
        {
            public T Value { get; set; }
            public int Id { get; set; }
#if CACHE_VALUE_TRACKING
            public string Name { get; set; }
#endif
        }
        public interface IValueCacher
        {
            public object Value { get; set; }
            public int Id { get; set; }
        }
        public interface IValueCacher<T>
        {
            public T Value { get; set; }
            public int Id { get; set; }
        }
    }
}
