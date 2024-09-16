using System;
using System.Collections.Generic;
namespace Gametamin.Core
{
    public static partial class DictionaryHelper
    {
        public static bool SafeContainsKey<T, V>(this Dictionary<T, V> dict, T key)
        {
            // Assert.IsInDictionary(dict, key);
            if (!dict.IsNullObjectSafe())
            {
                return dict.ContainsKey(key);
            }
            return false;
        }
        public static bool SafeContainsKey<K1, K2, V>(this Dictionary<(K1, K2), V> dict, (K1, K2) key)
        {
            // Assert.IsInDictionary(dict, key);
            if (!dict.IsNullObjectSafe())
            {
                return dict.ContainsKey(key);
            }
            return false;
        }
        public static bool TryGetValueSafe<T, V>(this Dictionary<T, V> dict, T key, out V value)
        {
            // Assert.IsInDictionary(dict, key);
            if (!dict.IsNullObjectSafe())
            {
                if (dict.TryGetValue(key, out value))
                {
                    return true;
                }
            }
            value = default;
            return false;
        }
        public static void SafeAdd<T, V>(this Dictionary<T, V> dict, T key, V value)
        {
            if (!dict.IsNullObjectSafe())
            {
                //Assert.IsNotNull(value, $"value of key {key} is null!");
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, value);
                }
            }
        }
        public static void SafeRemove<T, V>(this Dictionary<T, V> dict, T key)
        {
            //Assert.IsNotNull(value, $"value of key {key} is null!");
            if (dict.SafeContainsKey(key))
            {
                dict.Remove(key);
            }
        }
        public static V ForceGet<T, V>(this Dictionary<T, V> dict, T key, bool allowNull = false) where V : new()
        {
            if (!dict.IsNullObjectSafe())
            {
                // Assert.IsInDictionary(dict, key);
                if (dict.ContainsKey(key))
                {
                    return dict[key];
                }
                if (allowNull)
                {
                    return default;
                }
                else
                {
                    var newValue = new V();
                    dict.Add(key, newValue);
                    return newValue;
                }
            }
            return default;
        }
        public static void ForValue<T, V>(this Dictionary<T, V> dict, Action<V> callback = null) where V : new()
        {
            if (!dict.IsNullObjectSafe())
            {
                foreach (var item in dict)
                {
                    callback?.Invoke(item.Value);
                }
            }
        }
        /// <summary>
        /// return values and index
        /// </summary>
        public static void ForValue<T, V>(this Dictionary<T, V> dict, Action<V, int> callback = null) where V : new()
        {
            if (!dict.IsNullObjectSafe())
            {
                int index = 0;
                foreach (var item in dict)
                {
                    callback?.Invoke(item.Value, index++);
                }
            }
        }
        public static void ForKeys<T, V>(this Dictionary<T, V> dict, Action<T> callback = null) where V : new()
        {
            // Assert.IsNotNullOrEmpty(dict);
            if (!dict.IsNullObjectSafe())
            {
                foreach (var item in dict)
                {
                    callback?.Invoke(item.Key);
                }
            }
        }
        /// <summary>
        /// return keys and index
        /// </summary>
        public static void ForKeys<T, V>(this Dictionary<T, V> dict, Action<T, int> callback = null) where V : new()
        {
            // Assert.IsNotNullOrEmpty(dict);
            if (!dict.IsNullObjectSafe())
            {
                int index = 0;
                foreach (var item in dict)
                {
                    callback?.Invoke(item.Key, index++);
                }
            }
        }
        public static T GetEnum<T>(this Dictionary<string, object> dict, string key) where T : struct
        {
            if (dict.SafeContainsKey(key))
            {
                return dict[key].ToString().ToEnum<T>();
            }
            return default;
        }
        public static string GetString(this Dictionary<string, object> dict, string key)
        {
            if (dict.SafeContainsKey(key))
            {
                return dict[key].ToString();
            }
            return default;
        }
        public static int GetInt(this Dictionary<string, object> dict, string key)
        {
            if (dict.SafeContainsKey(key))
            {
                return dict[key].ToString().ToIntSafe();
            }
            return default;
        }
        public static float GetFloat(this Dictionary<string, object> dict, string key)
        {
            if (dict.SafeContainsKey(key))
            {
                return dict[key].ToString().ToFloatSafe();
            }
            return default;
        }
        public static int GetSimilarKeyCount(this Dictionary<string, object> dict, string keySimilar)
        {
            int count = 0;
            dict.ForKeys((key) =>
            {
                if (key.ContainsSafe(keySimilar))
                {
                    count++;
                }
            });
            return count;
        }
        public static void GetSimilarKeys(this Dictionary<string, object> dict, string keySimilar, Func<int, string> onGetKey, Action<string> callback = null)
        {
            var rewardCount = dict.GetSimilarKeyCount(keySimilar);
            rewardCount.For((index) =>
            {
                var rewardString = dict.GetString(onGetKey?.Invoke(index));
                if (!string.IsNullOrEmpty(rewardString))
                {
                    callback?.Invoke(rewardString);
                }
            });
        }
        public static void GetSimilarKeys(this Dictionary<string, object> dict, string keySimilar, Func<int, string> onGetKey, Action<string, int> callback = null, bool evenIfNotFound = false)
        {
            var rewardCount = dict.GetSimilarKeyCount(keySimilar);
            rewardCount.For((index) =>
            {
                var rewardString = dict.GetString(onGetKey?.Invoke(index));
                if (evenIfNotFound || !string.IsNullOrEmpty(rewardString))
                {
                    callback?.Invoke(rewardString, index);
                }
            });
        }
    }
}