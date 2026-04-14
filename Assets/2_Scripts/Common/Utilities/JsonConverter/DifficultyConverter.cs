using System;
using System.Collections.Generic;
using MyGame.Common.DataFormat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MyGame.Common.Utilities
{
    public class DifficultyConverter : JsonConverter<List<DifficultyData>>
    {
        public override List<DifficultyData> ReadJson(
            JsonReader reader,
            Type objectType,
            List<DifficultyData> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            var result = new List<DifficultyData>();

            foreach (var token in array)
            {
                string mode = token["mode"]?.ToString();
                string path = token["path"]?.ToString();

                if (!Enum.TryParse(mode, true, out Enums.Level level))
                {
                    UnityEngine.Debug.LogWarning($"Unknown difficulty mode: {mode}");
                    continue;
                }

                result.Add(new DifficultyData
                {
                    RawMode = mode,
                    ChartPath = path,
                    Difficulty = level
                });
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, List<DifficultyData> value, JsonSerializer serializer)
        {
            JArray array = new JArray();
            foreach (var difficulty in value)
            {
                array.Add(new JObject
                {
                    ["mode"] = difficulty.Difficulty.ToString(),
                    ["path"] = difficulty.Path
                });
            }
            array.WriteTo(writer);
        }
    }
}


