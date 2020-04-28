import { IGame } from "../../models/game";
import { IUser } from "../../models/user";
import { WindDirection } from "../../models/windEnum";
import { IRound } from "../../models/round";
import { runInAction } from "mobx";
import RoundStore from "../../stores/roundStore";
import { IRoundTile } from "../../models/tile";

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

export const sortTiles = (a: IRoundTile, b: IRoundTile) => {
  if(a.tile.tileType > b.tile.tileType) return -1;
  if(a.tile.tileType < b.tile.tileType) return 1;
  if(a.tile.tileValue > b.tile.tileValue) return 1;
  if(a.tile.tileValue < b.tile.tileValue) return -1;  
  return 0;
}

export const combineDateAndTime = (date: Date, time: Date) => {
    const dateString = date.toISOString().split('T')[0];
    const timeString = time.toISOString().split('T')[1];

    return new Date(dateString + ' ' + timeString);
}

export const setRoundProps = (round: IRound, user: IUser, roundStore: RoundStore) => {
  if(!roundStore.mainPlayer){
    runInAction("Updating Round Players", () =>{
      let mainPlayer = round.roundPlayers.find(
        p => p.userName === user.userName
      );
        if(mainPlayer){
        roundStore.mainPlayer = mainPlayer;
  
        let leftUserWind = GetOtherUserWindPosition(mainPlayer.wind, "left");
        roundStore.leftPlayer = round.roundPlayers.find(p => p.wind === leftUserWind)!;
    
        let topUserWind = GetOtherUserWindPosition(mainPlayer.wind, "top");
        roundStore.topPlayer = round.roundPlayers.find(p => p.wind === topUserWind)!;
    
        let rightUserWind = GetOtherUserWindPosition(mainPlayer.wind, "right");
        roundStore.rightPlayer = round.roundPlayers.find(p => p.wind === rightUserWind)!;
      }  
    })  
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