using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace MyGame.Core.Managers
{
    public class UserDataManager : Singleton<UserDataManager>
    {
        private UserConfigData _config;
        private Dictionary<string, KeyCode> _cachedKeyCodes;

        private UserRecordData _records;
        private Dictionary<string, PlayRecord> _recordIndex = new Dictionary<string, PlayRecord>();


        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            LoadConfigData();
            LoadUserRecordData();
        }

        // =====CONFIG======

        private void LoadConfigData()
        {
            string path = GetConfigPath();

            if (!File.Exists(path))
            {
                Debug.LogWarning("config.json not found. Creating default config.");

                CreateDefaultConfig();

                _cachedKeyCodes = _config.KeyBindings;
                return;
            }

            string jsonString = File.ReadAllText(path);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented
            };

            _config = JsonConvert.DeserializeObject<UserConfigData>(jsonString, settings);
            _cachedKeyCodes = _config.KeyBindings;
        }

        public void SaveConfig()
        {
            string path = GetConfigPath();

            string jsonString = JsonConvert.SerializeObject(_config, Formatting.Indented);

            File.WriteAllText(path, jsonString);
        }

        private string GetConfigPath()
        {
            return Path.Combine(Application.persistentDataPath, "config.json");
        }

        private void CreateDefaultConfig()
        {
            _config = new UserConfigData
            {
                MasterVolume = 1f,
                OffsetMs = 0,
                ResolutionIndex = 1,
                KeyBindings = new Dictionary<string, KeyCode>()
            };

            SetDefaultKeyBindings();

            string path = GetConfigPath();

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string json = JsonConvert.SerializeObject(_config, Formatting.Indented);

            File.WriteAllText(path, json);

            Debug.Log("Default config.json created.");
        }

        private void SetDefaultKeyBindings()
        {
            _config.KeyBindings = new Dictionary<string, KeyCode>()
    {
        { "Key1", KeyCode.Keypad1 },
        { "Key2", KeyCode.Keypad2 },
        { "Key3", KeyCode.Keypad3 },
        { "Key4", KeyCode.Keypad4 },
        { "Key5", KeyCode.Keypad5 },
        { "Key6", KeyCode.Keypad6 },
        { "Key7", KeyCode.Keypad7 },
        { "Key8", KeyCode.Keypad8 },
        { "Key9", KeyCode.Keypad9 }
    };
        }



        // =====RECORD=====

        private void LoadUserRecordData()
        {
            string path = GetRecordPath();

            if (!File.Exists(path))
            {
                Debug.LogWarning("record.json not found. Creating empty record data.");
                _records = new UserRecordData
                {
                    PlayRecords = new Dictionary<string, PlayRecord>()
                };
                _recordIndex = _records.PlayRecords;

                CreateNewRecord();
                return;
            }

            string json = File.ReadAllText(path);

            _records = JsonConvert.DeserializeObject<UserRecordData>(json);

            if (_records.PlayRecords == null)
                _records.PlayRecords = new Dictionary<string, PlayRecord>();

            _recordIndex = _records.PlayRecords;

            

            Debug.Log($"Loaded {_recordIndex.Count} play records.");
        }

        private string GetRecordPath()
        {
            return Path.Combine(Application.persistentDataPath, "record.json");
        }

        private void CreateNewRecord()
        {
            string path = GetRecordPath();

            string json = JsonConvert.SerializeObject(_records, Formatting.Indented);

            File.WriteAllText(path, json);

            Debug.Log("record.json created.");
        }


        public PlayRecord GetRecord(string beatmapID)
        {
            if (_recordIndex.TryGetValue(beatmapID, out var record))
                return record;

            return null;
        }

        public void SaveRecord(ResultData result)
        {
            string beatmapID = result.BeatmapID;

            if (_recordIndex.TryGetValue(beatmapID, out PlayRecord record))
            {
                record.PlayCount++;
                record.HighScore = Mathf.Max(record.HighScore, result.Score);
                record.MaxCombo = Mathf.Max(record.MaxCombo, result.Combo);
                record.HighestRank = GetHigherRank(record.HighestRank, result.Rank);
                record.ClearStatus = GetHigherClearStatus(record.ClearStatus, GetClearStatus(result));
            }
            else
            {
                _recordIndex[beatmapID] = new PlayRecord
                {
                    PlayCount = 1,
                    HighScore = result.Score,
                    MaxCombo = result.Combo,
                    HighestRank = result.Rank,
                    ClearStatus = GetClearStatus(result)
                };
            }

            _records.PlayRecords = _recordIndex;

            string json = JsonConvert.SerializeObject(_records, Formatting.Indented);
            string path = GetRecordPath();
            File.WriteAllText(path, json);
        }

        private Rank GetHigherRank(Rank recordRank, Rank resultRank)
        {
            return (Rank)Mathf.Min((int)recordRank, (int)resultRank);
        }

        private ClearStatus GetHigherClearStatus(ClearStatus recordStatus, ClearStatus resultStatus)
        {
            return (ClearStatus)Mathf.Max((int)recordStatus, (int)resultStatus);
        }

        private ClearStatus GetClearStatus(ResultData result)
        {
            Dictionary<JudgeType, int> judge = result.JudgeCount;

            if (judge.Values.Sum() == judge[JudgeType.Perfect])
            {
                return ClearStatus.PerfectPlay;
            }
            else if (judge[JudgeType.Miss] == 0 && judge[JudgeType.Bad] == 0)
            {
                return ClearStatus.FullCombo;
            }
            else return ClearStatus.Clear;
        }
    }
}
