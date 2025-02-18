using Game.Commons;
using Patterns;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Serializable]
        public struct AudioEntry
        {
            public string name;
            public AudioClip clip;
        }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioEntry[] _sfxClips;
        [SerializeField] private AudioEntry[] _backgroundMusics;

        [Header("Configs")]
        [SerializeField] private float _fadeSoundDuraton = 1.0f;

        private void Start()
        {
            _bgmSource.volume = Constants.AUDIO_DEFAULT_VOLUMN;
            _sfxSource.volume = Constants.AUDIO_DEFAULT_VOLUMN;
            _bgmSource.loop = true;
            _sfxSource.loop = false;
        }

        public void PlayBGM(AudioClip clip)
        {
            if (_bgmSource.clip == clip && _bgmSource.isPlaying) return;

            StopAllCoroutines(); // Stop any existing fade coroutines
            StartCoroutine(FadeBGM(clip));
        }

        private IEnumerator FadeBGM(AudioClip clip)
        {
            float startVolume = _bgmSource.volume;
            float timer = 0;

            while (timer < _fadeSoundDuraton)
            {
                timer += Time.deltaTime;
                _bgmSource.volume = Mathf.Lerp(startVolume, 0, timer / _fadeSoundDuraton);
                yield return null;
            }

            _bgmSource.Stop();
            _bgmSource.clip = clip;
            _bgmSource.volume = 0;
            _bgmSource.Play();

            timer = 0;
            while (timer < _fadeSoundDuraton)
            {
                timer += Time.deltaTime;
                _bgmSource.volume = Mathf.Lerp(0, Constants.AUDIO_DEFAULT_VOLUMN, timer / _fadeSoundDuraton);
                yield return null;
            }
            _bgmSource.volume = Constants.AUDIO_DEFAULT_VOLUMN; // Ensure it's exactly the target volume
        }

        public void PlayBGM(string name)
        {
            for (int i = 0; i < _backgroundMusics.Length; i++)
            {
                if (string.Equals(_backgroundMusics[i].name, name))
                {
                    PlayBGM(_backgroundMusics[i].clip);
                    return;
                }
            }
            Debug.LogWarning($"BGM with name '{name}' not found.");
        }

        public void PlaySFX(int index, AudioSource audioSource = null, bool replayIfDuplicate = false)
        {
            if (index < 0 || index >= _sfxClips.Length)
            {
                Debug.LogWarning($"SFX index '{index}' out of range.");
                return;
            }

            AudioSource src = audioSource ?? _sfxSource;
            AudioClip clip = _sfxClips[index].clip;

            if (src.isPlaying && src.clip == clip)
            {
                if (!replayIfDuplicate) return;
                src.Stop(); // Immediately stop to replay
                src.volume = Constants.AUDIO_DEFAULT_VOLUMN;
                src.PlayOneShot(clip);

            }
            else
            {
                src.volume = Constants.AUDIO_DEFAULT_VOLUMN;
                src.PlayOneShot(clip);
            }
        }


        public void PlaySFX(string name, AudioSource audioSource = null, bool replayIfDuplicate = false)
        {
            for (int i = 0; i < _sfxClips.Length; i++)
            {
                if (string.Equals(_sfxClips[i].name, name))
                {
                    PlaySFX(i, audioSource, replayIfDuplicate);
                    return;
                }
            }
            Debug.LogWarning($"SFX with name '{name}' not found.");
        }

        public void StopBGM(Action onCompleteCallback = null)
        {
            StopAllCoroutines(); // Stop any existing fade coroutines
            StartCoroutine(StopBGMCoroutine(onCompleteCallback));
        }

        private IEnumerator StopBGMCoroutine(Action onCompleteCallback)
        {
            float startVolume = _bgmSource.volume;
            float timer = 0;

            while (timer < _fadeSoundDuraton)
            {
                timer += Time.deltaTime;
                _bgmSource.volume = Mathf.Lerp(startVolume, 0, timer / _fadeSoundDuraton);
                yield return null;
            }

            _bgmSource.Stop();
            _bgmSource.volume = startVolume; // Reset the volume
            onCompleteCallback?.Invoke();
        }

        public void StopSFX(AudioSource audioSource = null, Action onCompleteCallback = null)
        {
            AudioSource src = audioSource ?? _sfxSource;
            StopCoroutine(StopSFXCoroutine(src, onCompleteCallback));
        }

        private IEnumerator StopSFXCoroutine(AudioSource src, Action onCompleteCallback)
        {
            float startVolume = src.volume;
            float timer = 0;

            while (timer < _fadeSoundDuraton)
            {
                timer += Time.deltaTime;
                src.volume = Mathf.Lerp(startVolume, 0, timer / _fadeSoundDuraton);
                yield return null;
            }

            src.Stop();
            src.volume = startVolume; // Reset the volume

            onCompleteCallback?.Invoke();
        }
    }
}