using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MahjongBuddy.EntityFramework.EntityFramework
{
    public class Seed
    {
        public static void SeedData(MahjongBuddyDbContext context)
        {
            if (!context.Games.Any())
            {
                var games = new List<Game>
                {
                    new Game
                    {
                        Title = "Game 1",
                        Date = DateTime.Now.AddMonths(-2),
                    },
                    new Game
                    {
                        Title = "Game 2",
                        Date = DateTime.Now.AddMonths(-1),
                    }
                };

                context.Games.AddRange(games);
                context.SaveChanges();

            }
        }
    }
}
