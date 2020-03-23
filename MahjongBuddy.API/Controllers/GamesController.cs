using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MahjongBuddy.Application.Games;
using MahjongBuddy.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MahjongBuddy.API.Controllers
{
    public class GamesController : BaseController
    {
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Game>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> Details(Guid id)
        {
            return await Mediator.Send(new Detail.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}
