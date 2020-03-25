import GameStore from "./gameStore";
import UserStore from "./userStore";
import { createContext } from "react";

export class RootStore{
    gameStore: GameStore;
    userStore: UserStore;

    constructor(){
        this.gameStore = new GameStore(this);
        this.userStore = new UserStore(this);
    }
}

export const RootStoreContext = createContext(new RootStore());