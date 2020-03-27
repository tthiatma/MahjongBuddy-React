export interface IGame{
    id: string;
    title: string;
    date: string;
    players: IPlayer[]
}

export interface IPlayer{
    userName: string;
    displayName: string;
    image: string;
    isHost: boolean;
}