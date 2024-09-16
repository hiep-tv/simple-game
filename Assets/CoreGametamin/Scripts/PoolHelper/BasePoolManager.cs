using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        partial class BasePoolManager<T> : IRelease
        {
            static BasePoolManager<T> _instance;
            public static BasePoolManager<T> Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = GetPoolManager<T>();
                    }
                    return _instance;
                }
            }
            Dictionary<T, IPoolData> _pools = new();
            public static bool Active => _instance != null;
            public static GameObject GetGameObject(T id)
            {
                return Instance.GetGameObjectInternal(id);
            }
            public static GameObject GetGameObject(T id, GameObject pool, Transform parent)
            {
                return Instance.GetGameObjectInternal(id, pool, parent);
            }
            public static void AddGameObject(T id, GameObject instance)
            {
                Instance.AddGameObjectInternal(id, instance);
            }
            public static GameObject GetGameObjectInGroup(T id, GameObject poolObject, Transform parent)
            {
                return Instance.GetGameObjectInGroupInternal(id, poolObject, parent);
            }
            public static GameObject GetPoolAtIndex(T id, int index)
            {
                return Instance.GetPoolAtIndexInternal(id, index);
            }
            public static void GetAllPool(T id, Action<GameObject> callback)
            {
                Instance.GetAllPoolInternal(id, callback);
            }
            public static void ReleasePool(T id, Action<GameObject> callback = null)
            {
                Instance.ReleasePoolInternal(id, callback);
            }
            protected GameObject GetGameObjectInternal(T id)
            {
                //Assert.IsInDictionary(_pools, id, $"No GameObject has id {id} found!");
                if (_pools.SafeContainsKey(id))
                {
                    var ipool = _pools[id].GetInstance();
                    if (ipool != null)
                    {
                        return ipool.gameObject;
                    }
                }
                return default;
            }
            protected GameObject GetGameObjectInternal(T id, GameObject pool, Transform parent)
            {
                //Assert.IsInDictionary(_pools, id, $"No GameObject has id {id} found!");
                if (_pools.SafeContainsKey(id))
                {
                    var ipool = _pools[id].GetInstance();
                    if (ipool != null)
                    {
                        return ipool.gameObject;
                    }
                    return default;
                }
                else
                {
                    return AddGameObjectInternal(id, pool.PoolItem(parent));
                }
            }
            protected GameObject AddGameObjectInternal(T id, GameObject instance)
            {
                //Assert.IsInDictionary(_pools, id);
                if (!_pools.SafeContainsKey(id))
                {
                    IPoolData newPool = new PoolData(null, instance);
                    _pools.SafeAdd(id, newPool);
                    return newPool.PoolObject;
                }
                else
                {
                    return _pools[id].PoolObject;
                }
            }
            protected GameObject GetGameObjectInGroupInternal(T id, GameObject poolObject, Transform parent)
            {
                if (_pools.SafeContainsKey(id))
                {
                    return _pools[id].GetInstance(parent);
                }
                else
                {
                    IPoolData newPool = new PoolDataMultiple(poolObject);
                    _pools.SafeAdd(id, newPool);
                    return newPool.GetInstance(parent);
                }
            }
            protected GameObject GetPoolAtIndexInternal(T id, int index)
            {
                if (_pools.SafeContainsKey(id))
                {
                    return _pools[id].GetObject(index);
                }
                return default;
            }
            protected void GetAllPoolInternal(T id, Action<GameObject> callback)
            {
                if (_pools.SafeContainsKey(id))
                {
                    _pools[id].GetAllObject(callback);
                }
            }
            protected void ReleasePoolInternal(T id, Action<GameObject> callback)
            {
                if (_pools.SafeContainsKey(id))
                {
                    var poolData = _pools[id];
                    poolData.ReleasePool(callback);
                }
            }
            public void OnRelease()
            {
                _pools.Clear();
            }
        }
    }
}
