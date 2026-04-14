using MyGame.Core.Managers;
using MyGame.UI.LevelSelect;
using UnityEngine;
using MyGame.Common.DataFormat;

public class LevelSceneManager : MonoBehaviour
{
    [SerializeField] LevelUIManager _UIManager;


    private void Awake()
    {
        InputManager.Instance.OnEscapePressed += HandleEscapePressed;
    }
    private void Start()
    {
        SoundManager.Instance.PlayBGM(BGMID.BGM_LevelSelect);
        _UIManager.Init();
    }

    private void HandleEscapePressed()
    {
        SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Click);
        _UIManager.OnClickReturnBtn();
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnEscapePressed -= HandleEscapePressed;
    }
}
