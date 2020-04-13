import { WindDirection } from "./windEnum";
import { IRoundTile } from "./tile";

export interface IRound{
    id : number;
    roundCounter : number;
    tileConter: number;
    wind : WindDirection;
    DateCreated : Date;
    isHalted : boolean;
    isOver : boolean;
    isPaused : boolean;
    isTied : boolean;
    isWinnerSelfPicked : boolean;
    gameId : number;
    roundTiles: IRoundTile[];
    roundPlayers: IRoundPlayer[];
    mainPlayer: IRoundPlayer;
    leftPlayer?: IRoundPlayer;
    rightPlayer?: IRoundPlayer;
    topPlayer?: IRoundPlayer;
}

export interface IRoundPlayer{
    userName: string;
    displayName: string;
    image: string;
    isDealer: boolean;
    isMyTurn: boolean;
    canDoNoFlower: boolean;
    wind: WindDirection;
    tiles: IRoundTile[];
}
