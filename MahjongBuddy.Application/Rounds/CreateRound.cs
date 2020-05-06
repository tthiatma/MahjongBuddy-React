using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
using MahjongBuddy.Application.Helpers;
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

namespace MahjongBuddy.Application.Rounds
{
    public class CreateRound
    {
        public class Command : IRequest<RoundDto>
        {
            public int GameId { get; set; }
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
                var game = await _context.Games.SingleOrDefaultAsync(x => x.Id == request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Game = "Game does not exist" });

                Round lastRound = _context.Rounds.OrderByDescending(r => r.DateCreated).FirstOrDefault(r => r.GameId == request.GameId);

                AppUser lastDealer;
                if (lastRound == null)
                    lastDealer = game.UserGames.First(u => u.InitialSeatWind == WindDirection.East).AppUser;
                else
                    lastDealer = lastRound.UserRounds.First(u => u.IsDealer).AppUser;

                //if(!lastRound.IsOver)
                //    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Last round is not over" });

                var newRound = new Round
                {
                    GameId = request.GameId,
                    DateCreated = DateTime.Now,
                    RoundTiles = RoundTileHelper.CreateTiles(_context).Shuffle(),
                    UserRounds = new List<UserRound>()                    
                };

                var players = game.UserGames.Select(x => x.AppUser);

                foreach (var player in players)
                {
                    if (player.Id == lastDealer.Id)
                        RoundTileHelper.AssignTilesToUser(14, player.UserName, newRound.RoundTiles);
                    else
                        RoundTileHelper.AssignTilesToUser(13, player.UserName, newRound.RoundTiles);
                }

                List<UserRound> userRounds = new List<UserRound>();
                if (lastRound == null)
                {
                    newRound.Wind = WindDirection.East;
                    newRound.RoundCounter = 1;
                    foreach (var ug in game.UserGames)
                    {
                        var ur = new UserRound { AppUser = ug.AppUser, Round = newRound, Wind = ug.InitialSeatWind };
                        if (ug.AppUserId == lastDealer.Id)
                        {
                            ur.IsDealer = true;
                            ur.IsMyTurn = true;
                        }
                        userRounds.Add(ur);
                    }
                }
                else
                {
                    newRound.RoundCounter = lastRound.RoundCounter + 1;

                    //if this is not the first round
                    //last round check
                    //1.) if the winner of last round == dealer then no wind change
                    //2.) if last round "IsTied" set to true then no wind change
                    if (lastRound.IsTied)
                    {
                        newRound.Wind = lastRound.Wind;
                        userRounds.AddRange(GetNewUserRounds(lastRound.UserRounds, sameRound: true, newRound: newRound));
                    }
                    else
                    {
                        //if last game is not tied, then there gotta be a winner here
                        var lastRoundWinners = lastRound.RoundResults.Where(x => x.IsWinner == true);
                        var dealerWonLastRound = lastRoundWinners.Any(x => x.AppUserId == lastDealer.Id);

                        if (dealerWonLastRound)
                        {
                            newRound.Wind = lastRound.Wind;
                            userRounds.AddRange(GetNewUserRounds(lastRound.UserRounds, sameRound: true, newRound: newRound));
                        }
                        else
                        {
                            newRound.Wind = NextRoundWind(lastRound, lastRound.Wind);                            
                            userRounds.AddRange(GetNewUserRounds(lastRound.UserRounds, sameRound: false, newRound: newRound));
                        }
                    }
                }

                foreach (var ur in userRounds)
                {
                    newRound.UserRounds.Add(ur);
                }

                _context.Rounds.Add(newRound);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<Round, RoundDto>(newRound);

                throw new Exception("Problem creating a new round");
            }

            private IEnumerable<UserRound> GetNewUserRounds(IEnumerable<UserRound> userRounds, bool sameRound, Round newRound)
            {
                List<UserRound> ret = new List<UserRound>();

                foreach (var lur in userRounds)
                {
                    var userWind = sameRound ? lur.Wind : NextWind(lur.Wind);
                    var ur = new UserRound
                    {
                        AppUserId = lur.AppUserId,
                        IsDealer = lur.IsDealer,
                        IsMyTurn = lur.IsMyTurn,
                        Wind = userWind
                    };
                    ret.Add(ur);
                }

                return ret;
            }

            private WindDirection NextRoundWind(Round lastRound, WindDirection lastRoundWind)
            {
                var lastRoundDealer = lastRound.UserRounds.First(u => u.IsDealer);
                if (lastRoundDealer.Wind != WindDirection.North)
                    return lastRoundWind;
                else
                    return NextWind(lastRoundWind);
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
