using System.Collections.Generic;
using System.Threading.Tasks;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Rounds;
using Microsoft.AspNetCore.Mvc;

namespace MahjongBuddy.API.Controllers
{
    public class RoundsController : BaseController
    {
        [HttpGet("{id}/roundTiles")]
        public async Task<ActionResult<List<RoundTileDto>>> RoundTile(int id)
        {
            return await Mediator.Send(new GetRoundTiles.Command { Id = id });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RoundDto>> Details(int id)
        {
            return await Mediator.Send(new Detail.Query { Id = id });
        }
    }
}
