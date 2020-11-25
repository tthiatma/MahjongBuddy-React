using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Hub
{
    public class OnConnected
    {
        public class Query: IRequest
        {
            public string ConnectionId { get; set; }
            public string UserName { get; set; }
            public string UserAgent { get; set; }
            public string GameId { get; set; }
        }

        public class Handler : IRequestHandler<Query>
        {
            private readonly MahjongBuddyDbContext _context;

            public Handler(MahjongBuddyDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var connection = await _context.Connections.SingleOrDefaultAsync(x => x.Id == request.ConnectionId);

                if (connection != null)
                {
                    if (!connection.Connected)
                    {
                        connection.Connected = true;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    //existing player reconnecting
                    var currentGamePlayer = await _context.GamePlayers.SingleOrDefaultAsync(gp => gp.AppUser.UserName == request.UserName && gp.GameId == int.Parse(request.GameId));
                    if(currentGamePlayer != null)
                    {
                        foreach (var uc in currentGamePlayer.Connections)
                        {
                            _context.Connections.Remove(uc);
                        }

                        Connection newCon = new Connection
                        {
                            Id = request.ConnectionId,
                            GamePlayerId = currentGamePlayer.Id,
                            Connected = true,
                            UserAgent = request.UserAgent
                        };
                        _context.Connections.Add(newCon);
                        _context.SaveChanges();
                    }
                }

                return Unit.Value;
            }
        }
    }
}
