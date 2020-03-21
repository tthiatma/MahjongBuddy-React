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
            }
        }
        private IEnumerable<Game> CreateGames()
        {
            List<Game> games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Title = "Game 1",
                    Date = DateTime.Now.AddMonths(-2),
                },
                new Game
                {
                    Id = 2,
                    Title = "Game 2",
                    Date = DateTime.Now.AddMonths(-1),
                }
            };

            return games;
        }

    }
}
