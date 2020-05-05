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
            var winner = round.UserRounds.FirstOrDefault(u => u.AppUser.UserName == winnerUserName);

            if(winner == null)
                throw new Exception("creating round not appropriately, winner need to be in the round");

            //something wrong here
            //if user has no flower at all
            if (winner.FlowerNum == 0)
                throw new Exception("creating round not appropriately, user need to have flower num set");

            //if user has flower corresponding to their num
            foreach (var flower in GetFlowerTileValue(winner.FlowerNum))
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
                extraPoints.Add(ExtraPoint.AllFourFlowerSameType);

            if (_successor != null)
                return _successor.HandleRequest(round, winnerUserName, extraPoints);
            else
                return extraPoints;
        }

        private List<TileValue> GetFlowerTileValue(int flowerNum)
        {
            List<TileValue> ret = new List<TileValue>();

            if(flowerNum == 1)
            {
                ret.Add(TileValue.FlowerRomanOne);
                ret.Add(TileValue.FlowerNumericOne);
            }

            if (flowerNum == 2)
            {
                ret.Add(TileValue.FlowerRomanTwo);
                ret.Add(TileValue.FlowerNumericTwo);
            }

            if (flowerNum == 3)
            {
                ret.Add(TileValue.FlowerRomanThree);
                ret.Add(TileValue.FlowerNumericThree);
            }

            if (flowerNum == 4)
            {
                ret.Add(TileValue.FlowerRomanFour);
                ret.Add(TileValue.FlowerNumericFour);
            }

            return ret;
        }
    }
}
