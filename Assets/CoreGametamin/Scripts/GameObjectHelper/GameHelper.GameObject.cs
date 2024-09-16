#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static Component SetActiveSafe(this Component component, bool active)
        {
            if (!IsNullSafe(component))
            {
                component.gameObject.SetActiveSafe(active);
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugGameObject("you're trying set active a null component!");
            }
#endif
            return component;
        }
        public static GameObject SetActiveSafe(this GameObject gameObject, bool active)
        {
            if (!IsNullSafe(gameObject))
            {
                gameObject.SetActive(active);
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugGameObject("you're trying set active a null gameObject!");
            }
#endif
            return gameObject;
        }
        public static bool IsActiveInHierarchySafe(this Component component)
        {
            if (!IsNullSafe(component))
            {
                return component.gameObject.IsActiveInHierarchySafe();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugGameObject("you're trying check activeInHierarchy a null component!");
            }
#endif
            return false;
        }
        public static bool IsActiveInHierarchySafe(this GameObject gameObject)
        {
            if (!IsNullSafe(gameObject))
            {
                return gameObject.activeInHierarchy;
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugGameObject("you're trying check activeInHierarchy a null gameobject!");
            }
#endif
            return false;
        }
        public static bool IsActiveSafe(this Component component)
        {
            if (!IsNullSafe(component))
            {
                return component.gameObject.IsActiveSafe();
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugGameObject("you're trying check active a null component!");
            }
#endif
            return false;
        }
        public static bool IsActiveSafe(this GameObject gameObject)
        {
            if (!IsNullSafe(gameObject))
            {
                return gameObject.activeSelf;
            }
#if GAMETAMIN_DEBUG
            else
            {
                LogDebugGameObject("you're trying check active a null gameobject!");
            }
#endif
            return false;
        }
        public static bool IsNullSafe(this Object gameObject)
        {
            return IsNullObjectSafe(gameObject);
        }
        public static bool IsNullObjectSafe(this object aObj)
        {
            return aObj == null || aObj.Equals(null);
        }
        public static int GetGameObjectInstanceIDSafe(this Component component, int defaultId = 0)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.GetGameObjectInstanceIDSafe();
            }
            return defaultId;
        }
        public static int GetGameObjectInstanceIDSafe(this GameObject @object, int defaultId = 0)
        {
            return @object.GetInstanceIDSafe();
        }
        public static int GetInstanceIDSafe(this Object @object, int defaultId = 0)
        {
            if (!@object.IsNullSafe())
            {
                return @object.GetInstanceID();
            }
            return defaultId;
        }
#if GAMETAMIN_DEBUG
        static void LogDebugGameObject(string message)
        {
            Debug.LogWarning(message);
        }
#endif
    }
}
