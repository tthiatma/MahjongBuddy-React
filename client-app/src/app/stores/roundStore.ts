import { observable, action, runInAction, toJS } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { setRoundProps } from "../common/util/util";
import { IRound } from "../models/round";
import { IRoundTile } from "../models/tile";

export default class RoundStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable selectedTile: IRoundTile | null = null;
  @observable round: IRound | null = null;
  @observable roundTiles: IRoundTile[] | null = null;
  @observable loadingRoundInitial = false;

  @action loadRound = async (id: number) => {
    let round: IRound;
    this.loadingRoundInitial = true;
    try {
      round = await agent.Rounds.detail(id);
      runInAction("getting round", () => {
        setRoundProps(round, this.rootStore.userStore.user!);
        this.round = round;
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
