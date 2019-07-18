using System;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// System.Actionの拡張クラス
    /// </summary>
    public static class ActionExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        public static void SafeInvoke<T>(this Action<T> action, T arg)
        {
            if (action != null)
            {
                action(arg);
            }
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action != null)
            {
                action(arg1, arg2);
            }
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (action != null)
            {
                action(arg1, arg2, arg3);
            }
        }
    }
}
