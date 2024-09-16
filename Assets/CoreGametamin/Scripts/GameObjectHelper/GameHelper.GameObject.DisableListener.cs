#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void AddOnDisableListener(this Component component, Action onDisable)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.AddOnDisableListener(onDisable);
            }
            else
            {
                onDisable?.Invoke();
            }
        }
        public static void AddOnDisableListener(this GameObject gameObject, Action onDisable, bool raiseIfDisabled = true)
        {
            if (!gameObject.IsNullSafe())
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                    listener.OnAddOnDisableCallback(onDisable, raiseIfDisabled);
                }
            }
            else
            {
                onDisable?.Invoke();
            }
        }
        public static void RemoveOnDisableListener(this Component component, Action onDisable)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.RemoveOnDisableListener(onDisable);
            }
            else
            {
                onDisable?.Invoke();
            }
        }
        public static void RemoveOnDisableListener(this GameObject gameObject, Action onDisable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (!gameObject.IsNullSafe())
            {
                var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                listener.OnRemoveOnDisableCallback(onDisable);
            }
            else
            {
                onDisable?.Invoke();
            }
        }
    }
}
