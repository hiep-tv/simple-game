using System;
using DG.Tweening;
using UnityEngine;
namespace Gametamin.Core
{
    public class TranslateAnchorPosAnimation : MonoBehaviour, IPlayAnimation, IShowHideAnimation
    {
        enum Direction
        {
            X,
            Y,
            Both
        }
        [SerializeField] RectTransform _target;
        [SerializeField] Direction _direction = Direction.Both;
        [SerializeField] Vector3 _startPosition, _endPosition;
        [SerializeField] float _duration;
        CanvasGroup _canvasGroup;
        CanvasGroup _CanvasGroup => _target.GetOrAddComponentSafe(ref _canvasGroup);
        Sequence _current;
        public void OnHide(Action onComplete = null)
        {
            MoveToHide(_target, _startPosition, _duration, onComplete);
        }

        public void OnShow(Action onComplete = null)
        {
            MoveToShow(_target, _endPosition, _duration, onComplete);
        }

        public void OnSetHide()
        {
            _target.localPosition = _startPosition;
        }

        public void OnSetShow()
        {
            _target.localPosition = _endPosition;
        }
        void MoveToShow(RectTransform target, Vector3 position, float duration, Action callback = null)
        {
            if (_current != null && _current.active)
            {
                _current.Kill();
            }
            _current = DOTween.Sequence();
            _current.Append(_CanvasGroup.DOFade(1f, duration));
            _current.Join(GetTween(target, position, duration));
            _current.OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        void MoveToHide(RectTransform target, Vector3 position, float duration, Action callback = null)
        {
            if (_current != null && _current.active)
            {
                _current.Kill();
            }
            _current = DOTween.Sequence();
            _current.Append(GetTween(target, position, duration));
            _current.Join(_CanvasGroup.DOFade(0f, duration));
            _current.AppendCallback(() =>
            {
                callback?.Invoke();
            });
        }
        Tween GetTween(RectTransform target, Vector3 position, float duration)
        {
            switch (_direction)
            {
                case Direction.X:
                    return target.DOAnchorPosX(position.x, duration).SetEase(Ease.InOutBack);
                case Direction.Y:
                    return target.DOAnchorPosY(position.y, duration).SetEase(Ease.InOutBack);
                case Direction.Both:
                    return target.DOAnchorPos(position, duration).SetEase(Ease.InOutBack);
            }
            return default;
        }
        private void OnDestroy()
        {
            if (_current != null && _current.active)
            {
                _current.Kill();
            }
        }
        [ContextMenu("Show")]
        void Show()
        {
            OnShow();
        }
        [ContextMenu("Hide")]
        void Hide()
        {
            OnHide();
        }
    }
}