import React, { useContext, useEffect, Fragment } from "react";
import {
  Grid,
  Button,
  Header,
  Item,
  Divider,
  Segment,
  Container,
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

interface DetailParams {
  roundId: string;
  id: string;
}

//https://github.com/clauderic/react-sortable-hoc

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadingGameInitial,
    loadGame,
    game,
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
  } = rootStore.roundStore;
  const {
    throwTile,
    orderTiles,
    hubLoading,
    createHubConnection,
    stopHubConnection,
    leaveGroup,
  } = rootStore.hubStore;

  //currently only support one winner
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
    createHubConnection(match.params!.id);
    return () => {
      leaveGroup(match.params.id);
    };
  }, [createHubConnection, stopHubConnection, leaveGroup, match.params]);

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
    loadGame(match.params!.id);
  }, [loadGame, match.params, match.params.id]);

  useEffect(() => {
    loadRound(match.params.roundId, match.params!.id);
  }, [loadRound, match.params, match.params.roundId]);

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
    return animationStyle;
  };

  const doThrowTile = (tileId: string) => {
    if (mainPlayer!.isMyTurn && mainPlayer!.mustThrow && !round.isOver) {
      const tempPlayerAction = Array.from(mainPlayer!.roundPlayerActions);
      runInAction("throwingtile", () => {
        rootStore.roundStore.selectedTile = mainPlayerAliveTiles?.find(
          (t) => t.id === tileId
        )!;
      });
      try {
        runInAction(() => {
          mainPlayer!.mustThrow = false;
          mainPlayer!.roundPlayerActions = [];
        });
        throwTile();
      } catch {
        runInAction(() => {
          mainPlayer!.mustThrow = true;
          mainPlayer!.roundPlayerActions = tempPlayerAction;
        });
      }
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
                    to={`/games/${game.id}`}
                  />
                </Grid.Column>
                <Grid.Column width={10}>
                  <TileListOtherPlayer
                    player={topPlayer!}
                  />
                </Grid.Column>
                <Grid.Column width={3} />
              </Grid.Row>

              <Grid.Row>
                {/* Left Player */}
                <Grid.Column width={3}>
                  <TileListOtherPlayerVertical
                    player={leftPlayer!}
                    isReversed={false}
                  />
                </Grid.Column>

                {/* Board */}
                <Grid.Column width={10}>
                  <Grid.Row style={{ paddingTop: "1em" }}>
                    <Grid>
                      <Grid.Column width={6} />
                      <Grid.Column width={4}>
                        <Grid.Row>
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
                  {mainPlayer?.mustThrow && !round.isOver && (
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
                  <TileListOtherPlayerVertical
                    player={rightPlayer!}
                    isReversed={true}
                  />
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
