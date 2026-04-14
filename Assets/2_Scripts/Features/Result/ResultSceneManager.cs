using System.Collections.Generic;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] ResultUIManager _UIManager;

    private Rank _rank;
    private int _score;
    private int _combo;
    private Dictionary<JudgeType, int> _judgeCount;

    private void Awake()
    {
        ResultData result = GameManager.Instance.ResultData;

        _rank = result.Rank;
        _score = result.Score;
        _combo = result.Combo;
        _judgeCount = result.JudgeCount;

        InputManager.Instance.OnMouseClicked += HandleSkip;
        InputManager.Instance.OnSpacePressed += HandleSkip;
        InputManager.Instance.OnEscapePressed += HandleSkip;
    }

    public void Start()
    {
        SoundManager.Instance.PlaySFX(SFXID.Game_Clear_Win);
        SoundManager.Instance.PlayBGM(BGMID.BGM_Result, false);
        _UIManager.Init(_rank, _score, _combo, _judgeCount);
    }

    private void HandleSkip()
    {
        _UIManager.SkipAnimation();
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnMouseClicked -= HandleSkip;
        InputManager.Instance.OnSpacePressed -= HandleSkip;
        InputManager.Instance.OnEscapePressed -= HandleSkip;
    }

}
