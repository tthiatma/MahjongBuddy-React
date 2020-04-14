import { observable, action, runInAction } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { setRoundProps } from "../common/util/util";
import { IRound, IRoundPlayer, IRoundSimple } from "../models/round";
import { IRoundTile } from "../models/tile";

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
