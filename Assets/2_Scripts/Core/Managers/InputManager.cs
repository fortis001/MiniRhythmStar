using System;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Core.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public event Action OnSpacePressed;
        public event Action OnEscapePressed;
        public event Action OnMouseClicked;

        private Dictionary<KeyCode, Action> _shortcuts;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            _shortcuts = new Dictionary<KeyCode, Action>
            {
                { KeyCode.Space,  () => OnSpacePressed?.Invoke() },
                { KeyCode.Escape, () => OnEscapePressed?.Invoke() },
                // 추가 단축키는 여기에
            };
        }

        private void Update()
        {
            foreach (var shortcut in _shortcuts)
            {
                if (Input.GetKeyDown(shortcut.Key))
                {
                    shortcut.Value.Invoke();
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
                OnMouseClicked?.Invoke();
        }

        private bool IsPointerOverUI()
        {
            return UnityEngine.EventSystems.EventSystem.current != null &&
                   UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }
    }
}

