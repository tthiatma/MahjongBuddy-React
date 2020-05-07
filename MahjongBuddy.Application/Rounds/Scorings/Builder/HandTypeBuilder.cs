﻿using MahjongBuddy.Application.Rounds.Scorings.HandTypes;
using MahjongBuddy.Core;
using System.Collections.Generic;

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

        public List<HandType> GetHandType(IEnumerable<RoundTile> tiles)
        {
            if (tiles != null)
                return _initial.HandleRequest(tiles, _handtypes);
            else
                return _handtypes;
        }
    }
}
