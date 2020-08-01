using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Application.PlayerAction;
using MahjongBuddy.Core;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace MahjongBuddy.Application.Tests.PlayerActions
{
    public class ThrowTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;

        public ThrowTest(GameFixture f)
        {
            _f = f;
        }
        public void Dispose()
        {
            _f.TestDataContext.RemoveRange(_f.TestDataContext.Rounds);
            _f.TestDataContext.RemoveRange(_f.TestDataContext.RoundPlayers);
            _f.TestDataContext.SaveChanges();
            _f.CreateRound();
            _f.TestDataContext.SaveChanges();
            _f.RoundId = _f.TestDataContext.Rounds.First().Id;
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.AppUser.UserName == _f.MainPlayerUserName);
            _f.MainPlayerRound.RoundPlayerActions.Clear();
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.AppUser.UserName == _f.OtherPlayerName);
            _f.OtherPlayerRound.RoundPlayerActions.Clear();
        }

        /// <summary>
        /// When other player throw a tile, player that has 2 same tile should get pong action
        /// </summary>
        [Fact]
        public void Player_Should_Get_PongAction()
        {
            var context = _f.TestDataContext;
            var pongTile = ThrowTileHelper.SetupForPong(context, _f.MainPlayerUserName);
            var round = _f.TestDataContext.Rounds.First();

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            //for simplicity remove 2 other players
            var westPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "west");
            var northPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "north");

            round.RoundPlayers.Remove(westPlayer);
            round.RoundPlayers.Remove(northPlayer);

            var throwCommand = new Throw.Command
            {
                RoundId = round.Id,
                GameId = round.Game.Id,
                UserName = _f.OtherPlayerName,
                TileId = pongTile.Id
            };

            var sut = new Throw.Handler(context, _f.RoundMapper, _f.PointCalculator);

            var result = sut.Handle(throwCommand, CancellationToken.None).Result;

            var playerThatHasAction = result.UpdatedRoundPlayers.Where(p => p.RoundPlayerActions.Count() > 0).First();

            Assert.Equal(2, result.UpdatedRoundPlayers.Count());

            Assert.Single(playerThatHasAction.RoundPlayerActions);

            Assert.Equal(ActionType.Pong, playerThatHasAction.RoundPlayerActions.First().PlayerAction);
        }

        /// <summary>
        /// When other player throw a tile, player that has 3 same tile should get kong action
        /// </summary>
        [Fact]
        public void Player_Should_Get_KongAction()
        {
            var context = _f.TestDataContext;
            var kongTile = ThrowTileHelper.SetupForKong(context, _f.MainPlayerUserName);
            var round = _f.TestDataContext.Rounds.First();

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            //for simplicity remove 2 other players
            var westPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "west");
            var northPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "north");

            round.RoundPlayers.Remove(westPlayer);
            round.RoundPlayers.Remove(northPlayer);

            var throwCommand = new Throw.Command
            {
                RoundId = round.Id,
                GameId = round.Game.Id,
                UserName = _f.OtherPlayerName,
                TileId = kongTile.Id
            };

            var sut = new Throw.Handler(context, _f.RoundMapper, _f.PointCalculator);

            var result = sut.Handle(throwCommand, CancellationToken.None).Result;

            var playerThatHasAction = result.UpdatedRoundPlayers.Where(p => p.RoundPlayerActions.Count() > 0).First();

            Assert.Equal(2, result.UpdatedRoundPlayers.Count());

            Assert.Single(playerThatHasAction.RoundPlayerActions);

            Assert.Equal(ActionType.Kong, playerThatHasAction.RoundPlayerActions.First().PlayerAction);
        }

        /// <summary>
        /// When other player throw a tile, player that can win should get win action
        /// </summary>
        [Fact]
        public void Player_Should_Get_WinAction()
        {
            var context = _f.TestDataContext;
            var winTile = ThrowTileHelper.SetupForWin(context, _f.MainPlayerUserName);
            var round = _f.TestDataContext.Rounds.First();

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            //for simplicity remove 2 other players
            var westPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "west");
            var northPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "north");

            round.RoundPlayers.Remove(westPlayer);
            round.RoundPlayers.Remove(northPlayer);

            var throwCommand = new Throw.Command
            {
                RoundId = round.Id,
                GameId = round.Game.Id,
                UserName = _f.OtherPlayerName,
                TileId = winTile.Id
            };

            var sut = new Throw.Handler(context, _f.RoundMapper, _f.PointCalculator);

            var result = sut.Handle(throwCommand, CancellationToken.None).Result;

            var playerThatHasAction = result.UpdatedRoundPlayers.Where(p => p.RoundPlayerActions.Count() > 0).First();

            Assert.Equal(2, result.UpdatedRoundPlayers.Count());

            Assert.Single(playerThatHasAction.RoundPlayerActions);

            Assert.Equal(ActionType.Win, playerThatHasAction.RoundPlayerActions.First().PlayerAction);
        }

        /// <summary>
        /// When other player throw a tile, player that can chow should get chow action
        /// </summary>
        [Fact]
        public void Player_Should_Get_ChowAction()
        {
            var context = _f.TestDataContext;
            var chowTile = ThrowTileHelper.SetupForChow(context, _f.MainPlayerUserName);
            var round = _f.TestDataContext.Rounds.First();

            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            //for simplicity remove 2 other players
            var westPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "west");
            var northPlayer = round.RoundPlayers.First(rp => rp.AppUser.UserName == "north");

            round.RoundPlayers.Remove(westPlayer);
            round.RoundPlayers.Remove(northPlayer);

            var throwCommand = new Throw.Command
            {
                RoundId = round.Id,
                GameId = round.Game.Id,
                UserName = _f.OtherPlayerName,
                TileId = chowTile.Id
            };

            var sut = new Throw.Handler(context, _f.RoundMapper, _f.PointCalculator);

            var result = sut.Handle(throwCommand, CancellationToken.None).Result;

            var playerThatHasAction = result.UpdatedRoundPlayers.Where(p => p.RoundPlayerActions.Count() > 0).First();

            Assert.Equal(2, result.UpdatedRoundPlayers.Count());

            Assert.Single(playerThatHasAction.RoundPlayerActions);

            Assert.Equal(ActionType.Chow, playerThatHasAction.RoundPlayerActions.First().PlayerAction);
        }
    }
}
