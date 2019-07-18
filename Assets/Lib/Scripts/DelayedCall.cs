using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// 遅延実行ユーティルクラス
    /// </summary>
    public class DelayedCall
    {
        /// <summary>
        /// 実行中のDelayedCallを保持
        /// </summary>
        private static List<DelayedCallData> _delayedCallDataList = new List<DelayedCallData>();

        /// <summary>
        /// 遅延実行
        /// 指定した秒数waitします
        /// キャンセル処理をしたい場合は戻り値のIDを引数にしてCancelメソッドを実行してください
        /// </summary>
        public static int Execute(float delay, Action action, MonoBehaviour invoker)
        {
            if (invoker == null || invoker.isActiveAndEnabled == false)
            {
                return -1;
            }

            if (delay <= 0f)
            {
                action.SafeInvoke();
                return -1;
            }

            var data = new DelayedCallData(delay, action, invoker);
            _delayedCallDataList.Add(data);
            data.coroutine = invoker.StartCoroutine(_Execute(data));
            return data.id;
        }

        /// <summary>
        /// 遅延実行
        /// 指定したフレーム数waitします
        /// キャンセル処理をしたい場合は戻り値のIDを引数にしてCancelメソッドを実行してください
        /// </summary>
        public static int ExecuteFrameDelay(int delayFrame, Action action, MonoBehaviour invoker)
        {
            if (invoker == null || invoker.isActiveAndEnabled == false)
            {
                return -1;
            }

            var data = new DelayedCallData(delayFrame, action, invoker);
            _delayedCallDataList.Add(data);
            data.coroutine = invoker.StartCoroutine(_Execute(data));
            return data.id;
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        public static void Cancel(int id)
        {
            var data = _delayedCallDataList.FirstOrDefault(x => x.id == id);

            if (data != null)
            {
                if (data.invoker != null && data.coroutine != null)
                {
                    data.invoker.StopCoroutine(data.coroutine);
                }

                _delayedCallDataList.Remove(data);
            }
        }

        private static IEnumerator _Execute(DelayedCallData data)
        {
            if (data.type == WaitType.FRAME)
            {
                yield return new WaitForFrameCount(data.frameDelay);
            }
            else
            {
                yield return new WaitForSeconds(data.delay);
            }

            if (data.invoker != null)
            {
                data.action.SafeInvoke();
            }

            _delayedCallDataList.Remove(data);
        }

        private enum WaitType
        {
            SECOND,
            FRAME,
        }

        class DelayedCallData
        {
            private static int _delayedId = 0;
            public WaitType type;
            public int id;
            public float delay = 0f;
            public int frameDelay = 0;
            public Action action;
            public MonoBehaviour invoker;
            public Coroutine coroutine;

            /// <summary>
            /// 秒数指定遅延実行データ
            /// </summary>
            public DelayedCallData(float delay, Action action, MonoBehaviour invoker)
            {
                id = _delayedId++;
                this.delay = delay;
                this.action = action;
                this.invoker = invoker;
                this.type = WaitType.SECOND;
            }

            /// <summary>
            /// フレーム指定遅延実行データ
            /// </summary>
            public DelayedCallData(int delay, Action action, MonoBehaviour invoker)
            {
                id = _delayedId++;
                this.frameDelay = delay;
                this.action = action;
                this.invoker = invoker;
                this.type = WaitType.FRAME;
            }
        }
    }
}
