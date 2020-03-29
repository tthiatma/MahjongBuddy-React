using MahjongBuddy.Application.ChatMsgs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MahjongBuddy.API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendChatMsg(Create.Command command)
        {
            string userName = GetUserName();

            command.UserName = userName;

            var chatMsg = await _mediator.Send(command);

            await Clients.Group(command.GameId.ToString()).SendAsync("ReceiveChatMsg", chatMsg);
        }

        private string GetUserName()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task AddToGroup(int groupId)
        {
            var groupName = groupId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToString());
            var userName = GetUserName();
            await Clients.OthersInGroup(groupName).SendAsync("Send", $"{userName} has joined the group");
        }

        public async Task RemoveFromGroup(int groupId)
        {
            var groupName = groupId.ToString();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            var userName = GetUserName();
            await Clients.OthersInGroup(groupName).SendAsync("Send", $"{userName} has left the group");
        }
    }
}
