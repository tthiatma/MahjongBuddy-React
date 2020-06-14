using MahjongBuddy.Application.ChatMsgs;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Games;
using MahjongBuddy.Application.Rounds;
using MahjongBuddy.Application.Tiles;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MahjongBuddy.API.SignalR
{
    public class GameHub : Hub
    {
        private readonly IMediator _mediator;
        
        public GameHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task StartRound(Application.Rounds.Create.Command command)
        {
            try
            {
                var roundTiles = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("RoundStarted", roundTiles);
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

        public async Task DisconnectFromGame(Disconnect.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("PlayerDisconnected", player);
        }

        public async Task ConnectToGame(Connect.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("PlayerConnected", player);
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
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task ThrowTile(Throw.Command command)
        {
            command.UserName = GetUserName();
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task PickTile(Pick.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task PongTile(Pong.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            try
            {
                var round = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
            }
            catch (RestException ex)
            {
                var test = new HubException("Can't Pong", ex);
                throw test;
            }
        }

        public async Task KongTile(Kong.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        public async Task ChowTile(Chow.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;

            try
            {
                var round = await _mediator.Send(command);
                await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
            }
            catch (RestException ex)
            {
                throw new HubException(ex.Message);
            }
        }

        public async Task WinRound(Win.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            var round = await _mediator.Send(command);
            await Clients.Group(command.GameId.ToString()).SendAsync("UpdateRound", round);
        }

        private string GetUserName()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
