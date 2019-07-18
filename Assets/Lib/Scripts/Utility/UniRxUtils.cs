using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Kosu.UnityLibrary
{
    public static class UniRxUtils
    {

        /// <summary>
        /// UniRx式コルーチン発動
        /// </summary>
        public static System.IDisposable StartCoroutine(System.Func<IEnumerator> func, System.Action onComplete = null)
        {
            return Observable.FromCoroutine(func).Subscribe(_ => onComplete.SafeInvoke());
        }

        /// <summary>
        /// Input.GetKeyDown監視
        /// </summary>
        public static System.IDisposable ObserveInputKeyDown(KeyCode code, System.Action callback, GameObject disposeTarget = null)
        {
            var disposable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(code)).Subscribe(_ => callback.SafeInvoke());

            if (disposeTarget != null)
            {
                disposable.AddTo(disposeTarget);
            }

            return disposable;
        }

        /// <summary>
        /// Input.GetKeyDown監視
        /// </summary>
        public static System.IDisposable ObserveInputKeyDown(KeyCode code1, KeyCode code2, System.Action callback, GameObject disposeTarget = null)
        {
            var disposable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(code1) || Input.GetKeyDown(code2)).Subscribe(_ => callback.SafeInvoke());

            if (disposeTarget != null)
            {
                disposable.AddTo(disposeTarget);
            }

            return disposable;
        }

        /// <summary>
        /// Input.GetKeyUp監視
        /// </summary>
        public static System.IDisposable ObserveInputKeyUp(KeyCode code, System.Action callback, GameObject disposeTarget = null)
        {
            var disposable = Observable.EveryUpdate().Where(_ => Input.GetKeyUp(code)).Subscribe(_ => callback.SafeInvoke());

            if (disposeTarget != null)
            {
                disposable.AddTo(disposeTarget);
            }

            return disposable;
        }

        /// <summary>
        /// Input.GetKey監視
        /// </summary>
        public static System.IDisposable ObserveInputKey(KeyCode code, System.Action callback, GameObject disposeTarget = null)
        {
            var disposable = Observable.EveryUpdate().Where(_ => Input.GetKey(code)).Subscribe(_ => callback.SafeInvoke());

            if (disposeTarget != null)
            {
                disposable.AddTo(disposeTarget);
            }

            return disposable;
        }

        /// <summary>
        /// Input.GetMouseButton監視
        /// </summary>
        public static System.IDisposable ObserveGetMouseButtonDown(int button, System.Action callback, GameObject disposeTarget = null)
        {
            var disposable = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(button)).Subscribe(_ => callback.SafeInvoke());

            if (disposeTarget != null)
            {
                disposable.AddTo(disposeTarget);
            }

            return disposable;
        }

    }
}
