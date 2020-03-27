import { observable, action, computed, runInAction } from "mobx";
import { SyntheticEvent } from "react";
import { IGame } from "../models/game";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from '../..';
import { toast } from 'react-toastify';

export default class GameStore {

  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable gameRegistry = new Map();
  @observable game: IGame | null = null;
  @observable loadingInitial = false;
  @observable submitting = false;
  @observable target = "";

  @computed get gamesByDate() {
    return this.groupGamesByDate( Array.from(this.gameRegistry.values()));
  }

  groupGamesByDate(games:IGame[]) {
    const sortedGames = games.sort(
      (a, b) => a.date.getTime() - b.date.getTime()
    )
    return Object.entries(sortedGames.reduce((games, game) => {
      const date = game.date.toISOString().split('T')[0];
      games[date] = games[date] ? [...games[date], game] : [game];
      return games
    }, {} as {[key: string] : IGame[]}));
  }

  @action clearGame = () => {
    this.game = null;
  };

  getGame = (id: string) => {
    return this.gameRegistry.get(id);
  };

  @action editGame = async (game: IGame) => {
    this.submitting = true;
    try {
      await agent.Games.update(game);
      runInAction("editing game", () => {
        this.gameRegistry.set(game.id, game);
        this.game = game;
        this.submitting = false;
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

  @action createGame = async (game: IGame) => {
    this.submitting = true;
    try {
      await agent.Games.create(game);
      runInAction("creating games", () => {
        this.gameRegistry.set(game.id, game);
        this.submitting = false;
      });
      history.push(`/lobby/${game.id}`)
    } catch (error) {
      runInAction("creating game error", () => {
        this.submitting = false;
      });
      toast.error('Problem submitting data');
      console.log(error.response);
    }
  };

  @action loadGame = async (id: string) => {
    let game = this.getGame(id);
    if (game) {
      this.game = game;
      return game;
    } else {
      this.loadingInitial = true;
      try {
        game = await agent.Games.detail(id);
        runInAction("getting game", () => {
          game.date = new Date(game.date);
          this.game = game;
          this.gameRegistry.set(game.id, game);
          this.loadingInitial = false;
        });
        return game;
      } catch (error) {
        runInAction("getting game error", () => {
          this.loadingInitial = false;
        });
        console.log(error);
      }
    }
  };

  @action loadGames = async () => {
    this.loadingInitial = true;
    try {
      const games = await agent.Games.list();
      runInAction("loading games", () => {
        games.forEach(game => {
          game.date = new Date(game.date);
          this.gameRegistry.set(game.id, game);
        });
        this.loadingInitial = false;
      });
    } catch (error) {
      runInAction("load games error", () => {
        this.loadingInitial = false;
      });
      console.log(error);
    }
  };
}