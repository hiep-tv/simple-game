#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static Vector2 GetRectTransformSizeDelta(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                var rectTransform = GetComponentSafe<RectTransform>(target);
                return rectTransform.GetRectTransformSizeDelta();
            }
            return default;
        }
        public static Vector2 GetRectTransformSizeDelta(this RectTransform target)
        {
            if (!target.IsNullSafe())
            {
                return target.sizeDelta;
            }
            return default;
        }
        public static Vector3 GetAnchoredPosition(this Component target)
        {
            var rectTransform = GetComponentSafe<RectTransform>(target);
            return rectTransform.anchoredPosition;
        }
        public static Vector3 GetAnchoredPosition(this GameObject target)
        {
            var rectTransform = GetComponentSafe<RectTransform>(target);
            return rectTransform.anchoredPosition;
        }
        public static void SetRectTransformSizeDelta(this GameObject target, Vector2 size)
        {
            if (!target.IsNullSafe())
            {
                var rectTransform = GetComponentSafe<RectTransform>(target);
                rectTransform.SetRectTransformSizeDelta(size);
            }
        }
        public static void SetRectTransformSizeDelta(this RectTransform target, Vector2 size)
        {
            if (!target.IsNullSafe())
            {
                target.sizeDelta = size;
            }
        }
        public static void SetRectTransformHeight(this Component target, float height)
        {
            if (!target.IsNullSafe())
            {
                var rectTransform = GetComponentSafe<RectTransform>(target);
                rectTransform.SetRectTransformHeight(height);
            }
        }
        public static void SetRectTransformHeight(this GameObject target, float height)
        {
            if (!target.IsNullSafe())
            {
                var rectTransform = GetComponentSafe<RectTransform>(target);
                rectTransform.SetRectTransformHeight(height);
            }
        }
        public static void SetRectTransformHeight(this Transform target, float height)
        {
            if (!target.IsNullSafe())
            {
                var rectTransform = GetComponentSafe<RectTransform>(target);
                rectTransform.SetRectTransformHeight(height);
            }
        }
        public static void SetRectTransformHeight(this RectTransform target, float height)
        {
            if (!target.IsNullSafe())
            {
                var sizeDelta = target.sizeDelta;
                sizeDelta.y = height;
                target.sizeDelta = sizeDelta;
            }
        }
        public static void SetRectTransformWidth(this RectTransform target, float width)
        {
            if (!target.IsNullSafe())
            {
                var sizeDelta = target.sizeDelta;
                sizeDelta.x = width;
                target.sizeDelta = sizeDelta;
            }
        }
        public static void SetAnchoredPosition(this GameObject target, Vector2 position)
        {
            var rectTransform = GetComponentSafe<RectTransform>(target);
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = position;
            }
        }
        public static void SetAnchoredPositionX(this GameObject target, float positionX)
        {
            var rectTransform = GetComponentSafe<RectTransform>(target);
            if (rectTransform != null)
            {
                var anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.x = positionX;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }
        public static void SetAnchoredPositionY(this GameObject target, float positionY)
        {
            var rectTransform = GetComponentSafe<RectTransform>(target);
            if (rectTransform != null)
            {
                var anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = positionY;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }
        public static void SetAnchoredPosition(this Transform target, Vector2 position)
        {
            var rectTransform = GetComponentSafe<RectTransform>(target);
            rectTransform.SetAnchoredPositionSafe(position);
        }
        public static void SetAnchoredPositionSafe(this RectTransform rectTransform, Vector2 position)
        {
            if (!rectTransform.IsNullSafe())
            {
                rectTransform.anchoredPosition = position;
            }
        }
        public static void SetAnchoredPositionYSafe(this RectTransform rectTransform, float y)
        {
            if (!rectTransform.IsNullSafe())
            {
                var anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.y = y;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }
        public static void SetAnchoredPositionXSafe(this RectTransform rectTransform, float x)
        {
            if (!rectTransform.IsNullSafe())
            {
                var anchoredPosition = rectTransform.anchoredPosition;
                anchoredPosition.x = x;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }
        public static void ForceRebuildLayoutReference(this GameObjectReference @ref, string parentId, string targetId)
        {
            var target = @ref.GetComponentReferenceLayer2<RectTransform>(targetId, parentId);
            target.ForceRebuildLayoutSafe();
        }
        public static void ForceRebuildLayoutReference(this GameObjectReference @ref, string targetId)
        {
            var target = @ref.GetComponentReference<RectTransform>(targetId);
            target.ForceRebuildLayoutSafe();
        }
        public static void ForceRebuildLayoutSafe(this RectTransform rectTransform)
        {
            if (!rectTransform.IsNullSafe())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }
        public static void CopyRectransform(this RectTransform destination, RectTransform source)
        {
            destination.localPosition = source.localPosition;
            destination.localScale = source.localScale;
            destination.sizeDelta = source.sizeDelta;
            destination.anchorMin = source.anchorMin;
            destination.anchorMax = source.anchorMax;
            destination.anchoredPosition = source.anchoredPosition;
            destination.pivot = source.pivot;
        }
        static Vector3[] _corners = new Vector3[4];
        public static float GetRectTransformWidth(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_corners);
            var width = _corners[2].x - _corners[0].x;
            return Mathf.RoundToInt(width / rectTransform.lossyScale.x);
        }
        public static float GetRectTransformHeight(this RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_corners);
            var height = _corners[2].y - _corners[0].y;
            return Mathf.RoundToInt(height / rectTransform.lossyScale.y);
        }
        public static void GetRectTransformSize(this RectTransform rectTransform, out float width, out float height)
        {
            rectTransform.GetWorldCorners(_corners);
            width = (_corners[2].x - _corners[0].x) / rectTransform.lossyScale.y;
            height = (_corners[2].y - _corners[0].y) / rectTransform.lossyScale.y;
        }
        public static Rect GetRectTransformWorldRect(this RectTransform rectTransform)
        {
            // This returns the world space positions of the corners in the order
            // [0] bottom left,
            // [1] top left
            // [2] top right
            // [3] bottom right
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector2 min = corners[0];
            Vector2 max = corners[2];
            Vector2 size = max - min;
            return new Rect(min, size);
        }
    }
}
