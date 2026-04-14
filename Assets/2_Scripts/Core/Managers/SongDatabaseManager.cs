using System;
using System.Collections.Generic;
using System.IO;
using MyGame.Common.DataFormat;
using Newtonsoft.Json;
using UnityEngine;

namespace MyGame.Core.Managers
{
    public class SongDatabaseManager : Singleton<SongDatabaseManager>
    {
        private bool _isMetaDataReady = false;
        private List<SongBaseMeta> _allSongs = new List<SongBaseMeta>();
        private Dictionary<string, DifficultyData> _index = new();
        private Dictionary<string, Sprite> _coverCache = new Dictionary<string, Sprite>();

        public event Action<List<SongBaseMeta>> OnMetadataReady;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {

        }


        public void LoadMetadata()
        {
            if (_isMetaDataReady) return;

            string beatmapRoot = Path.Combine(Application.streamingAssetsPath, "Beatmaps");
            string[] songDirs = Directory.GetDirectories(beatmapRoot);

            foreach (var dir in songDirs)
            {
                string folderName = Path.GetFileName(dir);
                string metaPath = Path.Combine(dir, "metadata.json");

                string json = File.ReadAllText(metaPath);

                SongBaseMeta meta = JsonConvert.DeserializeObject<SongBaseMeta>(json);

                meta.SongID = folderName;
                meta.SongDirectory = dir;

                foreach (var diff in meta.Difficulties)
                {
                    diff.Path = dir;
                    diff.BeatmapID = $"{meta.SongID}_{diff.RawMode}";
                    diff.ParentMeta = meta;

                    if (_index.ContainsKey(diff.BeatmapID))
                        Debug.LogError($"Duplicate difficulty: {diff.BeatmapID}");

                    _index[diff.BeatmapID] = diff;
                }

                _allSongs.Add(meta);
            }

            _isMetaDataReady = true;
            OnMetadataReady?.Invoke(_allSongs);
        }



        public void RequestMetaData()
        {
            if (_isMetaDataReady) OnMetadataReady?.Invoke(_allSongs);
            else LoadMetadata();
        }

        public Sprite GetCoverImage(DifficultyData diff)
        {
            string beatmapID = diff.BeatmapID;

            if (_coverCache.TryGetValue(beatmapID, out Sprite cachedSprite))
                return cachedSprite;

            string directory = diff.Path;
            // 2. 해당 폴더의 cover.jpg 경로 결합
            string coverPath = Path.Combine(directory, "cover.jpg");


            if (File.Exists(coverPath))
            {
                // 3. 파일의 모든 바이트를 읽어옴
                byte[] byteTexture = File.ReadAllBytes(coverPath);

                // 4. 새로운 Texture2D 생성 (크기는 LoadImage에서 자동 조절됨)
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(byteTexture))
                {

                    Sprite newSprite = Sprite.Create(texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));

                    _coverCache.Add(beatmapID, newSprite);
                    return newSprite;
                }
            }

            // 이미지가 없거나 로드 실패 시 기본 이미지(null 혹은 디폴트) 반환
            return null;


        }

    }
}


