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
import { IRoundTile, TileValue } from "../../../app/models/tile";

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
    topPlayer,
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
    pong,
    chow,
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
    
  const doChow = () => {
    let chowTiles: IRoundTile[] = [];
  
    let boardActiveTile = rootStore.roundStore.boardActiveTile;

    let sameTypeChowTiles = rootStore.roundStore.mainPlayerActiveTiles?.filter(t => t.tile.tileType == boardActiveTile?.tile.tileType);
    console.log('sameTypeChowTiles = ' + sameTypeChowTiles?.length);

    if(boardActiveTile?.tile.tileValue === TileValue.One){
      let possibleTiles = sameTypeChowTiles?.filter(t => t.tile.tileValue === TileValue.Two || t.tile.tileValue === TileValue.Three)
      console.log('possibleTiles = ' + possibleTiles?.length);
      if(possibleTiles && possibleTiles.length === 2)
        chowTiles = possibleTiles;
    }else if(boardActiveTile?.tile.tileValue === TileValue.Nine){
      let possibleTiles = sameTypeChowTiles?.filter(t => t.tile.tileValue === TileValue.Eight || t.tile.tileValue === TileValue.Seven)
      console.log('possibleTiles = ' + possibleTiles?.length);
      if(possibleTiles && possibleTiles.length === 2)
        chowTiles = possibleTiles;
    }else if(boardActiveTile!.tile.tileValue >= 2 && boardActiveTile!.tile.tileValue <= 8){
      console.log('its 2 to 8')
      console.log('sameTypeChowTiles length is ' + sameTypeChowTiles?.length )
      let possibleTiles = sameTypeChowTiles?.filter(t => 
        t.tile.tileValue === (boardActiveTile!.tile.tileValue - 2) 
        || t.tile.tileValue === (boardActiveTile!.tile.tileValue + 2)
        || t.tile.tileValue === (boardActiveTile!.tile.tileValue + 1)
        || t.tile.tileValue === (boardActiveTile!.tile.tileValue - 1)
        )
        if(possibleTiles && possibleTiles.length > 1){
          console.log(possibleTiles.length);

          for(var i = 0; i < possibleTiles.length; i ++ ){
            let t = possibleTiles[i];
            console.log('testing tile ' + t.tile.tileValue);
            let testChowTiles: IRoundTile[] = [];
            testChowTiles.push(t);
            testChowTiles.push(boardActiveTile!);
            testChowTiles.sort((a,b) => a!.tile.tileValue - b!.tile.tileValue);
          if((t.tile.tileValue + boardActiveTile!.tile.tileValue) % 2 !== 0){
              console.log('tile is connected')
              //then these two tiles is connected
              let probableTile = possibleTiles.find(t => t.tile.tileValue === (testChowTiles[0].tile.tileValue - 1));
              if(!probableTile)
                probableTile = possibleTiles.find(t => t.tile.tileValue === (testChowTiles[1].tile.tileValue + 1));
              
              if(probableTile){
                console.log('found matching tiles to chow');
                chowTiles.push(probableTile);
                chowTiles.push(t);
                break;
              }
            }else{
              //then these two tiles is not connected
              console.log('tile is NOT connected')
              let probableTile = possibleTiles.find(t => t.tile.tileValue === (testChowTiles[0].tile.tileValue + 1));              
              if(probableTile){
                console.log('found matching tiles to chow');
                chowTiles.push(probableTile);
                chowTiles.push(t);
                break;
              }
            }
          }
        }
        else{
          console.log('not possible to chow tiles because possible chow tiles is ' + possibleTiles?.length);
        }
    }

    if(chowTiles.length === 2){
      chowTiles.forEach(t => console.log(t.tile.tileValue))
      chow(chowTiles)
    }
    else
      console.log('cant chow because chowTiles length is ' + chowTiles.length);
    }

  const onDragEnd = (result: DropResult, provided: ResponderProvided) => {
    const { destination, draggableId } = result;
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
              player={topPlayer!}
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
              <Button loading={loading} onClick={pong}>
                Pong
              </Button>
              <Button loading={loading} onClick={doChow}>
                Chow
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
