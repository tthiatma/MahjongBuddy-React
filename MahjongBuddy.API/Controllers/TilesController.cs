using System.Collections.Generic;
using System.Threading.Tasks;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TilesController : ControllerBase
    {
        private readonly MahjongBuddyDbContext _context;

        public TilesController(MahjongBuddyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tile>>> Get()
        {
            var tiles = await _context.Tiles.ToListAsync();
            return Ok(tiles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tile>> Get(int id)
        {
            var tile = await _context.Tiles.FindAsync(id);
            return Ok(tile);
        }
    }
}