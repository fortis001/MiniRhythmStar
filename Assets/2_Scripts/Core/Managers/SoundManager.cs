using System.Collections.Generic;
using System.IO;
using MyGame.Common.DataFormat;
using UnityEngine;

namespace MyGame.Core.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {

        [Header("Data Source")]
        [SerializeField] private SoundData _soundData;

        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;


        private Dictionary<SFXID, AudioClip> _sfxDict = new();
        private Dictionary<BGMID, AudioClip> _bgmDict = new();

        private Dictionary<string, AudioClip> _previewCache = new();
        private string _latestRequestPath;

        public float CurrentTime => _bgmSource.time;
        public bool IsPlaying => _bgmSource.isPlaying;
        public float ClipLength => _bgmSource.clip != null ? _bgmSource.clip.length : 0f;
        public AudioClip CurrentClip => _bgmSource.clip;


        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            if (_soundData == null)
            {
                Debug.LogError("SoundData가 SoundManager에 할당되지 않았습니다!");
                return;
            }

            foreach (var entry in _soundData.sfxList) _sfxDict[entry.id] = entry.clip;
            foreach (var entry in _soundData.bgmList) _bgmDict[entry.id] = entry.clip;
        }


        // --- BGM 관련 ---
        public void SetClip(AudioClip clip, bool loop = false)
        {
            _bgmSource.Stop();
            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
        }

        public void PlayBGM(float startTime = 0f)
        {
            if (_bgmSource.clip == null) return;
            _bgmSource.time = startTime;
            _bgmSource.Play();
        }

        public void PlayBGM(BGMID id, bool loop = true)
        {
            if (!_bgmDict.TryGetValue(id, out AudioClip nextClip))
            {
                if (id != BGMID.None) Debug.LogWarning($"{id}에 해당하는 BGM이 없습니다.");
                StopBGM();
                return;
            }

            if (_bgmSource.clip == nextClip && _bgmSource.isPlaying) return;

            SetClip(nextClip, loop);
            PlayBGM();
        }

        public void ResumeBGM()
        {
            _bgmSource.Play();
        }

        public void PauseBGM()
        {
            _bgmSource.Pause();
        }

        public void StopBGM()
        {
            _bgmSource?.Stop();
        }

        public async void PlayPreview(string rawPath, float startTime)
        {
            string path = Path.Combine(rawPath, "source.ogg");
            _latestRequestPath = path;

            if (!_previewCache.TryGetValue(path, out AudioClip clip))
            {
                clip = await AudioLoader.LoadClip(path);
                if (clip != null && !_previewCache.ContainsKey(path))
                    _previewCache.Add(path, clip);
            }

            if (_latestRequestPath != path) return;

            SetClip(clip, loop: true);
            PlayBGM(startTime);
        }


        // --- SFX 관련 ---
        public void PlaySFX(SFXID id)
        {
            if (_sfxDict.TryGetValue(id, out AudioClip clip))
            {
                // SFX 전용 AudioSource에서 PlayOneShot 실행
                _sfxSource.PlayOneShot(clip);
            }
        }

        // --- 기타 기능 ---
        public void SeekTo(float time)
        {
            _bgmSource.time = Mathf.Clamp(time, 0f, _bgmSource.clip.length);
        }
    }
}

