using System.Collections.Generic;
using System.IO;
using MyGame.Common.Constants;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using MyGame.Core.RuntimeData;
using MyGame.Gameplay;
using MyGame.UI.Game;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    [SerializeField] private BeatmapLoader _beatmapLoader;
    [SerializeField] private GameSceneProvider _provider;
    [SerializeField] private LaneBuilder _laneBuilder;
    [SerializeField] private NotePoolManager _notePoolManager;
    [SerializeField] private GameUIManager _UIManager;
    [SerializeField] private NoteScheduler _noteScheduler;
    [SerializeField] private GameTimer _timer;

    private DifficultyData _difficultyData;
    private LevelData _levelData;

    private bool _isPlaying;

    private void Awake()
    {
        _difficultyData = GameManager.Instance.SelectedDifficulty;
        _levelData = GameManager.Instance.LevelData;

        _laneBuilder.OnNoteWidthCalculated += _notePoolManager.HandleNoteWidthCalculated;

        InputManager.Instance.OnEscapePressed += HandlePauseToggle;
        InputManager.Instance.OnSpacePressed += HandlePauseToggle;

        _UIManager.OnPauseBtnClicked += HandlePauseToggle;
        _UIManager.OnResumeBtnClicked += HandlePauseToggle;

        _UIManager.OnRestartBtnClicked += HandleRestart;

        _noteScheduler.OnGameEnd += HandleGameEnd;
    }

    private void Start()
    {
        string chartPath = Path.Combine(_difficultyData.Path, _difficultyData.ChartPath);

        ChartData chartData = _beatmapLoader.LoadChart(chartPath);
        Level difficulty = _difficultyData.Difficulty;

        Queue<RuntimeNote> noteQueue = _provider.ProcessRawChart(chartData, difficulty);

        _laneBuilder.Init(_levelData);
        _notePoolManager.Init(_levelData);
        _UIManager.Init(_difficultyData);
        _noteScheduler.Init(noteQueue, chartData.Offset, GameManager.Instance.NoteTravelTime);

        _isPlaying = true;

        SoundManager.Instance.StopBGM();
        SoundManager.Instance.SetClip(GameManager.Instance.CurrentBeatmapClip, loop: false);
        _timer.StartTimer();
    }

    private void HandlePauseToggle()
    {
        if (_isPlaying) HandlePause();
        else HandleResume();
    }

    private void HandlePause()
    {
        _isPlaying = false;
        _timer.Pause();
        SoundManager.Instance.PauseBGM();
        _UIManager.ShowPauseMenu();
    }

    private void HandleResume()
    {
        _isPlaying = true;
        _timer.Resume();
        SoundManager.Instance.ResumeBGM();
        _UIManager.HidePauseMenu();
    }

    private void HandleRestart()
    {
        TransitionManager.Instance.LoadNextScene(SceneNames.Game);
    }

    private void HandleGameEnd()
    {
        GameManager.Instance.EndGamePlay();
    }



    private void OnDestroy()
    {
        InputManager.Instance.OnEscapePressed -= HandlePauseToggle;
        InputManager.Instance.OnSpacePressed -= HandlePauseToggle;
    }
}
