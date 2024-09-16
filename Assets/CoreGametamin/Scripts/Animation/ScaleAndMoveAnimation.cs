using System;
using DG.Tweening;
using UnityEngine;

namespace Gametamin.Core
{
    public class ScaleAndMoveAnimation : MonoBehaviour, IPlayAnimation, IShowHideAnimation
    {
        [SerializeField] GameObject _target;
        [SerializeField] Vector3 _startPosition, _endPosition;
        [SerializeField] Vector3 _startScale, _endScale;
        [SerializeField] float _duration;
        RectTransform _rect;
        RectTransform _Rect => _target.GetComponentSafe(ref _rect);
        Sequence _current;
        public bool Showing { get; set; }
        public virtual void OnSetShow()
        {
            KillTween();
            _Rect.anchoredPosition = _endPosition;
            _target.transform.localScale = _endScale;
        }
        public virtual void OnSetHide()
        {
            KillTween();
            _Rect.anchoredPosition = _startPosition;
            _target.transform.localScale = _startScale;
        }
        public virtual void OnShow(Action callback = null)
        {
            MoveAndScale(_target.transform, _endPosition, _endScale, _duration, callback);
        }
        public virtual void OnHide(Action callback = null)
        {
            MoveAndScale(_target.transform, _startPosition, _startScale, _duration, callback);
        }
        void MoveAndScale(Transform target, Vector3 position, Vector3 scale, float duration, Action callback = null)
        {
            KillTween();
            _current = DOTween.Sequence();
            _current.Append(_Rect.DOAnchorPos(position, duration))
                .Join(target.DOScale(scale, duration)).OnComplete(() => callback?.Invoke());
        }
        protected virtual void OnDestroy()
        {
            KillTween();
        }

        void KillTween()
        {
            if (_current != null && _current.active)
            {
                _current.Kill();
            }
        }
    }
}