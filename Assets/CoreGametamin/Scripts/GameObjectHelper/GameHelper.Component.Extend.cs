#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static T GetComponentSafe<T>(this Component root, ref T component) where T : class
        {
            if (component.IsNullObjectSafe())
            {
                component = root.GetComponentSafe<T>();
            }
            return component;
        }
        public static T GetComponentSafe<T>(this GameObject gameObject, ref T component) where T : class
        {
            if (component.IsNullObjectSafe())
            {
                component = gameObject.GetComponentSafe<T>();
            }
            return component;
        }
    }
}
