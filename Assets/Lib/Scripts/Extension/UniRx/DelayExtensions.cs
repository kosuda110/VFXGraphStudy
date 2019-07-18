using System;
using UnityEngine;
using UniRx.Triggers;
using Kosu.UnityLibrary;
namespace UniRx
{
    public static class DelayExtensions
    {
        /// <summary>
        /// フレームでの遅延判定用Observer
        /// </summary>
        /// <param name="delayFrame">delay frame.</param>
        private static IObservable<Unit> OnDelayAsObservable(this GameObject gameObject, int delayFrame)
        {
            return gameObject.UpdateAsObservable()
                   .DelayFrame(delayFrame)
                   .Take(1);
        }

        public static void Delay(this GameObject gameObject, int delayFrame, Action onComplete)
        {
            gameObject.OnDelayAsObservable(delayFrame)
            .Subscribe(_ =>
            {
                onComplete.SafeInvoke();
            }).AddTo(gameObject);
        }
    }
}