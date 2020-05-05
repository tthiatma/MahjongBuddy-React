using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;

namespace MahjongBuddy.Application.Extensions
{
    public static class TileValueExtensions
    {
        public static ExtraPoint ToFlowerExtraPoint(this TileValue tv)
        {
            if (tv == TileValue.FlowerNumericOne
                || tv == TileValue.FlowerNumericTwo
                || tv == TileValue.FlowerNumericThree
                || tv == TileValue.FlowerNumericFour)
                return ExtraPoint.NumericFlower;

            if (tv == TileValue.FlowerRomanOne
                || tv == TileValue.FlowerRomanTwo
                || tv == TileValue.FlowerRomanThree
                || tv == TileValue.FlowerRomanFour)
                return ExtraPoint.RomanFlower;

            return ExtraPoint.None;
        }
    }
}
