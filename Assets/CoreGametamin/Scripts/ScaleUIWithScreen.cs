using UnityEngine;
namespace Gametamin.Core
{
    [DisallowMultipleComponent]
    public class ScaleUIWithScreen : MonoBehaviour
    {
        [SerializeField] protected ScaleType scaleType = ScaleType.Both, _keepSize = ScaleType.Non;
        ILocalScaleChange _iscaleChange;
        ILocalScaleChange _IScaleChange => gameObject.GetComponentSafe(ref _iscaleChange);
        private void Awake()
        {
            if (scaleType != ScaleType.Non)
            {
                SetScale();
            }
        }
        protected virtual void SetScale()
        {
            var localScale = transform.localScale;
            float scaleX = localScale.x;
            float scaleY = localScale.y;
            switch (scaleType)
            {
                case ScaleType.Horizontal:
                    scaleX *= DetectResolution.UIScaleFactor;
                    break;
                case ScaleType.Verticle:
                    scaleY *= DetectResolution.UIScaleFactor;
                    break;
                case ScaleType.Both:
                    scaleX *= DetectResolution.UIScaleFactor;
                    scaleY *= DetectResolution.UIScaleFactor;
                    break;
            }
            var scale = new Vector3(scaleX, scaleY, localScale.z);
            CheckKeepSize(scale);
            SetScale(scale);
        }
        protected virtual void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
            if (_IScaleChange != null)
            {
                _IScaleChange.OnScaleChanged();
            }
            else
            {
                var rectTransform = gameObject.GetComponentSafe<RectTransform>();
                if (rectTransform != null)
                {
                    var anchor = rectTransform.anchoredPosition;
                    anchor *= scale;
                    rectTransform.anchoredPosition = anchor;
                }
            }
        }
        void CheckKeepSize(Vector3 scale)
        {
            switch (_keepSize)
            {
                case ScaleType.Horizontal:
                    SetHorizontalSize(scale.x);
                    break;
                case ScaleType.Verticle:
                    SetVerticleSize(scale.y);
                    break;
                case ScaleType.Both:
                    SetSize(scale);
                    break;
            }
        }

        void SetHorizontalSize(float scaleX)
        {
            if (gameObject.TryGetComponent<RectTransform>(out var target))
            {
                SetHorizontalSize(target, scaleX);
            }
        }
        protected void SetHorizontalSize(RectTransform target, float scaleX)
        {
            var localPosition = target.localPosition;
            var width = target.GetRectTransformWidth();
            var center = Vector2.one / 2f;
            target.anchorMin = center;
            target.anchorMax = center;
            var sizeDelta = target.sizeDelta;
            sizeDelta.x = Mathf.RoundToInt(width / scaleX);
            target.sizeDelta = sizeDelta;
            target.localPosition = localPosition;
        }
        void SetVerticleSize(float scaleY)
        {
            if (gameObject.TryGetComponent<RectTransform>(out var target))
            {
                SetVerticleSize(target, scaleY);
            }
        }
        protected void SetVerticleSize(RectTransform target, float scaleY)
        {
            var height = target.GetRectTransformHeight();
            var center = Vector2.one / 2f;
            target.anchorMin = center;
            target.anchorMax = center;
            var sizeDelta = target.sizeDelta;
            sizeDelta.y = Mathf.RoundToInt(height / scaleY);
            target.sizeDelta = sizeDelta;
        }
        void SetSize(Vector3 scale)
        {
            if (gameObject.TryGetComponent<RectTransform>(out var target))
            {
                SetSize(target, scale);
            }
        }
        protected void SetSize(RectTransform target, Vector3 scale)
        {
            target.GetRectTransformSize(out float width, out float height);
            var center = Vector2.one / 2f;
            target.anchorMin = center;
            target.anchorMax = center;
            var sizeDelta = target.sizeDelta;
            sizeDelta.x = Mathf.RoundToInt(width / scale.y);
            sizeDelta.y = Mathf.RoundToInt(height / scale.x);
            target.sizeDelta = sizeDelta;
        }
        protected enum ScaleType
        {
            Non = -1,
            Horizontal,
            Verticle,
            Both
        }
    }
    public interface ILocalScaleChange
    {
        void OnScaleChanged();
    }
}