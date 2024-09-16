#if DOTWEEN
using DG.Tweening;
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameHelper
    {
        public static void SetupCanvasCamera(this GameObject target)
        {
            var canvas = target.GetComponentSafe<Canvas>();
            if (canvas != null)
            {
                canvas.worldCamera = PoolHelper.CameraUI;
            }
        }
        public static void For(this int length, Action<int> callback)
        {
            for (int i = 0; i < length; i++)
            {
                callback?.Invoke(i);
            }
        }
        public static void ForReverse(this int length, Action<int> callback)
        {
            for (int i = length; i >= 0; i--)
            {
                callback?.Invoke(i);
            }
        }
        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static void SetCanvasGroupInteractive(this GameObjectReference @ref, bool interactable)
        {
            var canvasGroup = @ref.GetComponentSafe<CanvasGroup>();
            canvasGroup.SetCanvasGroupInteractive(interactable);
        }
        public static void SetCanvasGroupInteractive(this CanvasGroup canvasGroup, bool interactable)
        {
            if (!canvasGroup.IsNullSafe())
            {
                canvasGroup.interactable = interactable;
            }
        }
#if DOTWEEN
        public static Tween DelayCall(this float time, Action callback = null)
        {
            return DOVirtual.DelayedCall(time, () => callback?.Invoke());
        }
        public static Tween DelayCallLoop(this float time, Action callback = null)
        {
            return DOVirtual.DelayedCall(time, () => callback?.Invoke()).SetLoops(-1, LoopType.Restart);
        }
        public static void SafeKillTween(this Tween tween)
        {
            tween?.Kill();
        }
        public static void SafeKillTween(this Sequence sequense)
        {
            sequense?.Kill();
        }
#endif
    }
}
