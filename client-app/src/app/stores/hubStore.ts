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
import { IRoundTile, TileType, TileValue } from "../models/tile";
import RoundStore from "./roundStore";
import GameStore from "./gameStore";

export default class HubStore {
  rootStore: RootStore;
  hubStore: HubStore;
  roundStore: RoundStore;
  gameStore: GameStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    this.hubStore = this.rootStore.hubStore;
    this.roundStore = this.rootStore.roundStore;
    this.gameStore = this.rootStore.gameStore;
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

  addHubConnectionHandler() {
    if (this.hubConnection) {

      this.hubConnection.on("UpdateRound", (round: IRound) => {
        console.log("update round called");
        if(round.isOver && round.roundResults){
          console.log(round.roundResults);  
          runInAction("updating round results", () => {
            this.roundStore.showResult = true;
            this.roundStore.roundResults = round.roundResults;                        
          })
        }
        
        setRoundProps(round, this.rootStore.userStore.user!, this.roundStore);
        //update players
        if (round.updatedRoundPlayers) {
          console.log("there is a new player update");
          round.updatedRoundPlayers.forEach((player) => {
            runInAction("updating players", () => {
              if (player.userName === this.roundStore.leftPlayer?.userName)
                this.roundStore.leftPlayer = player;

              if (player.userName === this.roundStore.rightPlayer?.userName)
                this.roundStore.rightPlayer = player;

              if (player.userName === this.roundStore.topPlayer?.userName)
                this.roundStore.topPlayer = player;

              if (player.userName === this.roundStore.mainPlayer?.userName)
                this.roundStore.mainPlayer = player;
            });
          });
        } else {
          console.log("no updated players");
        }

        //update tiles
        if (round.updatedRoundTiles) {
          console.log("there is a new updated tiles");
          round.updatedRoundTiles.forEach((tile) => {
            let objIndex = this.roundStore.roundTiles!.findIndex(
              (obj) => obj.id === tile.id
            );
            runInAction("updating tile", () => {
              this.roundStore.roundTiles![objIndex] = tile;
            });
          });
        } else {
          console.log("no updated tiles");
        }
        runInAction("updating round", () => {
          this.roundStore.roundSimple = round;
        });
      });

      this.hubConnection.on("UpdateTile", (tiles: IRoundTile[]) => {
        if (this.roundStore.roundTiles) {
          tiles.forEach((tile) => {
            let objIndex = this.roundStore.roundTiles!.findIndex(
              (obj) => obj.id === tile.id
            );
            runInAction("updating tile", () => {
              this.roundStore.roundTiles![objIndex] = tile;
            });
          });
        }
      });

      this.hubConnection.on("RoundStarted", (round: IRound) => {
        runInAction(() => {
          this.roundStore.roundSimple = round;
          this.roundStore.roundTiles = round.roundTiles;
          setRoundProps(round, this.rootStore.userStore.user!, this.roundStore);
        });
        history.push(
          `/games/${this.rootStore.gameStore.game!.id}/rounds/${
            this.roundStore.roundSimple!.id
          }`
        );
      });

      this.hubConnection.on("ReceiveChatMsg", (chatMsg) =>
        runInAction(() => {
          this.gameStore.game!.chatMsgs.push(chatMsg);
        })
      );

      this.hubConnection.on("PlayerDisconnected", (player) =>
        runInAction(() => {
          if (this.gameStore.game) {
            this.gameStore.game.players = this.gameStore.game.players.filter(
              (p) => p.userName !== player?.userName
            );
            if (this.rootStore.userStore.user?.userName === player.userName) {
              this.gameStore.game.initialSeatWind = WindDirection.Unassigned;
              this.gameStore.game.isCurrentPlayerConnected = false;
            }
            this.gameStore.gameRegistry.set(
              this.gameStore.game.id,
              this.gameStore.game
            );
            if (this.rootStore.userStore.user?.userName !== player.userName)
              toast.info(`${player.userName} has left the Game`);
          }
        })
      );

      this.hubConnection.on("PlayerConnected", (player) =>
        runInAction(() => {
          if (this.gameStore.game) {
            this.gameStore.game.players.push(player);
            if (this.rootStore.userStore.user?.userName === player.userName) {
              this.gameStore.game.initialSeatWind = player.initialSeatWind;
              this.gameStore.game.isCurrentPlayerConnected = true;
            }

            this.gameStore.gameRegistry.set(
              this.gameStore.game.id,
              this.gameStore.game
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
      this.addHubConnectionHandler();
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
          this.loading = false;
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
    values.gameId = this.gameStore.game!.id;
    try {
      this.hubConnection!.invoke("SendChatMsg", values).catch((e) => {
        toast.error("Unable to send chat message");
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action connectToGame = async (seat: WindDirection) => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
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
    values.gameId = this.gameStore.game!.id;
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

  @action endRound = async () => {
    this.loading = true;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        console.log('is hub connected and calling End Round')
        this.hubConnection!.invoke("EndRound", this.getGameAndRoundProps());
        this.loading = false;
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem calling End Round");
    }
  };

  @action nextRound = async () => {};

  @action startRound = async () => {
    let values: any = {};
    values.gameId = parseInt(this.gameStore.game!.id);
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

  @action winRound = async() => {
    this.loading = true;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        console.log('is hub connected and calling win')
        this.hubConnection!.invoke("WinRound", this.getGameAndRoundProps());
        this.loading = false;
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem calling win");
    }
  };

  @action throwTile = async () => {
    let values = this.getGameAndRoundProps();
    values.tileId = this.roundStore.selectedTile?.id;
    runInAction(() => {
      this.loading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        runInAction("throw tile initially", () => {
          const tileToThrow = this.roundStore.selectedTile;
          if (tileToThrow) {
            var tileInStore = this.roundStore.roundTiles?.find(
              (t) => t.id === tileToThrow?.id
            );
            if (tileInStore) tileInStore.owner = "board";
          }
        });
        this.hubConnection!.invoke("ThrowTile", values);
        runInAction(() => {
          this.loading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem throwing tile");
    }
  };

  @action throwAllTile = async () => {
    let values = this.getGameAndRoundProps();
    runInAction(() => {
      this.loading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection!.invoke("ThrowAllTiles", values);
        runInAction(() => {
          this.loading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem throwing all tile");
    }
  }

  @action pickTile = async () => {
    runInAction(() => {
      this.loading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection!.invoke("PickTile", this.getGameAndRoundProps());
        runInAction(() => {
          this.loading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem picking tile");
    }
  };

  @action pong = async() => {
    runInAction(() => {
      this.loading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection!.invoke('PongTile', this.getGameAndRoundProps())
        .catch(err => {
          toast.error(`can't pong`);
        });
        runInAction(() => {
          this.loading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem invoking pong to hub ");
    }
  }

  @action chow = async(tiles: any[]) =>{

    let values = this.getGameAndRoundProps();
    values.ChowTiles = tiles;

    runInAction(() => {
      this.loading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection!.invoke('ChowTile', values);
        runInAction(() => {
          this.loading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem invoking chow to hub ");
    }
  }

  @action kong = async(tileType: TileType, tileValue: TileValue) => {
    let values = this.getGameAndRoundProps();
    runInAction(() => {
      this.loading = true;
    });
    values.TileType = tileType;
    values.TileValue = tileValue;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection!.invoke('KongTile', values);
        runInAction(() => {
          this.loading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("problem invoking kong to hub ");
    }
  }

  @action win = async() => {
    try {
      this.loading = true;
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection
        .invoke("Win", this.getGameAndRoundProps())
        .catch((e) => {
          toast.error("Can't win");
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      toast.error("problem calling win");
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }
  getGameAndRoundProps = () => {
    let values: any = {};
    values.gameId = this.gameStore.game?.id;
    values.roundId = this.roundStore.roundSimple?.id;
    return values;
  };
}
