using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using MoreLinq;
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
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var playerThatSkippedAction = round.RoundPlayers.FirstOrDefault(p => p.GamePlayer.Player.UserName == request.UserName);

                if (playerThatSkippedAction == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                playerThatSkippedAction.RoundPlayerActions.Where(a => a.ActionStatus == ActionStatus.Active).ForEach(ac => ac.ActionStatus = ActionStatus.Skipped);

                /////////////
                playerThatSkippedAction.HasAction = false;
                ////////////

                //check in case of multiple winner and this winner skip the option to win because too greedy!
                var otherWinnerActiveAction = round.RoundPlayers.Where(
                rp => rp.GamePlayer.Player.UserName != playerThatSkippedAction.GamePlayer.Player.UserName
                    && rp.RoundPlayerActions.Any(
                    rpa => rpa.ActionType == ActionType.Win && rpa.ActionStatus == ActionStatus.Active
                    )
                ).FirstOrDefault();

                if (otherWinnerActiveAction != null)
                {
                    //then we gotta wait other winner skip the win
                }
                else
                {
                    //prioritize user that has pong or kong action 
                    var pongOrKongActionPlayer = round.RoundPlayers.FirstOrDefault( rp => 
                        rp.RoundPlayerActions.Any(
                        rpa => (rpa.ActionType == ActionType.Pong && rpa.ActionStatus == ActionStatus.Inactive)
                        ||  (rpa.ActionType == ActionType.Kong && rpa.ActionStatus == ActionStatus.Inactive)));

                    if (pongOrKongActionPlayer != null)
                    {
                        pongOrKongActionPlayer.RoundPlayerActions
                            .Where(rpa => rpa.ActionType == ActionType.Pong || rpa.ActionType == ActionType.Kong)
                            .ForEach(a => a.ActionStatus = ActionStatus.Active);
                        pongOrKongActionPlayer.HasAction = true;
                    }
                    else
                    {
                        //now check other player that has chow action
                        var chowActionPlayer = round.RoundPlayers.FirstOrDefault(u =>
                        u.RoundPlayerActions.Any(a => a.ActionType == ActionType.Chow && a.ActionStatus == ActionStatus.Inactive));

                        if (chowActionPlayer != null)
                        {
                            chowActionPlayer.RoundPlayerActions
                                .Where(rpa => rpa.ActionType == ActionType.Chow)
                                .ForEach(a => a.ActionStatus = ActionStatus.Active);
                            chowActionPlayer.HasAction = true;
                        }
                        else
                        {
                            RoundHelper.SetNextPlayer(round, _pointCalculator);
                        }
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
