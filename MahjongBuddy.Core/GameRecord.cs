namespace MahjongBuddy.Core
{
    public class GameRecord
    {
        public int Id { get; set; }
        public int GameNo { get; set; }
        //public Player Winner { get; set; }
        //public Player Feeder { get; set; }
        public WinningTileSet WinningTileSet { get; set; }
        public bool NoWinner { get; set; }
    }
}
