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
                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                var currentPlayer = round.RoundPlayers.FirstOrDefault(p => p.AppUser.UserName == request.UserName);

                if (currentPlayer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find current player" });

                currentPlayer.HasAction = false;
                currentPlayer.RoundPlayerActions.Clear();

                updatedPlayers.Add(currentPlayer);

                //now check other player that has action
                var otherPlayer = round.RoundPlayers.FirstOrDefault(u => 
                u.AppUser.UserName != currentPlayer.AppUser.UserName 
                && u.IsMyTurn != true
                && u.RoundPlayerActions.Count() > 0);

                if(otherPlayer != null)
                {
                    otherPlayer.HasAction = true;
                    updatedPlayers.Add(otherPlayer);
                }

                var success = await _context.SaveChangesAsync() > 0;

                var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                if (success)
                    return roundToReturn;

                throw new Exception("Problem skipping action");
            }
        }
    }
}
