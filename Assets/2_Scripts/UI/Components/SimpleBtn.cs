using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame.UI.Components
{
    public class SimpleBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Serializable]
        public struct ButtonColorState
        {
            [SerializeField] private string imageHex;
            [SerializeField] private string outlineHex;

            public Color ImageColor { get; private set; }
            public Color OutlineColor { get; private set; }

            public void ParseColors()
            {
                if (!ColorUtility.TryParseHtmlString(imageHex, out var img))
                {
                    img = Color.white;
                }
                if (!ColorUtility.TryParseHtmlString(outlineHex, out var outline))
                {
                    outline = Color.black;
                }

                ImageColor = img;
                OutlineColor = outline;
            }

            public void SetNormalState(TextMeshProUGUI text)
            {
                ImageColor = Color.white;

                OutlineColor = text.fontMaterial.GetColor("_OutlineColor");
            }
        }


        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] Image _image;

        [SerializeField] ButtonColorState _highlightedState;
        [SerializeField] ButtonColorState _pressedState;

        private ButtonColorState _normalState;

        public Action OnHover;
        public Action OnClick;


        private void Awake()
        {
            _highlightedState.ParseColors();
            _pressedState.ParseColors();


            if (_text != null && _text.fontMaterial.HasProperty("_OutlineColor"))
            {
                _text.fontMaterial = new Material(_text.fontMaterial);
                _normalState.SetNormalState(_text);
            }

            
        }

        private void Start()
        {


            ApplyTransition(_normalState);
        }

        private void ApplyTransition(ButtonColorState state)
        {
            if (_image != null)
            {
                _image.color = state.ImageColor;
            }


            if (_text != null && _text.fontMaterial.HasProperty("_OutlineColor"))
            {
                _text.fontMaterial.SetColor("_OutlineColor", state.OutlineColor);
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ApplyTransition(_highlightedState);
            OnHover?.Invoke();
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            ApplyTransition(_normalState);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ApplyTransition(_pressedState);
            OnClick?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.fullyExited)
            {
                ApplyTransition(_normalState);
            }
            else
            {
                ApplyTransition(_highlightedState);
            }
        }

    }
}


