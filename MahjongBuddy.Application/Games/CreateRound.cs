using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
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

                var lastRound = _context.Rounds.OrderByDescending(r => r.DateCreated).FirstOrDefault(r => r.GameId == request.GameId);                

                //if(!lastRound.IsOver)
                //    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Last round is not over" });

                var newRound = new Round
                {
                    Wind = request.Wind,
                    GameId = request.GameId,
                    DateCreated = DateTime.Now,
                    RoundTiles = CreateGameTiless().Shuffle()
                };

                if (lastRound == null)
                    newRound.Counter = 1;
                else
                    newRound.Counter = lastRound.Counter + 1;

                var players = game.UserGames.Select(x => x.AppUser);
                foreach (var player in players)
                {
                    if(player.Id == request.DealerId)
                        AssignTilesToUser(14, player.Id, newRound.RoundTiles);
                    else
                        AssignTilesToUser(13, player.Id, newRound.RoundTiles);
                }
                //save it first to get the id
                _context.Rounds.Add(newRound);

                var successCreatingRound = await _context.SaveChangesAsync() > 0;

                if(successCreatingRound)
                {
                    List<UserRound> userRounds = new List<UserRound>();
                    if (lastRound == null)
                    {
                        foreach (var ug in game.UserGames)
                        {
                            var ur = new UserRound { AppUser = ug.AppUser, RoundId = newRound.Id, Wind = ug.InitialSeatWind };
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
            private List<RoundTile> CreateGameTiless()
            {
                List<RoundTile> tiles = new List<RoundTile>();
                var game = _context.Games.First(g => g.Id == 1);

                for (var i = 1; i < 5; i++)
                {
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(1) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(2) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(3) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(4) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(5) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(6) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(7) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(8) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(9) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(11) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(12) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(13) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(14) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(15) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(16) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(17) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(18) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(19) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(21) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(22) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(23) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(24) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(25) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(26) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(27) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(28) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(29) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(31) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(32) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(33) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(41) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(42) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(43) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(44) });
                };

                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(51) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(52) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(53) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(54) });

                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(61) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(62) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(63) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = _context.Tiles.Find(64) });

                return tiles;
            }

            private void AssignTilesToUser(int tilesCount, string userId, IEnumerable<RoundTile> roundTiles)
            {
                var newTiles = roundTiles.Where(x => string.IsNullOrEmpty(x.Owner));
                int x = 0;
                foreach (var playTile in newTiles)
                {
                    playTile.Owner = userId;
                    if (playTile.Tile.TileType == TileType.Flower)
                    {
                        playTile.Status = TileStatus.UserGraveyard;                    
                    }
                    else
                    {
                        playTile.Status = TileStatus.UserActive;
                        x++;
                    }
                    if (x == tilesCount)
                    return;
                }
            }
        }
    }
}
