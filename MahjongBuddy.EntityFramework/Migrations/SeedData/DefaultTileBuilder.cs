using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.EntityFramework.Migrations.SeedData
{
    public class DefaultTileBuilder
    {
        private readonly MahjongBuddyDbContext _context;
        public DefaultTileBuilder(MahjongBuddyDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            if (!_context.Tiles.Any())
            {
                var tiles = CreateTiles();

                _context.Tiles.AddRange(tiles);
                _context.SaveChanges();
            }
        }
        private IEnumerable<Tile> CreateTiles()
        {
            List<Tile> _tiles = new List<Tile>();
            _tiles.Add(new Money1());
            _tiles.Add(new Money2());
            _tiles.Add(new Money3());
            _tiles.Add(new Money4());
            _tiles.Add(new Money5());
            _tiles.Add(new Money6());
            _tiles.Add(new Money7());
            _tiles.Add(new Money8());
            _tiles.Add(new Money9());

            _tiles.Add(new Round1());
            _tiles.Add(new Round2());
            _tiles.Add(new Round3());
            _tiles.Add(new Round4());
            _tiles.Add(new Round5());
            _tiles.Add(new Round6());
            _tiles.Add(new Round7());
            _tiles.Add(new Round8());
            _tiles.Add(new Round9());

            _tiles.Add(new Stick1());
            _tiles.Add(new Stick2());
            _tiles.Add(new Stick3());
            _tiles.Add(new Stick4());
            _tiles.Add(new Stick5());
            _tiles.Add(new Stick6());
            _tiles.Add(new Stick7());
            _tiles.Add(new Stick8());
            _tiles.Add(new Stick9());

            _tiles.Add(new DragonGreen());
            _tiles.Add(new DragonRed());
            _tiles.Add(new DragonWhite());

            _tiles.Add(new WindNorth());
            _tiles.Add(new WindEast());
            _tiles.Add(new WindSouth());
            _tiles.Add(new WindWest());

            _tiles.Add(new FlowerNumeric1());
            _tiles.Add(new FlowerNumeric2());
            _tiles.Add(new FlowerNumeric3());
            _tiles.Add(new FlowerNumeric4());

            _tiles.Add(new FlowerRoman1());
            _tiles.Add(new FlowerRoman2());
            _tiles.Add(new FlowerRoman3());
            _tiles.Add(new FlowerRoman4());

            return _tiles;
        }
    }
}
