using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Core;
using MahjongBuddy.Core.Enums;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    public class RandomizeWind
    {
        public class Command : IRequest<IEnumerable<GamePlayerDto>>
        {
            public int GameId { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, IEnumerable<GamePlayerDto>>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<IEnumerable<GamePlayerDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                if (game.Status == GameStatus.Playing || game.Status == GameStatus.Over)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Can't randomize wind to a game that's already started/over" });

                var userInGames = game.GamePlayers.ToArray();

                if (userInGames.Count() != 4)
                    throw new RestException(HttpStatusCode.BadRequest, new { players = "not enough player in the game" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var playerInGame = await _context.GamePlayers.SingleOrDefaultAsync(x => x.GameId == game.Id && x.PlayerId == user.Id);

                if (playerInGame == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { player = "user not in the game" });

                //only host can randomize the intiial wind
                if (playerInGame.IsHost)
                {
                    WindDirection[] RandomWind = (WindDirection[])Enum.GetValues(typeof(WindDirection));
                    RandomWind.Shuffle();

                    for (int i = 0; i < 4; i++)
                    {
                        userInGames[i].InitialSeatWind = RandomWind[i];
                    }
                }
                try
                {
                    await _context.SaveChangesAsync();
                    return _mapper.Map<IEnumerable<GamePlayer>, IEnumerable<GamePlayerDto>>(userInGames);
                }
                catch (Exception)
                {
                    throw new Exception("Problem randomizing user's wind");
                }
            }
        }
    }
}
