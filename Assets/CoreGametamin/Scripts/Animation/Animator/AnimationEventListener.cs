using System;
using UnityEngine;
namespace Gametamin.Core
{
    public class AnimationEventListener : MonoBehaviour
    {
        bool _initialized;
        static string _eventName = "OnComplete", _showName = "appear", _hideName = "disappear";
        Action _onCloseComplete, _onOpenComplete, _onCompleted, _onLoop;
        public Action OnCloseComplete { get => _onCloseComplete; set => _onCloseComplete = value; }
        public Action OnOpenComplete { get => _onOpenComplete; set => _onOpenComplete = value; }
        public Action OnCompleted { get => _onCompleted; set => _onCompleted = value; }
        public Action OnLooped { get => _onLoop; set => _onLoop = value; }
        public void AddEvent(Action onCompleted)
        {
            OnCompleted = onCompleted;
            if (!_initialized)
            {
                _initialized = true;
                var animator = GetComponent<Animator>();
                if (animator != null)
                {
                    SetEvents(animator);
                }
            }
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
        /// <summary>
        /// call via animation event
        /// </summary>
        void OnComplete()
        {
            _onCompleted?.Invoke();
        }
        /// <summary>
        /// call via animation event
        /// </summary>
        void OnLoop()
        {
            _onLoop?.Invoke();
        }
        void SetEvents(Animator animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            clips.For(clip =>
            {
                if (clip.name.EqualsSafe(_showName) || clip.name.EqualsSafe(_hideName))
                {
                    SetEndEvent(clip, _eventName);
                }
            });
        }
        void SetEndEvent(AnimationClip clip, string method)
        {
            if (clip.events.GetCountSafe() <= 0)
            {
                AnimationEvent animationStartEvent = new()
                {
                    time = clip.length,
                    functionName = method,
                    stringParameter = clip.name
                };
                clip.AddEvent(animationStartEvent);
            }
        }
    }
}
