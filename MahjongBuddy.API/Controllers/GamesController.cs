using System.Collections.Generic;
using System.Threading.Tasks;
using MahjongBuddy.API.SignalR;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Games;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MahjongBuddy.API.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IHubContext<GameHub> _hubContext;

        public GamesController(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> Details(int id)
        {
            return await Mediator.Send(new Detail.Query { Id = id });
        }

        [HttpGet("{id}/latestRound")]
        public async Task<ActionResult<RoundDto>> LatestRound(int id)
        {
            return await Mediator.Send(new LatestRound.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "IsGameHost")]
        public async Task<ActionResult<Unit>> Edit(int id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsGameHost")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var cancelGame = await Mediator.Send(new Delete.Command { Id = id });
            await _hubContext.Clients.Group(id.ToString()).SendAsync("GameCancelled", id.ToString());
            return cancelGame;
        }

        [HttpPost("{id}/end")]
        [Authorize(Policy = "IsGameHost")]
        public async Task<ActionResult<GameDto>> End(int id)
        {
            var game = await Mediator.Send(new End.Command { GameId = id });
            await _hubContext.Clients.Group(game.Id.ToString()).SendAsync("GameEnded", game);
            return game;
        }
    }
}
