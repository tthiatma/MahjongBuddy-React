import { observable, action, computed, runInAction } from "mobx";
import { SyntheticEvent } from "react";
import { IGame } from "../models/game";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from '../..';
import { toast } from 'react-toastify';
import { setGameProps, createPlayer } from "../common/util/util";
import {HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';

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
  @observable loading = false;
  @observable.ref hubConnection: HubConnection | null = null;

  @action createHubConnection = (gameId: string) => {
    if(!this.hubConnection){
      this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/game', {
        accessTokenFactory: () => this.rootStore.commonStore.token!
      })
      .configureLogging(LogLevel.Information)
      .build();

      this.hubConnection.on("ReceiveChatMsg", chatMsg =>
        runInAction(() => {
          this.game!.chatMsgs.push(chatMsg);
        })
      );  
    
      this.hubConnection.on("Send", message => {
        console.log(`send is called with message ${message}`);
        toast.info(message);
      });  
    }  
      console.log(`Initial Connection State = ${this.hubConnection!.state}`)
      if (this.hubConnection!.state === 'Disconnected')
      {
        console.log(`about to start hub connection`);
        this.hubConnection
        .start()
        .then(() => console.log(`Connection State = ${this.hubConnection!.state}`))
        .then(() => {
          console.log(`attempting to join group ${gameId}`);
          if (this.hubConnection!.state === 'Connected')
            this.hubConnection?.invoke('AddToGroup', gameId)
            .then(() => console.log(`Added to group`));
          })
        .catch(error => console.log("Error establishing connection", error));  
      }    
  }

  @action stopHubConnection = () => {
    if (this.hubConnection!.state === 'Connected'){
      this.hubConnection!.invoke("RemoveFromGroup", this.game!.id)
      .then(() => {
        console.log(`Connection State = ${this.hubConnection!.state}`)
        console.log(`Calling hubConnection.stop`)
        this.hubConnection!.stop()
        .then(() => {
          console.log(`Connection State = ${this.hubConnection!.state}`)          
        });
      })
      .then(() => {
        console.log(`Connection State = ${this.hubConnection!.state}`)          
      })
      .catch(error => console.log(error));
    }
  }

  @action addChatMsg = async (values: any) => {
    values.gameId = this.game!.id;
    try{
      this.hubConnection!.invoke("SendChatMsg", values);
    }catch (error){
      console.log(error);
    }
  }

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
      const player = createPlayer(this.rootStore.userStore.user!);
      player.isHost = true;
      let players = [];
      players.push(player);
      game.players = players;
      game.chatMsgs = [];
      game.isHost = true;
      game.isConnected = true;
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
          setGameProps(game, this.rootStore.userStore.user!);
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
          setGameProps(game, this.rootStore.userStore.user!);
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

  @action connectToGame = async () => {
    const player = createPlayer(this.rootStore.userStore.user!);
    this.loading = true;
    try{
      await agent.Games.connect(this.game!.id);
      runInAction(() => {
        if(this.game){
          this.game.players.push(player);
          this.game.isConnected = true;
          this.gameRegistry.set(this.game.id, this.game);
        }
        this.loading = false;
      })
    }catch(error){
      runInAction(() =>{
        this.loading = false
      })
      toast.error('problem connecting to game');
    }
  }

  @action disconnectFromGame = async () => {
    this.loading = true;
    try{
      await agent.Games.disconnect(this.game!.id);
      runInAction(() => {
        if(this.game){
          this.game.players = this.game.players.filter(p => p.userName !== this.rootStore.userStore.user?.userName)
          this.game.isConnected = false;
          this.gameRegistry.set(this.game.id, this.game);
        }
        this.loading = false;
      })
    }catch(error){
      runInAction(() =>{
        this.loading = false
      })
      toast.error('problem disconnecting from game');
    }
  }
}