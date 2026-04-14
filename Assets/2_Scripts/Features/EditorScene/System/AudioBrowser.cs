using System;
using System.Collections;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Networking;

public class AudioBrowser : MonoBehaviour
{

    public event Action<AudioClip, string> OnAudioLoaded;

    public void OpenFileBrowser()
    {
        var extensions = new[] {
                new ExtensionFilter("Audio Files", "ogg")
                // new ExtensionFilter("Audio Files", "mp3", "wav", "ogg") 차후 다른 포맷 확장
            };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("음원 선택", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            StartCoroutine(LoadAudioCoroutine(paths[0]));
        }
    }

    private IEnumerator LoadAudioCoroutine(string path)
    {
        string uri = "file://" + path;
        AudioType audioType = GetAudioType(path);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, audioType))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"음원 로드 실패: {www.error}");
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                clip.name = Path.GetFileNameWithoutExtension(path);

                // 1. SoundManager에게 클립을 전달하여 세팅하게 함
                // 예: SoundManager.Instance.SetMusic(clip);

                // 2. 다른 에디터 컴포넌트(타임라인 등)를 위해 이벤트 발생
                OnAudioLoaded?.Invoke(clip, path);

                Debug.Log($"음원 로드 성공 및 SoundManager 전달 완료: {clip.name}");
            }
        }
    }

    private AudioType GetAudioType(string path)
    {
        string ext = Path.GetExtension(path).ToLower();
        return ext switch
        {
            ".mp3" => AudioType.MPEG,
            ".wav" => AudioType.WAV,
            ".ogg" => AudioType.OGGVORBIS,
            _ => AudioType.UNKNOWN,
        };
    }
}
