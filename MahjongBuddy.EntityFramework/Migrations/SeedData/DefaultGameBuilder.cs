using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.EntityFramework.Migrations.SeedData
{
    public class DefaultGameBuilder
    {
        private readonly MahjongBuddyDbContext _context;

        public DefaultGameBuilder(MahjongBuddyDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            if (!_context.Games.Any())
            {
                var games = CreateGames();
                _context.Games.AddRange(games);
                _context.SaveChanges();
            }
        }
        private IEnumerable<Game> CreateGames()
        {
            Round firstRound = new Round
            {
                Id = 1,
                Index = 1,
                Wind = WindDirection.East
            };
            List<Game> games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Title = "Game 1",
                    Date = DateTime.Now.AddMonths(-2),
                    UserGames = new List<UserGame>
                    {
                        new UserGame
                        {
                            IsHost = true,
                            AppUserId = "a",
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "b",
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "c",
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "d",
                        }
                    }
                },
                new Game
                {
                    Id = 2,
                    Title = "Game 2",
                    Date = DateTime.Now.AddMonths(-1),
                    UserGames = new List<UserGame>
                    {
                        new UserGame
                        {
                            IsHost = true,
                            AppUserId = "a",
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "b",
                        },
                        new UserGame
                        {
                            IsHost = false,
                            AppUserId = "c",
                        }
                    }
                }
            };

            return games;
        }

    }
}
