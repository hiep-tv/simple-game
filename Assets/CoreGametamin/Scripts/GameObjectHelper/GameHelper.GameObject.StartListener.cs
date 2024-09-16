#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void AddOnStartListener(this Component component, Action onStart)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.AddOnStartListener(onStart);
            }
            else
            {
                onStart?.Invoke();
            }
        }
        public static void AddOnStartListener(this GameObject gameObject, Action onStart)
        {
            if (!gameObject.IsNullSafe())
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    var listener = gameObject.GetOrAddComponentSafe<UnityMessageEventListener>();
                    listener.OnAddOnStartCallback(onStart);
                }
            }
            else
            {
                onStart?.Invoke();
            }
        }
    }
}
