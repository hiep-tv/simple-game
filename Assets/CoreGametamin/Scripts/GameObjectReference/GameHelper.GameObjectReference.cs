#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static GameObjectReference SetActiveGameObjectReference(this GameObject root, string refId, bool isActive)
        {
            return GetComponentSafe<GameObjectReference>(root).SetActiveGameObjectReference(refId, isActive);
        }
        public static GameObjectReference SetActiveGameObjectReferenceLayer2(this GameObject root, string parentId, string refId, bool isActive)
        {
            return GetComponentSafe<GameObjectReference>(root).SetActiveGameObjectReferenceLayer2(parentId, refId, isActive);
        }
        public static GameObjectReference SetActiveGameObjectReference(this GameObjectReference root, string refId, bool isActive)
        {
            var gameObject = root.GetGameObjectReference(refId);
            gameObject.SetActiveSafe(isActive);
            return root;
        }
        public static Component SetActiveGameObjectReference(this Component root, string refId, bool isActive)
        {
            var gameObject = root.GetGameObjectReference(refId);
            gameObject.SetActiveSafe(isActive);
            return root;
        }
        public static GameObjectReference SetActiveGameObjectReferenceLayer2(this GameObjectReference root, string parentId, string refId, bool isActive)
        {
            var gameObject = root.GetGameObjectReference(parentId, refId);
            gameObject.SetActiveSafe(isActive);
            return root;
        }
        public static GameObject GetGameObjectReference(this Component component, string refId)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.GetGameObjectReference(refId);
            }
            return default;
        }
        public static GameObject GetGameObjectReferenceLayer3(this GameObjectReference @ref, string parentId, string subParentId, string refId)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetGameObjectReference(refId);
        }
        public static GameObject GetGameObjectReferenceLayer2(this GameObjectReference @ref, string parentId, string refId)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            return parent.GetGameObjectReference(refId);
        }
        public static GameObject GetGameObjectReferenceLayer2(this GameObject @ref, string parentId, string refId)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            return parent.GetGameObjectReference(refId);
        }
        public static GameObject GetGameObjectReference(this GameObject root, string refId)
        {
            var @ref = GetComponentSafe<GameObjectReference>(root);
            return @ref.GetGameObjectReference(refId);
        }
        public static GameObject GetGameObjectReference(this GameObjectReference @ref, string refId)
        {
            if (!@ref.IsNullSafe())
            {
                return @ref.OnGetGameObject(refId);
            }
            return default;
        }
        public static Transform GetTransformReference(this Component component, string refId)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.GetTransformReference(refId);
            }
            return default;
        }
        public static Transform GetTransformReferenceLayer2(this GameObject root, string parentId, string refId)
        {
            var parent = root.GetGameObjectReference(parentId);
            return parent.GetTransformReference(refId);
        }
        public static Transform GetTransformReference(this GameObject root, string refId)
        {
            var @ref = GetComponentSafe<GameObjectReference>(root);
            return @ref.GetTransformReference(refId);
        }
        public static Transform GetTransformReference(this GameObjectReference @ref, string refId)
        {
            if (!@ref.IsNullSafe())
            {
                return @ref.OnGetTransform(refId);
            }
            return default;
        }
        public static T GetComponentReference<T>(this Component component, string refId)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.GetComponentReference<T>(refId);
            }
            return default;
        }
        public static T GetComponentReference<T>(this GameObject root, string refId)
        {
            var @ref = GetComponentSafe<GameObjectReference>(root);
            return @ref.GetComponentReference<T>(refId);
        }
        public static T GetComponentReference<T>(this GameObjectReference @ref, string refId)
        {
            if (!@ref.IsNullSafe())
            {
                return @ref.OnGetComponent<T>(refId);
            }
            return default;
        }

        public static bool HasGameObjectReference(this GameObject root, string refId)
        {
            var @ref = GetComponentSafe<GameObjectReference>(root);
            if (!@ref.IsNullSafe())
            {
                return @ref.HasGameObjectReference(refId);
            }
            return false;
        }
        public static bool HasGameObjectReference(this Component component, string refId)
        {
            var @ref = GetComponentSafe<GameObjectReference>(component);
            if (!@ref.IsNullSafe())
            {
                return @ref.HasGameObjectReference(refId);
            }
            return false;
        }
        public static bool HasGameObjectReference(this GameObjectReference @ref, string refId)
        {
            if (!@ref.IsNullSafe())
            {
                return @ref.HasGameObject(refId);
            }
            return default;
        }
    }
}
