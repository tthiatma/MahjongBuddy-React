namespace MahjongBuddy.Core
{
    public class RoundResultHand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HandType HandType { get; set; }
        public virtual RoundResult RoundResult { get; set; }
        public int Point { get; set; }
    }
}
