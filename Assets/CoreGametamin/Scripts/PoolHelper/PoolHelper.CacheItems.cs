#if DEBUG_MODE||UNITY_EDITOR
#define CACHE_ITEM_TRACKING
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        static List<ICacher> _Cachers = new();
        public static List<ICacher> GetCachers()
        {
            _Cachers.Clear();
            return _Cachers;
        }
        /// <summary>
        /// using 
        /// </summary>
        static Dictionary<int, List<ICacher>> _cacheItems;
        static Dictionary<int, List<ICacher>> _CacheItems => _cacheItems ??= new();
        public static void CacheItem(this Component parent, ICacher childData)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            parent.AddTracker(childData);
            CacheItem(instanceID, childData);
        }
        public static void CacheItem(this Component parent, GameObject child, int childId)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            var data = CacheItem(instanceID, child, childId);
            parent.AddTracker(data);
        }
        public static void CacheItem(this GameObject parent, ICacher childData)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            CacheItem(instanceID, childData);
            parent.AddTracker(childData);
        }
        public static void CacheItem(this GameObject parent, GameObject child, int childId)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            var data = CacheItem(instanceID, child, childId);
            parent.AddTracker(data);
        }
        public static ICacher CacheItem(this int instanceID, GameObject child, int childId)
        {
            var data = new CacheItemData(child, childId);
            CacheItem(instanceID, data);
            return data;
        }
        public static void CacheItem(this int instanceID, ICacher childData)
        {
            if (_CacheItems.SafeContainsKey(instanceID))
            {
                var items = _CacheItems[instanceID];
                items.AddSafe(childData);
            }
            else
            {
                var newList = new List<ICacher>()
                {
                    childData
                };
                _CacheItems.SafeAdd(instanceID, newList);
            }
        }
        public static ICacher GetCacheItem(this Component parent, int id, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return GetCacheItem(instanceID, id, force);
        }
        public static ICacher GetCacheItem(this GameObject parent, int id, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return GetCacheItem(instanceID, id, force);
        }
        static ICacher GetCacheItem(this int instanceID, int id, bool force = false)
        {
            ICacher result = default;
            var items = instanceID.GetCacheItems(force);
            items.ForBreakable(item =>
            {
                var exist = item.ItemId == id;
                if (exist)
                {
                    result = item;
                }
                return exist;
            });
            return result;
        }
        public static List<ICacher> GetCacheItems(this Component @ref, bool force = false)
        {
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            return instanceID.GetCacheItems(force);
        }
        public static List<ICacher> GetCacheItems(this GameObject parent, bool force = false)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            return instanceID.GetCacheItems(force);
        }
        public static void GetCacheItems(this GameObject parent, Action<ICacher> callback)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            var caches = instanceID.GetCacheItems();
            caches.For(item => callback?.Invoke(item));
        }
        public static List<ICacher> GetCacheItems(this int instanceID, bool force = false)
        {
            if (_CacheItems.SafeContainsKey(instanceID))
            {
                var items = _CacheItems[instanceID];
                return items;
            }
            else if (force)
            {
                var newList = new List<ICacher>();
                _CacheItems.SafeAdd(instanceID, newList);
                return newList;
            }
            return default;
        }
        public static void ClearCacheItem(this Component @ref, int id, Action<ICacher> callback = null, bool complete = true)
        {
            ICacher result = default;
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            var items = instanceID.GetCacheItems();
            items.ForReverseBreakable((item, index) =>
            {
                var exist = item.ItemId == id;
                if (exist)
                {
                    result = item;
                    @ref.ClearTracker(item);
                    items.RemoveAt(index);
                }
                return exist;
            }, false);
            callback?.Invoke(result);
        }
        public static void ClearCacheItems(this Component @ref, Action<ICacher> callback = null, bool complete = true)
        {
            var instanceID = @ref.GetGameObjectInstanceIDSafe();
            instanceID.ClearCacheItems(callback, complete);
            @ref.ClearTracker();
        }
        public static void ClearCacheItems(this GameObject parent, Action<ICacher> callback = null, bool complete = true)
        {
            var instanceID = parent.GetGameObjectInstanceIDSafe();
            instanceID.ClearCacheItems(callback, complete);
            parent.ClearTracker();
        }
        public static void ClearCacheItems(this int instanceID, Action<ICacher> callback = null, bool complete = true)
        {
            if (_CacheItems.SafeContainsKey(instanceID))
            {
                var items = _CacheItems[instanceID];
                items.For(item => callback?.Invoke(item));
                items.SafeClear();
                if (complete)
                {
                    _CacheItems.Remove(instanceID);
                }
            }
        }
        public class CacheItemData : CacheItemData<int>, ICacher
        {
            public CacheItemData(GameObject itemObject, int itemId) : base(itemObject, itemId)
            {
            }

            public virtual bool IsMatchCondition
            {
                get;
            }
        }
        public class CacheItemData<T>
        {
            T _itemId;
            GameObject _itemObject;
            public T ItemId => _itemId;
            public GameObject ItemObject => _itemObject;
            public CacheItemData(GameObject itemObject, T itemId)
            {
                _itemId = itemId;
                _itemObject = itemObject;
            }
        }
        public interface ICacher
        {
            public GameObject ItemObject { get; }
            public int ItemId { get; }
            public bool IsMatchCondition { get; }
            public string ToString();
        }
    }
}
