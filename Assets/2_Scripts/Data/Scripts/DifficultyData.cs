using Newtonsoft.Json;


namespace MyGame.Common.DataFormat
{
    public class DifficultyData
    {
        [JsonIgnore]  // RawDifficultyData 의 mode를 그대로 받는 필드
        public string RawMode { get; set; }

        [JsonIgnore]
        public string ChartPath { get; set; }

        public Enums.Level Difficulty { get; set; }

        public string Path { get; set; }

        public string BeatmapID { get; set; }

        [JsonIgnore]
        public SongBaseMeta ParentMeta;
    }
}
