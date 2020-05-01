using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Application.Helpers;
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
        public UserRound MainPlayerRound { get; set; }
        public UserRound OtherPlayerRound { get; set; }
        public int RoundId { get; set; }

        public GameFixture()
        {
            MainPlayerUserName = "mainplayer";
            OtherPlayerName = "south";
            GameId = 1;
            RoundId = 1;

            CreateBaseData();

            MainPlayerRound = TestDataContext.Rounds.First().UserRounds.First(u => u.AppUser.UserName == MainPlayerUserName);
            OtherPlayerRound = TestDataContext.Rounds.First().UserRounds.First(u => u.AppUser.UserName == OtherPlayerName);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        private void CreateUser()
        {
            TestDataContext.Users.AddRange(new List<AppUser>
            {
                new AppUser
                {
                    Id = "a",
                    Email = "mainplayer@gmail.com",
                    UserName = MainPlayerUserName
                },
                new AppUser
                {
                    Id = "b",
                    Email = "south@gmail.com",
                    UserName = OtherPlayerName
                },
                new AppUser
                {
                    Id = "c",
                    Email = "west@gmail.com",
                    UserName = "west"
                },
                new AppUser
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
                UserGames = new List<UserGame>
                    {
                        new UserGame
                        {
                            IsHost = true,
                            AppUserId = "a",
                            InitialSeatWind = WindDirection.East
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "b",
                            InitialSeatWind = WindDirection.South
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "c",
                            InitialSeatWind = WindDirection.West
                        },
                        new UserGame
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
                GameId = RoundId,
                DateCreated = DateTime.Now,
                RoundTiles = RoundTileHelper.CreateTiles(TestDataContext).Shuffle(),
                UserRounds = new List<UserRound>
                {
                    new UserRound
                    {
                        AppUserId = "a"
                    },
                    new UserRound
                    {
                        AppUserId = "b"
                    },
                    new UserRound
                    {
                        AppUserId = "c"
                    },
                    new UserRound
                    {
                        AppUserId = "d"
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
