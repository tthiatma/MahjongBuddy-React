﻿using AutoMapper;
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
    public class End
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
            private readonly IPointsCalculator _pointCalculator;

            public Handler(MahjongBuddyDbContext context, IMapper mapper, IPointsCalculator pointCalculator)
            {
                _context = context;
                _mapper = mapper;
                _pointCalculator = pointCalculator;
            }
            public async Task<RoundDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FindAsync(request.GameId);
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var remainingTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));
                //can only call end game when only 1 tile left or no more tile

                if (remainingTiles.Count() <= 1)
                {
                    round.IsOver = true;
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var roundToReturn = _mapper.Map<Round, RoundDto>(round);
                        return roundToReturn;
                    }
                }
                else
                    throw new RestException(HttpStatusCode.BadRequest, new { Win = "Not enough point to win with this hand" });

                throw new Exception("Problem calling end round");
            }
        }
    }
}
