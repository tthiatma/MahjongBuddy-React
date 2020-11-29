using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.ExtraPoints
{
    class ExtraFlower : FindExtraPoint
    {
        public override List<ExtraPoint> HandleRequest(Round round, string winnerUserName, List<ExtraPoint> extraPoints)
        {
            var tiles = round.RoundTiles.Where(t => t.Owner == winnerUserName);
            var winner = round.RoundPlayers.FirstOrDefault(u => u.GamePlayer.Player.UserName == winnerUserName);

            if(winner == null)
                throw new Exception("creating round not appropriately, winner need to be in the round");

            //if user has flower corresponding to their num
            foreach (var flower in GetFlowerTileValue(winner.Wind))
            {
                var flowerTile = tiles.FirstOrDefault(t => t.Tile.TileValue == flower);
                if (flowerTile != null)
                    extraPoints.Add(flower.ToFlowerExtraPoint());
            }

            //if user has same type of flower 1,2,3,4
            var allFourRomanFlowers = tiles.Where(t => t.Tile.TileValue == TileValue.FlowerRomanOne
            || t.Tile.TileValue == TileValue.FlowerRomanTwo
            || t.Tile.TileValue == TileValue.FlowerRomanThree
            || t.Tile.TileValue == TileValue.FlowerRomanFour);
            if(allFourRomanFlowers.Count() == 4)
                extraPoints.Add(ExtraPoint.AllFourFlowerSameType);

            var allFourNumericFlowers = tiles.Where(t => t.Tile.TileValue == TileValue.FlowerNumericOne
            || t.Tile.TileValue == TileValue.FlowerNumericTwo
            || t.Tile.TileValue == TileValue.FlowerNumericThree
            || t.Tile.TileValue == TileValue.FlowerNumericFour);
            if (allFourNumericFlowers.Count() == 4)
                extraPoints.Add(ExtraPoint.AllFourFlowerSameType);

            //if user doesn't have flower at all
            var hasFlower = tiles.Any(t => t.Tile.TileType == TileType.Flower);
            if (!hasFlower)
                extraPoints.Add(ExtraPoint.NoFlower);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }

        private List<TileValue> GetFlowerTileValue(WindDirection wd)
        {
            List<TileValue> ret = new List<TileValue>();

            if(wd == WindDirection.East)
            {
                ret.Add(TileValue.FlowerRomanOne);
                ret.Add(TileValue.FlowerNumericOne);
            }

            if (wd == WindDirection.South)
            {
                ret.Add(TileValue.FlowerRomanTwo);
                ret.Add(TileValue.FlowerNumericTwo);
            }

            if (wd == WindDirection.West)
            {
                ret.Add(TileValue.FlowerRomanThree);
                ret.Add(TileValue.FlowerNumericThree);
            }

            if (wd == WindDirection.North)
            {
                ret.Add(TileValue.FlowerRomanFour);
                ret.Add(TileValue.FlowerNumericFour);
            }

            return ret;
        }
    }
}
