using AutoMapper;
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
        public class Command: IRequest<List<RoundTileDto>>
        {
            public WindDirection Wind  { get; set; }
            public int GameId { get; set; }
        }

        public class Handler: IRequestHandler<Command, List<RoundTileDto>>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(MahjongBuddyDbContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }
            public async Task<List<RoundTileDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.SingleOrDefaultAsync(x => x.Id == request.GameId);

                if (game == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Game = "Game does not exist" });

                var lastRound = _context.Rounds.OrderByDescending(r => r.DateCreated).FirstOrDefault(r => r.GameId == request.GameId);

                //make host as dealer for convenient for now
                AppUser dealer;
                if (lastRound == null)
                    dealer = game.UserGames.First(u => u.IsHost).AppUser;
                else
                    dealer = lastRound.UserRounds.First(u => u.IsDealer).AppUser;

                //if(!lastRound.IsOver)
                //    throw new RestException(HttpStatusCode.BadRequest, new { Round = "Last round is not over" });

                var newRound = new Round
                {
                    Wind = request.Wind,
                    GameId = request.GameId,
                    DateCreated = DateTime.Now,
                    RoundTiles = CreateGameTiless().Shuffle()
                };

                var players = game.UserGames.Select(x => x.AppUser);
                foreach (var player in players)
                {
                    if(player.Id == dealer.Id)
                        AssignTilesToUser(14, player.Id, newRound.RoundTiles);
                    else
                        AssignTilesToUser(13, player.Id, newRound.RoundTiles);
                }

                List<UserRound> userRounds = new List<UserRound>();
                if (lastRound == null)
                {
                    newRound.Counter = 1;
                    foreach (var ug in game.UserGames)
                    {
                        var ur = new UserRound { AppUser = ug.AppUser, Round= newRound, Wind = ug.InitialSeatWind };
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
                    newRound.Counter = lastRound.Counter + 1;

                    //if this is not the first round
                    //last round check
                    //1.) if the winner of last round == dealer then no wind change
                    //2.) if last round "IsTied" set to true then no wind change
                    if (lastRound.IsTied)
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
                        //if last game is not tied, then there gotta be a winner here
                        var lastRoundWinners = lastRound.RoundResults.Where(x => x.IsWinner == true);
                        var dealerWonLastRound = lastRoundWinners.Any(x => x.AppUserId == dealer.Id);

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

                var rounTilesToReturn = _mapper.Map<List<RoundTile>, List<RoundTileDto>>(newRound.RoundTiles.ToList());
                if (success) return rounTilesToReturn;

                throw new Exception("Problem creating a new round");
                
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
                var allTiles = _context.Tiles.ToList();
                List<RoundTile> tiles = new List<RoundTile>();

                for (var i = 1; i < 5; i++)
                {
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 1) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 2) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 3) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 4) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 5) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 6) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 7) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 8) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 9) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 11) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 12) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 13) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 14) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 15) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 16) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 17) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 18) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 19) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 21) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 22) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 23) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 24) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 25) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 26) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 27) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 28) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 29) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 31) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 32) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 33) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 41) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 42) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 43) });
                    tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 44) });
                };

                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 51) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 52) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 53) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 54) });

                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 61) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 62) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 63) });
                tiles.Add(new RoundTile { RoundId = 1, Tile = allTiles.Find(x => x.Id == 64) });

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
