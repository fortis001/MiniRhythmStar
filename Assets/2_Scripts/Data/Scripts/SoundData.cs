

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Common.DataFormat
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Audio/SoundData")]
    public class SoundData : ScriptableObject
    {
        [Serializable]
        public struct SFXEntry
        {
            public SFXID id;
            public AudioClip clip;
        }

        [Serializable]
        public struct BGMEntry
        {
            public BGMID id;
            public AudioClip clip;
        }

        public List<SFXEntry> sfxList;
        public List<BGMEntry> bgmList;
    }
}