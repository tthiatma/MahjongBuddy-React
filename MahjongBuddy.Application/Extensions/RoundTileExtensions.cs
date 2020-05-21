using System.Collections.Generic;
using System.Linq;
using MahjongBuddy.Core;

namespace MahjongBuddy.Application.Extensions
{
    public static class RoundTileExtensions
    {
        public static ICollection<RoundTile> PickTile(this ICollection<RoundTile> tiles, string userName)
        {
            //loop 8 times because there are 8 flowers
            List<RoundTile> ret = new List<RoundTile>();
            for (var i = 0; i < 8; i++)
                {
                    var newTile = tiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                    
                    if (newTile == null)
                        return null;

                    newTile.Owner = userName;

                    if (newTile.Tile.TileType != TileType.Flower)
                    {
                        newTile.Status = TileStatus.UserJustPicked;
                        ret.Add(newTile);
                        break;
                    }
                    else
                    {
                        newTile.Status = TileStatus.UserGraveyard;
                        ret.Add(newTile);
                    }
                }

            return ret;
        }
        public static void GoGraveyard(this ICollection<RoundTile> tiles, string userName, TileSetGroup setGroup, int groupIndex)
        {
            foreach(var tile in tiles)
            {
                tile.Owner = userName;
                tile.TileSetGroup = setGroup;
                tile.Status = TileStatus.UserGraveyard;
                tile.TileSetGroupIndex = groupIndex;
            }
        }
        public static int GetLastGroupIndex(this ICollection<RoundTile> tiles, string userName)
        {
            int groupIndex = 1;
            var tileSets = tiles.Where(t => t.Owner == userName && t.TileSetGroup != TileSetGroup.None);
            if (tileSets.Count() > 0)
            {
                var highestNumber = tileSets.Select(t => t.TileSetGroupIndex).Distinct().Max();
                groupIndex = highestNumber + 1;
            }
            return groupIndex;
        }
    }
}
