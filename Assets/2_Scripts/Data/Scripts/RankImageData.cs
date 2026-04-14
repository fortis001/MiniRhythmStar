using MyGame.Common.Enums;
using UnityEngine;


namespace MyGame.Common.DataFormat
{
    [CreateAssetMenu(fileName = "RankImageData", menuName = "ScriptableObjects/RankImageData")]
    public class RankImageData : ScriptableObject
    {
        [Header("Rank Sprites")]
        public Sprite RankS;
        public Sprite RankA;
        public Sprite RankB;
        public Sprite RankC;
        public Sprite RankD;
        public Sprite NoRank;

        public Sprite GetRankImg(Rank rank)
        {
            return rank switch
            {
                Rank.SS => RankS,
                Rank.S => RankS,
                Rank.A => RankA,
                Rank.B => RankB,
                Rank.C => RankC,
                Rank.D => RankD,
                _ => NoRank
            };
        }
    }
}
