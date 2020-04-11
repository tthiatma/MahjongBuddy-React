import { IGame, IPlayer } from "../../models/game";
import { IUser } from "../../models/user";
import { WindDirection } from "../../models/windEnum";
import { IRoundTile } from "../../models/tile";
import { IRound, IRoundPlayer } from "../../models/round";

export const GetOtherUserTiles = (roundTiles: IRoundTile[], players: IRoundPlayer[], currentPlayerWind: WindDirection, direction: string ) => {
  
  switch(direction){
    case 'left':
      switch(currentPlayerWind){
        case WindDirection.East:
        const leftPlayerWind = GetOtherUserWindPosition(WindDirection.East, 'left');
        const leftPlayer = players.find(p => p.wind === leftPlayerWind);
        return roundTiles.filter(rt => rt.owner === leftPlayer!.userName)
      }
      break;
    case 'right':
      break;
    case 'top':
      break;
    }  
}

export const GetOtherUserWindPosition = (currentUserWind:WindDirection , direction: string) => {
  switch(direction){
    case 'left':
      switch(currentUserWind){
        case WindDirection.East:
          return WindDirection.South;
        case WindDirection.South:
          return WindDirection.West;
        case WindDirection.West:
          return WindDirection.North;
        case WindDirection.North:
          return WindDirection.East;
      }
    break;
    case 'right':
      switch(currentUserWind){
        case WindDirection.East:
          return WindDirection.North;
        case WindDirection.South:
          return WindDirection.East;
        case WindDirection.West:
          return WindDirection.South;
        case WindDirection.North:
          return WindDirection.West;
      }
    break;    
    case 'top':
      switch(currentUserWind){
        case WindDirection.East:
          return WindDirection.West;
        case WindDirection.South:
          return WindDirection.North;
        case WindDirection.West:
          return WindDirection.East;
        case WindDirection.North:
          return WindDirection.South;
      }
    break;
  }
}

export const combineDateAndTime = (date: Date, time: Date) => {
    const dateString = date.toISOString().split('T')[0];
    const timeString = time.toISOString().split('T')[1];

    return new Date(dateString + ' ' + timeString);
}

export const setRoundProps = (round: IRound, user: IUser) => {
  let currentPlayer = round.roundPlayers.find(
    p => p.userName === user.userName
  );
  round.currentRoundPlayer = currentPlayer!;
}

export const setGameProps = (game: IGame, user: IUser) => {
    
  let currentPlayer = game.players.find(
    p => p.userName === user.userName
  );
  game.date = new Date(game.date);
    
    game.isCurrentPlayerConnected = game.players.some(
      p => p.userName === user.userName
    )

    game.isHost = game.hostUserName === user.userName;

    if(currentPlayer?.initialSeatWind !== undefined){
      game.initialSeatWind = game.players.find(
        p => p.userName === user.userName
      )!.initialSeatWind        
    }else{
      game.initialSeatWind = WindDirection.Unassigned
    }
  return game;
}