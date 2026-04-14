using System.IO;
using MyGame.Common.DataFormat;
using Newtonsoft.Json;
using UnityEngine;

public class BeatmapLoader : MonoBehaviour
{


    public ChartData LoadChart(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"[ChartLoader] 파일을 찾을 수 없습니다: {path}");
            return null;
        }

        try
        {
            // 1. 파일로부터 텍스트 읽기
            string jsonText = File.ReadAllText(path);

            // 2. Newtonsoft.Json으로 역직렬화
            ChartData data = JsonConvert.DeserializeObject<ChartData>(jsonText);

            Debug.Log($"[ChartLoader] 로드 완료: {data.BeatmapID} (노트 {data.Notes.Count}개)");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ChartLoader] 파싱 중 오류 발생: {e.Message}");
            return null;
        }
    }


}
