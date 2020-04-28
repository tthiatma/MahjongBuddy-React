import { observable, action, runInAction, computed } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { setRoundProps, sortTiles } from "../common/util/util";
import { IRound, IRoundPlayer, IRoundSimple } from "../models/round";
import { IRoundTile } from "../models/tile";
import { TileStatus } from "../models/tileStatus";
import _ from "lodash";

export default class RoundStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable selectedTile: IRoundTile | null = null;
  @observable roundSimple: IRoundSimple | null = null;
  @observable roundTiles: IRoundTile[] | null = null;
  @observable loadingRoundInitial = false;
  @observable mainPlayer: IRoundPlayer | null = null;
  @observable leftPlayer: IRoundPlayer | null = null;
  @observable rightPlayer: IRoundPlayer | null = null;
  @observable topPlayer: IRoundPlayer | null = null;

  @computed get boardActiveTile() {
    return this.roundTiles
    ? this.roundTiles.find((rt) => rt.status === TileStatus.BoardActive)
    : null;
  } 

  @computed get boardGraveyardTiles() {
    return this.roundTiles
  ? this.roundTiles.filter((rt) => rt.status === TileStatus.BoardGraveyard)
  : null;
  }

  @computed get mainPlayerTiles() {
    return this.roundTiles && this.roundSimple && this.leftPlayer
    ? this.roundTiles
    .filter((rt) => rt.owner === this.rootStore.userStore.user!.userName)
    : null;
  }

  @computed get mainPlayerAliveTiles(){
    return this.roundTiles
  ? this.roundTiles
  .filter((rt) => rt.owner === this.rootStore.userStore.user!.userName 
    && (rt.status === TileStatus.UserActive || rt.status === TileStatus.UserJustPicked))
  .sort(sortTiles)
  : null;
  }

  @computed get mainPlayerActiveTiles(){
    return this.roundTiles
  ? this.roundTiles
  .filter((rt) => rt.owner === this.rootStore.userStore.user!.userName && rt.status === TileStatus.UserActive)
  .sort(sortTiles)
  : null;
  }

  @computed get mainPlayerGraveYardTiles() {
    return this.roundTiles
  ? this.roundTiles
  .filter((rt) => rt.owner === this.rootStore.userStore.user?.userName && rt.status === TileStatus.UserGraveyard)
  .sort(sortTiles)
  : null;
  }

  @computed get mainPlayerJustPickedTile () {   
    return this.roundTiles 
  ? this.roundTiles.filter((rt) => rt.owner === this.rootStore.userStore.user?.userName && rt.status === TileStatus.UserJustPicked)
  : null;
  }

  @computed get leftPlayerTiles () {
    return this.roundTiles && this.roundSimple && this.leftPlayer
    ? this.roundTiles
    .filter((rt) => rt.owner === this.leftPlayer?.userName)
    .sort(sortTiles)
    : null;
  }  

  @computed get topPlayerTiles() {
    return this.roundTiles && this.roundSimple && this.topPlayer
      ? this.roundTiles
      .filter((rt) => rt.owner === this.topPlayer?.userName)
      .sort(sortTiles)
      : null;
  } 

  @computed get rightPlayerTiles() {
    return this.roundTiles && this.roundSimple && this.rightPlayer
    ? this.roundTiles
    .filter((rt) => rt.owner === this.rightPlayer?.userName)
    .sort(sortTiles)
    : null;
  } 

  @action loadRound = async (id: number) => {
    let round: IRound;
    this.loadingRoundInitial = true;
    try {
      round = await agent.Rounds.detail(id);
      runInAction("getting round", () => {
        setRoundProps(round, this.rootStore.userStore.user!, this);
        this.roundSimple = round;
        this.roundTiles = round.roundTiles;
        this.loadingRoundInitial = false;
      });
      return round;
    } catch (error) {
      runInAction("getting round error", () => {
        this.loadingRoundInitial = false;
      });
      console.log(error);
    }
  };
}
