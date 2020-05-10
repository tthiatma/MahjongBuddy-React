using AutoMapper;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Core;
using Moq;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace MahjongBuddy.Application.Tests.Rounds
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
            _f.CreateRound();
            _f.TestDataContext.SaveChanges();
            _f.RoundId = _f.TestDataContext.Rounds.First().Id;
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().UserRounds.First(u => u.AppUser.UserName == _f.MainPlayerUserName);
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().UserRounds.First(u => u.AppUser.UserName == _f.OtherPlayerName);
        }

        public void Dispose()
        {
            _f.TestDataContext.RemoveRange(_f.TestDataContext.Rounds);
            _f.TestDataContext.SaveChanges();
        }

        [Fact]
        public void Should_Be_Valid_Win()
        {
            var context = _f.TestDataContext;

            //setup common scenario to win
            var ut = context.RoundTiles.Where(t => t.Owner == _f.MainPlayerUserName);
            WinTilesHelper.SetupForStraight(context, _f.MainPlayerUserName, selfPick: true);
            ut = context.RoundTiles.Where(t => t.Owner == _f.MainPlayerUserName);

            var winCommand = new Win.Command
            {
                GameId = _f.GameId,
                RoundId = _f.RoundId,
                UserName = _f.MainPlayerUserName
            };

            var sut = new Win.Handler(context, _mapper, _f.PointCalculator);
            ut = context.RoundTiles.Where(t => t.Owner == _f.MainPlayerUserName);
            var result = sut.Handle(winCommand, CancellationToken.None).Result;

            //Assert.Equal("Game1", result.round);
        }
    }
}
