using System;
using System.Collections.Generic;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

public class TimelineSetup : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private RectTransform _content;
    [SerializeField] private EditorNoteManager _noteManager;
    [SerializeField] private Timeline _timelineLanePrefab;
    [SerializeField] private float _pixelsPerSecond = 750f;
    public float PixelsPerSecond => _pixelsPerSecond;

    private AudioClip _currentClip;
    private Level _currentLevel = Level.None;
    private List<Timeline> _activeLanes = new List<Timeline>();

    public event Action<List<Timeline>> OnCreateTimeline;

    public void SetClip(AudioClip clip)
    {
        _currentClip = clip;
        BuildTimeline();
    }

    public void SetLevel(Level level)
    {
        _currentLevel = level;
        BuildTimeline();
    }


    private void BuildTimeline()
    {
        if (_currentClip == null || _currentLevel == Level.None)
            return;
        ClearTimeline();


        int laneCount = GetLaneCount(_currentLevel);
        float scrollbarWidth = _scrollRect.verticalScrollbar.GetComponent<RectTransform>().rect.width;
        float contentWidth = _content.rect.width - scrollbarWidth;

        float laneWidth = contentWidth / laneCount;

        float clipLength = _currentClip.length;
        float contentHeight = clipLength * _pixelsPerSecond;

        _content.sizeDelta = new Vector2(_content.sizeDelta.x, contentHeight);


        for (int i = 0; i < laneCount; i++)
        {
            CreateLane(i, laneWidth, contentWidth);
        }

        InitializePosition();

        OnCreateTimeline.Invoke(_activeLanes);
    }

    private void CreateLane(int laneIndex, float laneWidth, float contentWidth)
    {
        Timeline laneObj = Instantiate(_timelineLanePrefab, _content);
        RectTransform rect = laneObj.GetComponent<RectTransform>();

        float laneCount = GetLaneCount(_currentLevel);
        float anchorMinX = laneIndex / laneCount;
        float anchorMaxX = (laneIndex + 1) / laneCount;

        rect.anchorMin = new Vector2(anchorMinX, 0);
        rect.anchorMax = new Vector2(anchorMaxX, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // 레인 정보 초기화
        float minX = anchorMinX * contentWidth;
        float maxX = anchorMaxX * contentWidth;

        laneObj.Initialize(laneIndex, minX, maxX, _pixelsPerSecond, _content);

        laneObj.OnLaneClicked += _noteManager.HandleLaneClick;
        _activeLanes.Add(laneObj);
    }

    private void InitializePosition()
    {
        _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, 0);
    }

    private void ClearTimeline()
    {
        foreach (var lane in _activeLanes)
        {
            if (lane != null)
            {
                lane.OnLaneClicked -= _noteManager.HandleLaneClick;
                Destroy(lane.gameObject);
            }
        }
        _activeLanes.Clear();
    }

    private int GetLaneCount(Level level) => level switch
    {
        Level.Easy => 3,
        Level.Normal => 6,
        Level.Hard => 9,
        Level.Shared => 9,
        _ => 0
    };
}
