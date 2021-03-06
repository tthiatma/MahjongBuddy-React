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

        public static void CheckSelfAction(Round round, RoundPlayer roundPlayer, IPointsCalculator pointsCalculator)
        {
            if(DetermineIfUserCanWin(round, roundPlayer, pointsCalculator))
                roundPlayer.RoundPlayerActions.Add(new RoundPlayerAction { ActionType = ActionType.SelfWin });

            CheckPossibleSelfKong(round, roundPlayer);
        }

        public static void SetNextPlayer(Round round, IPointsCalculator pointCalculator)
        {
            var playerThatHasTurn = round.RoundPlayers.FirstOrDefault(p => p.IsMyTurn == true);
            playerThatHasTurn.IsMyTurn = false;
            playerThatHasTurn.MustThrow = false;

            var nextPlayer = GetNextPlayer(round.RoundPlayers, playerThatHasTurn.Wind);
            nextPlayer.IsMyTurn = true;
            nextPlayer.MustThrow = true;

            //in case there is other player that has my turn set to true
            var otherPlayers = round.RoundPlayers.Where(u => u.IsMyTurn == true
                && u.GamePlayer.Player.UserName != nextPlayer.GamePlayer.Player.UserName
                && u.GamePlayer.Player.UserName != playerThatHasTurn.GamePlayer.Player.UserName);

            foreach (var otherPlayerTurn in otherPlayers)
            {
                otherPlayerTurn.IsMyTurn = false;
            }

            //automatically pick tile for next player
            //unless remaining tile is 1, give user give up action
            var unopenTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));

            if(unopenTiles.Count() == 1)
            {
                nextPlayer.RoundPlayerActions.Add(new RoundPlayerAction { ActionType = ActionType.GiveUp });
                nextPlayer.MustThrow = false;
            }            
            else
            {
                var newTiles = RoundTileHelper.PickTile(round, nextPlayer.GamePlayer.Player.UserName);
                if (newTiles == null)
                    round.IsEnding = true;
                CheckSelfAction(round, nextPlayer, pointCalculator);
            }
        }

        public static bool DetermineIfUserCanWin(Round round, RoundPlayer player, IPointsCalculator pointCalculator)
        {
            HandWorth handWorth = pointCalculator.Calculate(round, player.GamePlayer.Player.UserName);
            if (handWorth == null) return false;
            return handWorth.Points >= round.Game.MinPoint;
        }

        public static void CheckPossibleSelfKong(Round round, RoundPlayer player)
        {
            var unopenTiles = round.RoundTiles.Where(t => string.IsNullOrEmpty(t.Owner));
            if (unopenTiles.Count() == 0)
                return;

            var playerTiles = round.RoundTiles.Where(rt => rt.Owner == player.GamePlayer.Player.UserName && rt.TileSetGroup != TileSetGroup.Kong && rt.TileSetGroup != TileSetGroup.Chow);
            int possibleKongCount = playerTiles
                .GroupBy(t => new { t.Tile.TileType, t.Tile.TileValue })
                .Where(grp => grp.Count() == 4)
                .Count(); ;
            if (possibleKongCount > 0)
            {
                for (int i = 0; i < possibleKongCount; i++)
                {
                    player.RoundPlayerActions.Add(new RoundPlayerAction { ActionType = ActionType.SelfKong });
                }
            }
        }
    }
}
