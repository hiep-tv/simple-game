using System;
using UnityEngine;
namespace Gametamin.Core
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class PlayAnimator : MonoBehaviour, IPlayAnimation, IAnimationState
    {
        static string _showEvent = "OpenComplete", _hideEvent = "CloseComplete", _showName = "appear", _hideName = "disappear";
        bool _showAnimation = false;
        public bool Showing
        {
            get => _showAnimation;
            set => _showAnimation = value;
        }
        Action _onOpenComplete;
        Action _onCloseComplete;
        Animator _animator;
        protected Animator _Animator
        {
            get
            {
                GetAnimator();
                return _animator;
            }
        }
        private void Awake()
        {
            GetAnimator();
        }
        void GetAnimator()
        {
            if (_animator == null)
            {
                var canvasGroup = gameObject.GetComponentSafe<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f;
                }
                _animator = gameObject.GetComponentSafe<Animator>();
                if (_animator != null)
                {
                    SetEvents(_animator);
                }
            }
        }
        void SetEvents(Animator animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            clips.For(clip =>
            {
                if (clip.name.EqualsSafe(_showName))
                {
                    SetEndEvent(clip, _showEvent);
                }
                else if (clip.name.EqualsSafe(_hideName))
                {
                    SetEndEvent(clip, _hideEvent);
                }
            });
        }
        void SetEndEvent(AnimationClip clip, string method)
        {
            if (clip.events.GetCountSafe() <= 0)
            {
                AnimationEvent animationStartEvent = new AnimationEvent
                {
                    time = clip.length,
                    functionName = method,
                    stringParameter = clip.name
                };
                clip.AddEvent(animationStartEvent);
            }
        }
        public void OnShow(Action onComplete)
        {
            if (_showAnimation)
            {
                onComplete?.Invoke();
                return;
            }
            _showAnimation = true;
            if (CanPlayAnimation)
            {
                _onOpenComplete = onComplete;
                PlayAnimation(_showName);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        public void OnHide(Action onComplete)
        {
            if (!_showAnimation)
            {
                onComplete?.Invoke();
                return;
            }
            _showAnimation = false;
            if (CanPlayAnimation)
            {
                _onCloseComplete = onComplete;
                PlayAnimation(_hideName);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        bool CanPlayAnimation
        {
            get
            {
                return gameObject.IsActiveInHierarchySafe()
                    && !_Animator.IsNullSafe()
                    && _Animator.runtimeAnimatorController != null;
            }
        }
        protected virtual void PlayAnimation(string stateName)
        {
            _Animator.Play(stateName, 0, 0f);
        }
        /// <summary>
        /// call via animation event
        /// </summary>
        void CloseComplete()
        {
            _onCloseComplete?.Invoke();
        }
        /// <summary>
        /// call via animation event
        /// </summary>
        void OpenComplete()
        {
            _onOpenComplete?.Invoke();
        }
        private void OnDisable()
        {
            _showAnimation = false;
        }
        [NaughtyAttributes.Button("Show")]
        void TestShow()
        {
            OnShow(null);
        }
        [NaughtyAttributes.Button("Hide")]
        void TestHide()
        {
            OnHide(null);
        }
    }
}