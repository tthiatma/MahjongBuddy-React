using MahjongBuddy.Application.Errors;
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

namespace MahjongBuddy.Application.Games
{
    public class CreateRound
    {
        public class Command: IRequest
        {
            public WindDirection Wind  { get; set; }
            public int GameId { get; set; }
            public string DealerId { get; set; }
        }

        public class Handler: IRequestHandler<Command>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(MahjongBuddyDbContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var dealer = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.DealerId);

                if (dealer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Dealer does not exist" });

                var game = await _context.Games.SingleOrDefaultAsync(x => x.Id == request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Game = "Game does not exist" });

                var lastRound = await _context.Rounds.LastOrDefaultAsync(r => r.GameId == game.Id);                

                //if(!lastRound.IsOver)
                //    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Last round is not over" });

                var newRound = new Round
                {
                    Wind = request.Wind
                };

                _context.Rounds.Add(newRound);

                var successCreatingRound = await _context.SaveChangesAsync() > 0;

                if(successCreatingRound)
                {
                    List<UserRound> userRounds = new List<UserRound>();
                    var player1Round = new UserRound { RoundId = newRound.Id };
                    var player2Round = new UserRound { RoundId = newRound.Id };
                    var player3Round = new UserRound { RoundId = newRound.Id };
                    var player4Round = new UserRound { RoundId = newRound.Id };

                    if (lastRound == null)
                    {
                        foreach (var ug in game.UserGames)
                        {
                            var ur = new UserRound { AppUser = ug.AppUser, RoundId = newRound.Id };
                            if (ug.AppUserId == dealer.Id)
                            {
                                ur.IsDealer = true;
                                ur.IsMyTurn = true;
                            }
                            userRounds.Add(ur);
                        }
                    }
                    else
                    {
                        //if this is not the first round
                        //last round check
                        //1.) if the winner of last round == dealer then no wind change
                        //2.) if last round "IsTied" set to true then no wind change
                        var lastRoundWinners = lastRound.RoundResults.Where(x => x.IsWinner == true);
                        var dealerWonLastRound = lastRoundWinners.Any(x => x.AppUserId == dealer.Id);
                        if (lastRound.IsTied || dealerWonLastRound)
                        {
                            foreach (var lur in lastRound.UserRounds)
                            {
                                var ur = new UserRound { 
                                    AppUserId = lur.AppUserId, 
                                    IsDealer = lur.IsDealer, 
                                    IsMyTurn = lur.IsMyTurn,
                                    Wind = lur.Wind
                                };
                                userRounds.Add(ur);
                            }
                        }
                        else
                        {
                            //otherwise in the new round, every user's wind will be changed
                            //east -> south
                            //south -> west
                            //west -> north
                            //north -> east
                            foreach (var lur in lastRound.UserRounds)
                            {
                                var ur = new UserRound
                                {
                                    AppUserId = lur.AppUserId,
                                    IsDealer = lur.IsDealer,
                                    IsMyTurn = lur.IsMyTurn,
                                    Wind = NextWind(lur.Wind)
                                };
                                userRounds.Add(ur);
                            }
                        }
                    }

                    _context.UserRounds.AddRange(userRounds);

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success) return Unit.Value;

                    throw new Exception("Problem creating a new round");
                }
                else
                {
                    throw new Exception("Problem creating a new round");
                }
            }

            private WindDirection NextWind(WindDirection wind)
            {
                WindDirection ret;
                switch (wind)
                {
                    case WindDirection.East:
                        ret = WindDirection.South;
                        break;
                    case WindDirection.South:
                        ret = WindDirection.West;
                        break;
                    case WindDirection.West:
                        ret = WindDirection.North;
                        break;
                    case WindDirection.North:
                        ret = WindDirection.East;
                        break;
                    default:
                        ret = WindDirection.East;
                        break;
                }
                return ret;
            }
        }
    }
}
