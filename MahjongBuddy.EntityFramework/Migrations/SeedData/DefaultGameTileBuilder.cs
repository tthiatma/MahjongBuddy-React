using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.EntityFramework.Migrations.SeedData
{
    public class DefaulrGameTileBuilder
    {
        private readonly MahjongBuddyDbContext _context;

        public DefaulrGameTileBuilder(MahjongBuddyDbContext context)
        {
            _context = context;
        }
        public void Build()
        {
            if (!_context.GameTiles.Any())
            {
                var tiles = CreateGames();
                _context.GameTiles.AddRange(tiles);
            }
        }
        private IEnumerable<GameTile> CreateGames()
        {
            List<GameTile> tiles = new List<GameTile>();
            var game = _context.Games.First(g => g.Id == 1);
            for (var i = 1; i < 5; i++)
            {
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 1) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 2) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 3) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 4) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 5) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 6) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 7) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 8) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 9) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 11) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 12) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 13) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 14) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 15) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 16) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 17) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 18) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 19) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 21) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 22) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 23) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 24) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 25) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 26) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 27) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 28) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 29) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 31) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 32) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 33) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 41) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 42) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 43) });
                tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 44) });
            };

            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 51) });
            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 52) });
            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 53) });
            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 54) });

            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 61) });
            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 62) });
            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t=> t.Id == 63) });
            tiles.Add(new GameTile { Game = game, Tile = _context.Tiles.First(t => t.Id == 64) });

            return tiles;
        }
    }
}
