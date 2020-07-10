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
            List<Game> games = new List<Game>
            {
                new Game
                {
                    Title = "Game 1",
                    Date = DateTime.Now,
                    HostId = "a",
                    MinPoint = 3,
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
                }
            };

            return games;
        }
    }
}
