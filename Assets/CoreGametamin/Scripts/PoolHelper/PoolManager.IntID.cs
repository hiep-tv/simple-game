using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        public static GameObject GetGameObject(this GameObject poolObject, Transform parent)
        {
            var id = poolObject.GetInstanceID();
            return PoolIntManager.GetGameObject(id, poolObject, parent);
        }
        /// <summary>
        /// use InstanceID of poolObject as key
        /// </summary>
        public static GameObject GetGameObjectInGroup(this GameObjectReference @ref, string parentId, string poolId)
        {
            var poolObject = @ref.GetPoolReference(poolId);
            var parent = @ref.GetTransformReference(parentId);
            if (!poolObject.IsNullSafe())
            {
                var id = poolObject.GetInstanceID();
                return PoolIntManager.GetGameObjectInGroup(id, poolObject, parent);
            }
            return default;
        }
        /// <summary>
        /// use InstanceID of poolObject as key
        /// </summary>
        public static GameObject GetGameObjectInGroup(this GameObject poolObject, Transform parent)
        {
            if (!poolObject.IsNullSafe())
            {
                var id = poolObject.GetInstanceID();
                return PoolIntManager.GetGameObjectInGroup(id, poolObject, parent);
            }
            return default;
        }
        public static GameObject GetGameObjectInGroup(this GameObject poolObject, int id, Transform parent)
        {
            return PoolIntManager.GetGameObjectInGroup(id, poolObject, parent);
        }
        public static void ReleasePool(this GameObjectReference @ref, string poolId, Action<GameObject> callback = null)
        {
            if (!@ref.IsNullSafe())
            {
                var poolObject = @ref.GetPoolReference(poolId);
                poolObject.ReleasePool(callback);
            }
        }
        /// <summary>
        /// use InstanceID of poolObject as key
        /// </summary>
        public static void ReleasePool(this GameObject poolObject, Action<GameObject> callback = null)
        {
            if (!poolObject.IsNullSafe())
            {
                var id = poolObject.GetInstanceID();
                PoolIntManager.ReleasePool(id, callback);
            }
        }
        public static void ReleasePool(this int id, Action<GameObject> callback = null)
        {
            PoolIntManager.ReleasePool(id, callback);
        }
        partial class PoolIntManager : BasePoolManager<int>
        {
        }
    }
}
