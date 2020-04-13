import { observable, action, runInAction } from "mobx";
import { RootStore } from "./rootStore";
import { history } from "../..";
import { toast } from "react-toastify";
import { setRoundProps } from "../common/util/util";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { WindDirection } from "../models/windEnum";
import { IRound } from "../models/round";
import { IRoundTile } from "../models/tile";
import GameStore from "./gameStore";

export default class HubStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable loading = false;
  @observable.ref hubConnection: HubConnection | null = null;

  @action leaveGroup = (gameId: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("RemoveFromGroup", gameId);
  };

  @action joinGroup = (gameId: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("AddToGroup", gameId);
  };

  addHUbConnectionHandler() {
    if (this.hubConnection) {
      this.hubConnection.on("UpdateRound", (round: IRound) => {
        console.log("updating round");
        setRoundProps(round, this.rootStore.userStore.user!);
        runInAction("updating round", () => {
          this.rootStore.roundStore.round = round;
          this.rootStore.roundStore.roundRegistry.set(round.id, round);
        });
      });

      this.hubConnection.on("UpdateTile", (tile: IRoundTile) => {
        if (this.rootStore.roundStore.roundTiles) {
          let objIndex = this.rootStore.roundStore.roundTiles.findIndex(
            (obj) => obj.id == tile.id
          );
          runInAction('updating tile', () => {
            this.rootStore.roundStore.roundTiles![objIndex] = tile;
          })
        }
      });

      this.hubConnection.on("RoundStarted", (round: IRound) => {
        runInAction(() => {
          this.rootStore.roundStore.round = round;
          this.rootStore.roundStore.roundTiles = round.roundTiles;
          setRoundProps(round, this.rootStore.userStore.user!);
          this.rootStore.roundStore.roundRegistry.set(round.id, round);
        });
        history.push(
          `/games/${this.rootStore.gameStore.game!.id}/rounds/${
            this.rootStore.roundStore.round!.id
          }`
        );
      });

      this.hubConnection.on("ReceiveChatMsg", (chatMsg) =>
        runInAction(() => {
          this.rootStore.gameStore.game!.chatMsgs.push(chatMsg);
        })
      );

      this.hubConnection.on("PlayerDisconnected", (player) =>
        runInAction(() => {
          if (this.rootStore.gameStore.game) {
            this.rootStore.gameStore.game.players = this.rootStore.gameStore.game.players.filter(
              (p) => p.userName !== player?.userName
            );
            if (this.rootStore.userStore.user?.userName === player.userName) {
              this.rootStore.gameStore.game.initialSeatWind =
                WindDirection.Unassigned;
              this.rootStore.gameStore.game.isCurrentPlayerConnected = false;
            }
            this.rootStore.gameStore.gameRegistry.set(
              this.rootStore.gameStore.game.id,
              this.rootStore.gameStore.game
            );
            if (this.rootStore.userStore.user?.userName !== player.userName)
              toast.info(`${player.userName} has left the Game`);
          }
        })
      );

      this.hubConnection.on("PlayerConnected", (player) =>
        runInAction(() => {
          if (this.rootStore.gameStore.game) {
            this.rootStore.gameStore.game.players.push(player);
            if (this.rootStore.userStore.user?.userName === player.userName) {
              this.rootStore.gameStore.game.initialSeatWind =
                player.initialSeatWind;
              this.rootStore.gameStore.game.isCurrentPlayerConnected = true;
            }

            this.rootStore.gameStore.gameRegistry.set(
              this.rootStore.gameStore.game.id,
              this.rootStore.gameStore.game
            );
            if (this.rootStore.userStore.user?.userName !== player.userName)
              toast.info(`${player.userName} has joined the Game`);
          }
        })
      );

      this.hubConnection.on("Send", (message) => {
        toast.info(message);
      });
    }
  }

  @action createHubConnection = (gameId: string) => {
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(process.env.REACT_APP_API_GAME_HUB_URL!, {
          accessTokenFactory: () => this.rootStore.commonStore.token!,
        })
        .configureLogging(LogLevel.Information)
        .build();
      this.addHUbConnectionHandler();
    }
    if (this.hubConnection!.state === "Disconnected") {
      runInAction(() => {
        this.loading = true;
      });
      this.hubConnection
        .start()
        .then(() => {
          if (this.hubConnection!.state === "Connected")
            this.hubConnection?.invoke("AddToGroup", gameId);
        })
        .then(() => {
          runInAction(() => {
            this.loading = false;
          });
        })
        .catch((error) => {
          runInAction(() => {
            this.loading = false;
          });
          console.log("Error establishing connection", error);
        });
    } else if (this.hubConnection!.state === "Connected") {
      this.hubConnection?.invoke("AddToGroup", gameId);
    }
  };

  @action stopHubConnection = (gameId: string) => {
    if (this.hubConnection!.state === "Connected") {
      runInAction(() => {
        this.loading = true;
      });
      this.hubConnection!.invoke("RemoveFromGroup", gameId)
        .then(() => {
          this.hubConnection!.stop().then(() => {
            runInAction(() => {
              this.loading = false;
            });
            console.log(`Connection State = ${this.hubConnection!.state}`);
          });
        })
        .catch((error) => {
          runInAction(() => {
            this.loading = false;
          });
          console.log(error);
        });
    }
  };

  @action addChatMsg = async (values: any) => {
    values.gameId = this.rootStore.gameStore.game!.id;
    try {
      this.hubConnection!.invoke("SendChatMsg", values);
    } catch (error) {
      console.log(error);
    }
  };

  @action connectToGame = async (seat: WindDirection) => {
    let values: any = {};
    values.gameId = this.rootStore.gameStore.game!.id;
    values.InitialSeatWind = seat;
    runInAction(() => {
      this.loading = true;
    });
    try {
      this.hubConnection!.invoke("ConnectToGame", values);
      runInAction(() => {
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem connecting to game");
    }
  };

  @action disconnectFromGame = async () => {
    let values: any = {};
    values.gameId = this.rootStore.gameStore.game!.id;
    runInAction(() => {
      this.loading = true;
    });
    try {
      this.hubConnection!.invoke("DisconnectFromGame", values);
      runInAction(() => {
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem disconnecting from game");
    }
  };

  @action nextRound = async () => {};

  @action startRound = async () => {
    let values: any = {};
    values.gameId = parseInt(this.rootStore.gameStore.game!.id);
    runInAction(() => {
      this.loading = true;
    });
    try {
      this.hubConnection!.invoke("StartRound", values);
      runInAction(() => {
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem disconnecting from game");
    }
  };

  @action throwTile = async () => {
    let values: any = {};
    values.gameId = this.rootStore.gameStore.game?.id.toString();
    values.roundId = this.rootStore.roundStore.round?.id;
    values.tileId = this.rootStore.roundStore.selectedTile?.id;
    runInAction(() => {
      this.loading = true;
    });
    try {
      if(this.hubConnection && this.hubConnection.state =="Connected"){
        this.hubConnection!.invoke("ThrowTile", values);
        runInAction(() => {
          this.loading = false;
        });  
      }else{
        toast.error('not connected to hub')
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem throwing tile");
    }
  };
}
