using MahjongBuddy.Core.Enums;

namespace MahjongBuddy.Core
{
    public class RoundResultExtraPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExtraPoint ExtraPoint { get; set; }
        public virtual RoundResult RoundResult { get; set; }
        public int Point { get; set; }
    }
}
