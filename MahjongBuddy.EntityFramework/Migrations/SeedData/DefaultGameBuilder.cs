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
                    Code = "RAWRR",
                    Date = DateTime.Now,
                    HostId = "a",
                    MinPoint = 3,
                    MaxPoint = 10,
                    GamePlayers = new List<GamePlayer>
                    {
                        //Tonny
                        new GamePlayer
                        {
                            IsHost = true,
                            PlayerId = "a",
                            InitialSeatWind = WindDirection.East
                        },
                        //Mei
                        new GamePlayer
                        {
                            IsHost = false,
                            PlayerId = "b",
                            InitialSeatWind = WindDirection.South
                        },
                        //Peter
                        new GamePlayer
                        {
                            IsHost = false,
                            PlayerId = "c",
                            InitialSeatWind = WindDirection.West
                        },
                        //Jason
                        new GamePlayer
                        {
                            IsHost = false,
                            PlayerId = "d",
                            InitialSeatWind = WindDirection.North
                        }
                    }
                }
            };

            return games;
        }
    }
}
