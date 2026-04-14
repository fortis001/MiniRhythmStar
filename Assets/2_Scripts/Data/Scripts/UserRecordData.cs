using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyGame.Common.DataFormat
{
    public class UserRecordData
    {
        [JsonProperty("playRecords")]
        public Dictionary<string, PlayRecord> PlayRecords { get; set; }
    }
}
