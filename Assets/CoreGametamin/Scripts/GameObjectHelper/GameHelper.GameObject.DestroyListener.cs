#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void AddDestroyListener(this Component component, Action onDestroy)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.AddDestroyListener(onDestroy);
            }
            else
            {
                onDestroy?.Invoke();
            }
        }
        public static void AddDestroyListener(this GameObject gameObject, Action onDestroy)
        {
            if (!gameObject.IsNullSafe())
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                    listener.OnAddOnDestroyCallback(onDestroy);
                }
            }
            else
            {
                onDestroy?.Invoke();
            }
        }
    }
}
