using System.Collections.Generic;
using System.Linq;
using MyGame.Common.Constants;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame.Core.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private LevelData[] _levelDatas;
        private Dictionary<Level, LevelData> _levelDataMap;

        private LevelData _levelData;
        private Level _selectedLevel;
        private float _noteTravelTime;

        private DifficultyData _selectedDifficulty;

        private ResultData _resultData;

        private AudioClip _currentBeatmapClip;


        public LevelData LevelData => _levelData;
        public Level SelectedLevel => _selectedLevel;
        public float NoteTravelTime => _noteTravelTime;
        public DifficultyData SelectedDifficulty => _selectedDifficulty;
        public ResultData ResultData => _resultData;
        public AudioClip CurrentBeatmapClip => _currentBeatmapClip;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            _levelDataMap = _levelDatas.ToDictionary(data => data.level);

            Debug.Log(Application.persistentDataPath);
        }

        public void SetLevel(Level level)
        {
            _selectedLevel = level;
            _levelData = _levelDataMap[level];
            _noteTravelTime = _levelData.noteTravelTime;
        }

        public void SetSelectedDifficulty(DifficultyData diff)
        {
            _selectedDifficulty = diff;
            _currentBeatmapClip = SoundManager.Instance.CurrentClip;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void EndGamePlay()
        {
            ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
            scoreManager.UpdateMaxCombo();

            _resultData = new ResultData
            {
                BeatmapID = _selectedDifficulty.BeatmapID,
                Rank = scoreManager.Rank,
                Score = scoreManager.CurrentScore,
                Combo = scoreManager.MaxCombo,
                JudgeCount = scoreManager.JudgeCount
            };

            UserDataManager.Instance.SaveRecord(_resultData);

            TransitionManager.Instance.LoadNextScene(SceneNames.Result, false);
        }

    }

}
