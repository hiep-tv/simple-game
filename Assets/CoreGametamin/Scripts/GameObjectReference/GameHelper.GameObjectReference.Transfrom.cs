#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static Vector3 GetPositionReference(this GameObject root, string refId)
        {
            var child = root.GetTransformReference(refId);
            return child.GetPositionSafe();
        }
        public static Vector3 GetPositionReference(this Component root, string refId)
        {
            var child = root.GetTransformReference(refId);
            return child.GetPositionSafe();
        }
    }
}
