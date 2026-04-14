using System;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI.Components
{
    public class ScrollItem : MonoBehaviour
    {
        [SerializeField] Image _rankImage;
        [SerializeField] TextMeshProUGUI _songName;
        [SerializeField] public RectTransform _visualRoot;
        [SerializeField] private RankImageData _rankData;
        [SerializeField] SimpleBtn _btn;

        private RectTransform _rectTransform;
        public RectTransform rectTransform => _rectTransform ??= GetComponent<RectTransform>();

        // [수정] 데이터 자체를 캐싱
        private ScrollItemData _itemData;
        public ScrollItemData ItemData => _itemData;

        public event Action<ScrollItem> OnClicked;

        public void Init(ScrollItemData data)
        {
            _songName.text = data.SongName;

            Sprite rankImg = _rankData.GetRankImg(data.Rank);
            _rankImage.sprite = rankImg;

            _itemData = data;

            _btn.OnHover += () => SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Hover);
            _btn.OnClick += () => OnClicked?.Invoke(this);
        }
    }
}

