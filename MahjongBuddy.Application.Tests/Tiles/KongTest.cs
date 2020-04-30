using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Application.Tiles;
using MahjongBuddy.Core;
using Moq;
using System.Linq;
using System.Threading;
using Xunit;

namespace MahjongBuddy.Application.Tests.Tiles
{
    public class KongTest : TestBase, IClassFixture<GameFixture>
    {
        private readonly GameFixture _f;

        public KongTest(GameFixture f)
        {
            _f = f;
        }

        [Fact]
        public void NotMyTurn_Should_Able_Kong_Board_Active()
        {
            var context = _f.TestDataContext;

            TileType circ = TileType.Circle;
            TileValue one = TileValue.One;

            KongTilesHelper.KongFromActiveBoard(context, _f.MainPlayerUserName, circ, one);

            //set mainplayer its not his turn
            _f.OtherPlayerRound.IsMyTurn = true;
            _f.MainPlayerRound.IsMyTurn = false;

            var kongCommand = new Kong.Command
            {
                RoundId = _f.RoundId,
                UserName = _f.MainPlayerUserName,
                TileType = circ,
                TileValue = one
            };

            var sut = new Kong.Handler(context, _f.RoundMapper);

            var result = sut.Handle(kongCommand, CancellationToken.None).Result;

            Assert.True(result.UpdatedRoundPlayers.First(u => u.UserName == _f.MainPlayerUserName).IsMyTurn);

        }
    }
}
