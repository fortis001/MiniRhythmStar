
using MyGame.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyGame.Common.DataFormat
{
    public class PlayRecord
    {
        public int HighScore { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Rank HighestRank { get; set; }
        public int PlayCount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ClearStatus ClearStatus { get; set; }
        public int MaxCombo { get; set; }
        public bool IsLocked { get; set; }
    }
}


