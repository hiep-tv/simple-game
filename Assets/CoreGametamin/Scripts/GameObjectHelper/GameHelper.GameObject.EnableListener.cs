#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void AddOnEnableListener(this Component component, Action onEnable)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.AddOnEnableListener(onEnable);
            }
            else
            {
                onEnable?.Invoke();
            }
        }
        public static void AddOnEnableListener(this GameObject gameObject, Action onEnable, bool raiseIfEnabled = true)
        {
            if (!gameObject.IsNullSafe())
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                    listener.OnAddOnEnableCallback(onEnable, raiseIfEnabled);
                }
            }
            else
            {
                onEnable?.Invoke();
            }
        }
        public static void RemoveOnEnableListener(this Component component, Action onEnable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (!component.IsNullSafe())
            {
                component.gameObject.RemoveOnEnableListener(onEnable);
            }
            else
            {
                onEnable?.Invoke();
            }
        }
        public static void RemoveOnEnableListener(this GameObject gameObject, Action onEnable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (!gameObject.IsNullSafe())
            {
                var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                listener.OnRemoveOnEnableCallback(onEnable);
            }
            else
            {
                onEnable?.Invoke();
            }
        }
    }
}
