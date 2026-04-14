using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Common.Utilities
{
    public class KeyBindingConverter : JsonConverter<Dictionary<string, KeyCode>>
    {
        public override Dictionary<string, KeyCode> ReadJson(
            JsonReader reader,
            Type objectType,
            Dictionary<string, KeyCode> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var result = new Dictionary<string, KeyCode>(StringComparer.OrdinalIgnoreCase);

            JObject obj = JObject.Load(reader);

            foreach (var kvp in obj)
            {
                if (Enum.TryParse(kvp.Value.ToString(), true, out KeyCode keyCode))
                    result[kvp.Key] = keyCode;
                else
                    Debug.LogWarning($"Invalid KeyCode in config.json: {kvp.Value}");
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, Dictionary<string, KeyCode> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteValue(kvp.Value.ToString()); // KeyCode ¡æ string
            }
            writer.WriteEndObject();
        }
    }
}


