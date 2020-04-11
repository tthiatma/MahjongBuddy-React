import { IGame } from "../../models/game";
import { IUser } from "../../models/user";
import { WindDirection } from "../../models/windEnum";
import { IRound, IRoundPlayer } from "../../models/round";

export const GetOtherUserTiles = (round: IRound, direction: string ) => {
  console.log(round);
  let playerWind: WindDirection | undefined;
  let player: IRoundPlayer| undefined;

  switch(direction){
    case 'left':
      playerWind = GetOtherUserWindPosition(round.currentRoundPlayer.wind, 'left');
      break;
    case 'right':
      playerWind = GetOtherUserWindPosition(round.currentRoundPlayer.wind, 'right');
      break;
    case 'top':
      playerWind = GetOtherUserWindPosition(round.currentRoundPlayer.wind, 'top');
      break;
    } 
    player =  round.roundPlayers.find(p => p.wind === playerWind); 
    return round.roundTiles.filter(rt => rt.owner === player!.userName);
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