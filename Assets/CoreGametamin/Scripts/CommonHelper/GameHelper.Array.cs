using System;
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class ArrayHelper
    {
        public static void SetSafe<T>(this T[] array, T item, int index)
        {
            //Assert.IsNotNull(array, "trying set an element to null array is invalid!");
            if (array != null)
            {
                var length = GetCountSafe(array);
                //Assert.IsInRange(index, 0, length - 1);
                index = Mathf.Clamp(index, 0, length - 1);
                array[index] = item;
            }
        }
        public static T GetSafe<T>(this T[] array, int index, bool nullIfError = false, T defaultValue = default)
        {
            //Assert.IsNotNull(array, "trying get element from null array is invalid!");
            if (array != null)
            {
                var length = GetCountSafe(array);
                if (nullIfError)
                {
                    if (index < 0 || index >= length)
                    {
                        return defaultValue;
                    }
                }
                //Assert.IsInRange(index, 0, length - 1);
                if (length > 0)
                {
                    index = Mathf.Clamp(index, 0, length - 1);
                    return array[index];
                }
            }
            return defaultValue;
        }
        public static T GetLastSafe<T>(this T[] array)
        {
            //Assert.IsNotNull(array, "trying get element from null array is invalid!");
            if (array != null)
            {
                var length = GetCountSafe(array);
                if (length > 0)
                {
                    return array[length - 1];
                }
            }
            return default;
        }
        public static void SetLast<T>(this T[] array, T value)
        {
            //Assert.IsNotNull(array, "trying get element from null array is invalid!");
            if (array != null)
            {
                var length = GetCountSafe(array);
                SetSafe(array, value, length - 1);
            }
        }
        public static int GetCountSafe<T>(this T[] array)
        {
            if (array != null)
            {
                return array.Length;
            }
            return 0;
        }
        public static void For<T>(this T[] array, Action<T> callback, bool assert = true)
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
                    callback?.Invoke(array[i]);
                }
            }
        }
        public static void ForBreakable<T>(this T[] array, Func<T, bool> callback, bool assert = true)
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
                    if (callback?.Invoke(array[i]) == true)
                    {
                        break;
                    }
                }
            }
        }
        public static void For<T>(this T[] array, Action<T, int> callback, bool assert = true)
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
                    callback?.Invoke(array[i], i);
                }
            }
        }

        //reverse
        public static void ForReverse<T>(this T[] array, Action<T, int> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                //Assert.IsNotNull(array, "Array is null");
            }
#endif
            if (array.GetCountSafe() > 0)
            {
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    callback?.Invoke(array[i], i);
                }
            }
        }
        public static void ForReverse<T>(this T[] array, Action<T> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                //Assert.IsNotNull(array, "Array is null");
            }
#endif
            if (array.GetCountSafe() > 0)
            {
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    callback?.Invoke(array[i]);
                }
            }
        }
        public static void ForBreakable<T>(this T[] array, Func<T, int, bool> callback, bool assert = true)
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
                    if (callback?.Invoke(array[i], i) == true)
                    {
                        break;
                    }
                }
            }
        }
        //reverse
        public static void ForBreakableReverse<T>(this T[] array, Func<T, int, bool> callback, bool assert = true)
        {
#if UNITY_EDITOR
            if (assert)
            {
                //Assert.IsNotNull(array, "Array is null");
            }
#endif
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (callback?.Invoke(array[i], i) == true)
                {
                    break;
                }
            }
        }
        public static void For(int length, Action callback)
        {
            for (int i = 0; i < length; i++)
            {
                callback?.Invoke();
            }
        }
        public static void ForReverse(int length, Action callback)
        {
            for (int i = length; i >= 0; i--)
            {
                callback?.Invoke();
            }
        }
        public static void For(int length, Action<int> callback)
        {
            for (int i = 0; i < length; i++)
            {
                callback?.Invoke(i);
            }
        }
        public static void ForReverse(int length, Action<int> callback)
        {
            for (int i = length; i >= 0; i--)
            {
                callback?.Invoke(i);
            }
        }
    }
}
