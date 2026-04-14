using System.Collections.Generic;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using MyGame.UI.SongSelect;
using UnityEngine;

public class SongSelectSceneManager : MonoBehaviour
{
    [SerializeField] SongSelectUIManager _UIManager;
    [SerializeField] SongSelectProvider _provider;
    [SerializeField] ScrollController _scrollController;

    private List<SongBaseMeta> _allSongs;
    private Level _level;

    private void Awake()
    {
        _level = GameManager.Instance.SelectedLevel;

        SongDatabaseManager.Instance.OnMetadataReady += HandleMetadataReady;
        InputManager.Instance.OnEscapePressed += HandleEscapePressed;
        InputManager.Instance.OnSpacePressed += HandleSpacePressed;
    }

    private void Start()
    {
        _UIManager.Init();
        SongDatabaseManager.Instance.RequestMetaData();
    }

    private void HandleMetadataReady(List<SongBaseMeta> allsongs)
    {
        _allSongs = allsongs;

        _provider.Init(_allSongs, _level);
    }

    private void HandleEscapePressed()
    {
        _UIManager.OnClickReturnBtn();
    }

    private void HandleSpacePressed()
    {
        _scrollController.ConfirmCurrentItem();
    }

    private void OnDestroy()
    {
        SongDatabaseManager.Instance.OnMetadataReady -= HandleMetadataReady;
        InputManager.Instance.OnEscapePressed -= HandleEscapePressed;
        InputManager.Instance.OnSpacePressed -= HandleSpacePressed;
    }
}
