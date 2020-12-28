using AutoMapper;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.PlayerAction;
using MahjongBuddy.Application.Rounds;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MahjongBuddy.Application.Tests.PlayerActions
{
    public class WinTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly GameFixture _f;


        public WinTest(GameFixture f)
        {
            _f = f;
            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); });
            _mapper = mockMapper.CreateMapper();
        }

        public void Dispose()
        {
            _f.TestDataContext.RemoveRange(_f.TestDataContext.Rounds);
            _f.TestDataContext.RemoveRange(_f.TestDataContext.RoundPlayers);
            _f.TestDataContext.RemoveRange(_f.TestDataContext.RoundResults);
            _f.TestDataContext.RemoveRange(_f.TestDataContext.RoundHands);
            _f.TestDataContext.RemoveRange(_f.TestDataContext.RoundExtraPoints);
            _f.TestDataContext.SaveChanges();
            _f.CreateRound();
            _f.TestDataContext.SaveChanges();
            _f.RoundId = _f.TestDataContext.Rounds.First().Id;
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.Player.UserName == _f.MainPlayerUserName);
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.Player.UserName == _f.OtherPlayerName);
        }

        //[Fact]
        //public void Should_Be_Valid_Win()
        //{
        //    var context = _f.TestDataContext;

        //    //setup common scenario to win
        //    WinTilesHelper.SetupForStraight(context, _f.MainPlayerUserName, selfPick: true);

        //    var winCommand = new Win.Command
        //    {
        //        GameId = _f.GameId,
        //        RoundId = _f.RoundId,
        //        UserName = _f.MainPlayerUserName
        //    };

        //    var sut = new Win.Handler(context, _mapper, _f.PointCalculator);
        //    var result = sut.Handle(winCommand, CancellationToken.None).Result;
        //    var winnerResult = result.RoundResults.First(u => u.IsWinner == true);
        //    var winner = result.RoundPlayers.First(u => u.UserName == winnerResult.UserName);
        //    Assert.Equal(9, winner.Points);
        //    Assert.Equal(4, result.RoundResults.Count());
        //}

        [Fact]
        public async Task Should_Be_Invalid_Win()
        {
            var context = _f.TestDataContext;

            //setup common scenario can't win
            WinTilesHelper.SetupForInvalidWin(context, _f.MainPlayerUserName);

            var winCommand = new Win.Command
            {
                GameCode = _f.GameCode,
                RoundId = _f.RoundId,
                UserName = _f.MainPlayerUserName
            };

            var sut = new Win.Handler(context, _mapper, _f.PointCalculator);
            await Assert.ThrowsAsync<RestException>(() => sut.Handle(winCommand, CancellationToken.None));
        }

        //[Fact]
        //public void AllOneSuit_Should_Be_Bao()
        //{
        //    var context = _f.TestDataContext;

        //    //setup allonesuit bao
        //    WinTilesHelper.SetupForBaoAllOneSuit(context, _f.MainPlayerUserName);

        //    var winCommand = new Win.Command
        //    {
        //        GameId = _f.GameId,
        //        RoundId = _f.RoundId,
        //        UserName = _f.MainPlayerUserName
        //    };

        //    var sut = new Win.Handler(context, _mapper, _f.PointCalculator);

        //    var result = sut.Handle(winCommand, CancellationToken.None).Result;

        //    var winnerResult = result.RoundResults.First(u => u.IsWinner == true);
            
        //    var winner = result.RoundPlayers.First(u => u.UserName == winnerResult.UserName);

        //    Assert.Equal(27, winner.Points);
        //    Assert.Equal(2, result.RoundResults.Count());
        //}
    }
}
