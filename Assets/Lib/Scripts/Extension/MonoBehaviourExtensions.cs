using UnityEngine;
using System.Collections;
using System;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// Monobehaviourのextensions.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Shaderの再設定
        /// 注意：Editor Only
        ///
        /// メモ：
        /// http://docs.unity3d.com/ScriptReference/Shader.Find.html
        /// "Shader.Find will work only in the editor"
        ///
        /// </summary>
        /// <param name="go">Go.</param>
        private static void ShaderFind(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                foreach (var mt in renderer.materials)
                {
                    mt.shader = Shader.Find(mt.shader.name);
                }
            }
        }

        /// <summary>
        /// SetActive
        /// </summary>
        /// <param name="behaviour">Behaviour.</param>
        /// <param name="flg">If set to <c>true</c> flg.</param>
        public static void SetActive(this MonoBehaviour behaviour, bool flg)
        {
            if (behaviour != null)
            {
                behaviour.gameObject.SetActive(flg);
            }
        }

        /// <summary>
        /// 待機して実行
        /// キャンセル処理をしたい場合はDelayedCall.Executeを使ってください
        /// </summary>
        public static IEnumerator DelayedCall(this MonoBehaviour m, float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.SafeInvoke();
        }

        public static Coroutine FrameDelayedCall(this MonoBehaviour m, Action action, int frame)
        {
            return m.StartCoroutine(_FrameDelayedCall(action, frame));
        }

        private static IEnumerator _FrameDelayedCall(Action action, int frame)
        {
            while (frame > 0)
            {
                yield return null;
                frame--;
            }

            action.SafeInvoke();
        }
    }
}
