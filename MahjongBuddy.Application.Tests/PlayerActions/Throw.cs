using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Application.PlayerAction;
using MahjongBuddy.Core;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using Xunit;

namespace MahjongBuddy.Application.Tests.PlayerActions
{
    public class Throw : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;

        public Throw(GameFixture f)
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

        /// <summary>
        /// When it's not player's turn, player should still be able to kong from board
        /// if player has 3 same tiles in their active tile
        /// </summary>
        //[Fact]
        //public void Player_Should_Get_PongAction()
        //{
        //    var context = _f.TestDataContext;

        //    TileType circ = TileType.Circle;
        //    TileValue one = TileValue.One;

        //    KongTilesHelper.SetupForBoard(context, _f.MainPlayerUserName, circ, one);

        //    _f.OtherPlayerRound.IsMyTurn = true;
        //    _f.MainPlayerRound.IsMyTurn = false;

        //    var kongCommand = new Kong.Command
        //    {
        //        RoundId = _f.RoundId,
        //        UserName = _f.MainPlayerUserName,
        //        TileType = circ,
        //        TileValue = one
        //    };

        //    var sut = new Kong.Handler(context, _f.RoundMapper);

        //    var result = sut.Handle(kongCommand, CancellationToken.None).Result;

        //    Assert.True(result.UpdatedRoundPlayers.First(u => u.UserName == _f.MainPlayerUserName).IsMyTurn);
        //}
    }
}
