using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
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
    public class SkipAction
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
                var updatedPlayers = new List<RoundPlayer>();
                var updatedTiles = new List<RoundTile>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var playerThatSkippedAction = round.RoundPlayers.FirstOrDefault(p => p.GamePlayer.Player.UserName == request.UserName);

                if (playerThatSkippedAction == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                playerThatSkippedAction.HasAction = false;
                playerThatSkippedAction.RoundPlayerActions.Clear();

                updatedPlayers.Add(playerThatSkippedAction);

                //prioritize user that has pong or kong action 
                var pongOrKongActionPlayer = round.RoundPlayers.Where(
                rp => rp.GamePlayer.Player.UserName != playerThatSkippedAction.GamePlayer.Player.UserName
                    && rp.RoundPlayerActions.Any(
                    rpa => rpa.PlayerAction == ActionType.Pong ||
                    rpa.PlayerAction == ActionType.Kong
                    )
                ).FirstOrDefault();

                if(pongOrKongActionPlayer != null)
                {
                    pongOrKongActionPlayer.HasAction = true;
                    updatedPlayers.Add(pongOrKongActionPlayer);
                }
                else
                {
                    //now check other player that has action
                    var chowPlayerActions = round.RoundPlayers.FirstOrDefault(u =>
                    u.GamePlayer.Player.UserName != playerThatSkippedAction.GamePlayer.Player.UserName
                    && u.IsMyTurn != true
                    && u.RoundPlayerActions.Count() > 0);

                    if (chowPlayerActions != null)
                    {
                        chowPlayerActions.HasAction = true;
                        updatedPlayers.Add(chowPlayerActions);
                    }
                    else
                    {
                        RoundHelper.SetNextPlayer(round, ref updatedPlayers, ref updatedTiles, _pointCalculator);
                    }
                }

                var success = await _context.SaveChangesAsync() > 0;

                List<RoundDto> results = new List<RoundDto>();

                foreach (var p in round.RoundPlayers)
                {
                    results.Add(_mapper.Map<Round, RoundDto>(round, opt => opt.Items["MainRoundPlayer"] = p));
                }

                if (success) return results;

                throw new Exception("Problem skipping action");
            }
        }
    }
}
