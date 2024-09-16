#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void GetGameObjectsReference(this GameObject root, string refId, Action<GameObject> callback)
        {
            GetComponentSafe<GameObjectReference>(root).GetGameObjectsReference(refId, callback);
        }
        public static void GetGameObjectsReference(this GameObjectReference iref, string refId, Action<GameObject> callback)
        {
            if (!iref.IsNullSafe())
            {
                iref.OnGetGameObjects(refId, callback);
            }
        }
        public static void GetGameObjectsReference(this GameObject root, string refId, Action<GameObject, int> callback)
        {
            GetComponentSafe<GameObjectReference>(root).GetGameObjectsReference(refId, callback);
        }
        public static void GetGameObjectsReference(this GameObjectReference iref, string refId, Action<GameObject, int> callback)
        {
            if (!iref.IsNullSafe())
            {
                iref.OnGetGameObjects(refId, callback);
            }
        }
        public static void SetGameObjectReference(this GameObjectReference @ref, GameObject target, string refId)
        {
            GetComponentSafe<GameObjectReference>(@ref).SetGameObjectReference(target, refId);
        }
        public static void SetGameObjectReference(this GameObject root, GameObject target, string refId)
        {
            GetComponentSafe<GameObjectReference>(root).SetGameObjectReference(target, refId);
        }
        public static void AddGameObjectReference(this GameObjectReference @ref, Component target, string refId)
        {
            @ref.AddGameObjectReference(target.gameObject, refId);
        }
        public static void AddGameObjectReference(this GameObjectReference @ref, GameObject target, string refId)
        {
            if (!target.IsNullSafe())
            {
                GetComponentSafe<GameObjectReference>(@ref).OnAddReference(refId, target);
            }
        }
        public static void AddGameObjectReferences(this GameObjectReference @ref, GameObjectReference irefOther)
        {
            if (!@ref.IsNullSafe())
            {
                @ref.OnAddReferences(irefOther);
            }
        }
        public static void AddGameObjectReferences(this GameObjectReference @ref, GameObject other)
        {
            var irefOther = other.GetComponent<GameObjectReference>();
            if (!irefOther.IsNullSafe())
            {
                @ref.OnAddReferences(irefOther);
            }
        }
        public static void AddGameObjectReferences(this GameObject root, GameObjectReference irefOther)
        {
            GetComponentSafe<GameObjectReference>(root).OnAddReferences(irefOther);
        }
        public static void AddGameObjectReferences(this GameObject root, GameObject other)
        {
            var irefOther = other.GetComponent<GameObjectReference>();
            if (!irefOther.IsNullSafe())
            {
                GetComponentSafe<GameObjectReference>(root).OnAddReferences(irefOther);
            }
        }
        public static void AddGameObjectReference(this GameObject root, GameObject target, string refId)
        {
            GetComponentSafe<GameObjectReference>(root).OnAddReference(refId, target);
        }
        public static void AddChildren(this GameObjectReference @ref, GameObject item, string childId)
        {
            var irefChild = GetComponentSafe<GameObjectReference>(item);
            if (irefChild != null)
            {
                var copyToRoot = irefChild.CopyType;
                if (copyToRoot != GameObjectReferenceCopyType.ChildrenOnly)
                {
                    @ref.OnSetReference(childId, item);
                }
                if (copyToRoot != GameObjectReferenceCopyType.Itself)
                {
                    @ref.OnSetReferences(irefChild);
                }
            }
            else
            {
                if (!childId.EqualsSafe(StringHelper.DefaultValue))
                {
                    @ref.OnSetReference(childId, item);
                }
            }
        }
        public static GameObject GetOrDuplicateGameObjectReference(this GameObject itemObject, string id, string sourceId)
        {
            var result = itemObject.GetGameObjectReference(id);
            if (result.IsNullSafe())
            {
                var board = itemObject.GetGameObjectReference(sourceId);
                if (!board.IsNullSafe())
                {
                    result = board.PoolItem(board.transform.parent);
                    itemObject.AddGameObjectReference(result, id);
                }
            }
            return result;
        }
    }
}
