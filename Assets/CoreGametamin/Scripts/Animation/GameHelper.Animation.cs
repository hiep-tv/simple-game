using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameHelper
    {
        public static bool ShowingAnimation(this Component animationObject)
        {
            if (!animationObject.IsNullSafe())
            {
                var ianimation = animationObject.GetComponentSafe<IAnimationState>();
                return ianimation != null && ianimation.Showing;
            }
            return false;
        }
        public static void PlayShowAnimation(this Component animationObject, Action onComplete = null)
        {
            if (!animationObject.IsNullSafe())
            {
                animationObject.gameObject.PlayShowAnimation(onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        public static void PlayShowAnimation(this GameObject animationObject, Action onComplete = null)
        {
            if (animationObject != null)
            {
                var ianimations = animationObject.GetComponentsSafe<IPlayAnimation>();
                var count = ianimations.GetCountSafe();
                if (count > 0)
                {
                    ianimations.For(ianimation => ianimation.OnShow(OnCompleted));
                    void OnCompleted()
                    {
                        count--;
                        if (count <= 0)
                        {
                            onComplete?.Invoke();
                        }
                    }
                }
                else
                {
                    animationObject.SetActive(true);
                    onComplete?.Invoke();
                }
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        public static void PlayHideAnimation(this Component animationObject, Action onComplete = null)
        {
            if (!animationObject.IsNullSafe())
            {
                animationObject.gameObject.PlayHideAnimation(onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        public static void PlayHideAnimation(this GameObject animationObject, Action onComplete = null)
        {
            if (animationObject != null)
            {
                var ianimation = animationObject.GetComponentSafe<IPlayAnimation>();
                if (ianimation != null)
                {
                    ianimation.OnHide(onComplete);
                }
                else
                {
                    animationObject.SetActiveSafe(false);
                    onComplete?.Invoke();
                }
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        public static string _AppearAnimationName = "appear", _DisappearAnimationName = "disappear";
        public static void PlayAppearAnimator(this GameObject target, Action onCompleted = null)
        {
            target.PlayAnimator(_AppearAnimationName, onCompleted);
        }
        public static void PlayDisappearAnimator(this GameObject target, Action onCompleted = null)
        {
            target.PlayAnimator(_DisappearAnimationName, onCompleted);
        }
        public static void PlayAnimator(this GameObject target, string stateName, Action onCompleted = null)
        {
            var _animator = target.GetComponentSafe<Animator>();
            if (_animator != null)
            {
                var listener = target.GetOrAddComponentSafe<AnimationEventListener>();
                listener.AddEvent(onCompleted);
                _animator.Play(stateName, 0, 0f);
            }
        }
        public static void PlayTriggerAppearAnimator(this GameObject target, Action onCompleted = null)
        {
            target.PlayTriggerAnimator(_AppearAnimationName, onCompleted);
        }
        public static void PlayTriggerDisappearAnimator(this GameObject target, Action onCompleted = null)
        {
            target.PlayTriggerAnimator(_DisappearAnimationName, onCompleted);
        }
        public static void PlayTriggerAnimator(this GameObject target, string stateName, Action onCompleted = null)
        {
            var _animator = target.GetComponentSafe<Animator>();
            if (_animator != null)
            {
                var listener = target.GetOrAddComponentSafe<AnimationEventListener>();
                listener.AddEvent(onCompleted);
                _animator.SetTrigger(stateName);
            }
        }
    }
}
