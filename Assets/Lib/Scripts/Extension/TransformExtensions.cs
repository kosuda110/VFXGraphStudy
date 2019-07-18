using UnityEngine;
using System.Collections.Generic;

namespace Kosu.UnityLibrary
{
    public static class TransformExtensions
    {
        public static void SetGlobalPositionX(this Transform t, float x)
        {
            var p = t.position;
            p.x = x;
            t.position = p;
        }

        public static void SetGlobalPositionY(this Transform t, float y)
        {
            var p = t.position;
            p.y = y;
            t.position = p;
        }

        public static void SetGlobalPositionZ(this Transform t, float z)
        {
            var p = t.position;
            p.z = z;
            t.position = p;
        }

        public static void SetLocalPositionX(this Transform t, float x)
        {
            var p = t.localPosition;
            p.x = x;
            t.localPosition = p;
        }

        public static void SetLocalPositionY(this Transform t, float y)
        {
            var p = t.localPosition;
            p.y = y;
            t.localPosition = p;
        }

        public static void SetLocalPositionZ(this Transform t, float z)
        {
            var p = t.localPosition;
            p.z = z;
            t.localPosition = p;
        }

        /// <summary>
        /// 再帰チェックして指定した名前のTransformを返却する
        /// </summary>
        /// <returns>The recursively.</returns>
        /// <param name="checkTF">Check T.</param>
        /// <param name="searchName">Search name.</param>
        public static Transform SearchRecursively(this Transform checkTF, string searchName, bool isActive = true)
        {
            if (checkTF == null)
            {
                return null;
            }

            Transform targetTF = checkTF.Find(searchName);

            if (targetTF != null &&
                ((isActive && targetTF.gameObject.activeSelf) || isActive == false))
            {
                return targetTF;
            }

            // 再帰チェック(子階層チェック)
            foreach (Transform child in checkTF)
            {
                if (isActive && child.gameObject.activeSelf == false)
                {
                    continue;
                }

                targetTF = SearchRecursively(child, searchName);

                if (targetTF != null)
                {
                    break;
                }
            }

            return targetTF;
        }

        /// <summary>
        /// 子供を全てDestroyする
        /// </summary>
        public static void DestroyChildren(this Transform tr)
        {
            if (tr != null)
            {
                foreach (Transform child in tr)
                {
                    if (child != null)
                    {
                        Object.Destroy(child.gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 子供を全て即座にDestroyする
        /// </summary>
        public static void DestroyImmediateChildren(this Transform tr)
        {
            if (tr != null)
            {
                foreach (Transform child in tr)
                {
                    if (child != null)
                    {
                        Object.DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 安全に親子関係を構築する
        /// </summary>
        public static bool SafeSetParent(this Transform child, Transform parent, bool worldPositionStays = true)
        {
            if (parent != null && child != null)
            {
                child.SetParent(parent, worldPositionStays);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transformのローカル値の初期化
        /// </summary>
        /// <param name="t">T.</param>
        public static void Initialize(this Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localEulerAngles = Vector3.zero;
            t.localScale = Vector3.one;
        }

        /// <summary>
        /// 子ノードを全て取得します
        /// </summary>
        public static Transform[] GetAllChildren(this Transform t)
        {
            List<Transform> children = new List<Transform>();

            foreach (Transform child in t)
            {
                children.Add(child);
            }

            return children.ToArray();
        }
    }
}