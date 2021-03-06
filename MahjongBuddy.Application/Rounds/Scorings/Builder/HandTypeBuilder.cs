﻿using MahjongBuddy.Application.Rounds.Scorings.HandTypes;
using MahjongBuddy.Core;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.Scorings.Builder
{
    public class HandTypeBuilder
    {
        readonly FindHandType _initial;
         public HandTypeBuilder()
        {
            _initial = new InitialHandType();
            FindHandType sevenPairs = new SevenPairs();
            FindHandType thirteenOrphans = new ThirteenOrphans();
            FindHandType legitSet = new LegitSet();
            FindHandType oneSuit = new OneSuit();
            FindHandType dragon = new Dragon();
            FindHandType fourWind = new FourWind();
            FindHandType hiddenTreasure = new HiddenTreasure();
            FindHandType allTerminals = new AllTerminals();
            FindHandType allKongs = new AllKongs();
            FindHandType allHonors = new AllHonors();

            _initial.SetSuccessor(sevenPairs);
            sevenPairs.SetSuccessor(thirteenOrphans);
            thirteenOrphans.SetSuccessor(legitSet);
            legitSet.SetSuccessor(oneSuit);
            oneSuit.SetSuccessor(dragon);
            dragon.SetSuccessor(fourWind);
            fourWind.SetSuccessor(hiddenTreasure);
            hiddenTreasure.SetSuccessor(allTerminals);
            allTerminals.SetSuccessor(allKongs);
            allKongs.SetSuccessor(allHonors);
        }

        public List<HandType> GetHandType(Round round, string winnerUserName)
        {
            var _handtypes = new List<HandType>();
            if (round != null && !string.IsNullOrEmpty(winnerUserName))
            {
                var boardTile = round.RoundTiles.FirstOrDefault(t => t.Owner == DefaultValue.board && t.Status == TileStatus.BoardActive);
                var userTiles = round.RoundTiles.Where(t => t.Owner == winnerUserName).ToList();
                var justPicked = userTiles.FirstOrDefault(t => t.Status == TileStatus.UserJustPicked);

                if(justPicked == null && boardTile != null)
                    userTiles.Add(boardTile);

                return _initial.HandleRequest(userTiles, _handtypes);
            }
            else
                return _handtypes;
        }
    }
}
