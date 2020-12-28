using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class Ending
    {
        public class Command : IRequest<IEnumerable<RoundDto>>
        {
            public int RoundId { get; set; }
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, IEnumerable<RoundDto>>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<RoundDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var round = await _context.Rounds.FindAsync(request.RoundId);
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
                        List<RoundDto> results = new List<RoundDto>();

                        foreach (var p in round.RoundPlayers)
                        {
                            results.Add(_mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = p));
                        }
                        return results;
                    }
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Ending = "Can't set round to ending" });

                throw new Exception("Problem calling ending round");
            }
        }
    }
}
