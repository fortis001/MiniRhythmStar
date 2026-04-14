using System.Collections;
using System.Collections.Generic;
using MyGame.Common.Constants;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIManager : BaseUIManager
{
    [Header("RankImageData")]
    [SerializeField] RankImageData _rankImageData;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _totalScoreText;
    [SerializeField] TextMeshProUGUI _comboCountText;
    [SerializeField] TextMeshProUGUI _perfectCountText;
    [SerializeField] TextMeshProUGUI _greatCountText;
    [SerializeField] TextMeshProUGUI _goodCountText;
    [SerializeField] TextMeshProUGUI _badCountText;
    [SerializeField] TextMeshProUGUI _missCountText;

    [Header("Image")]
    [SerializeField] Image _rankImg;

    private Rank _rank;
    private int _score;
    private int _combo;
    private Dictionary<JudgeType, int> _judgeCount;

    public void Init(Rank rank, int score, int combo, Dictionary<JudgeType, int> judgeCount)
    {
        _rank = rank;
        _score = score;
        _combo = combo;
        _judgeCount = judgeCount;

        _rankImg.sprite = _rankImageData.GetRankImg(rank);
        _rankImg.color = new Color(1f, 1f, 1f, 0f);

        StartCoroutine(AnimateResults());

        base.Init();
    }

    private IEnumerator AnimateResults()
    {
        foreach (var (text, value, format) in GetScoreTargets())
            yield return StartCoroutine(AnimateScore(text, value, format));
        yield return StartCoroutine(FadeInImage(_rankImg, 1f));
    }

    private IEnumerator AnimateScore(TextMeshProUGUI targetText, int targetScore, string format = null)
    {
        float duration = 0.8f;
        float elapsed = 0f;
        int currentScore = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentScore = Mathf.FloorToInt(Mathf.Lerp(0, targetScore, elapsed / duration));
            targetText.text = format != null ? currentScore.ToString(format) : currentScore.ToString();
            yield return null;
        }
        targetText.text = format != null ? targetScore.ToString(format) : targetScore.ToString();
    }

    private IEnumerator FadeInImage(Image image, float duration)
    {
        float elapsed = 0f;
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            image.color = color;
            yield return null;
        }

        color.a = 1f;
        image.color = color;
    }

    public void SkipAnimation()
    {
        StopAllCoroutines();
        foreach (var (text, value, format) in GetScoreTargets())
            text.text = value.ToString(format);
        _rankImg.color = Color.white;
    }

    public void OnClickOKBtn()
    {
        TransitionManager.Instance.LoadNextScene(SceneNames.SongSelect, false);
    }

    public void OnClickRestartBtn()
    {
        TransitionManager.Instance.LoadNextScene(SceneNames.Game, false);
    }

    

    private (TextMeshProUGUI text, int value, string format)[] GetScoreTargets()
    {
        return new[]
        {
        (_totalScoreText, _score, "D8"),
        (_comboCountText, _combo, "D8"),
        (_perfectCountText, _judgeCount[JudgeType.Perfect], "D4"),
        (_greatCountText, _judgeCount[JudgeType.Great], "D4"),
        (_goodCountText, _judgeCount[JudgeType.Good], "D4"),
        (_badCountText, _judgeCount[JudgeType.Bad], "D4"),
        (_missCountText, _judgeCount[JudgeType.Miss], "D4"),
    };
    }
}
