using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
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

                var activeActions = playerThatSkippedAction.RoundPlayerActions.Where(a => a.ActionStatus == ActionStatus.Active);

                if (activeActions.Count() == 0)
                    throw new RestException(HttpStatusCode.BadRequest, new { Action = "no active action to skip" });

                activeActions.ForEach(ac => ac.ActionStatus = ActionStatus.Skipped);

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
                    //weird case when multiple winner, there is one declared win but somehow other player can win but skip lol wth then just set the round to be over
                    var activatedWin = round.RoundPlayers.Where(p => p.RoundPlayerActions.Any(a => a.ActionType == ActionType.Win && a.ActionStatus == ActionStatus.Activated));
                    if (activatedWin.Count() > 0)
                    {
                        round.IsOver = true;
                        round.IsEnding = false;
                    }
                    else
                    {
                        //prioritize user that has pong or kong action 
                        var pongOrKongActionPlayer = round.RoundPlayers.FirstOrDefault(rp =>
                           rp.RoundPlayerActions.Any(
                           rpa => (rpa.ActionType == ActionType.Pong && rpa.ActionStatus == ActionStatus.Inactive)
                           || (rpa.ActionType == ActionType.Kong && rpa.ActionStatus == ActionStatus.Inactive)));

                        if (pongOrKongActionPlayer != null)
                        {
                            pongOrKongActionPlayer.RoundPlayerActions
                                .Where(rpa => rpa.ActionType == ActionType.Pong || rpa.ActionType == ActionType.Kong)
                                .ForEach(a => a.ActionStatus = ActionStatus.Active);
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
                            }
                            else
                            {
                                RoundHelper.SetNextPlayer(round, _pointCalculator);
                            }
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
