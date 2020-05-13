using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Dtos
{
    public class RoundResultHandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HandType HandType { get; set; }
        public int Point { get; set; }
    }
}
