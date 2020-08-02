using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongBuddy.Application.Helpers
{
    public static class RoundHelper
    {
        public static RoundPlayer GetNextPlayer(ICollection<RoundPlayer> players, WindDirection currentPlayerWind)
        {
            RoundPlayer ret;
            switch (currentPlayerWind)
            {
                case WindDirection.East:
                    ret = players.First(p => p.Wind == WindDirection.South);
                    break;
                case WindDirection.South:
                    ret = players.First(p => p.Wind == WindDirection.West);
                    break;
                case WindDirection.West:
                    ret = players.First(p => p.Wind == WindDirection.North);
                    break;
                case WindDirection.North:
                    ret = players.First(p => p.Wind == WindDirection.East);
                    break;
                default:
                    throw new Exception("Error when getting next player");
            }

            return ret;
        }

        public static void SetNextPlayer(Round round, ref List<RoundPlayer> updatedPlayers, ref List<RoundTile> updatedTiles)
        {
            var playerThatHasTurn = round.RoundPlayers.FirstOrDefault(p => p.IsMyTurn == true);
            playerThatHasTurn.IsMyTurn = false;
            playerThatHasTurn.MustThrow = false;

            var nextPlayer = GetNextPlayer(round.RoundPlayers, playerThatHasTurn.Wind);
            nextPlayer.IsMyTurn = true;
            nextPlayer.MustThrow = true;

            //in case there is other player that has my turn set to true
            var otherPlayers = round.RoundPlayers.Where(u => u.IsMyTurn == true
                && u.AppUser.UserName != nextPlayer.AppUser.UserName
                && u.AppUser.UserName != playerThatHasTurn.AppUser.UserName);

            foreach (var otherPlayerTurn in otherPlayers)
            {
                otherPlayerTurn.IsMyTurn = false;
                updatedPlayers.Add(otherPlayerTurn);
            }

            //automatically pick tile for next player
            RoundTileHelper.PickTile(round, nextPlayer.AppUser.UserName, ref updatedTiles);
            updatedPlayers.Add(nextPlayer);
            updatedPlayers.Add(playerThatHasTurn);
        }
    }
}
