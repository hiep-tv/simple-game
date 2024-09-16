using System;

namespace Gametamin.Core
{
    public interface IPlayAnimation
    {
        void OnShow(Action onComplete = null);
        void OnHide(Action onComplete = null);
    }
    public interface IAnimationState
    {
        bool Showing { get; }
    }
    public interface IShowHideAnimation
    {
        void OnSetShow();
        void OnSetHide();
    }
}
