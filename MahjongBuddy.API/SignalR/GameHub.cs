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
using MahjongBuddy.Application.Dtos;
using System.Collections.Generic;

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
            var userName = GetUserName();
            var requestContext = Context.GetHttpContext().Request;
            var gameId = requestContext.Query["gid"].ToString();
            var connectionId = Context.ConnectionId;
            var userAgent = requestContext.Headers["User-Agent"];
            var player = await _mediator.Send(new Join.Command { ConnectionId = connectionId, UserAgent = userAgent, UserName = userName, GameId = int.Parse(gameId) });
            await base.OnConnectedAsync();
            await Groups.AddToGroupAsync(connectionId, gameId);
            await Clients.Group(gameId).SendAsync("PlayerConnected", player);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            await _mediator.Send(new OnDisConnected.Query { ConnectionId = connectionId });
            await base.OnDisconnectedAsync(exception);
        }

        public async Task StartRound(Application.Rounds.Create.Command command)
        {
            try
            {
                var updates = await _mediator.Send(command);
                await SendClientRoundUpdates(updates, "RoundStarted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DetailRound(Application.Rounds.Detail.Query query)
        {
            try
            {
                var roundDetail = await _mediator.Send(query);

                await Clients.Caller.SendAsync("LoadRound", roundDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task EndingRound(Ending.Command command)
        {
            var updates = await _mediator.Send(command);
            await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
        }

        public async Task TiedRound(Tied.Command command)
        {
            var updates = await _mediator.Send(command);
            await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
        }

        public async Task EndGame(string gameId)
        {
            var game = await _mediator.Send(new End.Command { GameId = int.Parse(gameId) });
            await Clients.Group(gameId).SendAsync("GameEnded", game);
        }

        public async Task CancelGame(string gameId)
        {
            var userName = GetUserName();

            await _mediator.Send(new Delete.Command
            {
                Id = int.Parse(gameId),
                UserName = userName
            });
            await Clients.Group(gameId).SendAsync("GameCancelled", gameId);
        }

        public async Task JoinGame(string gameId)
        {
            var player = await _mediator.Send(new Join.Command
            {
                GameId = int.Parse(gameId)
                ,
                UserName = GetUserName()
                ,
                ConnectionId = Context.ConnectionId,
                UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"]
            });
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId).SendAsync("PlayerConnected", player);
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

        public async Task RemoveFromGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task ThrowAllTiles(ThrowAll.Command command)
        {
            command.UserName = GetUserName();
            var updates = await _mediator.Send(command);
            await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
        }

        public async Task ThrowTile(Throw.Command command)
        {
            command.UserName = GetUserName();
            var updates = await _mediator.Send(command);
            //check if there's action

            var hasAction = updates.FirstOrDefault(r => r.MainPlayer.RoundPlayerActiveActions.Count() > 0);
            var roundUpdateMethod = hasAction == null ? "UpdateRound" : "UpdateRoundNoLag";
            await SendClientRoundUpdates(updates, roundUpdateMethod);
        }

        public async Task SortTiles(SortTiles.Command command)
        {
            command.UserName = GetUserName();
            var update = await _mediator.Send(command);
            foreach (var c in update.MainPlayer.Connections)
            {
                await Clients.Client(c.Id).SendAsync("UpdateRoundNoLag", update);
            }
        }

        public async Task PickTile(Pick.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            var updates = await _mediator.Send(command);
            await SendClientRoundUpdates(updates, "UpdateRoundNoLag");

        }

        public async Task PongTile(Pong.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            try
            {
                var updates = await _mediator.Send(command);
                await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
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
                var updates = await _mediator.Send(command);
                await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
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
                var updates = await _mediator.Send(command);
                await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
            }
            catch (RestException ex)
            {
                throw new HubException(ex.Message);
            }
        }

        public async Task SkipAction(SkipAction.Command command)
        {
            command.UserName = GetUserName();
            var updates = await _mediator.Send(command);
            await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
        }

        public async Task WinRound(Win.Command command)
        {
            string userName = GetUserName();
            command.UserName = userName;
            try
            {
                var updates = await _mediator.Send(command);
                await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
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

        private async Task SendClientRoundUpdates(IEnumerable<RoundDto> updates, string command)
        {
            foreach (var u in updates)
            {
                if (u.MainPlayer.Connections != null)
                {
                    foreach (var c in u.MainPlayer.Connections)
                    {
                        await Clients.Client(c.Id).SendAsync(command, u);
                    }
                }
            }
        }
    }
}
