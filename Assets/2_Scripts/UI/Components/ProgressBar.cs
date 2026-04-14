using MyGame.Core.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private bool _isInteractable = false;
    [SerializeField] private Slider _slider;
    private bool _isDragging = false;

    private void Awake()
    {
        _slider.interactable = _isInteractable;
        _slider.onValueChanged.AddListener(OnSliderChanged);

        EventTrigger trigger = _slider.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener(_ => {
            _isDragging = true;
            HandleSeek(_slider.value);
        });
        trigger.triggers.Add(pointerDown);

        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener(_ => _isDragging = false);
        trigger.triggers.Add(pointerUp);
    }

    private void Update()
    {
        if (SoundManager.Instance.ClipLength <= 0) return;
        if (!_isDragging)
            _slider.value = SoundManager.Instance.CurrentTime / SoundManager.Instance.ClipLength;
    }

    private void OnSliderChanged(float value)
    {
        if (!_isDragging) return;
        HandleSeek(value);
    }

    private void HandleSeek(float value)
    {
        if (SoundManager.Instance.ClipLength <= 0) return;
        float time = value * SoundManager.Instance.ClipLength;
        SoundManager.Instance.SeekTo(time);
    }
}