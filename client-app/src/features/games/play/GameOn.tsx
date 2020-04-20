import React, { useContext, useEffect, Fragment } from "react";
import { Grid, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import TileList from "./TileList";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";
import TileListBoard from "./TileListBoard";
import TileListMainPlayer from "./TileListMainPlayer";
import TileListOtherPlayer from "./TileListOtherPlayer";
import { DragDropContext, DropResult, Droppable, ResponderProvided } from "react-beautiful-dnd";
import { toJS, runInAction } from "mobx";

interface DetailParams {
  roundId: string;
  id: string;
}

//https://github.com/clauderic/react-sortable-hoc

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const { loadingGameInitial, loadGame, game } = rootStore.gameStore;
  const {
    loadingRoundInitial,
    roundSimple: round,
    loadRound,
    mainPlayer,
    leftPlayer,
    rightPlayer,
    mainPlayerActiveTiles,
    mainPlayerGraveYardTiles,
    mainPlayerJustPickedTile,
    boardActiveTile,
    boardGraveyardTiles,
    leftPlayerTiles,
    topPlayerTiles,
    rightPlayerTiles,
    roundTiles
  } = rootStore.roundStore;
  const {
    throwTile,
    pickTile,
    loading,
    createHubConnection,
    stopHubConnection,
    leaveGroup,
  } = rootStore.hubStore;

  const getStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "lightblue" : "lightgrey",
    display: "flex",
    overflow: "auto",
    transitionDuration: `0.001s`,
  });

  useEffect(() => {
    loadGame(match.params!.id);
    loadRound(parseInt(match.params.roundId));
    createHubConnection(match.params!.id);
    return () => {
      leaveGroup(match.params.id);
    };
  }, [
    createHubConnection,
    stopHubConnection,
    loadGame,
    loadRound,
    leaveGroup,
    match.params,
    match.params.id,
    match.params.roundId,
  ]);

  if (loadingGameInitial || loadingRoundInitial || !game || !round || loading)
    return <LoadingComponent content="Loading round..." />;

  const onDragEnd = (result: DropResult, provided: ResponderProvided) => {
    const { source, destination, draggableId } = result;
    if (!destination) {
      return;
    }

    if (destination.droppableId === "board")
      if (mainPlayerActiveTiles) {

        runInAction("throwingtile", () => {
          rootStore.roundStore.selectedTile = toJS(
            roundTiles!.find(t => t.id === draggableId)!
          );
        });
        throwTile();
      }

    if (destination.droppableId === "tile") console.log("dropped to tile");
  };

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <Grid>
        {/* Top Player */}
        <Grid.Row className="zeroPadding">
          <Grid.Column width={3} />
          <Grid.Column width={10}>
            <TileListOtherPlayer
              roundTiles={topPlayerTiles!}
              tileStyleName="flexTiles"
              containerStyleName="flexTilesContainer"
            />
          </Grid.Column>
          <Grid.Column width={3} />
        </Grid.Row>

        <Grid.Row className="zeroPadding">
          {/* Left Player */}
          <TileList
              player={leftPlayer!} 
              tileStyleName="tileVertical"
              containerStyleName="tileVerticalContainer"
              rotation="rotate90"
              roundTiles={leftPlayerTiles!}
            />

          {/* Board */}
          <Grid.Column width={10}>          
            <TileListBoard graveyardTiles={boardGraveyardTiles!} activeTile={boardActiveTile!} />
            <Droppable droppableId="board">
              {(provided, snapshot) => (
                <div
                  ref={provided.innerRef}
                  style={getStyle(snapshot.isDraggingOver)}
                  {...provided.droppableProps}
                >
                  <div
                    style={{
                      height: "50px",
                    }}
                  />
                  {provided.placeholder}
                </div>
              )}
            </Droppable>
          </Grid.Column>

          {/* Right Player */}
          <TileList
              player={rightPlayer!}
              tileStyleName="tileVertical"
              containerStyleName="tileVerticalContainer"
              rotation="rotateMinus90"
              roundTiles={rightPlayerTiles!}
            />
        </Grid.Row>

        {/* Main Player */}
        <Grid.Row className="zeroPadding">
          <Grid.Column width={3} />
          <Grid.Column width={10}>
            <Grid.Row>
              {mainPlayer && (
                <Fragment>
                  <span>{`IsMyTurn: ${mainPlayer.isMyTurn.toString()} `}</span>
                  <span>
                    {` Current User Wind:${WindDirection[mainPlayer.wind]} `}
                  </span>
                  <span>
                    {` Current Wind: ${WindDirection[round.wind]}`} 
                    </span>
                </Fragment>
              )}
              <Button loading={loading} onClick={throwTile}>
                Throw
              </Button>
              <Button loading={loading} onClick={pickTile}>
                Pick
              </Button>
            </Grid.Row>
            <Grid.Row>
              <TileListMainPlayer
                tileStyleName="tileHorizontal"
                containerStyleName="tileHorizontalContainer"
                mainPlayerGraveYardTiles={mainPlayerGraveYardTiles!}
                mainPlayerActiveTiles={mainPlayerActiveTiles!}
                mainPlayerJustPickedTile={mainPlayerJustPickedTile!}
              />
            </Grid.Row>
          </Grid.Column>
          <Grid.Column width={3} />
        </Grid.Row>
      </Grid>
    </DragDropContext>
  );
};

export default observer(GameOn);
