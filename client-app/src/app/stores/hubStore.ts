import { observable, action, runInAction } from "mobx";
import { RootStore } from "./rootStore";
import { history } from "../..";
import { toast } from "react-toastify";
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
import jwt from "jsonwebtoken";
import agent from "../api/agent";
import UserStore from "./userStore";
import { IPlayer, IRoundOtherPlayer } from "../models/player";

export default class HubStore {
  rootStore: RootStore;
  hubStore: HubStore;
  roundStore: RoundStore;
  gameStore: GameStore;
  userStore: UserStore;
  cooldownTime: number = 2800;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    this.hubStore = this.rootStore.hubStore;
    this.roundStore = this.rootStore.roundStore;
    this.gameStore = this.rootStore.gameStore;
    this.userStore = this.rootStore.userStore;
  }

  @observable hubActionLoading: boolean = false;
  @observable hubLoading: boolean = false;
  @observable.ref hubConnection: HubConnection | null = null;

  sleep(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  updateOtherPlayers(roundOtherPlayers: IRoundOtherPlayer[]) {
    roundOtherPlayers.forEach(() => {
      runInAction("updating other players", () => {
        this.roundStore.round!.otherPlayers = roundOtherPlayers;
      });
    });
  }

  checkTokenAndRefreshIfExpired = async () => {
    const token = localStorage.getItem("jwt");
    const refreshToken = localStorage.getItem("refreshToken");
    if (token && refreshToken) {
      const decodedToken: any = jwt.decode(token);
      if (decodedToken && Date.now() >= decodedToken.exp * 1000 - 5000) {
        try {
          return await agent.User.refreshToken(token, refreshToken);
        } catch (error) {
          toast.error("Problem connecting to the game");
        }
      } else {
        return token;
      }
    }
  };

  addHubConnectionHandler() {
    if (this.hubConnection) {
      this.hubConnection.on("UpdatePlayersWind", (players: IPlayer[]) => {
        runInAction(() => {
          players.forEach((p) => {
            let curPlayer = this.rootStore.gameStore.game?.players.find(
              (x) => x.userName === p.userName
            );
            if (curPlayer) {
              curPlayer.initialSeatWind = p.initialSeatWind;

              if (curPlayer.userName === this.userStore.user?.userName)
                this.rootStore.gameStore.game!.initialSeatWind =
                  p.initialSeatWind;
            }
          });
        });
      });

      this.hubConnection.on("PlayerStoodUp", (player: IPlayer) => {
        runInAction(() => {
          let curPlayer = this.rootStore.gameStore.game?.players.find(
            (x) => x.userName === player.userName
          );
          if (curPlayer) curPlayer.initialSeatWind = undefined;
        });
      });

      this.hubConnection.on("PlayerSat", (player: IPlayer) => {
        runInAction(() => {
          let curPlayer = this.rootStore.gameStore.game?.players.find(
            (x) => x.userName === player.userName
          );
          if (curPlayer) curPlayer.initialSeatWind = player.initialSeatWind;
        });
      });

      // this.hubConnection.on("LoadRound", (round: IRound) => {
      //   runInAction("getting round", () => {
      //     setRoundProps(round, this.rootStore.userStore.user!, this.roundStore);
      //     this.roundStore.roundSimple = round;
      //     this.roundStore.roundTiles = round.roundTiles;
      //     this.roundStore.loadingRoundInitial = false;
      //   });
      // });

      this.hubConnection.on("UpdateRoundNoLag", (round: IRound) => {
        runInAction("updating round", () => {
          this.roundStore.round = round;
        });
      });

      this.hubConnection.on("UpdateRound", (round: IRound) => {
        //update board tiles
        runInAction("updating necessary round prop", () => {
          if(this.roundStore.round){
            if(this.roundStore.round.mainPlayer.mustThrow){
              this.roundStore.round.mainPlayer = round.mainPlayer; 
            }else{
              this.roundStore.round.mainPlayer.mustThrow = round.mainPlayer.mustThrow;
              this.roundStore.round.mainPlayer.isManualSort = round.mainPlayer.isManualSort;  
            }

            this.roundStore.round.boardTiles = round.boardTiles;
            this.roundStore.round.otherPlayers.forEach(op => {
              var matchingOp = round.otherPlayers.find(rop => rop.userName === op.userName);
              op.mustThrow = matchingOp!.mustThrow;
            })
          }
        });

        this.sleep(this.cooldownTime).then(() => {
          runInAction(() => {
            this.roundStore.round = round;
          })
        });
      });

      this.hubConnection.on("RoundStarted", (round: IRound) => {
        runInAction(() => {
          this.roundStore.showResult = false;
          this.roundStore.round = round;
        });
        history.push(
          `/games/${this.rootStore.gameStore.game!.id}/rounds/${
            this.roundStore.round!.id
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
              toast.info(`${player.displayName} has left the Game`);
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
              toast.info(`${player.displayName} has joined the Game`);
          }
        })
      );

      this.hubConnection.on("Send", (message) => {
        toast.info(message);
      });
    }
  }

  // @action loadRoundDetail = async (id: string, gameId: string) => {
  //   if (this.hubConnection!.state === "Connected"){
  //     let values: any = {};
  //     values.id = id;
  //     values.gameId = gameId;
  //     this.hubConnection!.invoke("DetailRound", values);
  //   }
  // };

  @action leaveGroup = (gameId: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("RemoveFromGroup", gameId);
  };

  @action joinGroup = (gameId: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("AddToGroup", gameId);
  };

  @action createHubConnection = (gameId: string) => {
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${process.env.REACT_APP_API_GAME_HUB_URL!}/?gid=${gameId}`, {
          accessTokenFactory: () => this.checkTokenAndRefreshIfExpired(),
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Debug)
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

  @action renewConnection = (gameId: string) => {
    if (this.hubConnection && this.hubConnection!.state === "Connected") {
      runInAction(() => {
        this.hubLoading = true;
      });
      this.hubConnection!.invoke("RemoveFromGroup", gameId)
        .then(() => {
          this.hubConnection!.stop().then(() => {
            runInAction(() => {
              this.hubLoading = false;
            });
          });
        })
        .then(() => {
          this.createHubConnection(gameId);
        })
        .catch((error) => {
          runInAction(() => {
            this.hubLoading = false;
          });
          console.log(error);
        });
    }
  };

  @action stopHubConnection = (gameId: string) => {
    if (this.hubConnection && this.hubConnection!.state === "Connected") {
      runInAction(() => {
        this.hubLoading = true;
      });
      this.hubConnection!.invoke("RemoveFromGroup", gameId)
        .then(() => {
          this.hubConnection!.stop().then(() => {
            runInAction(() => {
              this.hubLoading = false;
            });
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
      this.hubConnection!.invoke("SendChatMsg", values).catch(() => {
        toast.error("Unable to send chat message");
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action joinGame = async () => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("JoinGame", values);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem joining to game");
    }
  };

  @action leaveGame = async () => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("LeaveGame", values);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem leaving from game");
    }
  };
  @action sitGame = async (seat: WindDirection) => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
    values.InitialSeatWind = seat;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("SitGame", values);
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

  @action standUpGame = async () => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("StandUpGame", values);
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

  @action randomizeWind = async () => {
    let values: any = {};
    values.gameId = this.gameStore.game!.id;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("RandomizeWind", values);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem randomizing game's wind");
    }
  };

  @action tiedRound = async () => {
    this.hubActionLoading = true;
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
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
        ).catch(() => {
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

  @action orderTiles = async (
    reorderedTiles: IRoundTile[],
    originalTiles: IRoundTile[],
    originalManualSort: boolean
  ) => {
    let values = this.getGameAndRoundProps();
    values.RoundTiles = reorderedTiles;
    values.IsManualSort = this.rootStore.roundStore.isManualSort;
    runInAction(() => {
      this.hubActionLoading = true;
    });
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke("SortTiles", values).catch(() => {
          toast.error(`failed ordering tile`);
          for (let i = 0; i < originalTiles!.length; i++) {
            let objIndex = this.rootStore.roundStore.mainPlayerAliveTiles!.findIndex(
              (obj) => obj.id === originalTiles![i].id
            );
            runInAction("updating reordered tile", () => {
              this.rootStore.roundStore.mainPlayerAliveTiles![
                objIndex
              ].activeTileCounter = originalTiles![i].activeTileCounter;
            });
          }

          runInAction("manual Sort", () => {
            this.rootStore.roundStore.isManualSort = originalManualSort;
          });
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
      toast.error("problem ordering tile");
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
            var tileInStore = this.roundStore.mainPlayerAliveTiles?.find(
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
        await this.hubConnection!.invoke(
          "PickTile",
          this.getGameAndRoundProps()
        );
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
        ).catch(() => {
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
        await this.hubConnection!.invoke("ChowTile", values).catch(() => {
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
        await this.hubConnection!.invoke("KongTile", values).catch(() => {
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
    const tempAction = [...this.roundStore.mainPlayer!.roundPlayerActions];
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke(
          "SkipAction",
          this.getGameAndRoundProps()
        ).catch(() => {
          runInAction(() => {
            this.roundStore.mainPlayer!.roundPlayerActions = tempAction;
            this.hubActionLoading = false;
          });
          toast.error(`can't skip`);
        });
        runInAction(() => {
          this.roundStore.mainPlayer!.roundPlayerActions = [];
          this.hubActionLoading = false;
        });
      } else {
        toast.error("not connected to hub");
      }
    } catch (error) {
      runInAction(() => {
        this.roundStore.mainPlayer!.roundPlayerActions = tempAction;
        this.hubActionLoading = false;
      });
      toast.error("problem skipping action");
    }
  };

  getGameAndRoundProps = () => {
    let values: any = {};
    values.gameId = this.gameStore.game?.id;
    values.roundId = this.roundStore.round?.id;
    return values;
  };
}
