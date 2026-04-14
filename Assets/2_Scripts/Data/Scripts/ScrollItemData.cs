using MyGame.Common.Enums;


namespace MyGame.Common.DataFormat
{
    public class ScrollItemData
    {
        public string SongName { get; set; }
        public string Artist { get; set; }
        public int HighScore { get; set; }
        public Rank Rank { get; set; }
        public DifficultyData DifficultyData { get; set; }
    }
}

