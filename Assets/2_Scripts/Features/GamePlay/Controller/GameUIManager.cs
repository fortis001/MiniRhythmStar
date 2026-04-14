using System.Collections;
using MyGame.Core.RuntimeData;
using TMPro;
using UnityEngine;
using MyGame.Gameplay;
using UnityEngine.UI;
using MyGame.Common.DataFormat;
using MyGame.Core.Managers;
using System;
using MyGame.Common.Constants;

namespace MyGame.UI.Game
{
    public class GameUIManager : BaseUIManager
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] GameObject _comboTextParent;
        [SerializeField] TextMeshProUGUI _comboText;
        [SerializeField] TextMeshProUGUI _judgeText;
        [SerializeField] TextMeshProUGUI _songNameText;
        [SerializeField] Image _coverImage;
        [SerializeField] GameObject _pauseMenuPanel;
        [SerializeField] Slider _progressBar;

        private ScoreManager _scoreManager;
        private Coroutine _comboDisplayTimer;
        private const float COMBO_DISPLAY_TIME = 0.75f;
        private float _totalDuration;

        public event Action OnPauseBtnClicked;
        public event Action OnResumeBtnClicked;
        public event Action OnRestartBtnClicked;

        public void OnClickPauseBtn() => OnPauseBtnClicked?.Invoke();
        public void OnClickResumeBtn() => OnResumeBtnClicked?.Invoke();
        public void OnClickRestartBtn() => OnRestartBtnClicked?.Invoke();

        protected void Awake()
        {
            _scoreManager = ScoreManager.Instance;

            _scoreManager.OnScoreChanged += UpdateScoreDisplay;
            _scoreManager.OnComboIncrease += PopUpCombo;
            _scoreManager.OnComboBreak += PopUpComboBreakAnim;

        }

        public void Init(DifficultyData diff)
        {
            _songNameText.text = diff.ParentMeta.SongName;
            _coverImage.sprite = SongDatabaseManager.Instance.GetCoverImage(diff);
            _totalDuration = SoundManager.Instance.ClipLength;

            base.Init();
        }

        private void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            _progressBar.value = (GameTimer.Instance.CurrentTime / _totalDuration);
        }


        private void UpdateScoreDisplay(int newScore)
        {
            string formattedScore = newScore.ToString("D8");

            if (_scoreText != null)
            {
                _scoreText.text = formattedScore;
            }
        }

        private void PopUpCombo(ComboUpdateData data)
        {
            if (_comboDisplayTimer != null)
            {
                StopCoroutine(_comboDisplayTimer);
            }

            _comboTextParent.SetActive(true);

            _judgeText.text = data.Judge.ToString().ToUpper();
            _comboText.text = data.ComboCount.ToString("D3");

            _comboDisplayTimer = StartCoroutine(ComboDisplayTimer());
        }

        private IEnumerator ComboDisplayTimer()
        {
            yield return new WaitForSeconds(COMBO_DISPLAY_TIME);

            _comboTextParent.SetActive(false);

            _comboDisplayTimer = null;
        }

        private void PopUpComboBreakAnim()
        {
            if (_comboDisplayTimer != null)
            {
                StopCoroutine(_comboDisplayTimer);
            }

            SoundManager.Instance.PlaySFX(SFXID.Note_Combo_Break);

            _comboTextParent.SetActive(false);

            _comboDisplayTimer = null;
        }

        public void ShowPauseMenu()
        {
            SoundManager.Instance.PlaySFX(SFXID.UI_Menu_Open);
            _pauseMenuPanel.SetActive(true);
        }

        public void HidePauseMenu()
        {
            SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Confirm);
            _pauseMenuPanel.SetActive(false);
        }

        public void OnClickExitBtn()
        {
            TransitionManager.Instance.LoadNextScene(SceneNames.SongSelect);
        }


    }

}
