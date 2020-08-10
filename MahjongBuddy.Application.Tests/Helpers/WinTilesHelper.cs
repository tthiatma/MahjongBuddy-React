using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Linq;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Tests.Helpers
{
    public static class WinTilesHelper
    {
        public static IEnumerable<RoundTile> SetupForSevenPairs(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var twoRedDragon = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonRed).Take(2);
            userTiles.AddRange(twoRedDragon);

            var twoSticks = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Stick && t.Tile.TileValue == TileValue.Eight).Take(2);
            userTiles.AddRange(twoSticks);

            var fourEast = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindEast);
            userTiles.AddRange(fourEast);

            var twoNorth = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindNorth).Take(2);
            userTiles.AddRange(twoNorth);

            var twoCircle = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Circle && t.Tile.TileValue == TileValue.Eight).Take(2);
            userTiles.AddRange(twoCircle);

            var twoMoney = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Money && t.Tile.TileValue == TileValue.Eight).Take(2);
            userTiles.AddRange(twoMoney);

            var oneGreenDragon = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonGreen);
            userTiles.Add(oneGreenDragon);

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.DragonGreen);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForThirteenOrphans(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            //setup tiles where user have thirteen orphans
            List<RoundTile> userTiles = new List<RoundTile>();
            var circleTiles = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Circle);
            userTiles.Add(circleTiles.First(t => t.Tile.TileValue == TileValue.One));
            userTiles.Add(circleTiles.First(t => t.Tile.TileValue == TileValue.Nine));

            var stickTiles = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Stick);
            userTiles.Add(stickTiles.First(t => t.Tile.TileValue == TileValue.One));
            userTiles.Add(stickTiles.First(t => t.Tile.TileValue == TileValue.Nine));

            var moneyTiles = context.RoundTiles.Where(t => t.Tile.TileType == TileType.Money);
            userTiles.Add(moneyTiles.First(t => t.Tile.TileValue == TileValue.One));
            userTiles.Add(moneyTiles.First(t => t.Tile.TileValue == TileValue.Nine));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonRed));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonGreen));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonWhite));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindSouth));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindWest));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindNorth));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindNorth);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForStraight(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var round = context.Rounds.First();

            var oneCircle = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            var matchingTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast);

            userTiles.Add(matchingTile);

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = round.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast && t.Id != matchingTile.Id);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
                lastTile.ThrownBy = "south";
            }

            userTiles.Add(lastTile);

            context.SaveChanges();


            var ut = context.RoundTiles.Where(t => t.Owner == userId).ToList();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForInvalidWin(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var round = context.Rounds.First();

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Stick));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Stick));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Stick));

            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(round.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            var matchingTile = round.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast);
            userTiles.Add(matchingTile);
            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = round.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast && t.Id != matchingTile.Id);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForTriplets(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForMixedOneSuit(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            //userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Nine && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindWest).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Eight && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Nine && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));
            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForAllOneSuit(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Nine && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Eight && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Circle));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Seven
            && t.Tile.TileType == TileType.Circle);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForAllOneSuitStraight(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            var firsRoundThree = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            userTiles.Add(firsRoundThree);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Eight && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Nine && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Eight && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Nine && t.Tile.TileType == TileType.Circle));

            var matchingLastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            userTiles.Add(matchingLastTile);

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.First(t =>
            t.Tile.TileValue == TileValue.Three
            && t.Tile.TileType == TileType.Circle
            && t.Id != matchingLastTile.Id
            && t.Id != firsRoundThree.Id);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForAllOneSuitTriplets(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Nine && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Circle));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Seven
            && t.Tile.TileType == TileType.Circle);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForSmallDragon(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonGreen).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonRed).Take(3));


            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.DragonWhite));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.DragonWhite);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForBigDragon(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            var fourWhiteDragon = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonWhite).Take(4);
            fourWhiteDragon.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Kong);
            userTiles.AddRange(fourWhiteDragon);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonGreen).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonRed).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Money));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Money);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForBigFourWind(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var fourEast = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindEast).Take(4);
            fourEast.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Kong);
            userTiles.AddRange(fourEast);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindSouth).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindWest).Take(3));

            var fourNorth = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindNorth).Take(4);
            fourNorth.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Kong);
            userTiles.AddRange(fourNorth);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Money));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Seven && t.Tile.TileType == TileType.Money);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForSmallFourWind(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));

            var fourEast = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindEast).Take(4);
            fourEast.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Kong);
            userTiles.AddRange(fourEast);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindSouth).Take(2));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindWest).Take(3));

            var fourNorth = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindNorth).Take(4);
            fourNorth.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Kong);
            userTiles.AddRange(fourNorth);

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointsRedDragon(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var fourRedDragon = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonRed).Take(4);
            fourRedDragon.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Kong);
            userTiles.AddRange(fourRedDragon);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();

            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointsGreenDragon(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var threeGreenDragon = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonGreen).Take(3);
            threeGreenDragon.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Pong);
            userTiles.AddRange(threeGreenDragon);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointsWhiteDragon(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var threeWhiteDragon = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.DragonWhite).Take(3);
            threeWhiteDragon.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Pong);
            userTiles.AddRange(threeWhiteDragon);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointsSeatWind(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var threeWindSouth = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindSouth).Take(3);
            threeWindSouth.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Pong);
            userTiles.AddRange(threeWindSouth);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointsPrevailingWind(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var threeWindEast = context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.WindEast).Take(3);
            threeWindEast.ToList().ForEach(t => t.TileSetGroup = TileSetGroup.Pong);
            userTiles.AddRange(threeWindEast);

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.AddRange(context.RoundTiles.Where(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money).Take(3));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointSelfPick(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);
            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointConcealed(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);
            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointNoFlower(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);
            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointRomanFlower(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            //because test setup new round for new user set to wind south, set the 2nd flower roman for this data
            var romanFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerRomanTwo);
            romanFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(romanFlower);

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);
            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointNumericFlower(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            //because test setup new round for new user set to wind south, set the 2nd flower roman for this data
            var romanFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericTwo);
            romanFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(romanFlower);

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);
            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointFourNumericFlower(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            //because test setup new round for new user set to wind south, set the 2nd flower roman for this data
            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var twoNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericTwo);
            twoNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(twoNumericFlower);

            var threeNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericThree);
            threeNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(threeNumericFlower);

            var fourNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericFour);
            fourNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(fourNumericFlower);

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);
            context.SaveChanges();
            return userTiles;
        }

        public static IEnumerable<RoundTile> SetupForExtraPointLastTile(MahjongBuddyDbContext context, string userId, bool selfPick)
        {
            List<RoundTile> userTiles = new List<RoundTile>();

            var oneNumericFlower = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.FlowerNumericOne);
            oneNumericFlower.Status = TileStatus.UserGraveyard;
            userTiles.Add(oneNumericFlower);

            var oneCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Circle);
            oneCircle.Status = TileStatus.UserGraveyard;
            oneCircle.TileSetGroup = TileSetGroup.Chow;
            userTiles.Add(oneCircle);

            var twoCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Circle);
            twoCircle.TileSetGroup = TileSetGroup.Chow;
            twoCircle.Status = TileStatus.UserGraveyard;
            userTiles.Add(twoCircle);

            var threeCircle = context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Circle);
            threeCircle.TileSetGroup = TileSetGroup.Chow;
            threeCircle.Status = TileStatus.UserGraveyard;
            userTiles.Add(threeCircle);

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Circle));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Circle));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.One && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Two && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Three && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Four && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Five && t.Tile.TileType == TileType.Money));
            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.Six && t.Tile.TileType == TileType.Money));

            userTiles.Add(context.RoundTiles.First(t => t.Tile.TileValue == TileValue.WindEast));

            foreach (var t in userTiles)
            {
                t.Owner = userId;
                t.Status = TileStatus.UserActive;
            }

            var lastTile = context.RoundTiles.Last(t => t.Tile.TileValue == TileValue.WindEast);

            if (selfPick)
            {
                lastTile.Owner = userId;
                lastTile.Status = TileStatus.UserJustPicked;
            }
            else
            {
                lastTile.Owner = "board";
                lastTile.Status = TileStatus.BoardActive;
            }

            userTiles.Add(lastTile);

            context.SaveChanges();
            //set all other tiles' owner to be board for easy test lol
            context.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner)).ToList().ForEach(t => t.Owner = "board");

            context.SaveChanges();
            return userTiles;
        }
    }
}
