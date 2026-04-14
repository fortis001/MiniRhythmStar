using MyGame.Core.Managers;
using MyGame.UI.Title;
using MyGame.Common.DataFormat;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] TitleUIManager _UIManager;

    private void Awake()
    {
        InputManager.Instance.OnEscapePressed += HandleEscapePressed;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(BGMID.BGM_Lobby);
        _UIManager.Init();
    }

    private void HandleEscapePressed()
    {
        SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Click);
        _UIManager.OnClickExitBtn();
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnEscapePressed -= HandleEscapePressed;
    }
}
