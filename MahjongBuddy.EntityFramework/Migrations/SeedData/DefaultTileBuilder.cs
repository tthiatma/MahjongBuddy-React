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

            _tiles.Add(new Circle1()); //11
            _tiles.Add(new Circle2()); //12
            _tiles.Add(new Circle3()); //13
            _tiles.Add(new Circle4()); //14
            _tiles.Add(new Circle5()); //15
            _tiles.Add(new Circle6()); //16
            _tiles.Add(new Circle7()); //17
            _tiles.Add(new Circle8()); //18
            _tiles.Add(new Circle9()); //19

            _tiles.Add(new Stick1()); //21
            _tiles.Add(new Stick2()); //22
            _tiles.Add(new Stick3()); //23
            _tiles.Add(new Stick4()); //24
            _tiles.Add(new Stick5()); //25
            _tiles.Add(new Stick6()); //26
            _tiles.Add(new Stick7()); //27
            _tiles.Add(new Stick8()); //28
            _tiles.Add(new Stick9()); //29

            _tiles.Add(new DragonGreen()); //31
            _tiles.Add(new DragonRed());   //32
            _tiles.Add(new DragonWhite()); //33

            _tiles.Add(new WindNorth()); //41
            _tiles.Add(new WindEast());  //42
            _tiles.Add(new WindSouth()); //43
            _tiles.Add(new WindWest());  //44

            _tiles.Add(new FlowerNumeric1()); //51
            _tiles.Add(new FlowerNumeric2()); //52
            _tiles.Add(new FlowerNumeric3()); //53
            _tiles.Add(new FlowerNumeric4()); //54

            _tiles.Add(new FlowerRoman1()); //61
            _tiles.Add(new FlowerRoman2()); //62
            _tiles.Add(new FlowerRoman3()); //63
            _tiles.Add(new FlowerRoman4()); //64

            return _tiles;
        }
    }
}
