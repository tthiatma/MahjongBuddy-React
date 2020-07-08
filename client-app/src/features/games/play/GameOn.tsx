import React, { useContext, useEffect, useState, SyntheticEvent } from "react";
import {
  Grid,
  Button,
  Card,
  Image,
  CardProps,
  Header,
  Item,
  Divider,
  Segment,
  Label,
} from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";
import TileListBoard from "./TileListBoard";
import TileListMainPlayer from "./TileListMainPlayer";
import TileListOtherPlayer from "./TileListOtherPlayer";
import { DragDropContext, DropResult, Droppable } from "react-beautiful-dnd";
import { toJS, runInAction } from "mobx";
import { IRoundTile, TileValue, TileSetGroup } from "../../../app/models/tile";
import _ from "lodash";
import { TileStatus } from "../../../app/models/tileStatus";
import TileListOtherPlayerVertical from "./TileListOtherPlayerVertical";
import ResultModal from "./ResultModal";

interface DetailParams {
  roundId: string;
  id: string;
}

//https://github.com/clauderic/react-sortable-hoc

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const { loadingGameInitial, loadGame, game } = rootStore.gameStore;
  const {
    canPick,
    pickCounter,
    loadingRoundInitial,
    roundSimple: round,
    loadRound,
    mainPlayer,
    leftPlayer,
    rightPlayer,
    topPlayer,
    mainPlayerTiles,
    mainPlayerActiveTiles,
    mainPlayerAliveTiles,
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
    pickTile,
    pong,
    kong,
    chow,
    loading,
    createHubConnection,
    stopHubConnection,
    leaveGroup,
    winRound,
    endingRound,
    tiedRound,
    throwAllTile,
  } = rootStore.hubStore;

  //currently only support one winner
  const [chowOptions, setChowOptions] = useState<any[]>([]);
  const square = { width: 50, height: 50, padding: "0.5em" };
  const getStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "lightblue" : "lightgrey",
    display: "flex",
    overflow: "auto",
    transitionDuration: `0.001s`,
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
    loadRound(parseInt(match.params.roundId));
  }, [loadRound, match.params.roundId]);

  if (loadingGameInitial || loadingRoundInitial || !game || !round || loading)
    return <LoadingComponent content="Loading round..." />;

    const cancelEnding = () => {
      runInAction(() => {
        rootStore.roundStore.roundSimple!.isEnding = false;
      })
    }
  const doChow = () => {
    let chowTilesOptions: Array<IRoundTile[]> = [];

    let boardActiveTile = rootStore.roundStore.boardActiveTile;

    let sameTypeChowTiles = rootStore.roundStore.mainPlayerActiveTiles?.filter(
      (t) =>
        t.tile.tileType === boardActiveTile?.tile.tileType &&
        t.tile.tileValue !== boardActiveTile?.tile.tileValue
    );

    if (boardActiveTile?.tile.tileValue === TileValue.One) {
      const tileTwo = sameTypeChowTiles?.find(
        (t) => t.tile.tileValue === TileValue.Two
      );
      const tileThree = sameTypeChowTiles?.find(
        (t) => t.tile.tileValue === TileValue.Three
      );

      if (tileTwo && tileThree) {
        let twoThreeArray: IRoundTile[] = [tileTwo, tileThree];
        chowTilesOptions.push(twoThreeArray);
      }
    } else if (boardActiveTile?.tile.tileValue === TileValue.Nine) {
      console.log("board tile is 9");
      const tileSeven = sameTypeChowTiles?.find(
        (t) => t.tile.tileValue === TileValue.Seven
      );
      const tileEight = sameTypeChowTiles?.find(
        (t) => t.tile.tileValue === TileValue.Eight
      );

      if (tileSeven && tileEight) {
        let sevenEightArray: IRoundTile[] = [tileSeven, tileEight];
        chowTilesOptions.push(sevenEightArray);
      }
    } else {
      let possibleTiles = sameTypeChowTiles?.filter(
        (t) =>
          t.tile.tileValue === boardActiveTile!.tile.tileValue - 2 ||
          t.tile.tileValue === boardActiveTile!.tile.tileValue + 2 ||
          t.tile.tileValue === boardActiveTile!.tile.tileValue + 1 ||
          t.tile.tileValue === boardActiveTile!.tile.tileValue - 1
      );

      if (possibleTiles && possibleTiles.length > 1) {
        //remove dups
        possibleTiles = _.uniqWith(
          possibleTiles,
          (a, b) =>
            a.tile.tileType === b.tile.tileType &&
            a.tile.tileValue === b.tile.tileValue
        );

        for (var i = 0; i < possibleTiles.length; i++) {
          let t = possibleTiles[i];
          let testChowTiles: IRoundTile[] = [];
          testChowTiles.push(t);
          testChowTiles.push(boardActiveTile!);
          testChowTiles.sort((a, b) => a!.tile.tileValue - b!.tile.tileValue);

          let probableTile;
          if ((t.tile.tileValue + boardActiveTile!.tile.tileValue) % 2 !== 0) {
            //then these two tiles is connected
            probableTile = possibleTiles.find(
              (t) => t.tile.tileValue === testChowTiles[1].tile.tileValue + 1
            );
          } else {
            //then these two tiles is not connected
            probableTile = possibleTiles.find(
              (t) => t.tile.tileValue === testChowTiles[0].tile.tileValue + 1
            );
          }
          if (probableTile) {
            chowTilesOptions.push(
              [probableTile, t].sort(
                (a, b) => a!.tile.tileValue - b!.tile.tileValue
              )
            );
          }
        }
      } else {
        console.log(
          "not possible to chow tiles because possible chow tiles is " +
            possibleTiles?.length
        );
      }
    }
    //remove dups
    chowTilesOptions = _.uniqWith(chowTilesOptions, (a, b) => {
      let foundDups = true;
      var firsta = _.find(b, function (t) {
        return t.tile.tileValue === a[0].tile.tileValue;
      });
      var seconda = _.find(b, function (t) {
        return t.tile.tileValue === a[1].tile.tileValue;
      });

      if (!firsta || !seconda) foundDups = false;

      return foundDups;
    });

    if (chowTilesOptions.length === 1) {
      const ct = chowTilesOptions[0];
      chow([ct[0].id, ct[1].id]);
    } else if (chowTilesOptions.length > 1) {
      let cardDisplay: CardProps[] = [];
      _.forEach(chowTilesOptions, function (tileSet) {
        let cardObj: CardProps = {};
        const tileOne = tileSet[0];
        const tileTwo = tileSet[1];
        cardObj.key = tileOne.id;
        cardObj.className = "raised";
        cardObj.content = (
          <div className="content">
            <Image src={tileOne.tile.image} />
            <Image src={tileTwo.tile.image} />
          </div>
        );
        cardObj.onClick = selectTilesToChow;
        cardObj.chowtiles = [tileOne.id, tileTwo.id];
        cardDisplay.push(cardObj);
      });
      setChowOptions(cardDisplay);
    }
  };

  const selectTilesToChow = (event: SyntheticEvent, data: any) => {
    console.log("selected tiles to chow");
    console.log(data);
    try {
      console.log("chowing");
      console.log(data.chowtiles);
      chow(data.chowtiles);
      setChowOptions([]);
    } catch (ex) {
      console.log("failed chowing");
    }
  };

  const doKong = () => {
    let validTileForKongs: IRoundTile[] = [];
    let mpt = mainPlayerTiles?.map((t) => toJS(t));

    var kongTiles = _.chain(mpt)
      .groupBy((asd) => `${asd.tile.tileType}-${asd.tile.tileValue}`)
      .filter((asd) => asd.length > 2)
      .value();

    _.forEach(kongTiles, function (ts) {
      if (ts.length === 3) {
        //if player has 3 same tiles and not in graveyard, they can kong matching board active tile anytime even if its not their turn
        let userActive = _.filter(
          ts,
          (t) => t.status !== TileStatus.UserGraveyard
        );
        //console.log(userActive);
        if (
          userActive.length === 3 &&
          boardActiveTile &&
          userActive[0].tile.tileType === boardActiveTile.tile.tileType &&
          userActive[0].tile.tileValue === boardActiveTile.tile.tileValue
        )
          validTileForKongs.push(boardActiveTile);
      }

      if (ts.length === 4) {
        //if its player's turn
        if (mainPlayer && mainPlayer.isMyTurn) {
          //if player has 3 same tiles and its already ponged, player can kong only from their active and just picked tile
          let pongedTile = _.filter(
            ts,
            (t) => t.tileSetGroup === TileSetGroup.Pong
          );
          if (pongedTile.length === 3) {
            //check if user active tile
            let tileIsAlive = _.filter(
              mainPlayerAliveTiles,
              (t) =>
                t.tile.tileValue === pongedTile[0].tile.tileValue &&
                t.tile.tileType === pongedTile[0].tile.tileType
            );

            if (tileIsAlive.length > 0) validTileForKongs.push(tileIsAlive[0]);
          }

          //if tile is not in graveyard, then player can kong when its his turn
          let allTileAlive = _.filter(
            ts,
            (t) => t.status !== TileStatus.UserGraveyard
          );
          if (allTileAlive.length === 4)
            validTileForKongs.push(allTileAlive[0]);
        }
      }
    });
    _.forEach(validTileForKongs, (t) => console.log(t));

    if (validTileForKongs.length === 1) {
      console.log("Valid tile to kong");
      console.log(toJS(validTileForKongs[0]));
      let kt = toJS(validTileForKongs[0]);
      kong(kt.tile.tileType, kt.tile.tileValue);
    } else {
      //display option which tile to kong
    }

    // kong only possible when there are 3 or 4 tiles in user posession
    // let sameTileType:any = _.groupBy(asd, 'tile.tileType');
    // let rawr = _.forEach(sameTileType, function(value, key) {
    //   sameTileType[key] = _.groupBy(sameTileType[key], 'tile.tileValue');
    // });
    // _.forEach(rawr, (v, k) => console.log(v))
    //check board active tile where any tile that can be kong

    //check if its user turn

    //if its user's turn, its possible that it could be multiple kong

    //TODO: present options which tile to kong
  };

  const onDragEnd = (result: DropResult) => {
    const { destination, draggableId } = result;
    if (!destination) {
      return;
    }

    if (destination.droppableId === "board")
      if (mainPlayerActiveTiles) {
        runInAction("throwingtile", () => {
          rootStore.roundStore.selectedTile = toJS(
            roundTiles!.find((t) => t.id === draggableId)!
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
                        <Header as="h3">{roundEndingCounter}</Header>
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
            />
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
          <Grid.Column width={3}>
            <TileListOtherPlayerVertical
              player={rightPlayer!}
              roundTiles={rightPlayerTiles!}
              isReversed={true}
            />
          </Grid.Column>
        </Grid.Row>

        {/* Main Player */}
        <Grid.Row className="zeroPadding">
          <Grid.Column width={3} />
          <Grid.Column width={10}>
            <Grid.Row>
              <TileListMainPlayer
                tileStyleName="tileHorizontal"
                containerStyleName="tileHorizontalContainer"
                mainPlayerGraveYardTiles={mainPlayerGraveYardTiles!}
                mainPlayerActiveTiles={mainPlayerActiveTiles!}
                mainPlayerJustPickedTile={mainPlayerJustPickedTile!}
              />
            </Grid.Row>

            <Grid.Row>
              <ResultModal
                roundResults={roundResults}
                roundTiles={roundTiles}
              />

              {chowOptions.length > 0 && (
                <Card.Group centered itemsPerRow={3} items={chowOptions} />
              )}

              <Button loading={loading} onClick={throwAllTile}>
                Throw All
              </Button>
              <Button loading={loading} onClick={throwTile}>
                Throw
              </Button>
              <Button disabled={!canPick} loading={loading} onClick={pickTile}>
                Pick
                {pickCounter > 0 && `(${pickCounter})`}
              </Button>
              <Button loading={loading} onClick={pong}>
                Pong
              </Button>
              <Button loading={loading} onClick={doChow}>
                Chow
              </Button>
              <Button loading={loading} onClick={doKong}>
                Kong
              </Button>
              <Button loading={loading} onClick={winRound}>
                Win
              </Button>
              <Button disabled={!canPick} loading={loading} onClick={endingRound}>
                Give Up
              </Button>
              <Button loading={loading} onClick={tiedRound}>
                Over
              </Button>
              <Button onClick={cancelEnding}>
                Cancel Ending
              </Button>

              {/* {remainingTiles === 1 &&
                mainPlayerJustPickedTile!.length === 0 &&
                mainPlayer!.isMyTurn && (
                  <Button loading={loading} onClick={endRound}>
                    Give Up
                  </Button>
                )} */}
            </Grid.Row>
            <Grid.Row centered>
              <div
                className="playerStatusContainer"
                {...(mainPlayer!.isMyTurn && {
                  className: "playerTurn playerStatusContainer",
                })}
              >
                {mainPlayer && (
                  <span
                    style={{
                      width: "100%",
                      textAlign: "center",
                      lineHeight: "40px",
                    }}
                  >
                    {mainPlayer.userName} - {WindDirection[mainPlayer.wind]} -{" "}
                    {mainPlayer.points} - isover: {round?.isOver.toString()} -
                    isEnding: {round?.isEnding.toString()}
                    myTurn: {mainPlayer!.isMyTurn.toString()}
                  </span>
                )}
              </div>
            </Grid.Row>
          </Grid.Column>
          <Grid.Column width={3} />
        </Grid.Row>
      </Grid>
    </DragDropContext>
  );
};

export default observer(GameOn);
