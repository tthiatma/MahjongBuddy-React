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
using MoreLinq;
using MahjongBuddy.Core.Enums;

namespace MahjongBuddy.Application.Rounds
{
    public class Tied
    {
        public class Command : IRequest<IEnumerable<RoundDto>>
        {
            public int GameId { get; set; }
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
                var game = await _context.Games.FindAsync(request.GameId);
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = game.Rounds.FirstOrDefault(r => r.Id == request.RoundId);
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var remainingTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));
                //can only call end round when only 1 tile left or no more tile

                if (remainingTiles.Count() <= 1)
                {
                    round.IsOver = true;
                    round.IsTied = true;
                    round.IsEnding = false;

                    round.RoundPlayers.ForEach(rp =>
                    {
                        round.RoundResults.Add(new RoundResult { PlayResult = PlayResult.Tie, Player = rp.GamePlayer.Player, Points = 0 });
                    });

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
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Not enough point to win with this hand" });

                throw new Exception("Problem calling end round");
            }
        }
    }
}
