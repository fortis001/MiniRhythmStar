using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using MyGame.Common.Utilities;

namespace MyGame.Common.DataFormat
{
    public class UserConfigData
    {
        [JsonProperty("masterVolume")]
        public float MasterVolume { get; set; } = 1f;

        [JsonProperty("offsetMs")]
        public float OffsetMs { get; set; } = 0;

        [JsonProperty("resolutionIndex")]
        public int ResolutionIndex { get; set; } = 0;

        // KeyBindings → KeyCode로 바로 파싱하기
        [JsonProperty("keyBindings")]
        [JsonConverter(typeof(KeyBindingConverter))]
        public Dictionary<string, KeyCode> KeyBindings { get; set; }
            = new Dictionary<string, KeyCode>();
    }
}

