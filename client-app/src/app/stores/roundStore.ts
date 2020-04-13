import { observable, action, runInAction, toJS } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import {
  setRoundProps,
} from "../common/util/util";
import { IRound } from "../models/round";
import { IRoundTile } from "../models/tile";

export default class RoundStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable selectedTile: IRoundTile | null = null;
  @observable round: IRound | null = null;
  @observable roundRegistry = new Map();
  @observable roundTiles: IRoundTile[] | null = null;
  @observable loadingRoundInitial = false;

  @action loadRound = async (id: number) => {
    let round = this.getRound(id);
    if (round) {
      this.round = round;
      return toJS(round);
    } else {
      this.loadingRoundInitial = true;
      try {
        round = await agent.Rounds.detail(id);
        runInAction("getting round", () => {
          setRoundProps(round, this.rootStore.userStore.user!);
          this.round = round;
          this.roundRegistry.set(round.id, round);
          this.roundTiles = round.roundTiles;
          this.loadingRoundInitial = false;
        });
        console.log("getting round");
        console.log(round);
        return round;
      } catch (error) {
        runInAction("getting round error", () => {
          this.loadingRoundInitial = false;
        });
        console.log(error);
      }
    }
  };

  getRound = (id: number) => {
    return this.roundRegistry.get(id);
  };
}
