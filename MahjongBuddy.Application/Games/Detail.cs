using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class Detail
    {
        public class Query : IRequest<GameDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, GameDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;


            public Handler(MahjongBuddyDbContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<GameDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var game = await _context.Games
                    .FindAsync(request.Id);

                if (game == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { game = "Not Found" });

                if(game.Status == GameStatus.Playing)
                {
                    bool inTheGame = game.UserGames.Any(p => p.AppUser.UserName == _userAccessor.GetCurrentUserName());
                    if(!inTheGame)
                        throw new RestException(HttpStatusCode.BadRequest, new { game = "This is not the game that you are looking for" });
                }

                var gameToReturn = _mapper.Map<Game, GameDto>(game);

                return gameToReturn;
            }
        }
    }
}
