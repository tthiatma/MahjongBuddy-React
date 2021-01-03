import React, { useContext, useEffect, Fragment } from "react";
import {
  Grid,
  Button,
  Header,
  Item,
  Divider,
  Segment,
  Container,
  List,
  Icon,
  Popup,
} from "semantic-ui-react";
import { toast } from "react-toastify";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";
import TileListBoard from "./TileListBoard";
import MainPlayerSection from "./MainPlayerSection";
import TileListOtherPlayer from "./TileListOtherPlayer";
import { DragDropContext, DropResult, Droppable } from "react-beautiful-dnd";
import { runInAction, toJS } from "mobx";
import TileListOtherPlayerVertical from "./TileListOtherPlayerVertical";
import ResultModal from "./ResultModal";
import { Link } from "react-router-dom";
import { IRoundTile } from "../../../app/models/tile";
import RulesModal from "./RulesModal";
import { IPayPoint } from "../../../app/models/game";

interface DetailParams {
  roundCounter: string;
  code: string;
}

//https://github.com/clauderic/react-sortable-hoc

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadingGameInitial,
    loadGame,
    game,
    gameIsOver,
  } = rootStore.gameStore;
  const {
    loadingRoundInitial,
    round,
    loadRound,
    mainPlayer,
    leftPlayer,
    rightPlayer,
    topPlayer,
    mainPlayerAliveTiles,
    mainPlayerGraveYardTiles,
    boardActiveTile,
    boardGraveyardTiles,
    remainingTiles,
    roundEndingCounter,
    openRulesModal,
  } = rootStore.roundStore;
  const {
    throwTile,
    orderTiles,
    hubLoading,
    createHubConnection,
    leaveGroup,
  } = rootStore.hubStore;

  const square = { width: 50, height: 50, padding: "0.5em" };
  const getStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "lightblue" : "",
    borderStyle: "dashed",
    borderColor: "rgb(211 211 244)",
    display: "flex",
    overflow: "none",
    alignItem: "center",
    justifyContent: "center",
    height: "45px",
  });

  useEffect(() => {
    createHubConnection(match.params.code);
    return () => {
      leaveGroup(match.params.code);
    };
  }, [createHubConnection, leaveGroup, match.params.code]);

  useEffect(() => {
    runInAction(() => {
      rootStore.commonStore.showNavBar = false;
      rootStore.commonStore.showFooter = false;
    });
    return () => {
      runInAction(() => {
        rootStore.commonStore.showNavBar = true;
        rootStore.commonStore.showFooter = true;
      });
    };
  }, [rootStore.commonStore.showNavBar, rootStore.commonStore.showFooter]);

  useEffect(() => {
    loadGame(match.params.code);
  }, [loadGame, match.params.code]);

  useEffect(() => {
    loadRound(match.params.roundCounter, match.params.code);
  }, [loadRound, match.params.roundCounter, match.params.code]);

  if (
    loadingGameInitial ||
    loadingRoundInitial ||
    !game ||
    !round ||
    hubLoading
  )
    return <LoadingComponent content="Loading round..." />;

  const getActiveTileAnimation = (): string => {
    let animationStyle: string = "";
    if (!gameIsOver) {
      switch (boardActiveTile?.thrownBy) {
        case leftPlayer?.userName: {
          animationStyle = "fly right";
          break;
        }
        case rightPlayer?.userName: {
          animationStyle = "fly left";
          break;
        }
        case topPlayer?.userName: {
          animationStyle = "fly down";
          break;
        }
        case mainPlayer?.userName: {
          animationStyle = "fly up";
          break;
        }
      }
    }

    return animationStyle;
  };

  const doThrowTile = (tileId: string) => {
    if (gameIsOver) {
      toast.warn("Can't throw because host ended this game");
      return;
    }

    if (
      mainPlayer!.isMyTurn &&
      mainPlayer!.mustThrow &&
      !round.isOver &&
      !round.isEnding
    ) {
      runInAction("throwingtile", () => {
        rootStore.roundStore.selectedTile = mainPlayerAliveTiles?.find(
          (t) => t.id === tileId
        )!;
      });
      throwTile();
    } else {
      toast.warn("Can't throw");
    }
  };

  const reorderTiles = (
    activeTiles: IRoundTile[],
    startIndex: number,
    endIndex: number
  ) => {
    const result = Array.from(activeTiles);
    const [removed] = result.splice(startIndex, 1);
    result.splice(endIndex, 0, removed);
    for (let i = 0; i < result.length; i++) {
      runInAction(() => {
        result[i].activeTileCounter = i;
      });
    }
    return result;
  };

  const onDragEnd = async (result: DropResult) => {
    const { destination, source, draggableId } = result;

    if (!destination) {
      return;
    }

    if (destination.droppableId === "board") doThrowTile(draggableId);

    if (destination.droppableId === "tile") {
      if (source.index === destination.index) return;

      const beforeOrderingTiles = Array.from(
        toJS(mainPlayerAliveTiles!, { recurseEverything: true })
      );
      const beforeOrderingManualSortValue = toJS(
        rootStore.roundStore.isManualSort
      );

      const reorderedTiles = reorderTiles(
        mainPlayerAliveTiles!,
        source.index,
        destination.index
      );

      runInAction("manual Sort", () => {
        rootStore.roundStore.isManualSort = true;
      });

      await orderTiles(
        reorderedTiles,
        beforeOrderingTiles,
        beforeOrderingManualSortValue
      );
    }
  };

  //there gotta be more elegant way to do this lol
  const getCalculationResult = (): IPayPoint[] => {
    //if there is no player with negative points then there gotta be something wrong lol
    let bodyResult: IPayPoint[] = [];

    const tiePlayers = game.gamePlayers.slice().filter((p) => p.points === 0);
    const gameWinners = Array.from(
      toJS(game.gamePlayers.filter((p) => p.points > 0)!, {
        recurseEverything: true,
      })
    );
    const gameLosers = Array.from(
      toJS(game.gamePlayers.filter((p) => p.points < 0)!, {
        recurseEverything: true,
      })
    );

    if (gameLosers.length === 1) {
      //then 1 person pays to all the winners
      var sadLoserName = gameLosers[0].displayName;
      gameWinners.forEach((w) => {
        const res: IPayPoint = {
          from: sadLoserName,
          to: w.displayName,
          point: w.points,
        };
        bodyResult.push(res);
      });
    } else if (
      gameLosers.length === 3 ||
      (gameLosers.length === 2 && tiePlayers.length > 0)
    ) {
      let proWinnerName = gameWinners[0].displayName;
      gameLosers.forEach((l) => {
        const res: IPayPoint = {
          from: l.displayName,
          to: proWinnerName,
          point: l.points * -1,
        };
        bodyResult.push(res);
      });
    } else if (gameLosers.length === 2) {
      //there should be 2 winners and 2 losers here
      //calculate with least amount of transaction
      const sortedLosersLessToMore = gameLosers
        .slice()
        .sort((a, b) => b.points - a.points);

      const sortedWinners = gameWinners
        .slice()
        .sort((a, b) => a.points - b.points);
      if (sortedLosersLessToMore[0].points * -1 === sortedWinners[0].points) {
        for (let i = 0; i < 2; i++) {
          const transfer: IPayPoint = {
            from: sortedLosersLessToMore[i].displayName,
            to: sortedWinners[i].displayName,
            point: sortedWinners[i].points,
          };
          bodyResult.push(transfer);
        }
      } else {
        //there will be 3 transaction here
        const remainder =
          sortedWinners[1].points - sortedLosersLessToMore[0].points * -1;
        const firstTransaction: IPayPoint = {
          from: sortedLosersLessToMore[0].displayName,
          to: sortedWinners[1].displayName,
          point: sortedLosersLessToMore[0].points * -1,
        };
        bodyResult.push(firstTransaction);

        const secondTransaction: IPayPoint = {
          from: sortedLosersLessToMore[1].displayName,
          to: sortedWinners[1].displayName,
          point: remainder,
        };
        bodyResult.push(secondTransaction);

        const lastRemainder = sortedLosersLessToMore[1].points * -1 - remainder;
        const thirdTransaction: IPayPoint = {
          from: sortedLosersLessToMore[1].displayName,
          to: sortedWinners[0].displayName,
          point: lastRemainder,
        };
        bodyResult.push(thirdTransaction);
      }
    }
    return bodyResult;
  };

  return (
    <Fragment>
      <Container>
        <Segment>
          <DragDropContext onDragEnd={onDragEnd}>
            <Grid>
              {/* Top Player */}
              <Grid.Row className="zeroPadding">
                <Grid.Column width={3}>
                  <Button
                    basic
                    size="small"
                    circular
                    icon="arrow circle left"
                    as={Link}
                    to={`/games/${game.code}`}
                  />
                </Grid.Column>
                <Grid.Column width={10}>
                  {topPlayer && <TileListOtherPlayer player={topPlayer!} />}
                </Grid.Column>
                <Grid.Column textAlign="right" width={3}>
                  <Button
                    basic
                    size="small"
                    circular
                    icon="book"
                    onClick={openRulesModal}
                  />
                  <RulesModal />
                </Grid.Column>
              </Grid.Row>

              <Grid.Row>
                {/* Left Player */}
                <Grid.Column width={3}>
                  {leftPlayer && (
                    <TileListOtherPlayerVertical
                      player={leftPlayer}
                      isReversed={false}
                    />
                  )}
                </Grid.Column>

                {/* Board */}
                <Grid.Column width={10}>
                  <Grid.Row style={{ paddingTop: "1em" }}>
                    <Grid>
                      <Grid.Column width={6} />
                      <Grid.Column width={4}>
                        <Grid.Row>
                          <Popup
                            content="Remaining unopen tiles"
                            trigger={
                              <Segment circular style={square}>
                                <Item.Group>
                                  <Item>
                                    <Item.Image
                                      size="mini"
                                      src="/assets/tiles/50px/face-down.png"
                                    />
                                    <Item.Content
                                      verticalAlign="middle"
                                      className="remainingTileHeader"
                                    >
                                      <Item.Header>{`${remainingTiles}`}</Item.Header>
                                    </Item.Content>
                                  </Item>
                                </Item.Group>
                              </Segment>
                            }
                          />
                          <Popup
                            content="Current prevailing wind. get 1 extra point if you have 3 or 4 of this tile"
                            trigger={
                              <Segment circular style={square}>
                                {round.wind === WindDirection.East && (
                                  <img
                                    src="/assets/tiles/50px/wind/wind-east.png"
                                    alt="wind-east"
                                  />
                                )}
                                {round.wind === WindDirection.South && (
                                  <img
                                    src="/assets/tiles/50px/wind/wind-south.png"
                                    alt="wind-south"
                                  />
                                )}
                                {round.wind === WindDirection.West && (
                                  <img
                                    src="/assets/tiles/50px/wind/wind-west.png"
                                    alt="wind-west"
                                  />
                                )}
                                {round.wind === WindDirection.North && (
                                  <img
                                    src="/assets/tiles/50px/wind/wind-north.png"
                                    alt="wind-north"
                                  />
                                )}
                              </Segment>
                            }
                          />

                          <Popup
                            content={`Minimum of ${game.minPoint} pts for the win button to appear. Maximum of ${game.maxPoint} pts per round.`}
                            trigger={
                              <Segment circular style={square}>
                                <Item.Group>
                                  <Item>
                                    <Item.Content
                                      verticalAlign="middle"
                                      className="remainingTileHeader"
                                    >
                                      {`Min:${game.minPoint}pts`}
                                      <br />
                                      {`Max:${game.maxPoint}pts`}
                                    </Item.Content>
                                  </Item>
                                </Item.Group>
                              </Segment>
                            }
                          />
                          {round.isEnding && (
                            <Segment circular style={square}>
                              <Header as="h3">
                                Ending in {roundEndingCounter}
                              </Header>
                            </Segment>
                          )}
                        </Grid.Row>
                      </Grid.Column>
                      <Grid.Column width={6} />
                    </Grid>
                  </Grid.Row>
                  <Grid.Row>
                    <Divider />
                  </Grid.Row>
                  <TileListBoard
                    graveyardTiles={boardGraveyardTiles!}
                    activeTile={boardActiveTile!}
                    activeTileAnimation={getActiveTileAnimation()}
                  />
                  {gameIsOver && (
                    <Segment>
                      <Grid>
                        <Grid.Column width="3"></Grid.Column>
                        <Grid.Column width="4">
                          <Header icon>
                            <Icon name="calculator" />
                            Game Over{" "}
                          </Header>
                        </Grid.Column>
                        <Grid.Column width="6">
                          {getCalculationResult().length > 0 ? (
                            <List>
                              {getCalculationResult().map((r, i) => (
                                <List.Item key={`ptresult${i}`}>
                                  <h3>
                                    {r.from} {"->"} {r.to} : {r.point} pts{" "}
                                  </h3>
                                </List.Item>
                              ))}
                            </List>
                          ) : (
                            <h3>
                              Hmmm... play more maybe? nothing to calculate
                            </h3>
                          )}
                        </Grid.Column>
                        <Grid.Column width="3"></Grid.Column>
                      </Grid>
                    </Segment>
                  )}

                  {mainPlayer?.mustThrow && !gameIsOver && !round.isOver && (
                    <Droppable droppableId="board">
                      {(provided, snapshot) => (
                        <div
                          ref={provided.innerRef}
                          style={getStyle(snapshot.isDraggingOver)}
                          {...provided.droppableProps}
                        >
                          <div
                            style={{
                              paddingTop: "10px",
                              height: "45px",
                            }}
                          >
                            <Header
                              as="h3"
                              style={{ color: "#d4d4d5" }}
                              content={`Throw tile by double clicking/drag and drop it here`}
                            />
                          </div>
                          {provided.placeholder}
                        </div>
                      )}
                    </Droppable>
                  )}
                </Grid.Column>

                {/* Right Player */}
                <Grid.Column width={3}>
                  {rightPlayer && (
                    <TileListOtherPlayerVertical
                      player={rightPlayer}
                      isReversed={true}
                    />
                  )}
                </Grid.Column>
              </Grid.Row>

              {/* Main Player */}
              <Grid.Row centered>
                <Grid.Column width={16}>
                  <Grid.Row centered style={{ textAlign: "center" }}>
                    <MainPlayerSection
                      mainPlayer={mainPlayer}
                      tileStyleName="tileHorizontal"
                      containerStyleName="tileHorizontalContainer"
                      mainPlayerGraveYardTiles={mainPlayerGraveYardTiles!}
                      mainPlayerAliveTiles={mainPlayerAliveTiles!}
                      doThrowTile={doThrowTile}
                    />
                  </Grid.Row>
                  <Grid.Row centered>
                    <ResultModal />
                  </Grid.Row>
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </DragDropContext>
        </Segment>
      </Container>
    </Fragment>
  );
};

export default observer(GameOn);
