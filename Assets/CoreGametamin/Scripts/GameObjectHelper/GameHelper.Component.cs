#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        static Component _DefaultComponent => default;
        public static T[] GetComponentsSafe<T>(this GameObject gameObject)
        {
            if (gameObject.IsNullSafe()) return default;
            var array = gameObject.GetComponents<T>();
            return array;
        }
        public static T[] GetComponentsInChildrenSafe<T>(this GameObject gameObject, bool assert = true)
        {
            if (gameObject.IsNullSafe()) return default;
            var array = gameObject.GetComponentsInChildren<T>(true);
            return array;
        }
        public static T[] GetComponentsInChildrenSafe<T>(this Component component, bool assert = true)
        {
            if (component.IsNullSafe()) return default;
            var array = component.GetComponentsInChildren<T>(true);
            return array;
        }
        public static bool HasComponentSafe<T>(this Component component)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.HasComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying check has any ({typeof(T).Name}) component on a null component!");
            }
#endif
            return false;
        }
        public static bool HasComponentSafe<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponentSafe<T>();
            return !component.IsNullObjectSafe();
        }
        public static T GetOrAddComponentSafe<T>(this Component component, ref T result) where T : Component
        {
            if (!result.IsNullSafe())
            {
                return result;
            }
            if (!component.IsNullSafe())
            {
                result = component.gameObject.GetOrAddComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying get or add ({typeof(T).Name}) component on a null component!");
            }
#endif
            return result;
        }
        public static T GetOrAddComponentSafe<T>(this Component component) where T : Component
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.GetOrAddComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying get or add ({typeof(T).Name}) component on a null component!");
            }
#endif
            return default;
        }
        public static T GetOrAddComponentSafe<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponentSafe<T>();
            if (component.IsNullObjectSafe())
            {
                return gameObject.AddComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying get or add ({typeof(T).Name}) component on a null gameObject!");
            }
#endif
            return component;
        }
        public static T GetComponentSafe<T>(this Component component)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.GetComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying get ({typeof(T).Name}) component on a null component!");
            }
#endif
            return default;
        }
        public static T GetComponentSafe<T>(this IGetGameObject component)
        {
            if (!component.IsNullObjectSafe())
            {
                return component.gameObject.GetComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying get ({typeof(T).Name}) component on a null component!");
            }
#endif
            return default;
        }
        public static T GetComponentSafe<T>(this GameObject gameObject)
        {
            if (!gameObject.IsNullSafe())
            {
                if (gameObject.TryGetComponent(out T result))
                {
                    return result;
                }
#if GAMETAMIN_DEBUG
                else
                {
                    LogDebugComponent($"GameObejct ({gameObject.name}) has no ({typeof(T).Name}) component!");
                }
#endif
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying get a ({typeof(T).Name}) component on a null gameObject!");
            }
#endif
            return default;
        }
        public static T AddComponentSafe<T>(this Component component) where T : Component
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.AddComponentSafe<T>();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying add a ({typeof(T).Name}) component to a null component!");
            }
#endif
            return _DefaultComponent as T;
        }
        public static T AddComponentSafe<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.IsNullSafe())
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Debug.LogError($"Adding a component in editor mode may not be allowed! Add {typeof(T)} to {gameObject.name}");
                }
#endif
                var component = gameObject.AddComponent<T>();
                return component;
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugComponent($"you're trying add a ({typeof(T).Name}) component to a null gameObject!");
            }
#endif
            return _DefaultComponent as T;
        }
#if GAMETAMIN_DEBUG
        static void LogDebugComponent(string message)
        {
            Debug.LogWarning(message);
        }
#endif
    }
}
