#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static GameObject GetGameObjectReference(this GameObject root, string parentId, string refId)
        {
            var subParent = root.GetGameObjectReference(parentId);
            return subParent.GetGameObjectReference(refId);
        }
        public static GameObject GetGameObjectReference(this GameObject root, string parentId, string subParentId, string refId)
        {
            var parent = root.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetGameObjectReference(refId);
        }
        public static Transform GetTransformReference(this GameObject root, string parentId, string refId)
        {
            var parent = root.GetGameObjectReference(parentId);
            return parent.GetTransformReference(refId);
        }
        public static Transform GetTransformReference(this GameObject root, string parentId, string subParentId, string refId)
        {
            var parent = root.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetTransformReference(refId);
        }
        public static T GetComponentReference<T>(this GameObject root, string parentId, string refId)
        {
            var parent = root.GetGameObjectReference(parentId);
            return parent.GetComponentReference<T>(refId);
        }
        public static GameObject GetGameObjectReference(this Component component, string parentId, string refId)
        {
            var subParent = component.GetGameObjectReference(parentId);
            return subParent.GetGameObjectReference(refId);
        }
        public static GameObject GetGameObjectReference(this Component component, string parentId, string subParentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetGameObjectReference(refId);
        }
        public static Transform GetTransformReferenceLayer2(this Component component, string parentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            return parent.GetTransformReference(refId);
        }
        public static Transform GetTransformReference(this Component component, string parentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            return parent.GetTransformReference(refId);
        }
        public static Transform GetTransformReferenceLayer3(this Component component, string parentId, string subParentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetTransformReference(refId);
        }
        public static Transform GetTransformReference(this Component component, string parentId, string subParentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetTransformReference(refId);
        }
        public static T GetComponentReferenceLayer2<T>(this Component component, string parentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            return parent.GetComponentReference<T>(refId);
        }
        public static T GetComponentReferenceLayer3<T>(this Component component, string parentId, string subParentId, string refId)
        {
            var parent = component.GetGameObjectReference(parentId);
            var subParent = parent.GetGameObjectReference(subParentId);
            return subParent.GetComponentReference<T>(refId);
        }
    }
}
