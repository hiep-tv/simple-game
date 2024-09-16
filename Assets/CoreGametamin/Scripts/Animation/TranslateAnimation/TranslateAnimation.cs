using System;
using DG.Tweening;
using UnityEngine;
namespace Gametamin.Core
{
    public class TranslateAnimation : MonoBehaviour, IPlayAnimation, IShowHideAnimation
    {
        [SerializeField] GameObject _target;
        [SerializeField] Vector3 _startPosition, _endPosition;
        [SerializeField] float _duration;
        Tween _current;
        public void OnHide(Action onComplete = null)
        {
            Move(_target.transform, _startPosition, _duration, onComplete);
        }

        public void OnShow(Action onComplete = null)
        {
            Move(_target.transform, _endPosition, _duration, onComplete);
        }

        public void OnSetHide()
        {
            _target.transform.localPosition = _startPosition;
        }

        public void OnSetShow()
        {
            _target.transform.localPosition = _endPosition;
        }
        void Move(Transform target, Vector3 position, float duration, Action callback = null)
        {
            if (_current != null && _current.active)
            {
                _current.Kill();
            }
            _current = target.DOLocalMove(position, duration).OnComplete(() => callback?.Invoke());
        }
        private void OnDestroy()
        {
            if (_current != null && _current.active)
            {
                _current.Kill();
            }
        }
    }
}