using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        interface IPoolData
        {
            public GameObject PoolObject { get; }
            GameObject GetInstance(Transform parent = null, bool spaw = true);
            void ClearPool();
            void ReleasePool(Action<GameObject> callback = null);
            void GetAllObject(Action<GameObject> callback = null);
            GameObject GetObject(int index);
        }
        class PoolData : IPoolData
        {
            public GameObject poolObject;
            public GameObject instanceObject;
            public GameObject PoolObject => poolObject;
            public PoolData(GameObject poolObject, GameObject instanceObject = null)
            {
                this.poolObject = poolObject;
                this.instanceObject = instanceObject;
            }
            public GameObject GetInstance(Transform parent = null, bool spaw = true)
            {
                if (!instanceObject.IsNullSafe())
                {
                    instanceObject.SetActiveSafe(true);
                    return instanceObject;
                }
                if (spaw)
                {
                    instanceObject = poolObject.PoolItem(parent);
                    return instanceObject;
                }
                return default;
            }
            public void ClearPool()
            {
                instanceObject = null;
            }
            public void ReleasePool(Action<GameObject> callback = null)
            {
                instanceObject.SetActiveSafe(false);
                callback?.Invoke(instanceObject);
            }
            public void GetAllObject(Action<GameObject> callback = null)
            {
                callback?.Invoke(instanceObject);
            }
            public GameObject GetObject(int index)
            {
                return instanceObject;
            }
        }
        class PoolDataMultiple : IPoolData
        {
            public GameObject poolObject;
            List<GameObject> _instanceObjects;
            public List<GameObject> instanceObjects => _instanceObjects ??= new();
            public GameObject PoolObject => poolObject;
            public PoolDataMultiple(GameObject poolObject)
            {
                this.poolObject = poolObject;
            }
            public GameObject GetInstance(Transform parent = null, bool spaw = true)
            {
                return instanceObjects.GetFreeGameObject(() => { return poolObject; }, parent);
            }
            public void ReleasePool(Action<GameObject> callback = null)
            {
                _instanceObjects.HideAllGameObjects(callback);
            }
            public void ClearPool()
            {
                _instanceObjects.SafeClear();
            }
            public void GetAllObject(Action<GameObject> callback = null)
            {
                _instanceObjects.For(callback);
            }

            public GameObject GetObject(int index)
            {
                return _instanceObjects.GetSafe(index);
            }
        }
    }
}
