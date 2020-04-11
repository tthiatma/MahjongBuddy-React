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
      playerWind = GetOtherUserWindPosition(round.mainPlayer.wind, 'left');
      break;
    case 'right':
      playerWind = GetOtherUserWindPosition(round.mainPlayer.wind, 'right');
      break;
    case 'top':
      playerWind = GetOtherUserWindPosition(round.mainPlayer.wind, 'top');
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
  let mainPlayer = round.roundPlayers.find(
    p => p.userName === user.userName
  );
  if(mainPlayer)
  {
    //now that we know the main player, figure out other players
    let leftPlayerWind = GetOtherUserWindPosition(mainPlayer.wind, 'left');
    let rightPlayerWind = GetOtherUserWindPosition(mainPlayer.wind, 'right');
    let topPlayerWind = GetOtherUserWindPosition(mainPlayer.wind, 'top');
    
    let leftPlayer = round.roundPlayers.find(
        p => p.wind === leftPlayerWind
      );    
      if(leftPlayer)
        leftPlayer.tiles = round.roundTiles.filter(t => t.owner === leftPlayer?.userName); 

    let rightPlayer = round.roundPlayers.find(
      p => p.wind === rightPlayerWind
    );    
    if(rightPlayer)
    rightPlayer.tiles = round.roundTiles.filter(t => t.owner === rightPlayer?.userName); 

    let topPlayer = round.roundPlayers.find(
      p => p.wind === topPlayerWind
    );    
    if(topPlayer)
    topPlayer.tiles = round.roundTiles.filter(t => t.owner === topPlayer?.userName); 

    round.leftPlayer = leftPlayer!;
    round.rightPlayer = rightPlayer!;
    round.topPlayer = topPlayer!;
    round.mainPlayer = mainPlayer!;
  }
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