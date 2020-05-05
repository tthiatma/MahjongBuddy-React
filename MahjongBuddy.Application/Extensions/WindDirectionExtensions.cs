using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Extensions
{
    public static class WindDirectionExtensions
    {
        public static TileValue ToTileValue(this WindDirection wd)
        {
            switch (wd)
            {
                case WindDirection.East:
                    return TileValue.WindEast;
                case WindDirection.South:
                    return TileValue.WindSouth;
                case WindDirection.West:
                    return TileValue.WindWest;
                case WindDirection.North:
                    return TileValue.WindNorth;
                default:
                    return TileValue.None;
            }
        }
    }
}
