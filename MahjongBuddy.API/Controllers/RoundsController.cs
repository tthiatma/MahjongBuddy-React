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
            return await Mediator.Send(new Detail.Query {Id = int.Parse(req.Id), GameId = int.Parse(req.GameId) });
        }
    }

    public class RoundReq
    {
        public string Id { get; set; }
        public string GameId { get; set; }
    }
}
