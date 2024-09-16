using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        public static T GetOrAddComponentReference<T>(this GameObjectReference @ref, string refId) where T : Component
        {
            var result = @ref.GetGameObjectReference(refId);
            return result.GetOrAddComponentSafe<T>();
        }
        public static T GetOrAddComponentReference<T>(this GameObjectReference @ref, string refId, string parentId, bool forceSetParent = false)
        {
            var result = @ref.GetOrAddGameObjectReference(refId, parentId, forceSetParent);
            return result.GetComponentSafe<T>();
        }
        public static GameObject GetOrAddGameObjectReference(this GameObjectReference @ref, string refId, string parentId, bool forceSetParent = false)
        {
            var parent = @ref.GetTransformReference(parentId);
            var result = @ref.GetOrAddGameObjectReference(refId, parent, forceSetParent);
            return result;
        }
        public static GameObject GetOrAddGameObjectReference(this GameObjectReference @ref, GameObject pool, string refId, string parentId, bool forceSetParent = false)
        {
            var parent = @ref.GetTransformReference(parentId);
            var result = @ref.GetOrAddGameObjectReference(pool, refId, parent, forceSetParent);
            return result;
        }
        public static GameObject GetOrAddGameObjectReference(this GameObjectReference @ref, string refId, Transform parent, bool forceSetParent = false)
        {
            var result = @ref.GetGameObjectReference(refId);
            if (result.IsNullSafe())
            {
                result = @ref.GetGameObjectFromPoolReference(refId, parent);
                if (result != null)
                {
                    @ref.AddChildren(result, refId);
                }
            }
            else if (forceSetParent)
            {
                result.transform.SetParent(parent);
            }
            return result;
        }
        public static GameObject GetOrAddGameObjectReference(this GameObjectReference @ref, GameObject pool, string refId, Transform parent, bool forceSetParent = false)
        {
            var result = @ref.GetGameObjectReference(refId);
            if (result.IsNullSafe())
            {
                result = pool.PoolItem(parent);
                if (result != null)
                {
                    @ref.AddChildren(result, refId);
                }
            }
            else if (forceSetParent)
            {
                result.transform.SetParent(parent);
            }
            return result;
        }
        public static GameObject GetGameObjectFromPoolReference(this GameObjectReference @ref, string refId, Transform parent)
        {
            var pool = @ref.GetPoolReference(refId);
            if (!pool.IsNullSafe())
            {
                return pool.PoolItem(parent);
            }
            return default;
        }

        public static GameObject GetPoolReference(this GameObjectReference @ref, string refId)
        {
            var poolRef = @ref.GetComponentSafe<PoolGameObjectReference>();
            if (!poolRef.IsNullSafe())
            {
                var pool = poolRef.OnGetPool(refId);
                return pool;
            }
            return default;
        }
    }
}
