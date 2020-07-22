using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class Ending
    {
        public class Command : IRequest<RoundDto>
        {
            public int GameId { get; set; }
            public int RoundId { get; set; }
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, RoundDto>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoundDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.GameId);
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = game.Rounds.FirstOrDefault(r => r.Id == request.RoundId);
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var remainingTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));
                //can only call end game when only 1 tile left or no more tile

                if (remainingTiles.Count() <= 1)
                {
                    round.IsEnding = true;

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var roundToReturn = _mapper.Map<Round, RoundDto>(round);
                        return roundToReturn;
                    }
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Ending = "Can't set round to ending" });

                throw new Exception("Problem calling ending round");
            }
        }
    }
}
