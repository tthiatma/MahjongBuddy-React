﻿using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Application.PlayerAction;
using MahjongBuddy.Application.Rounds.Scorings;
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

        public static void SetNextPlayer(Round round, ref List<RoundPlayer> updatedPlayers, ref List<RoundTile> updatedTiles, IPointsCalculator pointCalculator)
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
            //unless remaining tile is 1, give user give up action
            var unopenTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));

            if(unopenTiles.Count() == 1)
            {
                nextPlayer.RoundPlayerActions.Add(new RoundPlayerAction { PlayerAction = ActionType.GiveUp });
                nextPlayer.MustThrow = false;
            }            
            else           
                RoundTileHelper.PickTile(round, nextPlayer.AppUser.UserName, ref updatedTiles);            

            //check if there's any self action
            //self win
            if (DetermineIfUserCanWin(round, nextPlayer, pointCalculator))
                nextPlayer.RoundPlayerActions.Add(new RoundPlayerAction { PlayerAction = ActionType.SelfWin });

            //self kong
            CheckPossibleSelfKong(round, nextPlayer);

            updatedPlayers.Add(nextPlayer);
            updatedPlayers.Add(playerThatHasTurn);
        }

        public static bool DetermineIfUserCanWin(Round round, RoundPlayer player, IPointsCalculator pointCalculator)
        {
            HandWorth handWorth = pointCalculator.Calculate(round, player.AppUser.UserName);
            if (handWorth == null) return false;
            return handWorth.Points >= round.Game.MinPoint;
        }

        public static void CheckPossibleSelfKong(Round round, RoundPlayer player)
        {
            var playerTiles = round.RoundTiles.Where(rt => rt.Owner == player.AppUser.UserName && rt.TileSetGroup != TileSetGroup.Kong && rt.TileSetGroup != TileSetGroup.Chow);
            int possibleKongCount = playerTiles
                .GroupBy(t => new { t.Tile.TileType, t.Tile.TileValue })
                .Where(grp => grp.Count() == 4)
                .Count(); ;
            if (possibleKongCount > 0)
            {
                for (int i = 0; i < possibleKongCount; i++)
                {
                    player.RoundPlayerActions.Add(new RoundPlayerAction { PlayerAction = ActionType.SelfKong });
                }
            }
        }
    }
}
