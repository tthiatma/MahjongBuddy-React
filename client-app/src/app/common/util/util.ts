import { IGame, IPlayer } from "../../models/game";
import { IUser } from "../../models/user";

export const combineDateAndTime = (date: Date, time: Date) => {
    // const timeString = time.getHours() + ':' + time.getMinutes() + ':00';

    // const year = date.getFullYear();
    // const month = date.getMonth() + 1;
    // const day = date.getDate();
    // const dateString = `${year}-${month}-${day}`;

    const dateString = date.toISOString().split('T')[0];
    const timeString = time.toISOString().split('T')[1];

    return new Date(dateString + ' ' + timeString);
}

export const setGameProps = (game: IGame, user: IUser) => {
    game.date = new Date(game.date);
    game.isConnected = game.players.some(
      p => p.userName === user.userName
    )
    game.isHost = game.players.some(
      p => p.userName === user.userName && p.isHost
    )
    return game;
}

export const createPlayer = (user: IUser): IPlayer => {
    return {
        displayName: user.displayName,
        isHost: false,
        userName: user.userName,
        image: user.image!
    }
}