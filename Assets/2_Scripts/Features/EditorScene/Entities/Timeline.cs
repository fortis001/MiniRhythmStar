using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Timeline : MonoBehaviour, IPointerClickHandler
{
    
    private int _laneIndex;
    private float _centerX;
    private float _minX;
    private float _maxX;
    private RectTransform _content;

    public int LaneIndex => _laneIndex;
    public float CenterX => _centerX;
    public float MinX => _minX;
    public float MaxX => _maxX;
    private RectTransform _rectTransform;

    public event Action<int, float> OnLaneClicked;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(int index, float min, float max, float pixelsPerSecond, RectTransform content)
    {
        _laneIndex = index;
        _minX = min;
        _maxX = max;
        _centerX = (min + max) / 2f;
        _content = content;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        _content, // _rectTransform ´ë½Å
        eventData.position,
        eventData.pressEventCamera,
        out Vector2 localPoint
        );
        OnLaneClicked?.Invoke(_laneIndex, localPoint.y);
    }

    public float GetCenterXInNoteChart()
    {
        Vector3 worldCenter = _rectTransform.TransformPoint(_rectTransform.rect.center);
        return 1;
        // return _noteChart.InverseTransformPoint(worldCenter).x;
    }
}
