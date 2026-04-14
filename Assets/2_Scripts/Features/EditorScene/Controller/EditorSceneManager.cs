using System.Collections.Generic;
using MyGame.Common.Constants;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using UnityEngine;

public class EditorSceneManager : MonoBehaviour
{
    [SerializeField] private AudioBrowser _audioBrowser;
    [SerializeField] private TimelineSetup _timelineSetup;
    [SerializeField] private EditorUIManager _UIManager;
    [SerializeField] private EditorInputManager _inputManager;
    [SerializeField] private ChartSaveManager _chartSaveManager;
    [SerializeField] private EditorNoteManager _noteManager;

    private AudioClip _currentClip;
    private Level _currentLevel;
    private string _currentPath;
    private string _currentSongName;


    private void Awake()
    {
        _audioBrowser.OnAudioLoaded += HandleAudioLoaded;

        _UIManager.OnReturnButtonClicked += HandleReturn;
        _UIManager.OnEasyButtonClicked += () => HandleLevelSelect(Level.Easy);
        _UIManager.OnNormalButtonClicked += () => HandleLevelSelect(Level.Normal);
        _UIManager.OnHardButtonClicked += () => HandleLevelSelect(Level.Hard);
        _UIManager.OnSharedButtonClicked += () => HandleLevelSelect(Level.Shared);
        _UIManager.OnPlayButtonClicked += HandlePlay;
        _UIManager.OnPauseButtonClicked += HandlePause;
        _UIManager.OnSaveButtonClicked += HandleSave;
        _UIManager.OnLoadButtonClicked += HandleLoad;

        _inputManager.OnSpaceKeyPressed += HandleSpacePressed;
    }
    private void Start()
    {
        SoundManager.Instance.StopBGM();
    }


    private void HandleAudioLoaded(AudioClip clip, string path)
    {
        _currentClip = clip;
        _currentPath = path;
        _currentSongName = clip.name;

        _timelineSetup.SetClip(clip);
        SoundManager.Instance.SetClip(clip);
        _UIManager.UpdateSongName(_currentSongName);
    }

    private void HandleReturn()
    {
        SoundManager.Instance.StopBGM();
        TransitionManager.Instance.LoadNextScene(SceneNames.Title);
    }
    private void HandleLevelSelect(Level level)
    {
        _currentLevel = level;
        _timelineSetup.SetLevel(level);
    }
    private void HandlePlay()
    {
        SoundManager.Instance.ResumeBGM();
    }
    private void HandlePause()
    {
        SoundManager.Instance.PauseBGM();
    }
    private void HandleSave()
    {
        IReadOnlyList<EditorNote> notes = _noteManager.GetSortedNotes();
        _chartSaveManager.Save(_currentSongName, _currentPath, _currentLevel, notes);
    }
    private void HandleLoad()
    {
        _audioBrowser.OpenFileBrowser();
    }

    private void HandleSpacePressed()
    {
        if (SoundManager.Instance.IsPlaying)
        {
            HandlePause();
        }
        else
        {
            HandlePlay();
        }
    }
}
