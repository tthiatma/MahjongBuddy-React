import { observable, action, runInAction, computed, reaction, IReactionDisposer } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { setRoundProps, sortTiles } from "../common/util/util";
import {
  IRound,
  IRoundPlayer,
  IRoundSimple,
  IRoundResult,
} from "../models/round";
import { IRoundTile } from "../models/tile";
import { TileStatus } from "../models/tileStatus";
import { ActionType } from "../models/actionTypeEnum";

const pickDefaultCounter:number = 3;

export default class RoundStore {
  rootStore: RootStore;
  pickReaction: IReactionDisposer;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;

    reaction(
      () => this.roundSimple?.isOver,
      () => {
        if(this.roundSimple?.isOver){
          this.showResult = true;
        }
      }
    )

    //reaction to automatically call round tied
    //when 1 tile left and the person's turn give up
    //when no tile left
    reaction(
      () => this.roundSimple?.isEnding && this.roundEndingCounter,
      () => {
        if(this.roundSimple?.isEnding){
          this.roundEndingCounter > 0 &&
          setTimeout(() => runInAction(() => this.roundEndingCounter--), 1000);
          if (this.roundEndingCounter === 0) {
            //call round over if its the user's turn so that no multiple call
            if(this.mainPlayer?.isMyTurn)
            {
              rootStore.hubStore.tiedRound();  
            }
          };
        } else {
          runInAction(() => {
            this.roundEndingCounter = 5;
          });
        }
      }
    );
    
    //reaction for countdown before user can pick tile
    this.pickReaction = reaction(
      () => this.mainPlayer?.isMyTurn && this.pickCounter,
      () => {
        if (this.mainPlayer?.isMyTurn) {
          this.pickCounter > 0 &&
            setTimeout(() => runInAction(() => this.pickCounter--), 1000);
          if (this.pickCounter === 0) runInAction(() => this.canPick = true);
        } else {
          runInAction(() => {
            this.pickCounter = pickDefaultCounter;
            this.canPick = false;
          });
        }
      }
    );
  }

  @observable pickCounter: number = pickDefaultCounter;
  @observable canPick: boolean = false;
  @observable isMyTurn: boolean = false;
  @observable showResult: boolean = false;
  @observable roundOver: boolean = false;
  @observable roundEndingCounter: number = 5;
  @observable.shallow selectedTile: IRoundTile | null = null;
  @observable roundSimple: IRoundSimple | null = null;
  @observable.shallow roundTiles: IRoundTile[] | null = null;
  @observable loadingRoundInitial = false;
  @observable.shallow roundPlayers: IRoundPlayer[] | null = null;
  @observable mainPlayer: IRoundPlayer | null = null;
  @observable leftPlayer: IRoundPlayer | null = null;
  @observable rightPlayer: IRoundPlayer | null = null;
  @observable topPlayer: IRoundPlayer | null = null;
  @observable roundResults: IRoundResult[] | null = null;
  
  @computed get hasSelfKongAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.SelfKong).length > 0
      : false
  }

  @computed get hasSelfWinAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.SelfWin).length > 0
      : false
  }
  
  @computed get hasChowAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.Chow).length > 0
      : false
  }

  @computed get hasPongAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.Pong).length > 0
      : false
  }

  @computed get hasKongAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.Kong).length > 0
      : false
  }

  @computed get hasWinAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.Win).length > 0
      : false
  }

  @computed get hasGiveUpAction(){
    return this.mainPlayer!.roundPlayerActions    
      ? this.mainPlayer!.roundPlayerActions.filter((a) => a.playerAction === ActionType.GiveUp).length > 0
      : false
  }

  @computed get remainingTiles() {
    return this.roundTiles
      ? this.roundTiles.filter((rt) => rt.owner === null).length
      : null;
  }

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
      ? this.roundTiles.filter(
          (rt) => rt.owner === this.rootStore.userStore.user!.userName
        )
      : null;
  }

  @computed get mainPlayerAliveTiles() {
    return this.roundTiles
      ? this.roundTiles
          .filter(
            (rt) =>
              rt.owner === this.rootStore.userStore.user!.userName &&
              (rt.status === TileStatus.UserActive ||
                rt.status === TileStatus.UserJustPicked)
          )
          .sort(sortTiles)
      : null;
  }

  @computed get mainPlayerActiveTiles() {
    return this.roundTiles
      ? this.roundTiles
          .filter(
            (rt) =>
              rt.owner === this.rootStore.userStore.user!.userName &&
              rt.status === TileStatus.UserActive
          )
          .sort(sortTiles)
      : null;
  }

  @computed get mainPlayerGraveYardTiles() {
    return this.roundTiles
      ? this.roundTiles
          .filter(
            (rt) =>
              rt.owner === this.rootStore.userStore.user?.userName &&
              rt.status === TileStatus.UserGraveyard
          )
          .sort(sortTiles)
      : null;
  }

  @computed get mainPlayerJustPickedTile() {
    return this.roundTiles
      ? this.roundTiles.filter(
          (rt) =>
            rt.owner === this.rootStore.userStore.user?.userName &&
            rt.status === TileStatus.UserJustPicked
        )
      : null;
  }

  @computed get leftPlayerTiles() {
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

  @action openModal = () => {
    runInAction(() => {
      this.showResult = true;
    });
  };


  @action closeModal = () => {
    runInAction(() => {
      this.showResult = false;
    });
  };

  @action loadRound = async (id: string, gameId: string) => {
    let round: IRound;
    this.loadingRoundInitial = true;
    try {
      round = await agent.Rounds.detail(id, gameId);
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
