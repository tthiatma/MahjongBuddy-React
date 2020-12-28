using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.EntityFramework.EntityFramework;
using System;
using System.Linq;
using System.Text;

namespace MahjongBuddy.Infrastructure.Randomizer
{
    public class GameCodeGenerator : IGameCodeGenerator
    {
        private readonly MahjongBuddyDbContext _context;

        public GameCodeGenerator(MahjongBuddyDbContext context)
        {
            _context = context;

        }
        public string CreateCode()
        {
            //make sure the game code unique
            var gameCode = GenerateRandomCode(5);

            var codeUsed = _context.Games.Any(g => g.Code == gameCode);
           
            while(codeUsed == true)
            {
                gameCode = GenerateRandomCode(5);
                codeUsed = _context.Games.Any(g => g.Code == gameCode);
            }
            return gameCode;
        }

        private string GenerateRandomCode(int length)
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
