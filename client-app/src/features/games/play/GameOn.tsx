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
import { toJS, runInAction } from "mobx";
import TileListOtherPlayerVertical from "./TileListOtherPlayerVertical";
import ResultModal from "./ResultModal";
import { Link } from "react-router-dom";

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
    getMainUser,
  } = rootStore.gameStore;
  const {
    loadingRoundInitial,
    roundSimple: round,
    loadRound,
    mainPlayer,
    leftPlayer,
    rightPlayer,
    topPlayer,
    mainPlayerActiveTiles,
    mainPlayerGraveYardTiles,
    mainPlayerJustPickedTile,
    boardActiveTile,
    boardGraveyardTiles,
    leftPlayerTiles,
    topPlayerTiles,
    rightPlayerTiles,
    roundTiles,
    remainingTiles,
    roundResults,
    roundEndingCounter,
  } = rootStore.roundStore;
  const {
    throwTile,
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
    borderColor: "#a2a2f0",
    display: "flex",
    overflow: "none",
    transitionDuration: `0.001s`,
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

  const onDragEnd = (result: DropResult) => {
    const { destination, draggableId } = result;

    if (!destination) {
      return;
    }

    if (destination.droppableId === "board")
      if (mainPlayer!.isMyTurn && mainPlayer!.mustThrow) {
        runInAction("throwingtile", () => {
          rootStore.roundStore.selectedTile = toJS(
            roundTiles!.find((t) => t.id === draggableId)!
          );
        });
        try{
          runInAction(() => {
            mainPlayer!.mustThrow = false;
          })
          throwTile();
        }catch{
          runInAction(() => {
            mainPlayer!.mustThrow = true;
          })
        }
      } else {
        toast.warn("Can't throw");
      }

    //TODO allow user to arrange tile manually
    //if (destination.droppableId === "tile") console.log("dropped to tile");
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
                    roundTiles={topPlayerTiles!}
                  />
                </Grid.Column>
                <Grid.Column width={3} />
              </Grid.Row>

              <Grid.Row className="zeroPadding">
                {/* Left Player */}
                <Grid.Column width={3}>
                  <TileListOtherPlayerVertical
                    player={leftPlayer!}
                    roundTiles={leftPlayerTiles!}
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
                            <Header as="h3">{WindDirection[round.wind]}</Header>
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
                  {mainPlayer?.mustThrow && (
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
                              as="h2"
                              style={{ color: "#d4d4d5" }}
                              content={`Drag and drop tile here`}
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
                    roundTiles={rightPlayerTiles!}
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
                      mainPlayerActiveTiles={mainPlayerActiveTiles!}
                      mainPlayerJustPickedTile={mainPlayerJustPickedTile!}
                    />
                  </Grid.Row>
                  <Grid.Row centered>
                    <ResultModal
                      roundResults={roundResults}
                      roundTiles={roundTiles}
                      isHost={getMainUser!.isHost}
                    />
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
