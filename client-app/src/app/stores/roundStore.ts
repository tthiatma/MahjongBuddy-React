import { observable, action, runInAction, computed, reaction, IReactionDisposer } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import {sortTiles, sortByActiveCounter } from "../common/util/util";
import {
  IRound,
} from "../models/round";
import { IRoundTile } from "../models/tile";
import { TileStatus } from "../models/tileStatus";
import { ActionType } from "../models/actionTypeEnum";
import { SeatOrientation } from "../models/seatOrientationEnum";

const pickDefaultCounter:number = 3;

export default class RoundStore {
  rootStore: RootStore;
  pickReaction: IReactionDisposer;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;

    reaction(
      () => this.round?.isOver,
      () => {
        if(this.round?.isOver && !rootStore.gameStore.gameIsOver){
          this.showResult = true;
        }
      }
    )

    //reaction to automatically call round tied
    //when 1 tile left and the person's turn give up
    //when no tile left
    reaction(
      () => this.round?.isEnding && this.roundEndingCounter,
      () => {
        if(this.round?.isEnding){
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

  @observable round: IRound | null = null;
  @observable pickCounter: number = pickDefaultCounter;
  @observable canPick: boolean = false;
  @observable isMyTurn: boolean = false;
  @observable isManualSort: boolean = false;
  @observable showResult: boolean = false;
  @observable showRules: boolean = false;
  @observable roundOver: boolean = false;
  @observable roundEndingCounter: number = 5;
  @observable.shallow selectedTile: IRoundTile | null = null;
  @observable loadingRoundInitial = false;

  @computed get mainPlayer(){
    return this.round?.mainPlayer;
  }

  @computed get leftPlayer(){
    return this.round?.otherPlayers.filter(p => p.seatOrientation === SeatOrientation.Left)[0];
  }

  @computed get rightPlayer(){
    return this.round?.otherPlayers.filter(p => p.seatOrientation === SeatOrientation.Right)[0];
  }

  @computed get topPlayer(){
    return this.round?.otherPlayers.filter(p => p.seatOrientation === SeatOrientation.Top)[0];
  }

  @computed get hasSelfKongAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.SelfKong).length > 0
      : false
  }

  @computed get hasSelfWinAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.SelfWin).length > 0
      : false
  }
  
  @computed get hasChowAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.Chow).length > 0
      : false
  }

  @computed get hasPongAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.Pong).length > 0
      : false
  }

  @computed get hasKongAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.Kong).length > 0
      : false
  }

  @computed get hasWinAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.Win).length > 0
      : false
  }

  @computed get hasGiveUpAction(){
    return this.mainPlayer!.roundPlayerActiveActions    
      ? this.mainPlayer!.roundPlayerActiveActions.filter((a) => a.actionType === ActionType.GiveUp).length > 0
      : false
  }

  @computed get remainingTiles() {
    return this.round?.remainingTiles;
  }

  @computed get boardActiveTile() {
    return this.round?.boardTiles
      ? this.round?.boardTiles.find((rt) => rt.status === TileStatus.BoardActive)
      : null;
  }

  @computed get boardGraveyardTiles() {
    return this.round?.boardTiles
      ? this.round?.boardTiles.filter((rt) => rt.status === TileStatus.BoardGraveyard)
      : null;
  }

  @computed get mainPlayerAliveTiles() {
    return this.mainPlayer
      ? this.mainPlayer.playerTiles
          .filter(
            (rt) =>              
              (rt.status === TileStatus.UserActive ||
                rt.status === TileStatus.UserJustPicked)
          )
          .sort(sortByActiveCounter)
      : null;
  }

  @computed get mainPlayerActiveTiles() {
    return this.mainPlayer
      ? this.mainPlayer.playerTiles
          .filter(
            (rt) =>
              rt.status === TileStatus.UserActive
          )
          .sort(sortByActiveCounter)
      : null;
  }

  @computed get mainPlayerGraveYardTiles() {
    return this.mainPlayer
      ? this.mainPlayer.playerTiles
          .filter(
            (rt) =>
              rt.status === TileStatus.UserGraveyard
          )
          .sort(sortTiles)
      : null;
  }

  @computed get mainPlayerJustPickedTile() {
    return this.mainPlayer
      ? this.mainPlayer.playerTiles.filter(
          (rt) =>
            rt.status === TileStatus.UserJustPicked
        )
      : null;
  }

  @action openRulesModal = () => {
    runInAction(() => {
      this.showRules = true;
    });
  }

  @action openResultModal = () => {
    runInAction(() => {
      this.showResult = true;
    });
  };

  @action closeRulesModal = () => {
    runInAction(() => {
      this.showRules = false;
    });
  };

  @action closeResultModal = () => {
    runInAction(() => {
      this.showResult = false;
    });
  };

  @action loadRound = async (roundCounter: string, gameCode: string) => {
    let round: IRound;
    this.loadingRoundInitial = true;
    const currentUserName = this.rootStore.userStore.user!.userName;
    try {
      round = await agent.Rounds.detail(roundCounter, gameCode, currentUserName);
      runInAction("getting round", () => {
        this.round = round;
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
