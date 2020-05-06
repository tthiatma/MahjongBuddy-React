using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace MahjongBuddy.Application.Tests.Rounds.Scoring
{
    public class ExtraPointsTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;

        public ExtraPointsTest(GameFixture f)
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
        public void Detect_RedDragon()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointsRedDragon(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();
            
            var result = new ExtraPointBuilder(round, _f.MainPlayerUserName).GetExtraPoint();

            Assert.Single(result);
            Assert.Contains(ExtraPoint.RedDragon, result);
        }

        [Fact]
        public void Detect_GreenDragon()
        {

        }

        [Fact]
        public void Detect_WhiteDragon()
        {

        }

        [Fact]
        public void Detect_SeatWind()
        {

        }

        [Fact]
        public void Detect_PrevailingWind()
        {

        }

        [Fact]
        public void Detect_SelfPick()
        {

        }

        [Fact]
        public void Detect_ConcealedHand()
        {

        }

        [Fact]
        public void Detect_NoFlower()
        {

        }

        [Fact]
        public void Detect_RomanFlower()
        {

        }

        [Fact]
        public void Detect_NumericFlower()
        {

        }

        [Fact]
        public void Detect_AllFourFlowerSameType()
        {

        }

        [Fact]
        public void Detect_WinOnLastTile()
        {

        }
    }
}
