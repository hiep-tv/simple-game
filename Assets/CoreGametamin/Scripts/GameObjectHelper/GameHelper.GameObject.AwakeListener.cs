#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void AddOnAwakeListener(this Component component, Action onAwake)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.AddOnAwakeListener(onAwake);
            }
            else
            {
                onAwake?.Invoke();
            }
        }
        public static void AddOnAwakeListener(this GameObject gameObject, Action onAwake)
        {
            if (!gameObject.IsNullSafe())
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                    listener.OnAddOnAwakeCallback(onAwake);
                }
            }
            else
            {
                onAwake?.Invoke();
            }
        }
    }
}
