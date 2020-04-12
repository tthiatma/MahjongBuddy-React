import GameStore from "./gameStore";
import UserStore from "./userStore";
import { createContext } from "react";
import { configure } from "mobx";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import RoundStore from "./roundStore";
import HubStore from "./hubStore";

configure({ enforceActions: "always" });

export class RootStore{
    gameStore: GameStore;
    userStore: UserStore;
    commonStore: CommonStore;
    modalStore: ModalStore;
    roundStore: RoundStore;
    hubStore: HubStore;
    
    constructor(){
        this.gameStore = new GameStore(this);
        this.userStore = new UserStore(this);
        this.commonStore = new CommonStore(this);
        this.modalStore = new ModalStore(this);
        this.roundStore = new RoundStore(this);
        this.hubStore = new HubStore(this);
    }
}

export const RootStoreContext = createContext(new RootStore());