﻿using MahjongBuddy.Application.Rounds.Scorings.HandTypes;
using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.Builder
{
    public class HandTypeBuilder
    {
        readonly FindHandType _initial;
        List<HandType> _handtypes;
        public HandTypeBuilder()
        {
            _handtypes = new List<HandType>();
            _initial = new InitialHandType();
            FindHandType sevenPairs = new SevenPairs();
            FindHandType thirteenOrphans = new ThirteenOrphans();
            FindHandType legitSet = new LegitSet();
            FindHandType oneSuit = new OneSuit();
            FindHandType dragon = new Dragon();
            FindHandType fourWind = new FourWind();

            _initial.SetSuccessor(sevenPairs);
            sevenPairs.SetSuccessor(thirteenOrphans);
            thirteenOrphans.SetSuccessor(legitSet);
            legitSet.SetSuccessor(oneSuit);
            oneSuit.SetSuccessor(dragon);
            dragon.SetSuccessor(fourWind);
        }

        public List<HandType> GetHandType(Round round, string winnerUserName)
        {
            if (round != null && !string.IsNullOrEmpty(winnerUserName))
            {
                List<RoundTile> tiles = new List<RoundTile>();

                var boardTile = round.RoundTiles.FirstOrDefault(t => t.Owner == "board");
                var userTiles = round.RoundTiles.Where(t => t.Owner == winnerUserName).ToList();

                //check if user need to get the tile from board to win 

                //check if there is just picked tiles
                var justPicked = userTiles.FirstOrDefault(t => t.Status == TileStatus.UserJustPicked);

                if(justPicked != null)
                    tiles = userTiles;
                else
                {
                    if(boardTile != null)
                        tiles.Add(boardTile);
                }

                return _initial.HandleRequest(tiles, _handtypes);
            }
            else
                return _handtypes;
        }
    }
}
