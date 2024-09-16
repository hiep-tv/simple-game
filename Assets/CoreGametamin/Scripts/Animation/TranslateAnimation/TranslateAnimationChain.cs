using DG.Tweening;
using System;
using UnityEngine;
namespace Gametamin.Core
{
    public class TranslateAnimationChain : MonoBehaviour, IPlayAnimation
    {
        [SerializeField] bool _initializeOnStart;
        [SerializeField] StateAnimation _state = StateAnimation.Non;
        [SerializeField] float _delay;
        [SerializeField] TranslateData[] _translaters;
        Sequence _Sequence { get; set; }
        Tween _delayTween;
        bool _forceClose, _isDelay, _initialized;
        float _timeScale = 1f;
        StateAnimation _State
        {
            get => _state;
            set
            {
                _state = value;
            }
        }
        public bool Showing
        {
            get => _State == StateAnimation.Show;
            set => _State = value ? StateAnimation.Show : StateAnimation.Hide;
        }
        public float TimeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
                if (_Sequence != null)
                {
                    _Sequence.timeScale = value;
                }
            }
        }
        private void Start()
        {
            if (_initializeOnStart)
            {
                InitTranslators();
            }
            else if (_State != StateAnimation.Non)
            {
                _initialized = true;
            }
        }
        void TranslateToStart(Action onComplete)
        {
            _Sequence.Kill();
            _Sequence = DOTween.Sequence();
            _Sequence.timeScale = _timeScale;
            _Sequence.Append(_translaters[0].MoveToStart());
            _translaters.For((item, index) =>
             {
                 if (index > 0)
                 {
                     _Sequence.Join(item.MoveToStart());
                 }
             });
            //Debug.Log($"TranslateToStart={name}");
            _Sequence.OnComplete(() =>
            {
#if UNITY_EDITOR
                _Sequence = null;
#endif
                onComplete?.Invoke();
            });
        }
        void TranslateToEnd(Action onComplete)
        {
            _Sequence.Kill();
            _Sequence = DOTween.Sequence();
            _Sequence.timeScale = _timeScale;
            _Sequence.Append(_translaters[0].MoveToEnd());
            _translaters.For((item, index) =>
            {
                if (index > 0)
                {
                    _Sequence.Join(item.MoveToEnd());
                }
            });
            //Debug.Log($"TranslateToEnd={name}");
            _Sequence.OnComplete(() =>
            {
#if UNITY_EDITOR
                _Sequence = null;
#endif
                onComplete?.Invoke();
            });
        }
        public virtual void OnShow(Action onComplete = null)
        {
            if (_State == StateAnimation.Show)
            {
                onComplete?.Invoke();
                return;
            }
            _State = StateAnimation.Show;
            InitTranslators();
            TranslateToEnd(onComplete);
        }
        void InitTranslators()
        {
            if (!_initialized)
            {
                _initialized = true;
                _translaters.For((item, index) =>
                {
                    item.Init();
                });
            }
        }
        public virtual void OnHide(Action onComplete = null)
        {
            if (_State == StateAnimation.Hide)
            {
                onComplete?.Invoke();
                return;
            }
            _State = StateAnimation.Hide;
            InitTranslators();
            if (_forceClose)
            {
                _forceClose = false;
                if (_isDelay)
                {
                    _delayTween.Kill();
                    TranslateToStart(onComplete);
                }
                else
                {
                    TranslateToStart(onComplete);
                }
            }
            else
            {
                _isDelay = true;
                _delayTween = GameHelper.DelayCall(_delay, () =>
                 {
                     _isDelay = false;
                     TranslateToStart(onComplete);
                 });
            }
        }
        public virtual void ForceClose(float timeScale = 1f)
        {
            if (!_forceClose)
            {
                _forceClose = true;
            }
            TimeScale = timeScale;
        }
        [ContextMenu("Show")]
        public void TestShow()
        {
            OnShow(() => { Debug.LogError("Complete"); });
        }
        [ContextMenu("Hide")]
        public void TestHide()
        {
            OnHide(() => { Debug.LogError("Complete"); });
        }
        private void OnDestroy()
        {
            _delayTween.Kill();
            _Sequence.Kill();
            _translaters.For((item, index) =>
            {
                item.Stop();
            });
        }
        [Serializable]
        class TranslateData
        {
            public RectTransform target;
            public bool scaleWithScreen;
            public Vector3 startPosition, endPosition;
            public float toStarDuration, toEndDuration;
            public Direction direction = Direction.Both;
            public Ease toStartEase = Ease.Linear, toEndEase = Ease.Linear;
            bool _killed;
            public void Init()
            {
                endPosition = target.anchoredPosition;
                var currentPosition = target.anchoredPosition;
                switch (direction)
                {
                    case Direction.Both:
                        currentPosition = startPosition;
                        break;
                    case Direction.X:
                        currentPosition.x = startPosition.x;
                        break;
                    case Direction.Y:
                        currentPosition.y = startPosition.y;
                        break;
                    default:
                        break;
                }
                target.anchoredPosition = currentPosition;
            }
            public Tween MoveToStart()
            {
                if (toStartEase == Ease.Unset)
                {
                    toStartEase = Ease.Linear;
                }
                return MoveTo(startPosition, toStarDuration, toStartEase/* , () => { SetActiveTarget(false); } */);
            }
            public void SetActiveTarget(bool isActive)
            {
                target.gameObject.SetActive(isActive);
            }
            public Tween MoveToEnd()
            {
                if (toEndEase == Ease.Unset)
                {
                    toEndEase = Ease.Linear;
                }
                SetActiveTarget(true);
                var end = endPosition;
                return MoveTo(endPosition, toEndDuration, toEndEase);
            }
            Tween MoveTo(Vector3 endValue, float duration, Ease ease, Action onComplete = null)
            {
                if (_killed)
                {
                    return default;
                }
                switch (direction)
                {
                    case Direction.Both:
                        return target.DOAnchorPos(endValue, duration).SetEase(ease)
                            .OnComplete(() => { onComplete?.Invoke(); });
                    case Direction.X:
                        return target.DOAnchorPosX(endValue.x, duration).SetEase(ease)
                            .OnComplete(() => { onComplete?.Invoke(); });
                    case Direction.Y:
                        return target.DOAnchorPosY(endValue.y, duration).SetEase(ease)
                            .OnComplete(() => { onComplete?.Invoke(); });
                    default:
                        break;
                }
                return default;
            }
            public void Stop()
            {
                _killed = true;
                target.DOKill();
            }
        }
        enum Direction
        {
            Both,
            X,
            Y
        }
        enum StateAnimation
        {
            Non,
            Show,
            Hide
        }
    }
}