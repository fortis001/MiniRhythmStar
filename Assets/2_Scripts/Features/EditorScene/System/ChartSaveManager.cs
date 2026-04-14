using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using Newtonsoft.Json;
using UnityEngine;

public class ChartSaveManager : MonoBehaviour
{
    [SerializeField] private Texture2D _defaultCover;

    private readonly string _beatMapsPath = Path.Combine(Application.streamingAssetsPath, "BeatMaps");

    public void Save(string songName, string rawPath, Level level, IReadOnlyList<EditorNote> notes)
    {
        string songFolder = GetOrCreateSongFolder(songName);
        CopyAudioFile(rawPath, songFolder);
        CopyImageFile(songFolder);
        SaveMetadata(songName, songFolder, level);
        SaveChartData(songFolder, songName, level, notes);
    }

    private string GetOrCreateSongFolder(string songName)
    {
        string normalizedName = NormalizeFolderName(songName);

        string songFolder = Path.Combine(_beatMapsPath, normalizedName);
        if (!Directory.Exists(songFolder))
            Directory.CreateDirectory(songFolder);
        return songFolder;
    }

    private string NormalizeFolderName(string songName)
    {
        return songName.Replace(" ", "_").ToLower();
    }

    private void CopyAudioFile(string rawPath, string songFolder)
    {
        string destPath = Path.Combine(songFolder, "source.ogg");
        if (!File.Exists(destPath))
            File.Copy(rawPath, destPath);
    }

    private void CopyImageFile(string songFolder)
    {
        string destPath = Path.Combine(songFolder, "cover.jpg");
        if (!File.Exists(destPath))
        {
            byte[] bytes = _defaultCover.EncodeToJPG();
            File.WriteAllBytes(destPath, bytes);
        }
    }

    private void SaveMetadata(string songName, string songFolder, Level level)
    {
        string metaPath = Path.Combine(songFolder, "metadata.json");
        SongBaseMeta meta;

        if (File.Exists(metaPath))
        {
            string json = File.ReadAllText(metaPath);
            meta = JsonConvert.DeserializeObject<SongBaseMeta>(json);
        }
        else
        {
            meta = new SongBaseMeta
            {
                Version = 1.0f,
                SongName = songName,
                Artist = "Unknown Artist",
                AudioFile = "source.ogg",
                CoverImage = "cover.jpg",
                PreviewTime = 0,
                BPM = 120f,
                Difficulties = new List<DifficultyData>()
            };
        }

        // 난이도 항목 추가 (중복 방지)
        string chartFileName = $"chart_{level.ToString().ToLower()}.json";
        if (!meta.Difficulties.Exists(d => d.Difficulty == level))
        {
            meta.Difficulties.Add(new DifficultyData
            {
                Difficulty = level,
                Path = chartFileName
            });
        }

        File.WriteAllText(metaPath, JsonConvert.SerializeObject(meta, Formatting.Indented));
    }

    private void SaveChartData(string songFolder, string songName, Level level, IReadOnlyList<EditorNote> notes)
    {
        string chartFileName = $"chart_{level.ToString().ToLower()}.json";
        string beatmapID = $"{NormalizeFolderName(songName)}_{level.ToString().ToLower()}";

        ChartData chart = new ChartData
        {
            Version = 1,
            BeatmapID = beatmapID,
            Offset = 0,
            Notes = notes.Select(n => new NoteData
            {
                TimeMs = Mathf.RoundToInt(n.TimeInSeconds * 1000f),
                Lane = n.LaneIndex,
                Type = 0
            }).ToList()
        };

        string chartPath = Path.Combine(songFolder, chartFileName);
        File.WriteAllText(chartPath, JsonConvert.SerializeObject(chart, Formatting.Indented));
    }
}