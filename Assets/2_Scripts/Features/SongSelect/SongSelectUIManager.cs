using System.Collections.Generic;
using MyGame.Common.Constants;
using MyGame.Common.DataFormat;
using MyGame.Core.Managers;
using MyGame.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI.SongSelect
{
    public class SongSelectUIManager : BaseUIManager
    {

        [SerializeField] SongSelectProvider _provider;

        [SerializeField] ScrollItem _scrollItemPrefab;
        [SerializeField] Transform _content;
        [SerializeField] ScrollController _scrollController;
        [SerializeField] Image _coverImage;


        public override void Init()
        {
            _provider.OnBuildScrollData += BuildScrollView;

            _scrollController.OnItemCentered += HandleItemCentered;

            _scrollController.OnItemConfirmed += HandleItemConfirmed;

            base.Init();
        }


        private void BuildScrollView(List<ScrollItemData> items)
        {
            ClearContent();
            foreach (ScrollItemData data in items)
            {
                ScrollItem item = Instantiate(_scrollItemPrefab, _content);
                item.Init(data);

                _scrollController.RegisterItem(item);
            }
        }

        private void ClearContent()
        {
            foreach (Transform t in _content) Destroy(t.gameObject);
        }



        private void HandleItemCentered(ScrollItem item)
        {
            DifficultyData difficultyData = item.ItemData.DifficultyData;


            _coverImage.sprite = SongDatabaseManager.Instance.GetCoverImage(difficultyData);
            SoundManager.Instance.PlayPreview(difficultyData.Path, (float)difficultyData.ParentMeta.PreviewTimeSeconds);
        }

        private void HandleItemConfirmed(ScrollItem item)
        {
            GameManager.Instance.SetSelectedDifficulty(item.ItemData.DifficultyData);


            TransitionManager.Instance.LoadNextScene(SceneNames.Game);
        }

        public void OnClickReturnBtn()
        {
            SoundManager.Instance.StopBGM();
            TransitionManager.Instance.LoadNextScene(SceneNames.LevelSelect);
        }





        private void OnDestroy()
        {
            _provider.OnBuildScrollData -= BuildScrollView;
            _scrollController.OnItemCentered -= HandleItemCentered;
            _scrollController.OnItemConfirmed -= HandleItemConfirmed;
        }
    }
}

