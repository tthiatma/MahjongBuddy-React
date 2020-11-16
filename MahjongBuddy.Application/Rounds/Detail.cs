﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.Rounds
{
    public class Detail
    {
        public class Query : IRequest<RoundDto>
        {
            public string Id { get; set; }
            public string GameId { get; set; }
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
                var game = await _context.Games.FindAsync(int.Parse(request.GameId));
                if (game == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Game = "Could not find game" });

                var round = game.Rounds.FirstOrDefault(r => r.Id == int.Parse(request.Id));
                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { round = "Could Not find round" });

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                return roundToReturn;
            }
        }
    }
}
