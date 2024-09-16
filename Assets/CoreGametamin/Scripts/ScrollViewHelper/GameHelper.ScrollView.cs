using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class ScollViewHelper
    {
        public static void ResetHScrollReference(this GameObjectReference @ref, string parentId, bool toRight = false)
        {
            var scroll = @ref.GetGameObjectReference(parentId, GameObjectReferenceID.ScrollView);
            scroll.ResetHScroll(toRight);
        }
        public static void ResetHScroll(this GameObject scrollObject, bool toRight = false)
        {
            var scroll = scrollObject.GetComponentSafe<ScrollRect>();
            scroll.ResetHScroll(toRight);
        }
        public static void ResetHScroll(this ScrollRect scroll, bool toRight = false)
        {
            if (!scroll.IsNullSafe())
            {
                scroll.horizontalNormalizedPosition = toRight ? 1 : 0;
            }
        }
        public static void ResetVScrollReference(this GameObjectReference @ref, string parentId, bool toBottom = false)
        {
            var scroll = @ref.GetGameObjectReference(parentId, GameObjectReferenceID.ScrollView);
            scroll.ResetVScroll(toBottom);
        }
        public static void ResetVScroll(this GameObject scrollObject, bool toBottom = false)
        {
            var scroll = scrollObject.GetComponentSafe<ScrollRect>();
            scroll.ResetVScroll(toBottom);
        }
        public static void ResetVScroll(this ScrollRect scroll, bool toBottom = false)
        {
            if (!scroll.IsNullSafe())
            {
                scroll.verticalNormalizedPosition = toBottom ? 0 : 1;
            }
        }
    }
}
