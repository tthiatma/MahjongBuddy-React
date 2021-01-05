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
    public class KongTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;

        public KongTest(GameFixture f)
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
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.Player.UserName == _f.MainPlayerUserName);
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.Player.UserName == _f.OtherPlayerName);
        }

        /// <summary>
        /// When it's not player's turn, player should still be able to kong from board
        /// if player has 3 same tiles in their active tile
        /// </summary>
        //[Fact]
        //public void NotMyTurn_Should_Able_Kong_Board_Active()
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

        //    var sut = new Kong.Handler(context, _f.RoundMapper, _f.PointCalculator);

        //    var result = sut.Handle(kongCommand, CancellationToken.None).Result;

        //    Assert.True(result.UpdatedRoundPlayers.First(u => u.UserName == _f.MainPlayerUserName).IsMyTurn);
        //}

        /// <summary>
        /// When it's player's turn, player should still be able to kong from board
        /// if player has 3 same tiles in their active tile
        /// </summary>
        //[Fact]
        //public void MyTurn_Should_Able_Kong_Board_Active()
        //{
        //    var context = _f.TestDataContext;

        //    TileType circ = TileType.Circle;
        //    TileValue one = TileValue.One;

        //    KongTilesHelper.SetupForBoard(context, _f.MainPlayerUserName, circ, one);

        //    _f.OtherPlayerRound.IsMyTurn = false;
        //    _f.MainPlayerRound.IsMyTurn = true;

        //    var kongCommand = new Kong.Command
        //    {
        //        RoundId = _f.RoundId,
        //        UserName = _f.MainPlayerUserName,
        //        TileType = circ,
        //        TileValue = one
        //    };

        //    var sut = new Kong.Handler(context, _f.RoundMapper, _f.PointCalculator);

        //    var result = sut.Handle(kongCommand, CancellationToken.None).Result;

        //    Assert.True(result.UpdatedRoundPlayers.First(u => u.UserName == _f.MainPlayerUserName).IsMyTurn);
        //}

        /// <summary>
        /// When it's player's turn, player should be able to kong from it's own active tile
        /// if player has 4 same tiles in their active tile
        /// </summary>
        //[Fact]
        //public void MyTurn_Should_Able_Kong_Self_Active()
        //{
        //    var context = _f.TestDataContext;

        //    TileType circ = TileType.Circle;
        //    TileValue one = TileValue.One;

        //    KongTilesHelper.SetupForSelfActive(context, _f.MainPlayerUserName, circ, one);

        //    _f.OtherPlayerRound.IsMyTurn = false;
        //    _f.MainPlayerRound.IsMyTurn = true;

        //    var kongCommand = new Kong.Command
        //    {
        //        RoundId = _f.RoundId,
        //        UserName = _f.MainPlayerUserName,
        //        TileType = circ,
        //        TileValue = one
        //    };

        //    var sut = new Kong.Handler(context, _f.RoundMapper, _f.PointCalculator);

        //    var result = sut.Handle(kongCommand, CancellationToken.None).Result;

        //    Assert.True(result.UpdatedRoundPlayers.First(u => u.UserName == _f.MainPlayerUserName).IsMyTurn);
        //}

        /// <summary>
        /// when its player's turn, user can kong active tile if theres ponged tile with matching one
        /// </summary>
        //[Fact]
        //public void MyTurn_Should_Able_Kong_Self_Pong()
        //{
        //    var context = _f.TestDataContext;

        //    TileType circ = TileType.Circle;
        //    TileValue one = TileValue.One;

        //    KongTilesHelper.SetupForPongUser(context, _f.MainPlayerUserName, circ, one);

        //    _f.OtherPlayerRound.IsMyTurn = false;
        //    _f.MainPlayerRound.IsMyTurn = true;

        //    var kongCommand = new Kong.Command
        //    {
        //        RoundId = _f.RoundId,
        //        UserName = _f.MainPlayerUserName,
        //        TileType = circ,
        //        TileValue = one
        //    };

        //    var sut = new Kong.Handler(context, _f.RoundMapper, _f.PointCalculator);

        //    var result = sut.Handle(kongCommand, CancellationToken.None).Result;

        //    Assert.True(result.UpdatedRoundPlayers.First(u => u.UserName == _f.MainPlayerUserName).IsMyTurn);
        //}

        /// <summary>
        /// when its player's turn, user can't kong from board active even if theres ponged tile with matching one
        /// </summary>
        [Fact]
        public async void MyTurn_Should_Not_Able_Kong_Board_Pong()
        {
            var context = _f.TestDataContext;

            TileType circ = TileType.Circle;
            TileValue one = TileValue.One;

            KongTilesHelper.SetupForPongBoard(context, _f.MainPlayerUserName, circ, one);

            _f.OtherPlayerRound.IsMyTurn = false;
            _f.MainPlayerRound.IsMyTurn = true;

            var kongCommand = new Kong.Command
            {
                RoundId = _f.RoundId,
                UserName = _f.MainPlayerUserName,
                TileType = circ,
                TileValue = one
            };

            var sut = new Kong.Handler(context, _f.RoundMapper, _f.PointCalculator);

            var ex = await Assert.ThrowsAsync<RestException>(() => sut.Handle(kongCommand, CancellationToken.None));

            Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
        }
    }
}
