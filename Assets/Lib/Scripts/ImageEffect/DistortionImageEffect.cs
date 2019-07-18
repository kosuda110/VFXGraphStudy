using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Kosu.UnityLibrary
{
    [ExecuteInEditMode]
    public class DistortionImageEffect : MyImageEffectBase
    {

        [SerializeField]
        private float _frequency = 0;

        [SerializeField]
        private float _amplitude = 0.03f;

        [SerializeField]
        private float _phase = 0;

        private Tween _loopAnim;

        private void Awake()
        {
            var cam = GetComponent<Camera>();
            cam.depthTextureMode |= DepthTextureMode.Depth;
        }

        public void StartAnimation()
        {
            if (_loopAnim == null)
            {
                _loopAnim = DOTween.To(() => _phase, (x) => _phase = x, 10f, 10f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
                return;
            }

            if (_loopAnim.IsPlaying())
            {
                return;
            }

            _loopAnim.Play();
        }

        public void StopAnimation()
        {
            if (_loopAnim == null)
            {
                return;
            }

            if (!_loopAnim.IsPlaying())
            {
                return;
            }

            _loopAnim.Pause();
        }

        public void SetAmplitude(float amplitude)
        {
            _amplitude = amplitude;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Mat.SetFloat("_Frequency", _frequency);
            Mat.SetFloat("_Amplitude", _amplitude);
            Mat.SetFloat("_Phase", _phase);
            Graphics.Blit(source, destination, Mat);
        }

    }
}
