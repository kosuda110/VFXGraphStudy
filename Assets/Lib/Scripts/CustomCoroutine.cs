using UnityEngine;
using System;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// 指定したフレーム待つ
    /// </summary>
    public class WaitForFrameCount : CustomYieldInstruction
    {

        private int _waitFrameCount;

        private int _elapsedFrame;

        public override bool keepWaiting
        {
            get
            {
                return ++_elapsedFrame <= _waitFrameCount;
            }
        }

        public WaitForFrameCount()
        {
            _elapsedFrame = 0;
            _waitFrameCount = 0;
        }

        public WaitForFrameCount(int waitFrameCount)
        {
            _waitFrameCount = waitFrameCount;
            _elapsedFrame = 0;
        }
    }

    /// <summary>
    /// TimeOutつきWaitUntil
    /// Time.deltaTimeで計算
    /// </summary>
    public class WaitUntilWithTimeOut : CustomYieldInstruction
    {
        private float _timeOut;

        private System.Func<bool> _predicate;

        private float _elapsedTime;

        public override bool keepWaiting
        {
            get
            {
                _elapsedTime += Time.deltaTime;
                return !_predicate() && _elapsedTime < _timeOut;
            }
        }

        public WaitUntilWithTimeOut()
        {
            Debug.LogError("Complete Immediately");
            _timeOut = 0f;
            _predicate = () => true;
            _elapsedTime = 0f;
        }

        public WaitUntilWithTimeOut(Func<bool> predicate, float timeOut)
        {
            _timeOut = timeOut;
            _predicate = predicate;
            _elapsedTime = 0f;
        }
    }
}