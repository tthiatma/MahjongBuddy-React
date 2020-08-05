using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MahjongBuddy.Application.Helpers
{
    public static class RoundTileHelper
    {

        //custom setup for win, pong, and chow priority
        public static void SetupForWinPongChowPriority(IEnumerable<RoundTile> roundTiles)
        {
            string tonny = "Tonny";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Three).Take(1).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserJustPicked; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.One).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Two).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Three).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Four).Take(1).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });

            //mei can chow
            string mei = "Mei";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.One & string.IsNullOrEmpty(x.Owner)).Take(1).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Two & string.IsNullOrEmpty(x.Owner)).Take(1).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Seven & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Eight & string.IsNullOrEmpty(x.Owner)).Take(4).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Nine & string.IsNullOrEmpty(x.Owner)).Take(4).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });

            //peter can pong
            string peter = "Peter";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Three & string.IsNullOrEmpty(x.Owner)).Take(2).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Five).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Six).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Eight).Take(3).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });

            //jason can win
            string jason = "Jason";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Four).Take(1).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Five).Take(1).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Three).Take(3).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Four).Take(3).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Five).Take(3).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Eight).Take(2).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
        }


        //custom tile assignment for easy win and chow debugging
        public static void SetupForChowAndWin(IEnumerable<RoundTile> roundTiles)
        {
            string tonny = "Tonny";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Three).Take(1).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserJustPicked; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.One).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Two).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Three).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Four).Take(1).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });

            string mei = "Mei";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.One & string.IsNullOrEmpty(x.Owner)).Take(4).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Two & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Three & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Four & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Five).Take(1).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });

            string peter = "Peter";
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Five).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Six).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Seven & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Eight).Take(2).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });

            string jason = "Jason";
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Five).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Six).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Seven & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Eight).Take(2).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
        }


        //custom tile assignment for easy kong debugging
        public static void SetupForKong(IEnumerable<RoundTile> roundTiles)
        {
            string tonny = "Tonny";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Three).Take(1).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserJustPicked; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.One).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Two).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Three).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Circle && x.Tile.TileValue == TileValue.Four).Take(1).ForEach(rt => { rt.Owner = tonny; rt.Status = TileStatus.UserActive; });

            string mei = "Mei";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.One).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Two).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Three & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Four).Take(2).ForEach(rt => { rt.Owner = mei; rt.Status = TileStatus.UserActive; });

            string peter = "Peter";
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Five).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Six).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Seven & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Money && x.Tile.TileValue == TileValue.Eight).Take(2).ForEach(rt => { rt.Owner = peter; rt.Status = TileStatus.UserActive; });

            string jason = "Jason";
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Five).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Six).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Seven & string.IsNullOrEmpty(x.Owner)).Take(3).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
            roundTiles.Where(x => x.Tile.TileType == TileType.Stick && x.Tile.TileValue == TileValue.Eight).Take(2).ForEach(rt => { rt.Owner = jason; rt.Status = TileStatus.UserActive; });
        }

        public static void AssignTilesToUser(int tilesCount, string userId, IEnumerable<RoundTile> roundTiles)
        {
            var newTiles = roundTiles.Where(x => string.IsNullOrEmpty(x.Owner));
            int x = 0;
            foreach (var playTile in newTiles)
            {
                playTile.Owner = userId;
                if (playTile.Tile.TileType == TileType.Flower)
                {
                    playTile.Status = TileStatus.UserGraveyard;
                }
                else
                {
                    playTile.Status = TileStatus.UserActive;
                    x++;
                }
                if (x == tilesCount)
                    return;
            }
        }

        public static List<RoundTile>CreateTiles(MahjongBuddyDbContext context)
        {
            var allTiles = context.Tiles.ToList();
            List<RoundTile> tiles = new List<RoundTile>();

            for (var i = 1; i < 5; i++)
            {
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.One) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Two) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Three) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Four) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Five) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Six) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Seven) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Eight) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Money && x.TileValue == TileValue.Nine) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.One) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Two) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Three) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Four) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Five) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Six) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Seven) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Eight) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Circle && x.TileValue == TileValue.Nine) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.One) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Two) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Three) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Four) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Five) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Six) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Seven) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Eight) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Stick && x.TileValue == TileValue.Nine) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Dragon && x.TileValue == TileValue.DragonGreen) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Dragon && x.TileValue == TileValue.DragonRed) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Dragon && x.TileValue == TileValue.DragonWhite) });

                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindEast) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindSouth) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindWest) });
                tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Wind && x.TileValue == TileValue.WindNorth) });

            };

            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanOne) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanTwo) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanThree) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerRomanFour) });

            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericOne) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericTwo) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericThree) });
            tiles.Add(new RoundTile {Tile = allTiles.Find(x => x.TileType == TileType.Flower && x.TileValue == TileValue.FlowerNumericFour) });

            return tiles;
        }

        public static void PickTile(Round round, string pickerUserName, ref List<RoundTile> updatedTiles)
        {
            //we loop 8 times because there are total of 8 flowers. get more tiles if its a flower
            for (var i = 0; i < 8; i++)
            {
                var newTile = round.RoundTiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                if (newTile == null)
                {
                    round.IsEnding = true;
                    break;
                }

                newTile.Owner = pickerUserName;
                round.IsHalted = true;

                if (newTile.Tile.TileType != TileType.Flower)
                {
                    newTile.Status = TileStatus.UserJustPicked;
                    updatedTiles.Add(newTile);
                    break;
                }
                else
                {
                    newTile.Status = TileStatus.UserGraveyard;
                    updatedTiles.Add(newTile);
                }
            }
        }

        public static HandType DetermineHandCanWin(IEnumerable<RoundTile> tiles)
        {
            //TODO leverage tile that already in user graveyard in determining user hand type

            //if all weird combo hand is checked, in order to win 
            //tiles needs to be either, chicken, straight, or triplet

            var listTiles = tiles.ToList();
            //check if there's kong tiles, because the total tiles will be more than 14
            //we want to take off 1 kong tiles to make it total of 14 tiles to easily determine valid tiles to win
            var kongTiles = tiles
                .Where(t => t.TileSetGroup == TileSetGroup.Kong)
                .GroupBy(t => new { t.Tile.TileType, t.Tile.TileValue });

            foreach (var group in kongTiles)
            {
                //take off 1 tile
                listTiles.Remove(group.First());
            }
            tiles = listTiles;
            if (tiles.Count() != 14)
                return HandType.None;

            //get possible eyes
            List<IEnumerable<RoundTile>> eyeCollection = new List<IEnumerable<RoundTile>>();
            foreach (var t in tiles)
            {
                var sameTiles = tiles.Where(ti => ti.Tile.TileValue == t.Tile.TileValue 
                && ti.Tile.TileType == t.Tile.TileType);

                if (sameTiles != null && sameTiles.Count() > 1)
                {
                    bool exist = false;
                    var tv = sameTiles.First().Tile.TileValue;
                    var tp = sameTiles.First().Tile.TileType;
                    foreach (IEnumerable<RoundTile> e in eyeCollection)
                    {
                        if (e.Where(t => t.Tile.TileType == tp && t.Tile.TileValue == tv).Count() > 0)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)
                        eyeCollection.Add(sameTiles.Take(2));
                }
            }

            if (eyeCollection.Count() == 0)
                return HandType.None;

            //test for triplet first because it has higher point
            bool allPong = false;
            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all pong
                for (int i = 0; i < 4; i++)
                {
                    var pongSet = GetPongSet(tilesWithoutEyes);
                    if(pongSet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(pongSet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        allPong = true;
                }
            }
            if (allPong)
                return HandType.Triplets;


            //test for straight
            bool allStraight = false;

            //if there are 3+ wind or 3+ dragon, then its not possible for the tiless to be straight
            var windOrDragonTiles = tiles.Where(t => t.Tile.TileType == TileType.Dragon || t.Tile.TileType == TileType.Wind);

            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all straight
                for (int i = 0; i < 4; i++)
                {
                    var straightSet = GetStraightSet(tilesWithoutEyes);
                    if (straightSet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(straightSet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        allStraight = true;
                }
            }
            if (allStraight)
                return HandType.Straight;

            //test for chicken
            bool isChicken = false;
            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all set
                for (int i = 0; i < 4; i++)
                {
                    var anySet = TryPongThenChowSet(tilesWithoutEyes);
                    if (anySet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(anySet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        isChicken = true;
                }
            }
            if (isChicken)
                return HandType.Chicken;

            //TODO definitely need to refactor this lol
            //another try for chicken (not sure if below is needed)
            //test for chicken
            foreach (var eyes in eyeCollection)
            {
                //remove possible eyes from tiles
                var tilesWithoutEyes = tiles.OrderBy(t => t.Tile.TileValue).ToList();
                foreach (var t in eyes)
                {
                    tilesWithoutEyes.Remove(t);
                }

                //try check all set
                for (int i = 0; i < 4; i++)
                {
                    var anySet = TryChowThenPongSet(tilesWithoutEyes);
                    if (anySet == null)
                    {
                        break;
                    }
                    tilesWithoutEyes = tilesWithoutEyes.Except(anySet).ToList();
                    //if it gets all the way to last set
                    if (i == 3)
                        isChicken = true;
                }
            }
            if (isChicken)
                return HandType.Chicken;

            return HandType.None;
        }

        public static IEnumerable<RoundTile> GetPongSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindPongTiles(t, tiles);
                if (temp.Count() == 3)
                    return temp;
            }
            return null;
        }

        public static IEnumerable<RoundTile> TryPongThenChowSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindPongTiles(t, tiles);
                if (temp.Count() == 0)
                    temp = FindStraightTiles(t, tiles);
                if (temp != null && temp.Count() == 3)
                    return temp;
            }
            return null;
        }

        //not sure if below is needed lollll
        public static IEnumerable<RoundTile> TryChowThenPongSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                var temp = FindStraightTiles(t, tiles);
                if (temp.Count() == 0)
                    temp = FindPongTiles(t, tiles);
                if (temp != null && temp.Count() == 3)
                    return temp;
            }
            return null;
        }

        public static IEnumerable<RoundTile> GetStraightSet(IEnumerable<RoundTile> tiles)
        {
            foreach (var t in tiles)
            {
                if (t.Tile.TileType == TileType.Dragon || t.Tile.TileType == TileType.Wind)
                    return null;

                var temp = FindStraightTiles(t, tiles);
                if (temp.Count() == 3)
                    return temp;
            }
            return null;
        }

        public static List<RoundTile> FindStraightTiles(RoundTile theTile, IEnumerable<RoundTile> tiles)
        {
            var ret = new List<RoundTile>();

            var sameTypeTiles = tiles.Where(t => t.Tile.TileType == theTile.Tile.TileType);
            foreach (var t in sameTypeTiles)
            {
                if (sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue - 2)) && sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue - 1)))
                {
                    ret.Add(t);
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue == (t.Tile.TileValue - 2)).First());
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue == (t.Tile.TileValue - 1)).First());
                    break;
                }

                if (sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue - 1)) && sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue + 1)))
                {
                    ret.Add(t);
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue == (t.Tile.TileValue - 1)).First());
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue == (t.Tile.TileValue + 1)).First());
                    break;
                }

                if (sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue + 1)) && sameTypeTiles.Any(ti => ti.Tile.TileValue == (t.Tile.TileValue + 2)))
                {
                    ret.Add(t);
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue == (t.Tile.TileValue + 1)).First());
                    ret.Add(sameTypeTiles.Where(ti => ti.Tile.TileValue == (t.Tile.TileValue + 2)).First());
                    break;
                }
            }
            return ret;
        }

        public static List<List<RoundTile>> FindPossibleChowTiles(RoundTile theTile, IEnumerable<RoundTile> tiles)
        {
            var ret = new List<List<RoundTile>>();

            var sameTypeTiles = tiles.Where(t => t.Tile.TileType == theTile.Tile.TileType && t.Status == TileStatus.UserActive);

            if(theTile.Tile.TileValue == TileValue.One)
            {
                var tileTwo = sameTypeTiles.FirstOrDefault(t => t.Tile.TileValue == TileValue.Two);
                var tileThree = sameTypeTiles.FirstOrDefault(t => t.Tile.TileValue == TileValue.Three);
                if(tileTwo != null && tileThree != null)
                {
                    ret.Add(new List<RoundTile> { tileTwo, tileThree });
                }
            }
            else if (theTile.Tile.TileValue == TileValue.Nine)
            {
                var tileEight = sameTypeTiles.FirstOrDefault(t => t.Tile.TileValue == TileValue.Eight);
                var tileSeven = sameTypeTiles.FirstOrDefault(t => t.Tile.TileValue == TileValue.Seven);
                if (tileEight != null && tileSeven != null)
                {
                    ret.Add(new List<RoundTile> { tileEight, tileSeven});
                }
            }
            else
            {
                var possibleTiles = sameTypeTiles.Where(rt => 
                rt.Tile.TileValue == theTile.Tile.TileValue - 2 ||
                rt.Tile.TileValue == theTile.Tile.TileValue + 2 ||
                rt.Tile.TileValue == theTile.Tile.TileValue - 1 ||
                rt.Tile.TileValue == theTile.Tile.TileValue + 1);

                if(possibleTiles.Count() > 1)
                    possibleTiles = possibleTiles.DistinctBy(rt => rt.Tile.TileValue);

                foreach (var t in possibleTiles)
                {
                    List<RoundTile> testChowTiles = new List<RoundTile>();
                    testChowTiles.Add(t);
                    testChowTiles.Add(theTile);
                    testChowTiles.Sort((a, b) => a.Tile.TileValue - b.Tile.TileValue);

                    RoundTile probableTile;
                    if(((int)t.Tile.TileValue + (int)theTile.Tile.TileValue) % 2 != 0)
                    {
                        probableTile = possibleTiles.FirstOrDefault(rt => rt.Tile.TileValue == testChowTiles[1].Tile.TileValue + 1);
                    }
                    else
                    {
                        probableTile = possibleTiles.FirstOrDefault(rt => rt.Tile.TileValue == testChowTiles[0].Tile.TileValue + 1);
                    }

                    if(probableTile != null)
                    {
                        //check if its already exist
                        bool alreadyExist = false;
                        foreach (var rts in ret)
                        {
                            var existingTiles = rts.Where(rt => rt.Tile.TileValue == probableTile.Tile.TileValue || rt.Tile.TileValue == t.Tile.TileValue);
                            if(existingTiles.Count() == 2)
                            {
                                alreadyExist = true;
                                break;
                            }
                        }

                        if(!alreadyExist)
                        {
                            var foundChowTile = new List<RoundTile> { probableTile, t };
                            foundChowTile.Sort((a, b) => a.Tile.TileValue - b.Tile.TileValue);
                            ret.Add(foundChowTile);
                        }
                    }
                }
            }
            return ret;
        }

        public static List<RoundTile> FindPongTiles(RoundTile theTile, IEnumerable<RoundTile> tiles)
        {
            var ret = new List<RoundTile>();
            var sameTiles = tiles.Where(t => t.Tile.TileType == theTile.Tile.TileType && t.Tile.TileValue == theTile.Tile.TileValue);
            if (sameTiles != null && sameTiles.Count() == 3)
            {
                foreach (var t in sameTiles)
                {
                    ret.Add(t);
                }
            }
            return ret;
        }
    }
}
