using System.Collections.Generic;
using MyGame.Common.DataFormat;
using MyGame.Core.Managers;
using MyGame.Common.Enums;
using UnityEngine;
using System;

namespace MyGame.UI.SongSelect
{
    public class SongSelectProvider : MonoBehaviour
    {
        private Level _level;

        public event Action<List<ScrollItemData>> OnBuildScrollData;


        public void Init(List<SongBaseMeta> allsongs, Level level)
        {
            _level = level;

            BuildScrollItemData(allsongs);
        } 


        private void BuildScrollItemData(List<SongBaseMeta> allsongs)
        {

            List<ScrollItemData> result = new List<ScrollItemData>();

            foreach (SongBaseMeta song in allsongs)
            {
                List<DifficultyData> playableDifficulties =  song.GetPlayableDifficulties(_level);


                foreach (DifficultyData diff in playableDifficulties)
                {
                    ScrollItemData scrollData = CreateScrollItemData(song, diff);
                    result.Add(scrollData);
                }
            }


            OnBuildScrollData?.Invoke(result);

        }

        private ScrollItemData CreateScrollItemData(SongBaseMeta song, DifficultyData difficultydata)
        {
            PlayRecord record = UserDataManager.Instance.GetRecord(difficultydata.BeatmapID);


            ScrollItemData scrollData = new ScrollItemData
            {
                SongName = song.SongName,
                Artist = song.Artist,
                HighScore = record?.HighScore ?? 0,
                Rank = record?.HighestRank ?? Rank.NONE,
                DifficultyData = difficultydata,
            };

            return scrollData;
        }
    }
}

