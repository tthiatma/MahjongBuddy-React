import { WindDirection } from "./windEnum";
import { IRoundTile } from "./tile";

export interface IRound{
    id : number;
    counter : number;
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
    currentRoundPlayer: IRoundPlayer;
}

export interface IRoundPlayer{
    userName: string;
    displayName: string;
    image: string;
    isDealer: boolean;
    isMyTurn: boolean;
    canDoNoFlower: boolean;
    wind: WindDirection;
}
