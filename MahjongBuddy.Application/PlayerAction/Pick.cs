using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MahjongBuddy.Application.PlayerAction
{
    public class Pick
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
            private readonly IPointsCalculator _pointCalculator;

            public Handler(MahjongBuddyDbContext context, IMapper mapper, IPointsCalculator pointCalculator)
            {
                _context = context;
                _mapper = mapper;
                _pointCalculator = pointCalculator;
            }
            public async Task<IEnumerable<RoundDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                //Note to consider when picking tile:
                //-tile is flower
                //-no more tile to pick
                //-only 1 more tile to pick because player have an option not to take the last tile.
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var currentPlayer = round.RoundPlayers.FirstOrDefault(p => p.GamePlayer.Player.UserName == request.UserName);

                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                currentPlayer.MustThrow = true;

                //TODO only allow pick tile when it's player's turn

                var newTiles = RoundTileHelper.PickTile(round, request.UserName);

                if (newTiles == null)
                    round.IsEnding = true;

                //pick action now only available on last tile
                //check if user can win if they pick on last tile
                var remainingTiles = round.RoundTiles.FirstOrDefault(t => string.IsNullOrEmpty(t.Owner));
                if (remainingTiles == null)
                {
                    currentPlayer.RoundPlayerActions.Clear();
                    if (RoundHelper.DetermineIfUserCanWin(round, currentPlayer, _pointCalculator))
                    {
                        currentPlayer.RoundPlayerActions.Add(new RoundPlayerAction { ActionType = ActionType.Win });
                    }
                }

                try
                {
                    var success = await _context.SaveChangesAsync() > 0;

                    List<RoundDto> results = new List<RoundDto>();

                    foreach (var p in round.RoundPlayers)
                    {
                        results.Add(_mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = p));
                    }

                    if (success) return results;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "player status was modified" });
                }

                throw new Exception("Problem picking tile");
            }
        }
    }
}
