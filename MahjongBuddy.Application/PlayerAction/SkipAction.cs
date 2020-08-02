using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Helpers;
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
                var updatedPlayers = new List<RoundPlayer>();
                var updatedTiles = new List<RoundTile>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var playerThatSkippedAction = round.RoundPlayers.FirstOrDefault(p => p.AppUser.UserName == request.UserName);

                if (playerThatSkippedAction == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                playerThatSkippedAction.HasAction = false;
                playerThatSkippedAction.RoundPlayerActions.Clear();

                updatedPlayers.Add(playerThatSkippedAction);

                //prioritize user that has pong or kong action 
                var pongOrKongActionPlayer = round.RoundPlayers.Where(
                rp => rp.AppUser.UserName != playerThatSkippedAction.AppUser.UserName
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
                    u.AppUser.UserName != playerThatSkippedAction.AppUser.UserName
                    && u.IsMyTurn != true
                    && u.RoundPlayerActions.Count() > 0);

                    if (chowPlayerActions != null)
                    {
                        chowPlayerActions.HasAction = true;
                        updatedPlayers.Add(chowPlayerActions);
                    }
                    else
                    {
                        RoundHelper.SetNextPlayer(round, ref updatedPlayers, ref updatedTiles);
                    }
                }

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);
                roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem skipping action");
            }
        }
    }
}
