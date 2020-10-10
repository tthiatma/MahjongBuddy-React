using MahjongBuddy.Application.ChatMsgs;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Games;
using MahjongBuddy.Application.Rounds;
using MahjongBuddy.Application.PlayerAction;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MahjongBuddy.Application.Hub;

namespace MahjongBuddy.API.SignalR
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IMediator _mediator;
        public GameHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userAgent = Context.GetHttpContext().Request.Headers["User-Agent"];

            await _mediator.Send(new OnConnected.Query { ConnectionId = connectionId, UserAgent = userAgent });
            await base.OnConnectedAsync();
        }

        public async Task StartRound(Application.Rounds.Create.Command command)
        {
            try
            {
                var newRound = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("RoundStarted", newRound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task EndingRound(Ending.Command command)
        {
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task TiedRound(Tied.Command command)
        {
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task JoinGame(Join.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("PlayerConnected", player);
        }

        public async Task LeaveGame(Leave.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("PlayerDisconnected", player);
        }

        public async Task StandUpGame(StandUp.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("PlayerStoodUp", player);
        }

        public async Task SitGame(Sit.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("PlayerSat", player);
        }

        public async Task RandomizeWind(RandomizeWind.Command command)
        {
            command.UserName = GetUserName();

            var players = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("UpdatePlayersWind", players);
        }

        public async Task SendChatMsg(CreateChatMsg.Command command)
        {
            string userName = GetUserName();

            command.UserName = userName;

            var chatMsg = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("ReceiveChatMsg", chatMsg);
        }

        public async Task AddToGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task ThrowAllTiles(ThrowAll.Command command)
        {
            command.UserName = GetUserName();
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
        }

        public async Task ThrowTile(Throw.Command command)
        {
            command.UserName = GetUserName();
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task SortTiles(SortTiles.Command command)
        {
            command.UserName = GetUserName();
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
        }

        public async Task PickTile(Pick.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
        }

        public async Task PongTile(Pong.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            try
            {
                var round = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
            }
            catch (RestException ex)
            {
                throw new HubException("Can't Pong", ex);
            }
        }

        public async Task KongTile(Kong.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            try
            {
                var round = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
            }
            catch (RestException ex)
            {
                throw new HubException(ex.Message);
            }
        }

        public async Task ChowTile(Chow.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;

            try
            {
                var round = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
            }
            catch (RestException ex)
            {
                throw new HubException(ex.Message);
            }
        }

        public async Task SkipAction(SkipAction.Command command)
        {
            command.UserName = GetUserName();
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
        }

        public async Task WinRound(Win.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            try
            {
                var round = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRoundNoLag", round);
            }
            catch (RestException ex)
            {
                throw new HubException("Can't Win", ex);
            }
        }

        private string GetUserName()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
