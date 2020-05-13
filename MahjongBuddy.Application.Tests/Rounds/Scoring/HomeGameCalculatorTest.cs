using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using System;
using System.Linq;
using Xunit;

namespace MahjongBuddy.Application.Tests.Rounds.Scoring
{
    public class HomeGameCalculatorTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;
        private readonly IPointsCalculator _pc;

        public HomeGameCalculatorTest(GameFixture f)
        {
            _f = f;
            _pc = new HomeGameCalculator(new ExtraPointBuilder(), new HandTypeBuilder());
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
        public void Straight_SelfPick_Should_Get_Three_Points()
        {
            var context = _f.TestDataContext;

            HomeGameCalculatorHelper.SetupForStraight(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = _pc.Calculate(round, _f.MainPlayerUserName);

            Assert.Equal(3, result.Points);
        }
    }
}
