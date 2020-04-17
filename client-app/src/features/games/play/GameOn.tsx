import React, { useContext, useEffect } from "react";
import { Grid, Label, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import TileList from "./TileList";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";
import TileListBoard from "./TileListBoard";
import { TileStatus } from "../../../app/models/tileStatus";
import TileListMainPlayer from "./TileListMainPlayer";
import TileListOtherPlayer from "./TileListOtherPlayer";
import { DragDropContext, DropResult, Droppable } from "react-beautiful-dnd";
import { toJS, runInAction } from "mobx";

interface DetailParams {
  roundId: string;
  id: string;
}

//https://github.com/clauderic/react-sortable-hoc

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const { user } = rootStore.userStore;
  const { loadingGameInitial, loadGame, game } = rootStore.gameStore;
  const {
    loadingRoundInitial,
    roundSimple: round,
    loadRound,
    roundTiles,
    mainPlayer,
    leftPlayer,
    rightPlayer,
    topPlayer
  } = rootStore.roundStore;
  const {
    throwTile,
    pickTile, 
    loading,
    createHubConnection,
    stopHubConnection,
    leaveGroup,
  } = rootStore.hubStore;

  const mainPlayerActiveTiles = roundTiles
  ? roundTiles.filter((rt) => rt.owner === user?.userName && rt.status === TileStatus.UserActive)
  : null;

  const mainPlayerGraveYardTiles = roundTiles
  ? roundTiles.filter((rt) => rt.owner === user?.userName && rt.status === TileStatus.UserGraveyard)
  : null;

  const mainPlayerJustPickedTile = roundTiles
  ? roundTiles.filter((rt) => rt.owner === user?.userName && rt.status === TileStatus.UserJustPicked)
  : null;

  const boardActiveTile = roundTiles
    ? roundTiles.find((rt) => rt.status === TileStatus.BoardActive)
    : null;

  const boardGraveyardTiles = roundTiles
    ? roundTiles.filter((rt) => rt.status === TileStatus.BoardGraveyard)
    : null;

  const leftPlayerTiles =
    roundTiles && round && leftPlayer
      ? roundTiles.filter((rt) => rt.owner === leftPlayer?.userName)
      : null;

  const topPlayerTiles =
    roundTiles && round && topPlayer
      ? roundTiles.filter((rt) => rt.owner === topPlayer?.userName)
      : null;

  const rightPlayerTiles =
    roundTiles && round && rightPlayer
      ? roundTiles.filter((rt) => rt.owner === rightPlayer?.userName)
      : null;

  const getStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? 'lightblue' : 'lightgrey',
    display: 'flex',
    // padding: grid,
    overflow: 'auto',
    transitionDuration: `0.001s`
  });

    //  const onDragEnd = (result) {
    //     const { destination, source, draggableId } = result;
    
    //     if (!destination) {
    //       return;
    //     }
    
    //     if (
    //       destination.droppableId === source.droppableId &&
    //       destination.index === source.index
    //     ) {
    //       return;
    //     }
    
    //     const quotes = Object.assign([], this.state.quotes);
    //     const quote = this.state.quotes[source.index];
    //     quotes.splice(source.index, 1);
    //     quotes.splice(destination.index, 0, quote);
    
    //     this.setState({
    //       quotes: quotes
    //     });
    //   }

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

    const onDragEnd = (result: DropResult) => {
      const {source, destination} = result;

      if (!destination) {
        return;
      }

      if(destination.droppableId === 'board')
        console.log('dropped to board');
        console.log(source);
        console.log(destination);
        if(mainPlayerActiveTiles)
        {
          runInAction('throwingtile',() => {
            rootStore.roundStore.selectedTile = toJS(mainPlayerActiveTiles[source.index]);
          })
          throwTile();
        }
      
      if(destination.droppableId === 'tile')
        console.log('dropped to tile');

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
              tileStyleName="tileHorizontal"
              containerStyleName="tileHorizontalContainer"
            />
          </Grid.Column>
          <Grid.Column width={3} />
        </Grid.Row>

        <Grid.Row className="zeroPadding">
          {/* Left Player */}
          <Grid.Column width={1}></Grid.Column>
          <Grid.Column width={1}>
            {round && leftPlayer && <Label>{leftPlayer.userName}</Label>}
          </Grid.Column>
          <Grid.Column width={1}>
            <TileList
              tileStyleName="tileVertical"
              containerStyleName="tileVerticalContainer rotate90"
              roundTiles={leftPlayerTiles!}
            />
          </Grid.Column>

          {/* Board */}
          <Grid.Column width={10}>
            {round && (
              <div>
                <Label>Wind: {WindDirection[round.wind]}</Label>
              </div>
            )}
            {round && mainPlayer && (
              <div>
                <Label>
                  Current User Wind: {WindDirection[mainPlayer.wind]}
                </Label>
              </div>
            )}
            {boardActiveTile && (
              <div>
                <img src={boardActiveTile.tile.imageSmall} alt="tile" />
              </div>
            )}
            {boardGraveyardTiles && (
              <TileListBoard roundTiles={boardGraveyardTiles} />
            )}
          </Grid.Column>

          {/* Right Player */}
          <Grid.Column width={1}>
            <TileList
              tileStyleName="tileVertical"
              containerStyleName="tileVerticalContainer rotateMinus90"
              roundTiles={rightPlayerTiles!}
            />
          </Grid.Column>
          <Grid.Column width={1}>
            {round && rightPlayer && <Label>{rightPlayer.userName}</Label>}
          </Grid.Column>
          <Grid.Column width={1}></Grid.Column>
        </Grid.Row>

        {/* Main Player */}
        <Grid.Row className="zeroPadding">
          <Grid.Column width={3} />
          <Grid.Column width={10}>
            <Grid.Row>
              {mainPlayer && (
                <span>IsMyTurn:{mainPlayer.isMyTurn.toString()}</span>
              )}
              <Button loading={loading} onClick={throwTile}>
                Throw
              </Button>
              <Button loading={loading} onClick={pickTile}>
                Pick
              </Button>
              <Droppable droppableId="board">
              {(provided, snapshot) => (
                <div
                  ref={provided.innerRef}
                  style={getStyle(snapshot.isDraggingOver)}
                  {...provided.droppableProps}
                >
                  <div style={{width:'100px', backgroundColor:'red', height: '50px'}}/>
                  {provided.placeholder}
                </div>
              )}
            </Droppable>

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
