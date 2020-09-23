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

namespace MahjongBuddy.Application.PlayerAction
{
    public class ThrowAll
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
                var updatedTiles = new List<RoundTile>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var currentPlayer = round.RoundPlayers.FirstOrDefault(p => p.AppUser.UserName == request.UserName);

                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                //existing active tile on board to be no longer active
                var existingActiveTileOnBoard = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.BoardActive);
                if (existingActiveTileOnBoard != null)
                {
                    existingActiveTileOnBoard.Status = TileStatus.BoardGraveyard;
                    updatedTiles.Add(existingActiveTileOnBoard);
                }

                var userJustPickedTile = round.RoundTiles.Where(t => t.Owner == request.UserName && t.Status == TileStatus.UserJustPicked);
                if (userJustPickedTile != null && userJustPickedTile.Count() > 0)
                {
                    foreach (var t in userJustPickedTile)
                    {
                        t.Status = TileStatus.UserActive;
                        updatedTiles.Add(t);
                    }
                }
                var unopenTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));

                //throw all but one
                var tileCountMinusOne = unopenTiles.Count() - 1;

                foreach (var tileToThrow in unopenTiles.Take(tileCountMinusOne))
                {
                    tileToThrow.ThrownBy = request.UserName;
                    tileToThrow.Owner = DefaultValue.board;
                    tileToThrow.Status = TileStatus.BoardGraveyard;
                    tileToThrow.BoardGraveyardCounter = round.TileCounter;
                    round.TileCounter++;
                    updatedTiles.Add(tileToThrow);
                }

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem throwing all tile");
            }
        }
    }
}
