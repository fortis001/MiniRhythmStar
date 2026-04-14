using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyGame.Common.DataFormat
{
    public class NoteData
    {
        [JsonProperty("t")]
        public int TimeMs { get; set; }

        [JsonProperty("l")]
        public int Lane { get; set; }

        [JsonProperty("ty")]
        public int Type { get; set; }
    }

    public class ChartData
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("beatmapID")]
        public string BeatmapID { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("notes")]
        public List<NoteData> Notes { get; set; }
    }
}