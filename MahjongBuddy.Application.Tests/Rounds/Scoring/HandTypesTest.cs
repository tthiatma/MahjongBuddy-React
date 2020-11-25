using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Application.Tests.Fixtures;
using MahjongBuddy.Application.Tests.Helpers;
using MahjongBuddy.Core;
using System;
using System.Linq;
using Xunit;

namespace MahjongBuddy.Application.Tests.Rounds.Scoring
{
    public class HandTypesTest : TestBase, IClassFixture<GameFixture>, IDisposable
    {
        private readonly GameFixture _f;

        public HandTypesTest(GameFixture f)
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
            _f.MainPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.AppUser.UserName == _f.MainPlayerUserName);
            _f.OtherPlayerRound = _f.TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.AppUser.UserName == _f.OtherPlayerName);
        }

        [Fact]
        public void Detect_InvalidWin()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForInvalidWin(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Empty(result);
        }

        [Fact]
        public void Detect_SevenPairs()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForSevenPairs(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.SevenPairs, result);
        }

        [Fact]
        public void Detect_ThirteenOrphans()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForThirteenOrphans(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.ThirteenOrphans, result);
        }

        [Fact]
        public void Detect_AllHonors()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllHonors(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.AllHonors, result);
        }

        [Fact]
        public void Detect_No_AllHonors()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllTerminals(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.DoesNotContain(HandType.AllHonors, result);
        }

        [Fact]
        public void Detect_AllKongs()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllKongs(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.AllKongs, result);
        }

        [Fact]
        public void Detect_No_AllKongs()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllOneSuitTriplets(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.DoesNotContain(HandType.AllKongs, result);
        }

        [Fact]
        public void Detect_MixedAllTerminals()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForMixedAllTerminals(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.MixedTerminals, result);
        }

        [Fact]
        public void Detect_Not_MixedAllTerminals()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllTerminals(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.DoesNotContain(HandType.MixedTerminals, result);
        }

        [Fact]
        public void Detect_AllTerminals()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllTerminals(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.AllTerminals, result);
        }

        [Fact]
        public void Detect_Not_AllTerminals()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllOneSuitStraight(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.DoesNotContain(HandType.AllTerminals, result);
        }

        [Fact]
        public void Detect_Triplets()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForTriplets(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.Triplets, result);
        }

        [Fact]
        public void Detect_HiddenTreasure()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForHiddenTreasure(context, _f.MainPlayerUserName);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.HiddenTreasure, result);
        }

        [Fact]
        public void Detect_Not_HiddenTreasure()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForTriplets(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.DoesNotContain(HandType.HiddenTreasure, result);
        }

        [Fact]
        public void Detect_Straight()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForStraight(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.Straight, result);
        }

        [Fact]
        public void Detect_MixedOneSuit()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForMixedOneSuit(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.MixedOneSuit, result);
            Assert.Contains(HandType.Chicken, result);
        }

        [Fact]
        public void Detect_AllOneSuit()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllOneSuit(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.AllOneSuit, result);
            Assert.Contains(HandType.Chicken, result);
        }

        [Fact]
        public void Detect_AllOneSuit_Straight()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllOneSuitStraight(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Equal(2, result.Count());
            Assert.Contains(HandType.AllOneSuit, result);
            Assert.Contains(HandType.Straight, result);
        }

        [Fact]
        public void Detect_AllOneSuit_Triplets()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForAllOneSuitTriplets(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.AllOneSuit, result);
            Assert.Contains(HandType.Triplets, result);
        }

        [Fact]
        public void Detect_SmallDragon_Chicken()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForSmallDragon(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Equal(2, result.Count());
            Assert.Contains(HandType.SmallDragon, result);
            Assert.Contains(HandType.Chicken, result);
        }

        [Fact]
        public void Detect_BigDragon_Chicken()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForBigDragon(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Equal(2, result.Count());
            Assert.Contains(HandType.BigDragon, result);
            Assert.Contains(HandType.Chicken, result);
        }

        [Fact]
        public void Detect_BigFourWind_Chicken()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForBigFourWind(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Contains(HandType.BigFourWind, result);
            Assert.Contains(HandType.MixedOneSuit, result);
            Assert.Contains(HandType.Triplets, result);
        }

        [Fact]
        public void Detect_SmallFourWind_Chicken()
        {
            var context = _f.TestDataContext;

            WinTilesHelper.SetupForSmallFourWind(context, _f.MainPlayerUserName, selfPick: true);

            var round = _f.TestDataContext.Rounds.First();

            var result = new HandTypeBuilder().GetHandType(round, _f.MainPlayerUserName);

            Assert.Equal(3, result.Count());
            Assert.Contains(HandType.SmallFourWind, result);
            Assert.Contains(HandType.MixedOneSuit, result);
            Assert.Contains(HandType.Chicken, result);
        }
    }
}
