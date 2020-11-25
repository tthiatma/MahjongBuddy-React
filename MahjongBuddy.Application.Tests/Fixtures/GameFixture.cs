using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.Rounds.Scorings;
using MahjongBuddy.Application.Rounds.Scorings.Builder;
using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Tests.Fixtures
{
    public class GameFixture: BaseFixture, IDisposable
    {
        public string MainPlayerUserName { get; set; }
        public string OtherPlayerName { get; set; }
        public int GameId { get; set; }
        public RoundPlayer MainPlayerRound { get; set; }
        public RoundPlayer OtherPlayerRound { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; }
        public IPointsCalculator PointCalculator { get; set; }

        public GameFixture()
        {
            MainPlayerUserName = "mainplayer";
            OtherPlayerName = "south";
            GameId = 1;
            RoundId = 1;

            CreateBaseData();
            PointCalculator = new HomeGameCalculator(new ExtraPointBuilder(), new HandTypeBuilder());
            MainPlayerRound = TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.AppUser.UserName == MainPlayerUserName);
            OtherPlayerRound = TestDataContext.Rounds.First().RoundPlayers.First(u => u.GamePlayer.AppUser.UserName == OtherPlayerName);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        private void CreateUser()
        {
            TestDataContext.Users.AddRange(new List<Player>
            {
                new Player
                {
                    Id = "a",
                    Email = "mainplayer@gmail.com",
                    UserName = MainPlayerUserName
                },
                new Player
                {
                    Id = "b",
                    Email = "south@gmail.com",
                    UserName = OtherPlayerName
                },
                new Player
                {
                    Id = "c",
                    Email = "west@gmail.com",
                    UserName = "west"
                },
                new Player
                {
                    Id = "d",
                    Email = "north@gmail.com",
                    UserName = "north"
                },
            });
        }

        private void CreateGame()
        {
            TestDataContext.Games.Add(new Game
            {
                Id = GameId,
                Title = "Game 1",
                Date = DateTime.Now.AddMonths(-2),
                HostId = "a",
                MinPoint = 3,
                MaxPoint = 10,
                GamePlayers = new List<GamePlayer>
                    {
                        new GamePlayer
                        {
                            IsHost = true,
                            AppUserId = "a",
                            InitialSeatWind = WindDirection.East
                        },
                        new GamePlayer
                        {
                            IsHost = false,
                            AppUserId = "b",
                            InitialSeatWind = WindDirection.South
                        },
                        new GamePlayer
                        {
                            IsHost = false,
                            AppUserId = "c",
                            InitialSeatWind = WindDirection.West
                        },
                        new GamePlayer
                        {
                            IsHost = false,
                            AppUserId = "d",
                            InitialSeatWind = WindDirection.North
                        }
                    }
            });
        }

        public void CreateRound()
        {
            TestDataContext.Rounds.Add(new Round
            {
                GameId = GameId,
                Wind = WindDirection.East,
                DateCreated = DateTime.Now,
                RoundTiles = RoundTileHelper.CreateTiles(TestDataContext).Shuffle(),
                RoundResults = new List<RoundResult>(),
                RoundPlayers = new List<RoundPlayer>
                {
                    new RoundPlayer
                    {
                        GamePlayerId = 1,
                        Wind = WindDirection.South
                    },
                    new RoundPlayer
                    {
                        GamePlayerId = 2,
                        Wind = WindDirection.East
                    },
                    new RoundPlayer
                    {
                        GamePlayerId = 3,
                        Wind = WindDirection.West
                    },
                    new RoundPlayer
                    {
                        GamePlayerId = 4,
                        Wind = WindDirection.North
                    }
                }
            });
        }

        private void CreateBaseData()
        {
            CreateUser();
            CreateGame();
            CreateRound();
            TestDataContext.SaveChanges();
        }
    }
}
