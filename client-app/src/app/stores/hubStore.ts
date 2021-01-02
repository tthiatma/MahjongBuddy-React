import { observable, action, runInAction, toJS } from "mobx";
import { RootStore } from "./rootStore";
import { history } from "../..";
import { toast } from "react-toastify";
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from "@microsoft/signalr";
import { WindDirection } from "../models/windEnum";
import { IRound } from "../models/round";
import { IRoundTile, TileType, TileValue } from "../models/tile";
import RoundStore from "./roundStore";
import GameStore from "./gameStore";
import UserStore from "./userStore";
import { IGamePlayer, IRoundOtherPlayer } from "../models/player";
import { GameStatus } from "../models/gameStatusEnum";
import { IGame } from "../models/game";
import { TileStatus } from "../models/tileStatus";

export default class HubStore {
  rootStore: RootStore;
  hubStore: HubStore;
  roundStore: RoundStore;
  gameStore: GameStore;
  userStore: UserStore;
  cooldownTime: number = 1500;
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

  addHubConnectionHandler() {
    if (this.hubConnection) {
      this.hubConnection.on("UpdatePlayersWind", (players: IGamePlayer[]) => {
        runInAction(() => {
          players.forEach((p) => {
            let curPlayer = this.rootStore.gameStore.game?.gamePlayers.find(
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

      this.hubConnection.on("GameEnded", (game: IGame) => {
        runInAction(() => {
          if (
            this.rootStore.gameStore.game &&
            this.rootStore.gameStore.game.id === game.id
          ){
            this.rootStore.gameStore.game = game;
            const gameHost = game.gamePlayers.find(p => p.isHost);
            if(this.rootStore.userStore.user?.userName !== gameHost?.userName){
              toast.info(`Game ${game.code} has ended`);
            }
            this.rootStore.roundStore.closeResultModal();
          }
        });
      });

      this.hubConnection.on("PlayerStoodUp", (player: IGamePlayer) => {
        runInAction(() => {
          let curPlayer = this.rootStore.gameStore.game?.gamePlayers.find(
            (x) => x.userName === player.userName
          );
          if (curPlayer) curPlayer.initialSeatWind = undefined;
        });
      });

      this.hubConnection.on("PlayerSat", (player: IGamePlayer) => {
        runInAction(() => {
          let curPlayer = this.rootStore.gameStore.game?.gamePlayers.find(
            (x) => x.userName === player.userName
          );
          if (curPlayer) curPlayer.initialSeatWind = player.initialSeatWind;
        });
      });

      this.hubConnection.on("GameCancelled", (gameCode: string) => {
        toast.info(`Host cancelled game ${gameCode}`);
        history.push(`/`);
      });

      this.hubConnection.on("UpdateRoundNoLag", (round: IRound) => {
        runInAction("updating round", () => {
          this.roundStore.round = round;
        });
      });

      this.hubConnection.on("UpdateRound", (round: IRound) => {
        //update board tiles
        runInAction("updating necessary round prop", () => {
          if (this.roundStore.round) {
            if (this.roundStore.round.mainPlayer.mustThrow) {
              this.roundStore.round.mainPlayer = round.mainPlayer;
            }

            this.roundStore.round.mainPlayer.isManualSort = round.mainPlayer.isManualSort;
            this.roundStore.round.boardTiles = round.boardTiles;
            this.roundStore.round.otherPlayers.forEach((op) => {
              var matchingOp = round.otherPlayers.find(
                (rop) => rop.userName === op.userName
              );
              op.mustThrow = matchingOp!.mustThrow;
              op.activeTilesCount = matchingOp!.activeTilesCount;
            });
          }
        });

        this.sleep(this.cooldownTime).then(() => {
          runInAction(() => {
            this.roundStore.round = round;
          });
        });
      });

      this.hubConnection.on("RoundStarted", (round: IRound) => {
        runInAction(() => {
          this.roundStore.showResult = false;
          this.roundStore.round = round;
        });
        history.push(
          `/games/${this.rootStore.gameStore.game!.code}/rounds/${
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
            this.gameStore.game.gamePlayers = this.gameStore.game.gamePlayers.filter(
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
            if (
              this.rootStore.userStore.user?.userName !== player.userName &&
              this.gameStore.game.status === GameStatus.Created
            )
              toast.info(`${player.displayName} has left the Game`);
          }
        })
      );

      this.hubConnection.on("PlayerConnected", (player: IGamePlayer) => {
        runInAction(() => {
          if (this.gameStore.game) {
            const exist = this.gameStore.game.gamePlayers.find(
              (p) => p.userName === player.userName
            );
            if (!exist) {
              this.gameStore.game.gamePlayers.push(player);

              if (this.rootStore.userStore.user?.userName !== player.userName)
                toast.info(`${player.displayName} has joined the Game`);
            }

            if (this.rootStore.userStore.user?.userName === player.userName) {
              this.gameStore.game.initialSeatWind = player.initialSeatWind;
              this.gameStore.game.isCurrentPlayerConnected = true;
            }

            this.gameStore.gameRegistry.set(
              this.gameStore.game.id,
              this.gameStore.game
            );
          }
        });
      });

      this.hubConnection.on("Send", (message) => {
        toast.info(message);
      });
    }
  }

  @action leaveGroup = (gameCode: string) => {
    if (this.hubConnection!.state === "Connected")
      this.hubConnection!.invoke("RemoveFromGroup", gameCode);
  };

  @action createHubConnection = async (gameCode: string) => {
    if (this.hubConnection == null) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${process.env.REACT_APP_API_GAME_HUB_URL!}/?gcode=${gameCode}`, {
          accessTokenFactory: () => this.rootStore.commonStore.token!,
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.None)
        .build();
      this.addHubConnectionHandler();
    }
    if (
      this.hubConnection.state === HubConnectionState.Disconnected ||
      this.hubConnection.state === HubConnectionState.Disconnecting
    ) {
      runInAction(() => {
        this.hubLoading = true;
      });
      this.hubConnection
        .start()
        .then(() => {
          if (this.hubConnection!.state === "Connected")
            this.hubConnection?.invoke("JoinGame", gameCode);
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
      this.hubConnection?.invoke("JoinGame", gameCode);
    }
  };

  @action addChatMsg = async (values: any) => {
    values.gameCode = this.gameStore.game!.code;
    try {
      this.hubConnection!.invoke("SendChatMsg", values).catch(() => {
        toast.error("Unable to send chat message");
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action endGame = async () => {
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      const currentGameCode = this.rootStore.gameStore.game?.code;
      if(currentGameCode){
        this.hubConnection!.invoke("EndGame", currentGameCode).then(() => {
          runInAction(() => {
            this.hubLoading = false;
          });  
        });  
      }
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem ending game");
    }
  };

  @action cancelGame = async(gameCode: string) => {
    //actually deleting game lol
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("CancelGame", gameCode);
      runInAction(() => {
        this.hubLoading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.hubLoading = false;
      });
      toast.error("problem cancelling game");
    }
  }

  @action joinGame = async () => {
    const gameCode = this.gameStore.game!.code;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("JoinGame", gameCode);
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
    values.gameCode = this.gameStore.game!.code;
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
    values.gameCode = this.gameStore.game!.code;
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
    values.gameCode = this.gameStore.game!.code;
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
    values.gameCode = this.gameStore.game!.code;
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
        toast.error("disconnected from the game, please refresh your browser");
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
        toast.error("disconnected from the game, please refresh your browser");
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
    values.gameCode = this.gameStore.game!.code;
    runInAction(() => {
      this.hubLoading = true;
    });
    try {
      this.hubConnection!.invoke("StartRound", values).then(() => {
        runInAction(() => {
          this.hubLoading = false;
        });
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
        }).then(() => {
          runInAction(() => {
            this.roundStore.showResult = true;
            this.hubActionLoading = false;
          });  
        });
      } else {
        toast.error("disconnected from the game, please refresh your browser");
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
        toast.error("disconnected from the game, please refresh your browser");
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
    const currentRound = toJS(this.roundStore.round);
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        runInAction("throw tile initially", () => {
          const tileToThrow = this.roundStore.selectedTile;
          if (tileToThrow) {
            //throw tile locally to show responsiveness XD

            //update board tile
            tileToThrow.owner = "board";
            tileToThrow.status = TileStatus.BoardActive;
            tileToThrow.thrownBy = this.userStore.user!.userName;
            this.roundStore.round!.boardTiles.push(tileToThrow);

            //take off main player thrown tile
            const tilesWithoutThrown = currentRound!.mainPlayer?.playerTiles.filter(function (t) {return t.id !== tileToThrow.id});            
            if(tilesWithoutThrown){
              this.roundStore.round!.mainPlayer.playerTiles = tilesWithoutThrown;
            }
          }
        });
        this.hubConnection!.invoke("ThrowTile", values).then(() => {
          runInAction(() => {
            this.hubActionLoading = false;
          });  
        });
      } else {
        toast.error("disconnected from the game, please refresh your browser");
      }
    } catch (error) {
      runInAction(() => {
        //revert the optimistic throw tile lol
        const tileToThrow = this.roundStore.selectedTile;
        if(tileToThrow){
          this.roundStore.round!.mainPlayer.playerTiles.push(tileToThrow);
          const boardWithoutThrown = this.roundStore.round!.boardTiles.filter((t) => t.id === tileToThrow.id)  
          if(boardWithoutThrown){
            this.roundStore.round!.boardTiles = boardWithoutThrown;
          }

        }
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
        toast.error("disconnected from the game, please refresh your browser");
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
        toast.error("disconnected from the game, please refresh your browser");
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
        toast.error("disconnected from the game, please refresh your browser");
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
        toast.error("disconnected from the game, please refresh your browser");
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
        toast.error("disconnected from the game, please refresh your browser");
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
    const tempAction = [...this.roundStore.mainPlayer!.roundPlayerActiveActions];
    try {
      if (this.hubConnection && this.hubConnection.state === "Connected") {
        await this.hubConnection!.invoke(
          "SkipAction",
          this.getGameAndRoundProps()
        ).catch(() => {
          runInAction(() => {
            this.roundStore.mainPlayer!.roundPlayerActiveActions = tempAction;
            this.hubActionLoading = false;
          });
          toast.error(`can't skip`);
        });
        runInAction(() => {
          this.roundStore.mainPlayer!.roundPlayerActiveActions = [];
          this.hubActionLoading = false;
        });
      } else {
        toast.error("disconnected from the game, please refresh your browser");
      }
    } catch (error) {
      runInAction(() => {
        this.roundStore.mainPlayer!.roundPlayerActiveActions = tempAction;
        this.hubActionLoading = false;
      });
      toast.error("problem skipping action");
    }
  };

  updateMainPlayerTiles = (updatedTile: IRoundTile) => {
    let tileIdx = this.roundStore.round?.mainPlayer.playerTiles.findIndex(
      (t) => t.id === updatedTile.id
    );
    runInAction("updating tile", () => {
      if(tileIdx)
        this.roundStore.round!.mainPlayer.playerTiles[tileIdx] = updatedTile;
    });
}

  getGameAndRoundProps = () => {
    let values: any = {};
    values.gameId = this.gameStore.game?.id;
    values.gameCode = this.gameStore.game?.code;
    values.roundId = this.roundStore.round?.id;
    return values;
  };
}
