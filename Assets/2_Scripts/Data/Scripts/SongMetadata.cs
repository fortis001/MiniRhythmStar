using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MyGame.Common.Utilities;
using MyGame.Common.Enums;
using System.Linq;

namespace MyGame.Common.DataFormat
{
    [Serializable]
    public class SongBaseMeta
    {
        [JsonProperty("version")]
        public float Version { get; set; }

        [JsonProperty("songName")]
        public string SongName { get; set; }

        [JsonIgnore]  // JSON 외부
        public string SongID { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("audioFile")]
        public string AudioFile { get; set; }

        [JsonProperty("coverImage")]
        public string CoverImage { get; set; }

        [JsonProperty("previewTime")]
        public int PreviewTime { get; set; }

        [JsonIgnore]
        public float PreviewTimeSeconds => (float)PreviewTime / 1000f;

        [JsonProperty("bpm")]
        public float BPM { get; set; }

        [JsonIgnore]  // JSON 외부
        public string SongDirectory { get; set; }

        // JSON 파싱 → DifficultyData 자동 변환
        [JsonProperty("availableDifficulties")]
        [JsonConverter(typeof(DifficultyConverter))]
        public List<DifficultyData> Difficulties { get; set; }


        public List<DifficultyData> GetPlayableDifficulties(Level selectedLevel)
        {
            List<DifficultyData> filteredList = new List<DifficultyData>();

            return Difficulties
                .Where(d => d.Difficulty == selectedLevel || d.Difficulty == Level.Shared)
                .ToList();

        }
    }
}

