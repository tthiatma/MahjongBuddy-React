﻿using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MahjongBuddy.Application.Tests.Rounds
{
    public class WinTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;

        public WinTest(GameFixture f)
        {
            _f = f;
        }
        public void Dispose()
        {
            _f.TestDataContext.RemoveRange(_f.TestDataContext.Rounds);
            _f.TestDataContext.SaveChanges();
            _f.CreateRound();
            _f.TestDataContext.SaveChanges();
            _f.RoundId = _f.TestDataContext.Rounds.First().Id;
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().UserRounds.First(u => u.AppUser.UserName == _f.MainPlayerUserName);
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().UserRounds.First(u => u.AppUser.UserName == _f.OtherPlayerName);
        }

        [Fact]
        public void Detect_SevenPairs()
        {
            var context = _f.TestDataContext;

            var tiles = WinTilesHelper.SetupForSevenPairs(context, _f.MainPlayerUserName, selfPick: true);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var result = new HandTypeHelper(tiles).GetHandType();

            Assert.Contains(HandType.SevenPairs, result);
        }

        [Fact]
        public void Detect_ThirteenOrphans()
        {
            var context = _f.TestDataContext;

            var tiles = WinTilesHelper.SetupForThirteenOrphans(context, _f.MainPlayerUserName, selfPick: true);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            List<HandType> result = new HandTypeHelper(tiles).GetHandType();

            Assert.Contains(HandType.ThirteenOrphans, result);
        }

        [Fact]
        public void Detect_Triplets()
        {
            var context = _f.TestDataContext;

            var tiles = WinTilesHelper.SetupForTriplets(context, _f.MainPlayerUserName, selfPick: true);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var result = new HandTypeHelper(tiles).GetHandType();

            Assert.Contains(HandType.Triplets, result);
        }

        [Fact]
        public void Detect_Straight()
        {
            var context = _f.TestDataContext;

            var tiles = WinTilesHelper.SetupForStraight(context, _f.MainPlayerUserName, selfPick: true);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var result = new HandTypeHelper(tiles).GetHandType();

            Assert.Contains(HandType.Straight, result);
        }
        [Fact]
        public void Detect_MixedOneSuit()
        {
            var context = _f.TestDataContext;

            var tiles = WinTilesHelper.SetupForMixedOneSuit(context, _f.MainPlayerUserName, selfPick: true);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var result = new HandTypeHelper(tiles).GetHandType();

            Assert.Contains(HandType.MixedOneSuit, result);
            Assert.Contains(HandType.Chicken, result);
        }
    }
}
