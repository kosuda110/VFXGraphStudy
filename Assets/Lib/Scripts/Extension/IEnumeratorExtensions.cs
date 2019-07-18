using UnityEngine;
using System.Collections;

namespace Kosu.UnityLibrary
{
    public static class IEnumeratorExtensions
    {
        public static T Result<T>(this IEnumerator target) where T : class
        {
            return target.Current as T;
        }

        public static T ResultValueType<T>(this IEnumerator target) where T : struct
        {
            return (T)target.Current;
        }

    }
}