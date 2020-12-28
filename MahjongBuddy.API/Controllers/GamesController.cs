using System;
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
        public async Task<ActionResult<List.GamesEnvelope>> List(int? limit,
            int? offset, bool isInGame, bool isHost, DateTime? startDate)
        {
            return await Mediator.Send(new List.Query(limit,
                offset, isInGame, isHost, startDate));
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<GameDto>> DetailsByCode(string code)
        {
            return await Mediator.Send(new DetailByCode.Query { GameCode = code });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> Details(int id)
        {
            return await Mediator.Send(new Detail.Query { Id = id });
        }

        [HttpGet("code/{code}/latestRound")]
        public async Task<ActionResult<RoundDto>> LatestRound(string code)
        {
            return await Mediator.Send(new LatestRound.Query { GameCode = code });
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
        public async Task<ActionResult<Unit>> Delete(string gameCode)
        {
            var cancelGame = await Mediator.Send(new Delete.Command { GameCode = gameCode });
            await _hubContext.Clients.Group(gameCode).SendAsync("GameCancelled", gameCode);
            return cancelGame;
        }

        [HttpPost("{id}/end")]
        [Authorize(Policy = "IsGameHost")]
        public async Task<ActionResult<GameDto>> End(string gameCode)
        {
            var game = await Mediator.Send(new End.Command { GameCode = gameCode });
            await _hubContext.Clients.Group(game.Id.ToString()).SendAsync("GameEnded", game);
            return game;
        }
    }
}
