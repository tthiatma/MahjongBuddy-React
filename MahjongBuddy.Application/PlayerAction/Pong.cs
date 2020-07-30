﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Errors;
using MahjongBuddy.Application.Extensions;
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

namespace MahjongBuddy.Application.PlayerAction
{
    public class Pong
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
                //Note to consider when pong tile:
                //-player turn changed
                //-flag when user can pong
                //-when pong tile user need to throw a tile.
                //assign tile ownership to current user

                var updatedTiles = new List<RoundTile>();
                var updatedPlayers = new List<RoundPlayer>();

                var round = await _context.Rounds.FindAsync(request.RoundId);

                if (round == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Round = "Could not find round" });

                round.IsHalted = true;

                var boardActiveTiles = round.RoundTiles.Where(t => t.Status == TileStatus.BoardActive);
                if (boardActiveTiles == null)
                    throw new RestException(HttpStatusCode.BadRequest, new {Round = "there are no tile to pong" });

                var tileToPong = boardActiveTiles.First();

                updatedTiles.Add(tileToPong);

                //check if user currently have two tile to pong 

                var matchingUserTiles = round.RoundTiles
                    .Where(t => t.Owner == request.UserName 
                        && t.Tile.TileType == tileToPong.Tile.TileType 
                        && t.Tile.TileValue == tileToPong.Tile.TileValue);
                
                if(matchingUserTiles == null || matchingUserTiles.Count() < 2)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are more than one tiles to pong" });

                //if there is Userjustpicked status then pong is invalid
                var justPickedTile = round.RoundTiles.FirstOrDefault(t => t.Status == TileStatus.UserJustPicked);
                if (justPickedTile != null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "someone already picked tile, no pong is allowed" });

                foreach (var tile in matchingUserTiles)
                {
                    updatedTiles.Add(tile);
                }

                updatedTiles.GoGraveyard(request.UserName, TileSetGroup.Pong, round.RoundTiles.GetLastGroupIndex(request.UserName));

                var currentPlayer = round.RoundPlayers.FirstOrDefault(u => u.AppUser.UserName == request.UserName);
                if(currentPlayer == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "there are no user with this username in the round" });

                currentPlayer.IsMyTurn = true;
                currentPlayer.MustThrow = true;

                if (round.IsEnding)
                    round.IsEnding = false;

                var otherPlayers = round.RoundPlayers.Where(u => u.IsMyTurn == true && u.AppUser.UserName != request.UserName);
                foreach (var otherPlayerTurn in otherPlayers)
                {
                    otherPlayerTurn.IsMyTurn = false;
                    updatedPlayers.Add(otherPlayerTurn);
                }

                updatedPlayers.Add(currentPlayer);

                try
                {
                    var success = await _context.SaveChangesAsync() > 0;

                    var roundToReturn = _mapper.Map<Round, RoundDto>(round);

                    roundToReturn.UpdatedRoundTiles = _mapper.Map<ICollection<RoundTile>, ICollection<RoundTileDto>>(updatedTiles);

                    roundToReturn.UpdatedRoundPlayers = _mapper.Map<ICollection<RoundPlayer>, ICollection<RoundPlayerDto>>(updatedPlayers);

                    if (success)
                        return roundToReturn;

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Round = "tile was modified" });
                }

                throw new Exception("Problem pong ing tile");
            }
        }
    }
}