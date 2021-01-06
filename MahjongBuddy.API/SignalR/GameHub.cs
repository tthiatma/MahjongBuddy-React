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
            var gameCode = requestContext.Query["gcode"].ToString().ToUpper();
            var connectionId = Context.ConnectionId;
            var userAgent = requestContext.Headers["User-Agent"];
            var player = await _mediator.Send(new Join.Command { ConnectionId = connectionId, UserAgent = userAgent, UserName = userName, GameCode = gameCode });
            await base.OnConnectedAsync();
            await Groups.AddToGroupAsync(connectionId, gameCode);
            await Clients.Group(gameCode).SendAsync("PlayerConnected", player);
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

        public async Task EndGame(string gameCode)
        {
            var game = await _mediator.Send(new End.Command { GameCode = gameCode.ToUpper() });
            await Clients.Group(gameCode.ToUpper()).SendAsync("GameEnded", game);
        }

        public async Task CancelGame(string gameCode)
        {
            var userName = GetUserName();
            await _mediator.Send(new Delete.Command
            {
                GameCode = gameCode.ToUpper(),
                UserName = userName
            });
            await Clients.Group(gameCode.ToUpper()).SendAsync("GameCancelled", gameCode.ToUpper());
        }

        public async Task JoinGame(string gameCode)
        {
            var player = await _mediator.Send(new Join.Command
            {
                GameCode = gameCode.ToUpper(),
                UserName = GetUserName(),
                ConnectionId = Context.ConnectionId,
                UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"]
            });
            await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
            await Clients.Group(gameCode.ToUpper()).SendAsync("PlayerConnected", player);
        }

        public async Task LeaveGame(Leave.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameCode.ToUpper()).SendAsync("PlayerDisconnected", player);
        }

        public async Task StandUpGame(StandUp.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameCode.ToUpper()).SendAsync("PlayerStoodUp", player);
        }

        public async Task SitGame(Sit.Command command)
        {
            command.UserName = GetUserName();

            var player = await _mediator.Send(command);

            await Clients.Group(command.GameCode.ToUpper()).SendAsync("PlayerSat", player);
        }

        public async Task RandomizeWind(RandomizeWind.Command command)
        {
            command.UserName = GetUserName();

            var players = await _mediator.Send(command);

            await Clients.Group(command.GameCode.ToUpper()).SendAsync("UpdatePlayersWind", players);
        }

        public async Task SendChatMsg(CreateChatMsg.Command command)
        {
            string userName = GetUserName();

            command.UserName = userName;

            var chatMsg = await _mediator.Send(command);

            await Clients.Group(command.GameCode.ToUpper()).SendAsync("ReceiveChatMsg", chatMsg);
        }

        public async Task RemoveFromGroup(string gameCode)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameCode.ToUpper());
        }

        //for debugging
        //public async Task ThrowAllTiles(ThrowAll.Command command)
        //{
        //    command.UserName = GetUserName();
        //    var updates = await _mediator.Send(command);
        //    await SendClientRoundUpdates(updates, "UpdateRoundNoLag");
        //}

        public async Task ThrowTile(Throw.Command command)
        {
            command.UserName = GetUserName();
            var updates = await _mediator.Send(command);
            foreach (var u in updates)
            {
                //check if there's action
                //we do delay for self win and selfkong to avoid confusion
                var hasAction = u.MainPlayer.RoundPlayerActiveActions.Where(a => a.ActionType != Core.ActionType.SelfWin && a.ActionType != Core.ActionType.SelfKong).Count() > 0;
                var roundUpdateMethod = hasAction ? "UpdateRoundNoLag" : "UpdateRound";

                if (u.MainPlayer.Connections != null)
                {
                    foreach (var c in u.MainPlayer.Connections)
                    {
                        await Clients.Client(c.Id).SendAsync(roundUpdateMethod, u);
                    }
                }
            }
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
