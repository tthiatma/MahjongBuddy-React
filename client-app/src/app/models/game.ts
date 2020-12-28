import { WindDirection } from "./windEnum";
import { GameStatus } from "./gameStatusEnum";
import { IChatMsg } from "./chatMsg";
import { IGamePlayer } from "./player";

export interface IGamesEnvelope {
    games: IGame[];
    gameCount: number;
  }

export interface IGame{
    id: string;
    title: string;
    date: Date;
    minPoint: string;
    maxPoint: string;
    isHost: boolean;
    hostUserName: string;
    status: GameStatus;
    initialSeatWind?: WindDirection;
    isCurrentPlayerConnected: boolean;
    gamePlayers: IGamePlayer[];
    chatMsgs: IChatMsg[];
}

export interface IPayPoint{
    from: string,
    to: string,
    point: number
}

export interface IGameFormValues extends Partial<IGame> {
    time?: Date;
}

export class GameFormValues implements IGameFormValues {
    id?: string = undefined;
    title: string = '';
    minPoint: string = '';
    maxPoint: string = '';
    date?: Date = undefined;

    constructor(init?: IGameFormValues) {
        if (init && init.date) {
            init.time = init.date;
        }  
        Object.assign(this, init);
    }
}