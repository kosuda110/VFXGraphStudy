using UnityEngine;
using System.Collections.Generic;

namespace Kosu.UnityLibrary
{
    public static class IListExtensions
    {
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return (list == null || list.Count == 0);
        }

        public static bool IsNotNullOrEmpty<T>(this IList<T> list)
        {
            return !IsNullOrEmpty(list);
        }

        public static bool HasValue<T>(this IList<T> list)
        {
            return !IsNullOrEmpty(list);
        }

        public static T[] FindAll<T>(this IList<T> list, System.Predicate<T> match)
        {
            if (list == null || match == null)
            {
                return null;
            }

            List<T> newList = new List<T>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    newList.Add(list[i]);
                }
            }

            return newList.ToArray();
        }

        public static void ForEach<T>(this IList<T> list, System.Action<T, int> action)
        {
            if (action == null || list.IsNullOrEmpty())
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }

        public static T RandomSelect<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            return list[Random.Range(0, list.Count)];
        }

        public static T RandomSelect<T>(this IList<T> list, System.Random random)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }

            if (random == null || list.Count == 1)
            {
                return list[0];
            }

            return list[random.Next(0, list.Count)];
        }


        public static T[] Shuffle<T>(this IList<T> list)
        {
            if (list == null)
            {
                return null;
            }

            T[] newList = new T[list.Count];
            list.CopyTo(newList, 0);
            T val;
            int k, n = newList.Length;

            while (n > 1)
            {
                --n;
                k = Random.Range(0, n + 1);
                val = newList[k];
                newList[k] = newList[n];
                newList[n] = val;
            }

            return newList;
        }

        public static T[] Shuffle<T>(this IList<T> list, System.Random random)
        {
            if (list == null)
            {
                return null;
            }

            T[] newList = new T[list.Count];
            list.CopyTo(newList, 0);

            if (random == null)
            {
                return newList;
            }

            T val;
            int k, n = newList.Length;

            while (n > 1)
            {
                --n;
                k = random.Next(0, n + 1);
                val = newList[k];
                newList[k] = newList[n];
                newList[n] = val;
            }

            return newList;
        }


        /// <summary>
        /// insertItemがnullの場合は追加されない
        /// </summary>
        public static bool SafeInsert<T>(this IList<T> list, int index, T insertItem)
        {
            if ((!typeof(T).IsValueType && Equals(insertItem, default(T))))
            {
                // null check
                Debug.LogError("Insert Item is NULL.");
                return false;
            }

            if (list == null)
            {
                return false;
            }

            list.Insert(index, insertItem);
            return true;
        }

        public static bool SafeAddRange<T>(this IList<T> list, IEnumerable<T> addList)
        {
            if (list == null || addList == null)
            {
                return false;
            }

            foreach (var item in addList)
            {
                list.Add(item);
            }

            return true;
        }

        /// <summary>
        /// addItemがnullの場合は追加されない.
        /// </summary>
        /// <returns><c>true</c>, if except null was added, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="addItem">Add item.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool AddExceptNull<T>(this IList<T> list, T addItem)
        {
            if ((!typeof(T).IsValueType && Equals(addItem, default(T))))
            {
                Debug.LogError("AddItem is NULL.");
                return false;
            }

            if (list == null)
            {
                return false;
            }

            list.Add(addItem);
            return true;
        }

        /// <summary>
        /// 取得するIndexが範囲外だった場合はnullを返却します
        /// </summary>
        /// <returns>The safe value.</returns>
        /// <param name="list">List.</param>
        /// <param name="index">Index.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetSafeValue<T>(this IList<T> list, int index) where T : class
        {
            if (list.IsNullOrEmpty() || index < 0)
            {
                return null;
            }

            return index >= list.Count ? null : list[index];
        }

        /// <summary>
        /// 取得するIndexが範囲外だった場合はnullを返却します
        /// </summary>
        public static T GetSafeValue<T>(this IList<T> list, int index, T defaultValue) where T : struct
        {
            if (list.IsNullOrEmpty())
            {
                return defaultValue;
            }

            return (index >= list.Count || index < 0) ? defaultValue : list[index];
        }

        /// <summary>
        /// Reverse済みリストを返却します
        /// </summary>
        public static List<T> GetReverseList<T>(this IList<T> list)
        {
            if (list == null)
            {
                return null;
            }

            var newList = new List<T>(list);
            newList.Reverse();
            return newList;
        }
    }
}
