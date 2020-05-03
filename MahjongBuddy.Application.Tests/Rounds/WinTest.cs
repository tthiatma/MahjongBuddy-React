using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Core;
using System;
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

            var tiles = WinTilesHelper.SetupForSevenPairsWithBoard(context, _f.MainPlayerUserName);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var result = new HandTypeHelper(tiles).GetHandType();

            Assert.Equal(HandType.SevenPairs, result);
        }

        [Fact]
        public void Detect_ThirteenOrphans()
        {
            var context = _f.TestDataContext;

            var tiles = WinTilesHelper.SetupForThirteenOrphansWithBoard(context, _f.MainPlayerUserName);

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var result = new HandTypeHelper(tiles).GetHandType();

            Assert.Equal(HandType.ThirteenOrphans, result);
        }
    }
}
