using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorInputManager : MonoBehaviour
{
    [SerializeField] TimelineSetup _timelineSetup;

    private readonly KeyCode[] _allNumpadKeys = new KeyCode[]
        {
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
        KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9
        };

    private List<Timeline> _activeLanes;
    private List<KeyCode> _activeKeyCodes;

    private Dictionary<KeyCode, Action> _editorShortcuts;

    public event Action<int> OnNoteKeyPressed;
    public event Action OnDeleteKeyPressed;
    public event Action OnSpaceKeyPressed;

    private void Awake()
    {
        _timelineSetup.OnCreateTimeline += HandleCreateTimeline;
    }


    private void Start()
    {
        _editorShortcuts = new Dictionary<KeyCode, Action>
    {
        { KeyCode.Delete, () => OnDeleteKeyPressed?.Invoke() },
        { KeyCode.Space,  () => OnSpaceKeyPressed?.Invoke() },
        // 여기에 다른 단축키(Ctrl+S 등)를 계속 추가 가능
    };
    }

    private void Update()
    {
        foreach (var shortcut in _editorShortcuts)
        {
            if (Input.GetKeyDown(shortcut.Key))
            {
                shortcut.Value.Invoke();
                return;
            }
        }

        if (_activeKeyCodes == null) return;

        for (int i = 0; i < _activeKeyCodes.Count; i++)
        {
            if (Input.GetKeyDown(_activeKeyCodes[i]))
                OnNoteKeyPressed?.Invoke(i);
        }
    }

    private void HandleCreateTimeline(List<Timeline> activeLanes)
    {
        _activeLanes = activeLanes;
        _activeKeyCodes = _allNumpadKeys.Take(_activeLanes.Count).ToList();
    }
}
