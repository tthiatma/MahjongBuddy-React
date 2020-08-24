using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MahjongBuddy.Application.Profiles;

namespace MahjongBuddy.API.Controllers
{
    public class ProfileController: BaseController
    {
        [HttpGet("{userName}")]
        public async Task<ActionResult<Application.Profiles.Profile>> Get(string userName)
        {
            return await Mediator.Send(new Details.Query { UserName = userName });
        }
    }
}
