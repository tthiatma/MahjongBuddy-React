using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Games
{
    /// <summary>
    /// Get latest round for this game
    /// </summary>
    public class LatestRound
    {
        public class Query : IRequest<RoundDto>
        {
            public string Code { get; set; }
        }

        public class Handler : IRequestHandler<Query, RoundDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoundDto> Handle(Query request, CancellationToken cancellationToken)
            {
                //handler logic 
                var game = await _context.Games.Where(g => g.Code == request.Code).FirstOrDefaultAsync();

                if (game == null)
                    throw new Exception("Could not find game");

                var round = game.Rounds.LastOrDefault();

                if (round == null)
                    return null;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                return roundToReturn;

                throw new Exception("Problem retrieving round");
            }
        }
    }
}
