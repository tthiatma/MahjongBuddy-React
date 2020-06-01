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
    public class Create
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

                if (lastRound != null && !lastRound.IsOver)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Last round is not over" });

                var newRound = new Round
                {
                    GameId = request.GameId,
                    DateCreated = DateTime.Now,
                    RoundTiles = RoundTileHelper.CreateTiles(_context).Shuffle(),
                    RoundPlayers = new List<RoundPlayer>(),
                    RoundResults = new List<RoundResult>()
                };

                var players = game.UserGames.Select(x => x.AppUser);

                List<RoundPlayer> roundPlayers = new List<RoundPlayer>();
                if (lastRound == null)
                {
                    AppUser firstDealer = game.UserGames.First(u => u.InitialSeatWind == WindDirection.East).AppUser;
                    newRound.Wind = WindDirection.East;
                    newRound.RoundCounter = 1;
                    foreach (var ug in game.UserGames)
                    {
                        var rp = new RoundPlayer { AppUser = ug.AppUser, Round = newRound, Wind = ug.InitialSeatWind };
                        if (ug.AppUserId == firstDealer.Id)
                        {
                            rp.IsDealer = true;
                            rp.IsMyTurn = true;
                        }
                        roundPlayers.Add(rp);
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
                        roundPlayers.AddRange(GetNewUserRounds(lastRound.RoundPlayers, sameRound: true, newRound: newRound));
                    }
                    else
                    {
                        //if last game is not tied, then there gotta be a winner here
                        var lastRoundDealer = lastRound.RoundPlayers.First(u => u.IsDealer).AppUser;

                        //could be more than one winners here
                        var lastRoundWinners = lastRound.RoundResults.Where(x => x.IsWinner == true);
                        var dealerWonLastRound = lastRoundWinners.Any(x => x.AppUserId == lastRoundDealer.Id);

                        if (dealerWonLastRound)
                        {
                            newRound.Wind = lastRound.Wind;
                            roundPlayers.AddRange(GetNewUserRounds(lastRound.RoundPlayers, sameRound: true, newRound: newRound));
                        }
                        else
                        {
                            newRound.Wind = NextRoundWind(lastRound, lastRound.Wind);                            
                            roundPlayers.AddRange(GetNewUserRounds(lastRound.RoundPlayers, sameRound: false, newRound: newRound));
                        }
                    }
                }

                foreach (var ur in roundPlayers)
                {
                    newRound.RoundPlayers.Add(ur);
                }

                var dealerId = roundPlayers.First(u => u.IsDealer).AppUserId;
                //tiles assignment
                foreach (var player in players)
                {
                    if (player.Id == dealerId)
                        RoundTileHelper.AssignTilesToUser(14, player.UserName, newRound.RoundTiles);
                    else
                        RoundTileHelper.AssignTilesToUser(13, player.UserName, newRound.RoundTiles);
                }

                _context.Rounds.Add(newRound);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<Round, RoundDto>(newRound);

                throw new Exception("Problem creating a new round");
            }

            private IEnumerable<RoundPlayer> GetNewUserRounds(IEnumerable<RoundPlayer> userRounds, bool sameRound, Round newRound)
            {
                List<RoundPlayer> ret = new List<RoundPlayer>();

                foreach (var lur in userRounds)
                {
                    var userWind = sameRound ? lur.Wind : NextWind(lur.Wind);
                    var ur = new RoundPlayer
                    {
                        AppUserId = lur.AppUserId,
                        IsDealer = lur.IsDealer,
                        IsMyTurn = lur.IsDealer,
                        Wind = userWind
                    };
                    ret.Add(ur);
                }

                return ret;
            }

            private WindDirection NextRoundWind(Round lastRound, WindDirection lastRoundWind)
            {
                var lastRoundDealer = lastRound.RoundPlayers.First(u => u.IsDealer);
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
