export interface IGame{
    id: string;
    title: string;
    date: Date;
    players: IPlayer[]
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
}