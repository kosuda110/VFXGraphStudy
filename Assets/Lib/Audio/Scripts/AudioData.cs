using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Kosu.UnityLibrary
{
    public class AudioData : MonoBehaviour
    {

        [SerializeField]
        private AudioSource _source = null;

        [SerializeField]
        private bool _is3DAudioData = false;

        private bool _isStopping;

        public string ClipName
        {
            get
            {
                if (_source == null)
                {
                    return "";
                }

                return _source.clip.name;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public bool Is3DAudioData
        {
            get
            {
                return _is3DAudioData;
            }
        }

        public void Setup(AudioMixerGroup group, bool loop = false)
        {
            _source.outputAudioMixerGroup = group;
            _source.playOnAwake = false;
            _source.loop = loop;
        }

        public void Play(AudioClip clip, bool withFade = false)
        {
            _source.clip = clip;
            // default setting
            _source.pitch = 1;
            _source.maxDistance = 500;
            _source.volume = withFade ? 0 : 1;
            _source.Play();

            if (withFade)
            {
                FadeIn(1f);
            }
        }

        public void Play(AudioClip clip, float pitch, float duration, float maxDistance, float vol)
        {
            _source.clip = clip;
            _source.pitch = pitch;
            _source.maxDistance = maxDistance;
            _source.volume = vol;
            _source.Play();

            if (_source.loop == false || duration <= 0f)
            {
                return;
            }

            StartCoroutine(AutoStop(duration));
        }

        private IEnumerator AutoStop(float sec)
        {
            yield return new WaitForSeconds(sec);
            yield return _FadeOut(0.4f);
        }

        public void PlayOneShot(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }

        public void PlayDelayed(AudioClip clip, float delay)
        {
            _source.clip = clip;
            _source.PlayDelayed(delay);
        }

        public bool IsPlaying()
        {
            if (_source.clip == null ||
                _isStopping)
            {
                // 今作成したばっかのもの
                // or fade out中
                return true;
            }

            return _source.time < _source.clip.length && _source.isPlaying;
        }

        public void Stop()
        {
            _source.Stop();
        }

        public void FadeIn(float fadeTime)
        {
            StartCoroutine(_FadeIn(fadeTime));
        }

        private IEnumerator _FadeIn(float fadeTime)
        {
            _source.volume = 0f;

            while (_source.volume < 1f)
            {
                _source.volume += 1f * Time.deltaTime / fadeTime;
                yield return null;
            }

            _source.volume = 1f;
        }

        public void FadeOut(float fadeTime)
        {
            StartCoroutine(_FadeOut(fadeTime));
        }

        private IEnumerator _FadeOut(float fadeTime)
        {
            float startVol = _source.volume;
            _isStopping = true;

            while (_source.volume > 0f)
            {
                _source.volume -= startVol * Time.deltaTime / fadeTime;
                yield return null;
            }

            _isStopping = false;
            _source.Stop();
            _source.volume = startVol;
        }

    }
}
