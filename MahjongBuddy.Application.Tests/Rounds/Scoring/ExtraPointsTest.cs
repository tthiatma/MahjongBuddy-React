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
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.AppUser.UserName == _f.MainPlayerUserName);
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.AppUser.UserName == _f.OtherPlayerName);
        }

        [Fact]
        public void Detect_RedDragon()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointsRedDragon(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();
            
            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.RedDragon, result);
        }

        [Fact]
        public void Detect_GreenDragon()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointsGreenDragon(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.GreenDragon, result);           
        }

        [Fact]
        public void Detect_WhiteDragon()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointsWhiteDragon(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.WhiteDragon, result);
        }

        [Fact]
        public void Detect_SeatWind()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointsSeatWind(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.SeatWind, result);
        }

        [Fact]
        public void Detect_PrevailingWind()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointsPrevailingWind(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.PrevailingWind, result);
        }

        [Fact]
        public void Detect_SelfPick()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointSelfPick(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.SelfPick, result);
        }

        [Fact]
        public void Detect_ConcealedHand()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointConcealed(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.ConcealedHand, result);
        }

        [Fact]
        public void Detect_NoFlower()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointNoFlower(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.NoFlower, result);
        }

        [Fact]
        public void Detect_RomanFlower()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointRomanFlower(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.RomanFlower, result);
        }

        [Fact]
        public void Detect_NumericFlower()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointNumericFlower(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.NumericFlower, result);
        }

        [Fact]
        public void Detect_AllFourFlowerSameType()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointFourNumericFlower(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Equal(2, result.Count());
            Assert.Contains(ExtraPoint.NumericFlower, result);
            Assert.Contains(ExtraPoint.AllFourFlowerSameType, result);
        }

        [Fact]
        public void Detect_WinOnLastTile()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForExtraPointLastTile(context, _f.MainPlayerUserName, selfPick: false);

            var round = _f.TestDataContext.Rounds.First();

            var result = new ExtraPointBuilder().GetExtraPoint(round, _f.MainPlayerUserName);

            Assert.Single(result);
            Assert.Contains(ExtraPoint.WinOnLastTile, result);
        }
    }
}
