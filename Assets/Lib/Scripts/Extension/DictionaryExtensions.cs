using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// Dictionary型の拡張
    /// </summary>
    public static class DictionaryExtensions
    {
        public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> dic)
        {
            return dic == null || dic.Count <= 0;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source == null || source.Count == 0)
            {
                return default(TValue);
            }

            TValue result;

            if (source.TryGetValue(key, out result))
            {
                return result;
            }

            return default(TValue);
        }

        /// <summary>
        /// 安全にTryGetValueを実行
        /// </summary>
        public static bool SafeTryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue value)
        {
            if (!typeof(TKey).IsValueType && Equals(key, default(TKey)))
            {
                Debug.LogError("key is null value.");
                value = default(TValue);
                return false;
            }

            if (source == null)
            {
                value = default(TValue);
                return false;
            }

            return source.TryGetValue(key, out value);
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, System.Action<TKey, TValue, int> action)
        {
            if (action == null || source == null)
            {
                return;
            }

            int index = 0;

            foreach (KeyValuePair<TKey, TValue> item in source)
            {
                action(item.Key, item.Value, index);
                index += 1;
            }
        }

        /// <summary>
        /// 安全にDictionaryにAddします
        /// 既に存在する場合は上書きます
        /// </summary>
        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (!typeof(TKey).IsValueType && Equals(key, default(TKey)))
            {
                Debug.LogError("key is null value.");
                return;
            }

            if (source == null)
            {
                return;
            }

            if (source.ContainsKey(key) == false)
            {
                source.Add(key, value);
            }
            else
            {
                source[key] = value;
            }
        }

        /// <summary>
        /// 安全にDictionaryにRemoveします
        /// Keyがnullでも例外発生しません
        /// </summary>
        public static bool SafeRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (!typeof(TKey).IsValueType && Equals(key, default(TKey)))
            {
                Debug.LogError("key is null value.");
                return false;
            }

            if (source == null)
            {
                return false;
            }

            return source.Remove(key);
        }

        /// <summary>
        /// String型に変換
        /// ログ出力時などに使う
        /// </summary>
        public static string ToStringText<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            if (source == null)
            {
                return null;
            }

            string str = null;

            foreach (var dic in source)
            {
                str += "[" + dic.Key + " : " + dic.Value + "]";
            }

            return str;
        }
    }

}