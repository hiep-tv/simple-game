#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        static GameObject _DefaultValue => default;
        public static GameObject PoolItem(this GameObject pool, Transform parent = null)
        {
            GameObject newItem = _DefaultValue;
            if (pool.IsNullSafe())
            {
                return CreateGameObject(parent);
            }
            Assert.IsNotNull(parent, $"parent is null, {pool.name} will add to root.");
#if UNITY_EDITOR
            if (!Application.isPlaying && IsPrefab(pool))
            {
                newItem = (GameObject)PrefabUtility.InstantiatePrefab(pool, parent);
            }
            else
#endif
            {
                newItem = UnityEngine.Object.Instantiate(pool, parent);
            }
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Undo.RegisterCreatedObjectUndo(newItem, "Pool GameObject");
            }
#endif
            if (!newItem.IsActiveSafe())
            {
                newItem.SetActiveSafe(true);
            }
            return newItem;
        }
        const string _defaultGameObjectName = "GameObject";
        public static T CreateGameObject<T>(ref T instance, Transform parent, string name = _defaultGameObjectName) where T : Component
        {
            if (instance.IsNullSafe())
            {
                instance = CreateGameObject<T>(parent, name);
            }
            return instance;
        }
        public static T CreateGameObject<T>(Transform parent = null, string name = _defaultGameObjectName) where T : Component
        {
            var newGameObject = CreateGameObject(parent, name);
            return GetOrAddComponentSafe<T>(newGameObject);
        }
        public static GameObject CreateGameObject(Transform parent = null, string name = _defaultGameObjectName)
        {
            var newGameObject = new GameObject(name);
            var transform = newGameObject.transform;
            //Assert.IsNotNull(parent, $"parent is null, {name} will add to root!");
            if (parent != null)
            {
                transform.SetParent(parent);
            }
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(newGameObject, "New GameObject");
#endif
            return newGameObject;
        }
#if UNITY_EDITOR
        static bool IsPrefab(this GameObject gameObject)
        {
            return PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject) != null;
        }
#endif
    }
}
