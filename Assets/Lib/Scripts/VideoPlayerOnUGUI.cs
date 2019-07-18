using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

namespace Kosu.UnityLibrary
{
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerOnUGUI : MonoBehaviour
    {

        private static readonly string RESOURCES_PATH = "Videos/";

        private RawImage _image;

        public RawImage Image { get { return _image; } }

        public Vector2 Size { get { return _selfRectTR.sizeDelta; } }

        public Texture Tex { get { return _image.mainTexture; } }

        private VideoPlayer _vp;

        private int _loopBackFrame;

        private bool _isInited;

        private RectTransform _selfRectTR;

        // play->EndOfFrameで実行する関数
        private System.Action _onEndOfFrame;

        private bool _isStopping;

        private void OnEnable()
        {
            Init();
            _vp.loopPointReached += OnEndOfFrame;
        }

        private void OnDisable()
        {
            _vp.loopPointReached -= OnEndOfFrame;
        }

        private void Init()
        {
            if (_isInited)
            {
                return;
            }

            _vp = gameObject.GetOrAddComponent<VideoPlayer>();
            _image = gameObject.GetOrAddComponent<RawImage>();
            _selfRectTR = gameObject.GetOrAddComponent<RectTransform>();
            _isInited = true;
        }

        private void SetupVideo(string clipName,
                                bool loop,
                                int loopBackFrame)
        {
            if (!_isInited)
            {
                Init();
            }

            // video player
            var clip = Resources.Load<VideoClip>(RESOURCES_PATH + clipName);

            if (clip == null)
            {
                Debug.LogError("Not Found VideoClip : path = " + RESOURCES_PATH + clipName);
                return;
            }

            _vp.clip = clip;
            _vp.playOnAwake = false;
            _vp.isLooping = loop;
            _loopBackFrame = loopBackFrame;
            // raw image
            _image.DOComplete();
            _image.SetAlpha(0);
        }

        public void PlayVideo(string clipName,
                              bool loop,
                              int loopBackFrame,
                              bool withFade = false,
                              int endOfFrame = -1,
                              System.Action onStartOfFrame = null,
                              System.Action onEndOfFrame = null)
        {
            if (_vp.isPlaying)
            {
                return;
            }

            SetupVideo(clipName, loop, loopBackFrame);

            if (loop)
            {
                StartCoroutine(_PlayVideo(withFade, onStartOfFrame));
            }
            else
            {
                _onEndOfFrame = onEndOfFrame;
                StartCoroutine(_PlayOneShotVideo(withFade, endOfFrame, onStartOfFrame, onEndOfFrame));
            }
        }

        public void StopVideo(System.Action onStopped = null, bool withFade = false)
        {
            _isStopping = true;

            if (withFade)
            {
                _image.DOComplete();
                _image.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    if (_vp.isPlaying)
                    {
                        _vp.Stop();
                    }

                    _image.SetAlpha(0f);
                    onStopped.SafeInvoke();
                    _isStopping = false;
                });
            }
            else
            {
                if (_vp.isPlaying)
                {
                    _vp.Stop();
                }

                _image.SetAlpha(0f);
                onStopped.SafeInvoke();
                _isStopping = false;
            }
        }

        public bool TrySkipVideo(int frame = -1)
        {
            if (_isStopping || !_vp.isPlaying)
            {
                return false;
            }

            _image.DOComplete();
            _image.SetAlpha(1f);
            _vp.Pause();

            if (frame < 0)
            {
                frame = (int)_vp.frameCount;
            }

            _vp.frame = frame;
            _onEndOfFrame.SafeInvoke();
            return true;
        }

        private IEnumerator _PlayOneShotVideo(bool withFade, int endOfFrame, System.Action onStartOfFrame, System.Action onEndOfFrame)
        {
            yield return _PlayVideo(withFade, onStartOfFrame);
            int frameCount = 0;

            while (_vp.isPlaying)
            {
                if (endOfFrame > 0 && frameCount > endOfFrame)
                {
                    yield return null;
                    break;
                }

                frameCount++;
                yield return null;
            }

            _onEndOfFrame.SafeInvoke();
        }

        private IEnumerator _PlayVideo(bool withFade, System.Action onStartOfFrame)
        {
            _vp.Prepare();

            while (!_vp.isPrepared)
            {
                yield return null;
            }

            _vp.Play();
            _image.texture = _vp.texture;
            _selfRectTR.SetSize(_vp.texture.width, _vp.texture.height);
            onStartOfFrame.SafeInvoke();

            if (withFade)
            {
                _image.DOComplete();
                _image.DOFade(1f, 0.5f);
            }
            else
            {
                _image.SetAlpha(1);
            }
        }

        private void OnEndOfFrame(VideoPlayer vp)
        {
            if (!vp.isLooping)
            {
                return;
            }

            _vp.frame = _loopBackFrame;
        }

        public Texture2D GetNowTexture2D()
        {
            Texture mainTexture = _image.mainTexture;
            Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
            // mainTexture のピクセル情報を renderTexture にコピー
            Graphics.Blit(mainTexture, renderTexture);
            // renderTexture のピクセル情報を元に texture2D のピクセル情報を作成
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }

    }
}
