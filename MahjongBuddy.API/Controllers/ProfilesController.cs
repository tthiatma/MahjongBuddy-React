using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MahjongBuddy.Application.Profiles;
using MediatR;

namespace MahjongBuddy.API.Controllers
{
    public class ProfilesController: BaseController
    {
        [HttpGet("{userName}")]
        public async Task<ActionResult<Application.Profiles.Profile>> Get(string userName)
        {
            return await Mediator.Send(new Details.Query { UserName = userName });
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
