using System.Collections.Generic;
using MyGame.Common.Enums;

namespace MyGame.Common.DataFormat
{
    public class ResultData
    {
        public string BeatmapID { get; set; }
        public Rank Rank { get; set; }
        public int Score { get; set; }
        public int Combo { get; set; }
        public Dictionary<JudgeType, int> JudgeCount { get; set; }

    }
}
