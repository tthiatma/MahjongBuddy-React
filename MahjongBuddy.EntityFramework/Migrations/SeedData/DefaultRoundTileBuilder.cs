using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.EntityFramework.Migrations.SeedData
{
    public class DefaultRoundTileBuilder
    {
        private readonly MahjongBuddyDbContext _context;

        public DefaultRoundTileBuilder(MahjongBuddyDbContext context)
        {
            _context = context;
        }
        public void Build()
        {
            if (!_context.RoundTiles.Any())
            {
                var gameTiles = CreateGameTiless();
                _context.RoundTiles.AddRange(gameTiles);
                _context.SaveChanges();
            }
        }
        private IEnumerable<RoundTile> CreateGameTiless()
        {
            List<RoundTile> tiles = new List<RoundTile>();
            var game = _context.Games.First(g => g.Id == 1);
            
            for (var i = 1; i < 5; i++)
            {
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(1) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(2) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(3) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(4) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(5) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(6) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(7) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(8) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(9) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(11) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(12) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(13) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(14) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(15) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(16) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(17) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(18) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(19) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(21) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(22) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(23) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(24) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(25) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(26) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(27) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(28) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(29) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(31) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(32) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(33) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(41) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(42) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(43) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(44) });
            };

            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(51) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(52) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(53) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(54) });

            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(61) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(62) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(63) });
            tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(64) });

            return tiles;
        }
    }
}
