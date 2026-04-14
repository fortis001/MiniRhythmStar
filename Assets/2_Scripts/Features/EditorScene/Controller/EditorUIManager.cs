using System;
using TMPro;
using UnityEngine;

public class EditorUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _songName;

    public event Action OnReturnButtonClicked;
    public event Action OnEasyButtonClicked;
    public event Action OnNormalButtonClicked;
    public event Action OnHardButtonClicked;
    public event Action OnSharedButtonClicked;
    public event Action OnPlayButtonClicked;
    public event Action OnPauseButtonClicked;
    public event Action OnSaveButtonClicked;
    public event Action OnLoadButtonClicked;

    public void OnRetunButton() => OnReturnButtonClicked?.Invoke();
    public void OnEasyButton() => OnEasyButtonClicked?.Invoke();
    public void OnNormalButton() => OnNormalButtonClicked?.Invoke();
    public void OnHardButton() => OnHardButtonClicked?.Invoke();
    public void OnSharedButton() => OnSharedButtonClicked?.Invoke();
    public void OnPlayButton() => OnPlayButtonClicked?.Invoke();
    public void OnPauseButton() => OnPauseButtonClicked?.Invoke();
    public void OnSaveButton() => OnSaveButtonClicked?.Invoke();
    public void OnLoadButton() => OnLoadButtonClicked?.Invoke();

    public void UpdateSongName(string songName)
    {
        _songName.text = songName;
    }

}
