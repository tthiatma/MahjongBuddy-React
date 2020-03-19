import { observable, action, computed, configure, runInAction } from "mobx";
import { createContext, SyntheticEvent } from "react";
import { IGame } from "../models/game";
import agent from "../api/agent";

configure({ enforceActions: "always" });

export class GameStore {
  @observable gameRegistry = new Map();
  @observable loadingInitial = false;
  @observable game: IGame | null = null;
  @observable submitting = false;
  @observable target = "";

  @computed get gamesByDate() {
    return this.groupGamesByDate( Array.from(this.gameRegistry.values()));
  }

  groupGamesByDate(games:IGame[]) {
    const sortedGames = games.sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    )
    return Object.entries(sortedGames.reduce((games, game) => {
      const date = game.date.split('T')[0];
      games[date] = games[date] ? [...games[date], game] : [game];
      return games
    }, {} as {[key: string] : IGame[]}));
  }

  @action clearGame = () => {
    this.game = null;
  };

  @action loadGame = async (id: string) => {
    let game = this.getGame(id);
    if (game) {
      this.game = game;
    } else {
      this.loadingInitial = true;
      try {
        game = await agent.Games.detail(id);
        runInAction("getting game", () => {
          this.game = game;
          this.loadingInitial = false;
        });
      } catch (error) {
        runInAction("getting game error", () => {
          this.loadingInitial = false;
        });
        console.log(error);
      }
    }
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
    } catch (error) {
      runInAction("creating game error", () => {
        this.submitting = false;
      });
      console.log(error);
    }
  };

  @action loadGames = async () => {
    this.loadingInitial = true;
    try {
      const games = await agent.Games.list();
      runInAction("loading games", () => {
        games.forEach(game => {
          game.date = game.date.split(".")[0];
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

export default createContext(new GameStore());
