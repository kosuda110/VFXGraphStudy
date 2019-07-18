using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Kosu.UnityLibrary
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// ランダムで取得する
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="target">Target.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T RandomValue<T>(this IEnumerable<T> target) where T : class
        {
            if (target == null)
            {
                return null;
            }

            var array = target.ToArray();
            int len = array.Length;

            if (len == 0)
            {
                return null;
            }

            int index = Random.Range(0, len);

            return array[index];
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> target)
        {
            return (target == null || target.Any() == false);
        }

        public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
        {
            if (list == null || action == null)
            {
                return;
            }

            foreach (var v in list)
            {
                action(v);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, System.Action<T, int> action)
        {
            if (source == null || action == null)
            {
                return;
            }

            int i = 0;

            foreach (var v in source)
            {
                action(v, i);
                i++;
            }
        }

        public static T RandomSelect<T>(this IEnumerable<T> list, System.Random random)
        {
            if (list == null)
            {
                return default(T);
            }

            var array = list.ToArray();
            int len = array.Length;

            if (len == 0)
            {
                return default(T);
            }

            if (random == null)
            {
                return array[0];
            }

            return array[random.Next(0, len)];
        }

        /// <summary>
        /// 規定値を利用者が指定できるFirstOrDefault
        /// </summary>
        public static T FirstOrDefault<T>(this IEnumerable<T> source, System.Predicate<T> pred, T defaultValue)
        {
            foreach (var ie in source)
            {
                if (pred(ie))
                {
                    return ie;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// コレクションの中からランダムで要素を１つ取得
        /// 失敗したらdefaultを返す
        /// </summary>
        public static T RandomOrDefault<T>(this IEnumerable<T> source)
        {
            int length = source.Count();
            int index = Random.Range(0, length);
            return source.ElementAtOrDefault(index);
        }

        /// <summary>
        /// 比較ルールを利用者が決められるExcept
        /// </summary>
        public static IEnumerable<T1> Except<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2, System.Func<T1, T2, bool> comparer)
        {
            foreach (var ie1 in source1)
            {
                if (source2.Any(x => comparer(ie1, x)) == false)
                {
                    yield return ie1;
                }
            }
        }

        public static IEnumerable<T1> Intersect<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2, System.Func<T1, T2, bool> comparer)
        {
            foreach (var ie in source1)
            {
                if (source2.Any(x => comparer(ie, x)))
                {
                    yield return ie;
                }
            }
        }

        /// <summary>
        /// nullを除外したシーケンスを取得
        /// sourceが値型のコレクションの場合はそのまま返す
        /// </summary>
        public static IEnumerable<T> RemoveNull<T>(this IEnumerable<T> source)
        {
            if (typeof(T).IsValueType)
            {
                foreach (var ie in source)
                {
                    yield return ie;
                }
            }
            else
            {
                foreach (var ie in source)
                {
                    if (ie == null)
                    {
                        continue;
                    }

                    yield return ie;
                }
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T item)
        {
            foreach (var ie in src)
            {
                yield return ie;
            }

            yield return item;
        }

        /// <summary>
        /// 末尾count個を返す
        /// </summary>
        /// <returns>The last.</returns>
        /// <param name="src">Source.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> src, int count)
        {
            int length = src.Count();

            for (int i = 0; i < length; i++)
            {
                if (length - i <= count)
                {
                    yield return src.ElementAt(i);
                }
            }
        }

        /// <summary>
        /// 引数にnullを許可するSequenceEqual
        /// </summary>
        /// <returns><c>true</c>, if sequence equal was nullabled, <c>false</c> otherwise.</returns>
        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool NullableSequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first.IsNullOrEmpty())
            {
                return second.IsNullOrEmpty();
            }

            if (second.IsNullOrEmpty())
            {
                return first.IsNullOrEmpty();
            }

            return first.SequenceEqual(second);
        }

        private sealed class CommonSelector<T, TKey> : IEqualityComparer<T>
        {
            private System.Func<T, TKey> m_selector;

            public CommonSelector(System.Func<T, TKey> selector)
            {
                m_selector = selector;
            }

            public bool Equals(T x, T y)
            {
                return m_selector(x).Equals(m_selector(y));
            }

            public int GetHashCode(T obj)
            {
                return m_selector(obj).GetHashCode();
            }
        }

        /// <summary>
        /// Distinctの引数に条件式を追加
        /// </summary>
        public static IEnumerable<T> Distinct<T, TKey>(
            this IEnumerable<T> source,
            System.Func<T, TKey> selector
        )
        {
            return source.Distinct(new CommonSelector<T, TKey>(selector));
        }

    }
}