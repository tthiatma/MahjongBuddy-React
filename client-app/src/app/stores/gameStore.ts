import { observable, action, computed, runInAction, toJS } from "mobx";
import { SyntheticEvent } from "react";
import { IGame } from "../models/game";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from '../..';
import { toast } from 'react-toastify';
import { setGameProps } from "../common/util/util";
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

  @computed get gamesByDate() {
    return this.groupGamesByDate( Array.from(this.gameRegistry.values()));
  };

  //******************Start Signal R*********************
  @action createHubConnection = (gameId: string) => {
    if(!this.hubConnection){
      this.hubConnection = new HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_API_GAME_HUB_URL!, {
        accessTokenFactory: () => this.rootStore.commonStore.token!
      })
      .configureLogging(LogLevel.Information)
      .build();

      this.hubConnection.on("ReceiveChatMsg", chatMsg =>
        runInAction(() => {
          this.game!.chatMsgs.push(chatMsg);
        })
      );  

      this.hubConnection.on("PlayerDisconnected", (player) =>
        runInAction(() => {
          if (this.game) {
            this.game.players = this.game.players.filter(
              (p) => p.userName !== player?.userName
            );
            this.game.isCurrentPlayerConnected = this.rootStore.userStore.user?.userName !== player.userName;
            this.gameRegistry.set(this.game.id, this.game);
            if(this.rootStore.userStore.user?.userName !== player.userName)
            toast.info(`${player.userName} has left the Game`);
          }
        })
      );

      this.hubConnection.on("PlayerConnected", player =>
        runInAction(() => {
          if(this.game){
            this.game.players.push(player);
            this.game.isCurrentPlayerConnected = this.rootStore.userStore.user?.userName === player.userName;
            this.gameRegistry.set(this.game.id, this.game);
            if(this.rootStore.userStore.user?.userName !== player.userName)
              toast.info(`${player.userName} has joined the Game`);
          }
        })
      );

      this.hubConnection.on("Send", message => {
        toast.info(message);
      });  
    }  
      if (this.hubConnection!.state === 'Disconnected')
      {
        this.hubConnection
        .start()
        .then(() => {
          if (this.hubConnection!.state === 'Connected')
            this.hubConnection?.invoke('AddToGroup', gameId)
          })
        .catch(error => console.log("Error establishing connection", error));  
      }    
  };

  @action stopHubConnection = () => {
    if (this.hubConnection!.state === 'Connected'){
      this.hubConnection!.invoke("RemoveFromGroup", this.game!.id)
      .then(() => {
        this.hubConnection!.stop()
        .then(() => {
          console.log(`Connection State = ${this.hubConnection!.state}`)          
        });
      })
      .catch(error => console.log(error));
    }
  };

  @action addChatMsg = async (values: any) => {
    values.gameId = this.game!.id;
    try{
      this.hubConnection!.invoke("SendChatMsg", values);
    }catch (error){
      console.log(error);
    }
  };

  @action connectToGame = async () => {
    let values: any = {};
    values.gameId = this.game!.id;
    runInAction(() => {
      this.loading = true;
    })
    try{
      this.hubConnection!.invoke("ConnectToGame", values);
      runInAction(() =>{
        this.loading = false
      })
    }catch(error){
      runInAction(() =>{
        this.loading = false
      })
      toast.error('problem connecting to game');
    }
  };

  @action disconnectFromGame = async () => {
    let values: any = {};
    values.gameId = this.game!.id;
    runInAction(() => {
      this.loading = true;
    })
    try{
      this.hubConnection!.invoke("DisconnectFromGame", values);
      runInAction(() => {
        this.loading = false;
      })
    }catch(error){
      runInAction(() =>{
        this.loading = false
      })
      toast.error('problem disconnecting from game');
    }
  };
  //******************End Signal R***********************
  

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

  @action loadGame = async (id: string) => {
    let game = this.getGame(id);
    if (game) {
      this.game = game;
      return toJS(game);
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

  @action createGame = async (game: IGame) => {
    this.submitting = true;
    try {
      var newGame : IGame = await agent.Games.create(game);
      runInAction("creating games", () => {
        this.gameRegistry.set(newGame.id, newGame);
        this.submitting = false;
      });
      history.push(`/lobby/${newGame.id}`)
    } catch (error) {
      runInAction("creating game error", () => {
        this.submitting = false;
      });
      toast.error('Problem submitting data');
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

  groupGamesByDate(games:IGame[]) {
    const sortedGames = games.sort(
      (a, b) => a.date.getTime() - b.date.getTime()
    )
    return Object.entries(sortedGames.reduce((games, game) => {
      const date = game.date.toISOString().split('T')[0];
      games[date] = games[date] ? [...games[date], game] : [game];
      return games
    }, {} as {[key: string] : IGame[]}));
  };
}