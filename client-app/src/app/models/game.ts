import { WindDirection } from "./windEnum";

export interface IGame{
    id: string;
    title: string;
    date: Date;
    isHost: boolean;
    hostUserName: string;
    status: number;
    initialSeatWind?: WindDirection;
    isCurrentPlayerConnected: boolean;
    players: IPlayer[];
    chatMsgs: IChatMsg[];
}

export interface IChatMsg {
    id: string;
    createdAt: Date;
    body: string;
    userName: string;
    displayName: string;
    image: string;
}

export interface IGameFormValues extends Partial<IGame> {
    time?: Date;
}

export class GameFormValues implements IGameFormValues {
    id?: string = undefined;
    title: string = '';
    date?: Date = undefined;

    constructor(init?: IGameFormValues) {
        if (init && init.date) {
            init.time = init.date;
        }  
        Object.assign(this, init);
    }
}

export interface IPlayer{
    userName: string;
    displayName: string;
    image: string;
    isHost: boolean;
    initialSeatWind: WindDirection;
    currentSeatWind: WindDirection;
}