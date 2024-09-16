using System;
using UnityEngine;

namespace Gametamin.Core
{
    public class PlayAnimationGroup : MonoBehaviour, IPlayAnimation, IShowHideAnimation
    {
        [SerializeField][GameObjectReferenceID] string[] _targetIds;
        GameObject[] _targets;
        GameObject[] _Targets
        {
            get
            {
                var count = _targets.GetCountSafe();
                if (count == 0)
                {
                    _targets = new GameObject[_targetIds.GetCountSafe()];
                    _targetIds.For((id, index) =>
                    {
                        _targets[index] = gameObject.GetGameObjectReference(id);
                    });
                }
                return _targets;
            }
        }
        public void OnSetShow()
        {
            //TODO
            Debug.Log("TODO: OnSetShow()");
        }
        public void OnSetHide()
        {
            //TODO
            Debug.Log("TODO: OnSetHide()");
        }
        public void OnShow(Action onComplete = null)
        {
            var count = _Targets.GetCountSafe();
            if (count > 0)
            {
                _Targets.For(target =>
                {
                    target.PlayShowAnimation(OnCallback);
                });
            }
            else
            {
                OnCallback();
            }
            void OnCallback()
            {
                count--;
                if (count <= 0)
                {
                    onComplete?.Invoke();
                }
            }
        }
        public void OnHide(Action onComplete = null)
        {
            var count = _Targets.GetCountSafe();
            if (count > 0)
            {
                _Targets.For(target =>
                {
                    target.PlayHideAnimation(OnCallback);
                });
            }
            else
            {
                OnCallback();
            }
            void OnCallback()
            {
                count--;
                if (count <= 0)
                {
                    onComplete?.Invoke();
                }
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
