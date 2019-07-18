using UnityEngine;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// GameObjectの拡張クラス
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// 自身に加え子階層のGameObjectのレイヤーを一度に設定します
        /// </summary>
        public static void SetLayer(this GameObject go, int layer, bool isChildren = true)
        {
            go.layer = layer;

            if (isChildren)
            {
                foreach (Transform childTransform in go.transform)
                {
                    SetLayer(childTransform.gameObject, layer);
                }
            }
        }

        /// <summary>
        /// 指定コンポーネントの存在フラグを返却
        /// </summary>
        public static bool HasComponent<T>(this GameObject go) where T : Component
        {
            return go != null && go.GetComponent<T>() != null;
        }

        /// <summary>
        /// 安全に親子関係を構築する
        /// </summary>
        public static bool SafeSetParent(this GameObject child, Transform parent, bool worldPositionStays = true)
        {
            if (parent != null && child != null)
            {
                return child.transform.SafeSetParent(parent, worldPositionStays);
            }

            return false;
        }

        /// <summary>
        /// GetComponentもしくはAddComponentすることで2重のAddComponentを防ぐ
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (go == null)
            {
                return null;
            }

            if (go.HasComponent<T>())
            {
                return go.GetComponent<T>();
            }

            return go.AddComponent<T>();
        }

    }
}