using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace ridesync
{
    public class StatsFPS : MonoBehaviour
    {

        private int _frameCount;

        private float _prevTime;

        private float _fps;

        private System.IDisposable _updateStream;

        [SerializeField]
        private Text _fpsText = null;

        private void OnEnable()
        {
            _updateStream = Observable.EveryUpdate().Subscribe(_ =>
            {
                CountFPS();
            });
        }

        private void OnDisable()
        {
            if (_updateStream == null)
            {
                return;
            }

            _updateStream.Dispose();
            _updateStream = null;
        }

        private void CountFPS()
        {
            ++_frameCount;
            float time = Time.realtimeSinceStartup - _prevTime;

            if (time > 0.5f)
            {
                _fps = _frameCount / time;
                _frameCount = 0;
                _prevTime = Time.realtimeSinceStartup;
            }

            _fpsText.text = string.Format("FPS : {0}", _fps.ToString("F1"));
        }

    }
}
