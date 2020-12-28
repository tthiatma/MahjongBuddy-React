import {
  observable,
  action,
  computed,
  runInAction,
  toJS,
  reaction,
} from "mobx";
import { SyntheticEvent } from "react";
import { IGame } from "../models/game";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";
import { toast } from "react-toastify";
import { setGameProps } from "../common/util/util";
import { IRound } from "../models/round";
import { GameStatus } from "../models/gameStatusEnum";

const LIMIT = 10;

export default class GameStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;

    reaction(
      () => this.predicate.keys(),
      () => {
        this.page = 0;
        this.gameRegistry.clear();
        this.loadGames();
      }
    );
  }

  @observable gameRegistry = new Map();
  @observable game: IGame | null = null;
  @observable loadingGameInitial = false;
  @observable loadingLatestRoundInitial = false;
  @observable submitting = false;
  @observable target = "";
  @observable gameCount = 0;
  @observable page = 0;
  @observable latestRound: IRound | null = null;
  @observable predicate = new Map();

  @action setPredicate = (predicate: string, value: string | Date) => {
    this.predicate.clear();
    if (predicate !== "all") {
      this.predicate.set(predicate, value);
    }
  };

  @computed get axiosParams() {
    const params = new URLSearchParams();
    params.append("limit", String(LIMIT));
    params.append("offset", `${this.page ? this.page * LIMIT : 0}`);
    this.predicate.forEach((value, key) => {
      if (key === "startDate") {
        params.append(key, value.toISOString());
      } else {
        params.append(key, value);
      }
    });
    return params;
  }

  @computed get gamesGroupByDate() {
    return this.groupGamesByDate(Array.from(this.gameRegistry.values()));
  }

  @computed get gamesByDate() {
    return this.gamesByDateDesc(Array.from(this.gameRegistry.values()));
  }

  @computed get getMainUser() {
    return this.game && this.game.players
      ? this.game.players.find(
          (p) => p.userName === this.rootStore.userStore.user!.userName
        )
      : null;
  }

  @computed get gameIsOver() {
    let gameOver: boolean = false;

    if (this.game?.status === GameStatus.Over) gameOver = true;
    if (this.game?.status === GameStatus.OverPrematurely) gameOver = true;

    return gameOver;
  }

  @computed get userNoWind() {
    return this.game
      ? this.game.players.some(
          (p) => p.initialSeatWind === null || p.initialSeatWind === undefined
        )
      : false;
  }

  @action loadGames = async () => {
    this.loadingGameInitial = true;
    try {
      const gamesEnvelope = await agent.Games.list(this.axiosParams);
      const { games, gameCount } = gamesEnvelope;

      runInAction("loading games", () => {
        games.forEach((game) => {
          setGameProps(game, this.rootStore.userStore.user!);
          this.gameRegistry.set(game.id, game);
        });
        this.gameCount = gameCount;
        this.loadingGameInitial = false;
      });
    } catch (error) {
      runInAction("load games error", () => {
        this.loadingGameInitial = false;
      });
      console.log(error);
    }
  };

  @action getLatestRound = async (code: string) => {
    this.loadingLatestRoundInitial = true;
    try {
      const latestRound = await agent.Games.latestRound(code);
      runInAction("getting latest round", () => {
        this.latestRound = latestRound;
        this.loadingLatestRoundInitial = false;
      });
      return latestRound;
    } catch (error) {
      runInAction("getting game error", () => {
        this.loadingLatestRoundInitial = false;
      });
      console.log(error);
    }
  };

  @action loadGame = async (code: string) => {
    let game = this.getGame(code);
    if (game) {
      this.game = game;
      return toJS(game);
    } else {
      this.loadingGameInitial = true;
      try {
        game = await agent.Games.detailByCode(code);
        runInAction("getting game", () => {
          setGameProps(game, this.rootStore.userStore.user!);
          this.game = game;
          this.gameRegistry.set(game.id, game);
          this.loadingGameInitial = false;
        });
        return game;
      } catch (error) {
        runInAction("getting game error", () => {
          this.loadingGameInitial = false;
          history.push(`/`);
        });
      }
    }
  };

  @action joinGameByCode = async (gameCode: string) => {
    history.push(`/games/${gameCode}`);
    this.rootStore.modalStore.closeModal();
  };

  @action createGame = async (game: IGame) => {
    this.submitting = true;
    try {
      var newGame: IGame = await agent.Games.create(game);
      runInAction("creating games", () => {
        this.gameRegistry.set(newGame.id, newGame);
        this.game = newGame;
        //when creating a new game, there will be one user in the game
        var player = newGame.players[0];
        if (this.game) {
          if (this.rootStore.userStore.user?.userName === player.userName) {
            this.game.initialSeatWind = player.initialSeatWind;
            this.game.isCurrentPlayerConnected = true;
          }
        }

        this.submitting = false;
      });
      history.push(`/games/${newGame.id}`);
    } catch (error) {
      runInAction("creating game error", () => {
        this.submitting = false;
      });
      toast.error("Problem submitting data");
      console.log(error.response);
    }
  };

  @action editGame = async (game: IGame) => {
    this.submitting = true;
    try {
      await agent.Games.update(game);
      runInAction("editing game", () => {
        this.gameRegistry.set(game.id, game);
        this.game = game;
        this.submitting = false;
        history.push(`/games/${game.id}`);
      });
    } catch (error) {
      runInAction("editing game error", () => {
        this.submitting = false;
      });
      console.log(error);
    }
  };

  @action deleteGame = async (
    event: SyntheticEvent<HTMLButtonElement>,
    id: string
  ) => {
    this.submitting = true;
    this.target = event.currentTarget.name;
    try {
      await agent.Games.delete(id);
      runInAction("deleting game", () => {
        this.gameRegistry.delete(id);
        this.submitting = false;
        this.target = "";
      });
    } catch (error) {
      runInAction("deleting game error", () => {
        this.submitting = false;
        this.target = "";
      });
      console.log(error);
    }
  };

  getGame = (id: string) => {
    return this.gameRegistry.get(id);
  };

  gamesByDateDesc(games: IGame[]){
    const sortedGames: IGame[] = games.sort(
      (a, b) => b.date.getTime() - a.date.getTime()
    );
    return sortedGames;
  }

  groupGamesByDate(games: IGame[]) {
    const sortedGames = games.sort(
      (a, b) => b.date.getTime() - a.date.getTime()
    );
    return Object.entries(
      sortedGames.reduce((games, game) => {
        const date = game.date.toISOString().split("T")[0];
        games[date] = games[date] ? [...games[date], game] : [game];
        return games;
      }, {} as { [key: string]: IGame[] })
    );
  }
}
