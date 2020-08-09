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
import { IRound, IRoundPlayer } from "../models/round";
import { IRoundTile, TileType, TileValue } from "../models/tile";
import RoundStore from "./roundStore";
import GameStore from "./gameStore";
import { TileStatus } from "../models/tileStatus";

export default class HubStore {
  rootStore: RootStore;
  hubStore: HubStore;
  roundStore: RoundStore;
  gameStore: GameStore;
  cooldownTime: number = 2000;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    this.hubStore = this.rootStore.hubStore;
    this.roundStore = this.rootStore.roundStore;
    this.gameStore = this.rootStore.gameStore;
  }
  @observable hubActionLoading = false;
  @observable hubLoading = false;
  @observable.ref hubConnection: HubConnection | null = null;

  @action leaveGroup = (gameId: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("RemoveFromGroup", gameId);
  };

  @action joinGroup = (gameId: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("AddToGroup", gameId);
  };

  sleep(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  updateRoundTile(objIndex: number, tile: IRoundTile){
    runInAction("updating tile", () => {
      this.roundStore.roundTiles![objIndex] = tile;
    })
  }

  updateRoundPlayer(updatedRoundPlayers: IRoundPlayer[]){
    updatedRoundPlayers.forEach((player) => {
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
  }

  addHubConnectionHandler() {
    if (this.hubConnection) {
      this.hubConnection.on("UpdateRoundNoLag", (round: IRound) => {
        if (round.isOver && round.roundResults) {
          runInAction("updating round results", () => {
            this.roundStore.roundOver = true;
            this.roundStore.roundResults = round.roundResults;
          });
        }
        
        //update tiles
        if (round.updatedRoundTiles) {
          round.updatedRoundTiles.forEach((tile) => {
            let objIndex = this.roundStore.roundTiles!.findIndex(
              (obj) => obj.id === tile.id
            );
            this.updateRoundTile(objIndex, tile);
          });
        }

        //update players
        if (round.updatedRoundPlayers) {
          this.updateRoundPlayer(round!.updatedRoundPlayers!);              
        }

        runInAction("updating round", () => {
          this.roundStore.roundSimple = round;
        });
      });

      this.hubConnection.on("UpdateRound", (round: IRound) => {
        
        runInAction("updating round", () => {
          this.roundStore.roundSimple = round;
        });

        if (round.isOver && round.roundResults) {
          runInAction("updating round results", () => {
            this.roundStore.roundResults = round.roundResults;
          });
        }

        let noAction = false;
        if (round.updatedRoundPlayers){
          noAction = round.updatedRoundPlayers.filter((p) => p.hasAction).length === 0;
        }

        let hasTileSetGroup = false;
        if(round.updatedRoundTiles){
          hasTileSetGroup = round.updatedRoundTiles.filter((t) => t.tileSetGroup !== 0).length > 0;
        }
        
        //update players
        if (round.updatedRoundPlayers) {        
          if ((noAction && !hasTileSetGroup))
          {
            this.sleep(this.cooldownTime).then(() => {
              this.updateRoundPlayer(round!.updatedRoundPlayers!);              
            });
          }
          else
          {
            this.updateRoundPlayer(round!.updatedRoundPlayers!);              
          }
        }

        //update tiles
        if (round.updatedRoundTiles) {
          round.updatedRoundTiles.forEach((tile) => {
            let objIndex = this.roundStore.roundTiles!.findIndex(
              (obj) => obj.id === tile.id
            );

            if(noAction){
              //find just picked tile and give assign it after cooldown time
              if(tile.status === TileStatus.UserJustPicked){
                this.sleep(this.cooldownTime).then(() => {
                  this.updateRoundTile(objIndex, tile);
                })
              }else{
                this.updateRoundTile(objIndex, tile);
              }
            }
            else
            {
              this.updateRoundTile(objIndex, tile);
            }

            runInAction("updating tile", () => {
              
            });
          });
        }
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
          this.roundStore.showResult = false;
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
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();
      this.addHubConnectionHandler();
    }
    if (this.hubConnection!.state === "Disconnected") {
      runInAction(() => {
        this.hubLoading = true;
      });
      this.hubConnection
        .start()
        .then(() => {
          if (this.hubConnection!.state === "Connected")
            this.hubConnection?.invoke("AddToGroup", gameId);
        })
        .then(() => {
          runInAction(() => {
            this.hubLoading = false;
          });
        })
        .catch((error) => {
          runInAction(() => {
            this.hubLoading = false;
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
        this.hubLoading = true;
      });
      this.hubConnection!.invoke("RemoveFromGroup", gameId)
        .then(() => {
          this.hubConnection!.stop().then(() => {
            runInAction(() => {
              this.hubLoading = false;
            });
            console.log(`Connection State = ${this.hubConnection!.state}`);
          });
        })
        .catch((error) => {
          runInAction(() => {
            this.hubLoading = false;
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
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("ConnectToGame", values);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem connecting to game");
    }
  };

  @action disconnectFromGame = async () => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("DisconnectFromGame", values);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem disconnecting from game");
    }
  };

  @action tiedRound = async () => {
    this.hubActionLoading = true;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        console.log("is hub connected and calling Tied Round");
        this.hubConnection!.invoke("TiedRound", this.getGameAndRoundProps());
        runInAction(() => {
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem calling TiedRound");
    }
  };

  @action endingRound = async () => {
    this.hubActionLoading = true;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        console.log("is hub connected and calling Ending Round");
        this.hubConnection!.invoke("EndingRound", this.getGameAndRoundProps());
        runInAction(() => {
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem calling EndingRound");
    }
  };

  @action startRound = async () => {
    let values: any = {};
    values.gameId = parseInt(this.gameStore.game!.id);
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("StartRound", values);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem disconnecting from game");
    }
  };

  @action winRound = async () => {
    this.hubActionLoading = true;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke(
          "WinRound",
          this.getGameAndRoundProps()
        ).catch((err) => {
          toast.error(`can't win`);
        });
        runInAction(() => {
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem calling win");
    }
  };

  @action throwTile = async () => {
    let values = this.getGameAndRoundProps();
    values.tileId = this.roundStore.selectedTile?.id;
    runInAction(() => {
      this.hubActionLoading = true;
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
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem throwing tile");
    }
  };

  @action throwAllTile = async () => {
    let values = this.getGameAndRoundProps();
    runInAction(() => {
      this.hubActionLoading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        this.hubConnection!.invoke("ThrowAllTiles", values);
        runInAction(() => {
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem throwing all tile");
    }
  };

  @action pickTile = async () => {
    runInAction(() => {
      this.hubActionLoading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke("PickTile", this.getGameAndRoundProps());
        runInAction(() => {
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem picking tile");
    }
  };

  @action pong = async () => {
    runInAction(() => {
      this.hubActionLoading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke(
          "PongTile",
          this.getGameAndRoundProps()
        ).catch((err) => {
          toast.error(`can't pong`);
        });
        runInAction(() => {
          this.hubActionLoading = false;
          this.roundStore.pickCounter = 0;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      console.log(error);
      toast.error("problem invoking pong to hub ");
    }
  };

  @action chow = async (tiles: any[]) => {
    let values = this.getGameAndRoundProps();
    values.ChowTiles = tiles;

    runInAction(() => {
      this.hubActionLoading = true;
    });

    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke("ChowTile", values).catch((err) => {
          toast.error(`can't chow`);
        });
        runInAction(() => {
          this.hubActionLoading = false;
          this.roundStore.pickCounter = 0;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem invoking chow to hub ");
    }
  };

  @action kong = async (tileType: TileType, tileValue: TileValue) => {
    let values = this.getGameAndRoundProps();
    runInAction(() => {
      this.hubActionLoading = true;
    });
    values.TileType = tileType;
    values.TileValue = tileValue;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke("KongTile", values).catch((err) => {
          toast.error(`can't kong`);
        });
        runInAction(() => {
          this.hubActionLoading = false;
          this.roundStore.pickCounter = 0;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.hubActionLoading = false;
      });
      toast.error("problem invoking kong to hub ");
    }
  };

  @action skipAction = async () => {
    runInAction(() => {
      this.hubActionLoading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke(
          "SkipAction",
          this.getGameAndRoundProps()
        ).catch((err) => {
          runInAction(() => {
            this.roundStore.mainPlayer!.hasAction = true;
            this.hubActionLoading = false;  
          })  
          toast.error(`can't skip`);
        })
        runInAction(() => {
          this.roundStore.mainPlayer!.hasAction = false;
          this.hubActionLoading = false;  
        })
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.roundStore.mainPlayer!.hasAction = true;
        this.hubActionLoading = false;
      });
      toast.error("problem skipping action");
    }
  };

  getGameAndRoundProps = () => {
    let values: any = {};
    values.gameId = this.gameStore.game?.id;
    values.roundId = this.roundStore.roundSimple?.id;
    return values;
  };
}
