using AutoMapper;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Connect
    {
        public class Command : IRequest<PlayerDto>
        {
            public int GameId { get; set; }
            public string UserName { get; set; }
            public WindDirection InitialSeatWind { get; set; }

        }
        public class Handler : IRequestHandler<Command, PlayerDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<PlayerDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var usersInGame = game.UserGames.Count;
                if(usersInGame == 4)
                    throw new RestException(HttpStatusCode.BadRequest, new { Game = "Reached max players" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var connected = await _context.UserGames.SingleOrDefaultAsync(x => x.GameId == game.Id && x.AppUserId == user.Id);

                if(connected != null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Connect = "Player already in the game" });

                connected = new UserGame
                {
                    Game = game,
                    AppUser = user,
                    IsHost = false,
                    InitialSeatWind = request.InitialSeatWind
                };

                _context.UserGames.Add(connected);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<PlayerDto>(connected);

                throw new Exception("Problem connecting to game");
            }
        }
    }
}
