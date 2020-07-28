using System.Collections.Generic;
using System.Threading.Tasks;
using MahjongBuddy.Application.PlayerAction;
using MahjongBuddy.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MahjongBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tile>>> List()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}