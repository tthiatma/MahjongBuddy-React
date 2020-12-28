using System.Threading.Tasks;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Rounds;
using Microsoft.AspNetCore.Mvc;

namespace MahjongBuddy.API.Controllers
{
    public class RoundsController : BaseController
    {
        [HttpPost("details")]
        public async Task<ActionResult<RoundDto>> Details(RoundReq req)
        {
            return await Mediator.Send(new Detail.Query { Id = req.Id, GameCode = req.GameCode, UserName = req.UserName });
        }
    }

    public class RoundReq
    {
        public string Id { get; set; }
        public string GameCode { get; set; }
        public string UserName { get; set; }
    }
}