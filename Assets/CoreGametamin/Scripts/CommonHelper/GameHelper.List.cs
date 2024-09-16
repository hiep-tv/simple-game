#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
namespace Gametamin.Core
{
    public static partial class ListHelper
    {
        public static List<T> CreateList<T>(ref List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            return list;
        }
        public static void SafeClear<T>(this List<T> list)
        {
            if (list.GetCountSafe() > 0)
            {
                list.Clear();
            }
        }
        public static void LogSafe<T>(this List<T> list)
        {
            list.For(item => Debug.Log(item.ToString()));

        }
        public static void SafeAddRange<T>(this List<T> list, List<T> collection)
        {
            if (list != null && collection.GetCountSafe() > 0)
            {
                list.AddRange(collection);
            }
        }
        public static void SafeAddRange<T>(this List<T> list, T[] collection)
        {
            if (list != null && collection.GetCountSafe() > 0)
            {
                list.AddRange(collection);
            }
        }
        public static void SafeAddRange<T>(this List<T> list, IEnumerable<T> collection)
        {
            if (list != null && collection != null)
            {
                list.AddRange(collection);
            }
        }
        public static void Clean<T>(this List<T> list) where T : UnityEngine.Object
        {
            if (list != null)
            {
                list.ForReverse((item, index) =>
                {
                    if (item == null)
                    {
                        list.RemoveAt(index);
                    }
                });
            }
        }
        public static T GetLastSafe<T>(this List<T> list)
        {
            return GetSafe(list, GetCountSafe(list) - 1);
        }
        public static T GetSafe<T>(this List<T> list, int index)
        {
            //Assert.IsNotNull(list, "trying get element from null list is invalid!");
            var count = GetCountSafe(list);
            //Assert.IsInRange(index, 0, count - 1);
            if (count > 0)
            {
                index = Mathf.Clamp(index, 0, count - 1);
                return list[index];
            }
            return default;
        }
        public static T GetOrAddSafe<T>(this List<T> list, int index) where T : new()
        {
            Assert.IsNotNull(list, "trying get element from null list is invalid!");
            if (list != null)
            {
                var count = GetCountSafe(list);
                if (index >= count)
                {
                    var newItem = new T();
                    list.Add(newItem);
                    return newItem;
                }
                index = Mathf.Max(index, 0);
                return list[index];
            }
            return new T();
        }
        public static T GetSafe<T>(this List<T> list, int index, bool nullIfError = false)
        {
            Assert.IsNotNull(list, "trying get element from null list is invalid!");
            if (list != null)
            {
                var count = GetCountSafe(list);
                if (nullIfError)
                {
                    if (index < 0 || index >= count)
                    {
                        return default;
                    }
                }
                //Assert.IsInRange(index, 0, count - 1);
                if (count > 0)
                {
                    index = Mathf.Clamp(index, 0, count - 1);
                    return list[index];
                }
            }
            return default;
        }
        public static void SetSafe<T>(this List<T> list, T item, int index)
        {
            Assert.IsNotNull(list, "trying set element to a null list is invalid!");
            var count = GetCountSafe(list);
            //Assert.IsInRange(index, 0, count - 1);
            if (list != null)
            {
                index = Mathf.Clamp(index, 0, count - 1);
                list[index] = item;
            }
        }
        public static void SetOrAddSafe<T>(this List<T> list, T item, int index)
        {
            Assert.IsNotNull(list, "trying set element to a null list is invalid!");
            var count = GetCountSafe(list);
            //Assert.IsInRange(index, 0, count - 1);
            if (list != null)
            {
                if (count > 0 && index < count)
                {
                    list[index] = item;
                }
                else
                {
                    list.Add(item);
                }
            }
        }
        public static void AddSafe<T>(ref List<T> list, T value)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            list.Add(value);
        }
        public static void AddSafe<T>(this List<T> list, T value)
        {
            if (list != null)
            {
                list.Add(value);
            }
        }
        public static void InsertSafe<T>(this List<T> list, int index, T value)
        {
            Assert.IsNotNull(list, "trying insert element to a null list is invalid!");
            var count = GetCountSafe(list);
            //Assert.IsInRange(index, 0, count - 1);
            if (list != null)
            {
                index = Mathf.Clamp(index, 0, count - 1);
                list.Insert(index, value);
            }
        }
        public static void AddDuplicate<T>(this List<T> list, T value, int duplicate)
        {
            Assert.IsNotNull(list);
            if (list != null)
            {
                //For(duplicate, (index) =>
                //{
                //    list.Add(value);
                //});
            }
        }
        public static int GetCountSafe<T>(this List<T> list)
        {
            if (list != null)
            {
                return list.Count;
            }
            return 0;
        }

        #region List
        public static void For<T>(this List<T> list, Action<T> callback, bool assert = true)
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
                    callback?.Invoke(list[i]);
                }
            }
        }
        public static void ForBreakable<T>(this List<T> list, Func<T, bool> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                //Assert.IsNotNull(list);
            }
#endif
            if (list.GetCountSafe() > 0)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    if (callback?.Invoke(list[i]) == true)
                    {
                        break;
                    }
                }
            }
        }

        public static void For<T>(this List<T> list, Action<T, int> callback, bool assert = true)
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
        //reverse
        public static void ForReverse<T>(this List<T> list, Action<T, int> callback, bool assert = true)
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
                    callback?.Invoke(list[i], i);
                }
            }
        }

        public static void ForBreakable<T>(this List<T> list, Func<T, int, bool> callback, bool assert = true)
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
        //reverse
        public static void ForReverseBreakable<T>(this List<T> list, Func<T, int, bool> callback, bool assert = true)
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
                    if (callback?.Invoke(list[i], i) == true)
                    {
                        break;
                    }
                }
            }
        }
        #endregion
        public static void SafeRemoveAt<T>(this List<T> list, int index)
        {
            var count = list.GetCountSafe();
            if (count > 0 && index >= 0 && index < count)
            {
                list.RemoveAt(index);
            }
        }
    }
}
