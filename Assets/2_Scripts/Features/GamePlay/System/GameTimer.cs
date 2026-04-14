using System;
using MyGame.Core.Managers;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    private float _timer;
    private bool _isSongStarted;
    private float _noteTravelTime;

    private bool _isPlaying;

    public float CurrentTime => _isSongStarted && SoundManager.Instance.IsPlaying
        ? SoundManager.Instance.CurrentTime
        : _timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    public void StartTimer()
    {
        _isPlaying = true;

        _noteTravelTime = GameManager.Instance.NoteTravelTime;
        _timer = -_noteTravelTime;

        _isSongStarted = false;
    }

    private void Update()
    {
        if (_isSongStarted) return;
        if (!_isPlaying) return;

        _timer += Time.deltaTime;
        if (_timer >= 0f)
        {
            SoundManager.Instance.PlayBGM();
            _isSongStarted = true;
        }
    }

    public void Pause()
    {
        _isPlaying = false;
    }

    public void Resume()
    {
        _isPlaying = true;
    }
}