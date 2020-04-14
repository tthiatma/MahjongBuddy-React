import { WindDirection } from "./windEnum";
import { IRoundTile } from "./tile";

export interface IRoundSimple{
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
}
export interface IRound extends IRoundSimple{
    roundTiles: IRoundTile[];
    updatedRoundTiles?: IRoundTile[];
    updatedRoundPlayers?: IRoundPlayer[];
    roundPlayers: IRoundPlayer[];
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
