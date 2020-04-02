using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MahjongBuddy.Application.Games;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MahjongBuddy.API.Controllers
{
    public class GamesController : BaseController
    {
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
            return await Mediator.Send(new Delete.Command { Id = id });
        }

        [HttpPost("{id}/connect")]
        public async Task<ActionResult<Unit>> Connect(int id)
        {
            return await Mediator.Send(new Connect.Command { Id = id });
        }

        [HttpDelete("{id}/connect")]
        public async Task<ActionResult<Unit>> Disconnect(int id)
        {
            return await Mediator.Send(new Disconnect.Command { Id = id });
        }

        // [HttpPost("{id}/round")]
        // public async Task<ActionResult> Round()
        // {
        //     return await Mediator.Send(new Round.Command);
        // }
    }
}
