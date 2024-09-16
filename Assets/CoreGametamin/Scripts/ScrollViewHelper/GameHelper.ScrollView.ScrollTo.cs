#if DOTWEEN
using DG.Tweening;
#endif
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class ScollViewHelper
    {
        public enum ForcusItemType
        {
            Top,
            Center,
            Bottom
        }
        public static void ScrollVTo(this GameObjectReference @ref, string parentId, GameObject item, Action callback = null, float duration = 0.5f)
        {
            var scroll = @ref.GetComponentReferenceLayer2<ScrollRect>(parentId, GameObjectReferenceID.ScrollView);
            scroll.ScrollVTo(item, callback, duration);
        }
        public static void ScrollVTo(this GameObjectReference @ref, string parentId, GameObject item, ForcusItemType forcusType, Action callback = null, float duration = 0.5f)
        {
            var scroll = @ref.GetComponentReferenceLayer2<ScrollRect>(parentId, GameObjectReferenceID.ScrollView);
            scroll.ScrollVTo(item, forcusType, callback, duration);
        }
        public static void SetScrollToTop(this GameObjectReference @ref, string parentId)
        {
            var scroll = @ref.GetComponentReferenceLayer2<ScrollRect>(parentId, GameObjectReferenceID.ScrollView);
            scroll.SetScrollToTop();
        }
        public static void SetScrollVPosition(this GameObjectReference @ref, string parentId, GameObject item)
        {
            var scroll = @ref.GetComponentReferenceLayer2<ScrollRect>(parentId, GameObjectReferenceID.ScrollView);
            scroll.SetScrollVPosition(item);
        }
        public static void SetScrollVPosition(this GameObjectReference @ref, string parentId, GameObject item, ForcusItemType forcusType)
        {
            var scroll = @ref.GetComponentReferenceLayer2<ScrollRect>(parentId, GameObjectReferenceID.ScrollView);
            scroll.SetScrollVPosition(item, forcusType);
        }
        public static void SetScrollVPosition(this ScrollRect scroll, GameObject item)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                var viewport = scroll.viewport;
                var rectItem = item.GetComponentSafe<RectTransform>();
                var itemHeight = rectItem.GetRectTransformSizeDelta().y;
                var viewportCenter = viewport.rect.height / 2f - itemHeight / 2f;
                var anchorY = Vector3.Distance(rectItem.position, content.position) / rectItem.lossyScale.y - viewportCenter;
                anchorY = Mathf.Clamp(anchorY, 0f, content.sizeDelta.y - viewport.rect.height);
                content.SetAnchoredPositionYSafe(anchorY);
            }
        }
        public static void SetScrollVPosition(this ScrollRect scroll, GameObject item, ForcusItemType forcusType)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                var viewport = scroll.viewport;
                var rectItem = item.GetComponentSafe<RectTransform>();
                var itemHeight = rectItem.GetRectTransformSizeDelta().y;
                var delta = forcusType.GetDeltaHeight(itemHeight);
                var viewportCenter = viewport.rect.height / 2f + delta;
                var anchorY = Vector3.Distance(rectItem.position, content.position) / rectItem.lossyScale.y - viewportCenter;
                anchorY = Mathf.Clamp(anchorY, 0f, content.sizeDelta.y - viewport.rect.height);
                content.SetAnchoredPositionYSafe(anchorY);
            }
        }
        static float GetDeltaHeight(this ForcusItemType forcusType, float itemHeight)
        {
            return forcusType switch
            {
                ForcusItemType.Top => itemHeight / 2f,
                ForcusItemType.Center => itemHeight / 4f,
                ForcusItemType.Bottom => -itemHeight / 2f,
                _ => 0f,
            };
        }
        public static void ScrollVTo(this ScrollRect scroll, GameObject item, Action callback = null, float duration = 0.5f)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                var viewport = scroll.viewport;
                var rectItem = item.GetComponentSafe<RectTransform>();
                var itemHeight = rectItem.GetRectTransformSizeDelta().y;
                var viewportCenter = viewport.rect.height / 2f + itemHeight / 4f;
                var anchorY = Vector3.Distance(rectItem.position, content.position) / rectItem.lossyScale.y - viewportCenter;
                anchorY = Mathf.Clamp(anchorY, 0f, content.sizeDelta.y - viewport.rect.height);
#if DOTWEEN
                content.DOAnchorPosY(anchorY, duration).SetEase(Ease.Unset).OnComplete(() => callback?.Invoke());
#else
                content.SetAnchoredPositionYSafe(anchorY);
                callback?.Invoke();
#endif
            }
            else
            {
                callback?.Invoke();
            }
        }
        public static void ScrollVTo(this ScrollRect scroll, GameObject item, ForcusItemType forcusType, Action callback = null, float duration = 0.5f)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                var viewport = scroll.viewport;
                var rectItem = item.GetComponentSafe<RectTransform>();
                var itemHeight = rectItem.GetRectTransformSizeDelta().y;
                var delta = forcusType.GetDeltaHeight(itemHeight);
                var viewportCenter = viewport.rect.height / 2f + delta;
                var anchorY = Vector3.Distance(rectItem.position, content.position) / rectItem.lossyScale.y - viewportCenter;
                anchorY = Mathf.Clamp(anchorY, 0f, content.sizeDelta.y - viewport.rect.height);
#if DOTWEEN
                content.DOAnchorPosY(anchorY, duration).SetEase(Ease.Unset).OnComplete(() => callback?.Invoke());
#else
                content.SetAnchoredPositionYSafe(anchorY);
                callback?.Invoke();
#endif
            }
            else
            {
                callback?.Invoke();
            }
        }
        public static void ScrollHPosition(this ScrollRect scroll, GameObject item)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                var viewport = scroll.viewport;
                var rectItem = item.GetComponentSafe<RectTransform>();
                var itemWidth = rectItem.GetRectTransformSizeDelta().x;
                var viewportCenter = viewport.rect.width / 2f + itemWidth / 4f;
                var anchorX = Vector3.Distance(rectItem.position, content.position) / rectItem.lossyScale.x - viewportCenter;
                anchorX = Mathf.Clamp(anchorX, 0f, content.sizeDelta.x - viewport.rect.width);
                content.SetAnchoredPositionXSafe(anchorX);
            }
        }
        public static void ScrollHTo(this ScrollRect scroll, GameObject item, Action callback = null, float duration = 0.5f)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                var viewport = scroll.viewport;
                var rectItem = item.GetComponentSafe<RectTransform>();
                var itemWidth = rectItem.GetRectTransformSizeDelta().x;
                var viewportCenter = viewport.rect.width / 2f + itemWidth / 4f;
                var anchorX = Vector3.Distance(rectItem.position, content.position) / rectItem.lossyScale.x - viewportCenter;
                anchorX = Mathf.Clamp(anchorX, 0f, content.sizeDelta.x - viewport.rect.width);
#if DOTWEEN
                content.DOAnchorPosX(anchorX, duration).SetEase(Ease.Unset).OnComplete(() => callback?.Invoke());
#else
                content.SetAnchoredPositionXSafe(anchorX);
                callback?.Invoke();
#endif
            }
            else
            {
                callback?.Invoke();
            }
        }
        public static void SetScrollToTop(this ScrollRect scroll)
        {
            if (!scroll.IsNullSafe())
            {
                var content = scroll.content;
                content.SetAnchoredPositionYSafe(0);
            }
        }
    }
}
