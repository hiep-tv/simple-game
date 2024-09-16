#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void HideAllGameObjects(this List<GameObject> list, Action<GameObject> callback = null)
        {
            list.For(item =>
            {
                item.SetActiveSafe(false);
                callback?.Invoke(item);
            });
        }
        public static void DestroyImmediateAllGameObjects(this List<GameObject> list)
        {
            list.For(item =>
            {
                if (item != null)
                {
                    UnityEngine.Object.DestroyImmediate(item);
                }
            });
            list.Clear();
        }
        public static void HideAllGameObjects<T>(this List<T> list, Action<GameObject> callback = null) where T : Component
        {
            list.For(item =>
            {
                item.SetActiveSafe(false);
                callback?.Invoke(item?.gameObject);
            });
        }
        public static GameObject GetFreeGameObject(this List<GameObject> list, Func<GameObject> onGetPool, Transform parent = null)
        {
            GameObject result = default;
            var count = list.GetCountSafe();
            if (count > 0)
            {
                list.ForBreakable((item, index) =>
                {
                    bool inactive = !item.IsActiveSafe();
                    if (inactive)
                    {
                        result = item;
                        item.transform.SetParent(parent, false);
                        item.SetActiveSafe(true);
                    }
                    return inactive;
                });
                if (result != null)
                {
                    return result;
                }
            }
            var itemPool = onGetPool?.Invoke();
            result = PoolItem(itemPool, parent);
            result.SetActiveSafe(true);
            list.Add(result);
            return result;
        }
        public static T GetFreeObject<T>(this List<T> list, Func<GameObject> onGetPool, Transform parent = null)
        {
            T result = default;
            var count = list.GetCountSafe();
            if (count > 0)
            {
                list.ForBreakable((item) =>
                {
                    //var igameObject = item as IGetComponent;
                    //bool inactive = !igameObject.gameObject.activeSelf;
                    //if (inactive)
                    //{
                    //    igameObject.gameObject.SetActive(true);
                    //    result = item;
                    //}
                    //return inactive;
                    return false;
                });
                if (result != null)
                {
                    return result;
                }
            }
            var itemPool = onGetPool?.Invoke();
            var newGameObject = PoolItem(itemPool, parent);
            result = GetComponentSafe<T>(newGameObject);
            list.Add(result);
            return result;
        }
        public static void For<T>(this List<GameObject> list, Action<T> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    var component = list[i].GetComponentSafe<T>();
                    if (component != null)
                    {
                        callback?.Invoke(component);
                    }
                }
            }
        }
        public static void ForBreakable<T>(this List<GameObject> list, Func<T, bool> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    var component = list[i].GetComponentSafe<T>();
                    if (component != null)
                    {
                        if (callback?.Invoke(component) == true)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public static void ForBreakable<T>(this List<GameObject> list, Func<T, int, bool> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    var component = list[i].GetComponentSafe<T>();
                    if (component != null)
                    {
                        if (callback?.Invoke(component, i) == true)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static void For<T>(this List<GameObject> list, Action<T, int> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    var component = list[i].GetComponentSafe<T>();
                    if (component != null)
                    {
                        callback?.Invoke(component, i);
                    }
                }
            }
        }
        public static void For(this List<GameObject> list, Action<GameObject, int> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    callback?.Invoke(list[i], i);
                }
            }
        }
        public static void ForReverse<T>(this List<GameObject> list, Action<T, int> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var component = list[i].GetComponentSafe<T>();
                    callback?.Invoke(component, i);
                }
            }
        }
        public static void ForBreakable(this List<GameObject> list, Func<GameObject, int, bool> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    if (callback?.Invoke(list[i], i) == true)
                    {
                        break;
                    }
                }
            }
        }
        public static void For<T>(this GameObject[] array, Action<T, int> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                //Assert.IsNotNull(array, "Array is null");
            }
#endif
            if (array.GetCountSafe() > 0)
            {
                for (int i = 0, length = array.Length; i < length; i++)
                {
                    callback?.Invoke(array[i].GetComponentSafe<T>(), i);
                }
            }
        }
        public static void For<T>(this GameObject[] array, Action<T> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                //Assert.IsNotNull(array, "Array is null");
            }
#endif
            if (array.GetCountSafe() > 0)
            {
                for (int i = 0, length = array.Length; i < length; i++)
                {
                    callback?.Invoke(array[i].GetComponentSafe<T>());
                }
            }
        }
    }
}
