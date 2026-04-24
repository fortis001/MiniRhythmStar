using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class NotePositionEvent : UnityEvent<int, float, Vector2> { }

public class EditorNote : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    
    private RectTransform _rectTransform;
    private Vector2 _dragOffset;
    private int _laneIndex;
    private float _timeInSeconds;

    public int LaneIndex => _laneIndex;
    public float TimeInSeconds => _timeInSeconds;

    public Action<EditorNote, Vector2> OnPositionChanged;
    public Action<EditorNote> OnNoteClicked;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(int laneIndex, float timeInSeconds, Vector2 position, float width)
    {
        _laneIndex = laneIndex;
        _timeInSeconds = timeInSeconds;

        _rectTransform.sizeDelta = new Vector2(width, _rectTransform.sizeDelta.y);
        _rectTransform.anchoredPosition = position;

 
        _text.text = $"{laneIndex + 1}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        OnNoteClicked?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );
        _dragOffset = _rectTransform.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            _rectTransform.anchoredPosition = localPoint + _dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnPositionChanged?.Invoke(this, _rectTransform.anchoredPosition);
    }

}
