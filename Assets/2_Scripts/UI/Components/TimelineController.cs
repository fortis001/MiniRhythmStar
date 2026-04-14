using MyGame.Core.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private TimelineSetup _timelineSetup;

    private ScrollRect _scrollRect;
    private float _pixelsPerSecond;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _pixelsPerSecond = _timelineSetup.PixelsPerSecond;
    }

    private void Update()
    {
        if (SoundManager.Instance.IsPlaying)
        {
            float y = 0 - SoundManager.Instance.CurrentTime * _pixelsPerSecond;
            _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, y);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (SoundManager.Instance.IsPlaying)
            SoundManager.Instance.PauseBGM();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float time = -_content.anchoredPosition.y / _pixelsPerSecond;

        SoundManager.Instance.SeekTo(time);
    }
}
