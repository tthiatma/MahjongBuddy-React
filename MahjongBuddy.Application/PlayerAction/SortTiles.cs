﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.PlayerAction
{
    public class SortTiles
    {
        public class Command : IRequest<RoundDto>
        {
            public int RoundId { get; set; }
            public string UserName { get; set; }
            public bool IsManualSort { get; set; }
            public IEnumerable<RoundTile> RoundTiles { get; set; }
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
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var currentRoundPlayer = round.RoundPlayers.FirstOrDefault(p => p.GamePlayer.Player.UserName == request.UserName);
                if (currentRoundPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                currentRoundPlayer.IsManualSort = request.IsManualSort;
                //update the activetilecounter
                foreach (var t in request.RoundTiles)
                {
                    var updatedTile = round.RoundTiles.First(rt => rt.Id == t.Id);
                    updatedTile.ActiveTileCounter = t.ActiveTileCounter;
                }

                try
                {
                    var success = await _context.SaveChangesAsync() > 0;
                    var roundToReturn = _mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = currentRoundPlayer);
                    
                    if (success) return roundToReturn;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile was modified" });
                }

                throw new Exception("Problem sorting tile");
            }
        }
    }
}
