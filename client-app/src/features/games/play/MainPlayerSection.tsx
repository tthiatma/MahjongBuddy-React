import React, { Fragment, useContext } from "react";
import { observer } from "mobx-react-lite";
import { IRoundTile, TileType, TileValue } from "../../../app/models/tile";
import { Droppable } from "react-beautiful-dnd";
import { Grid, Button, Transition, Image } from "semantic-ui-react";
import PlayerAction from "./PlayerAction";
import PlayerStatus from "./PlayerStatus";
import DraggableTile from "./DraggableTile";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { runInAction, toJS } from "mobx";
import { sortTiles } from "../../../app/common/util/util";
import { IRoundPlayer } from "../../../app/models/player";

interface IProps {
  mainPlayer: IRoundPlayer | undefined;
  containerStyleName: string;
  tileStyleName: string;
  mainPlayerAliveTiles: IRoundTile[] | null;
  mainPlayerGraveYardTiles: IRoundTile[] | null;
  doThrowTile: (tileId: string) => void;
}

const isPlayerFlower = (rt: IRoundTile, player: IRoundPlayer): boolean => {
  if (rt.tile.tileType !== TileType.Flower) return false;

  const userFlowerNum = player.wind + 1;
  const tv = rt.tile.tileValue;
  let isUserFlower = false;
  switch (userFlowerNum) {
    case 1: {
      if (tv === TileValue.FlowerNumericOne || tv === TileValue.FlowerRomanOne)
        isUserFlower = true;
      break;
    }
    case 2: {
      if (tv === TileValue.FlowerNumericTwo || tv === TileValue.FlowerRomanTwo)
        isUserFlower = true;
      break;
    }
    case 3: {
      if (
        tv === TileValue.FlowerNumericThree ||
        tv === TileValue.FlowerRomanThree
      )
        isUserFlower = true;
      break;
    }
    case 4: {
      if (
        tv === TileValue.FlowerNumericFour ||
        tv === TileValue.FlowerRomanFour
      )
        isUserFlower = true;
      break;
    }
  }
  return isUserFlower;
};

const MainPlayerSection: React.FC<IProps> = ({
  mainPlayer,
  containerStyleName,
  mainPlayerAliveTiles,
  mainPlayerGraveYardTiles,
  doThrowTile,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { isManualSort } = rootStore.roundStore;
  const { orderTiles } = rootStore.hubStore;

  const autoSort = async () => {
    const beforeOrderingTiles = toJS(mainPlayerAliveTiles!, {
      recurseEverything: true,
    });
    const beforeOrderingManualSortValue = toJS(
      rootStore.roundStore.isManualSort
    );

    const sortedTile = mainPlayerAliveTiles!;
    sortedTile.sort(sortTiles);
    for (let i = 0; i < sortedTile.length; i++) {
      let objIndex = rootStore.roundStore.mainPlayer!.playerTiles!.findIndex(
        (obj) => obj.id === sortedTile[i].id
      );

      runInAction("updating reordered tile", () => {
        rootStore.roundStore.mainPlayer!.playerTiles![objIndex].activeTileCounter = i;
      });
    }
    runInAction("autosort", () => {
      rootStore.roundStore.isManualSort = false;
    });

    await orderTiles(
      sortedTile,
      beforeOrderingTiles,
      beforeOrderingManualSortValue
    );
  };

  return (
    <Grid id="mainPlayerTiles">
      <Grid.Column width={3}>
        <Grid.Row centered style={{ padding: "1px" }}>
          <div
            style={{
              display: "flex",
              flexWrap: "wrap",
              alignItems: "flex-start",
              alignContent: "flex-start",
            }}
          >
            {mainPlayerGraveYardTiles &&
              mainPlayerGraveYardTiles.map((rt) => (
                <Fragment key={rt.id}>
                  {rt.tile.tileType === TileType.Flower ? (
                    <Transition
                      transitionOnMount={true}
                      animation="shake"
                      duration={1000}
                    >
                      <Image
                        {...(isPlayerFlower(rt, mainPlayer!) && {
                          className: "goodFlowerTile",
                        })}
                        alt={rt.tile.title}
                        src={rt.tile.imageSmall}
                      />
                    </Transition>
                  ) : (
                    <Image alt={rt.tile.title} src={rt.tile.imageSmall} />
                  )}
                </Fragment>
              ))}
          </div>
        </Grid.Row>
      </Grid.Column>
      <Grid.Column width={10}>
        <Grid.Row>
          <Droppable droppableId="tile" direction="horizontal">
            {(provided, snapshot) => (
              <Fragment>
                <DraggableTile
                  doThrowTile={doThrowTile}
                  containerStyleName={containerStyleName}
                  tiles={mainPlayerAliveTiles!}
                  droppableSnapshot={snapshot}
                  droppableProvided={provided}
                />
              </Fragment>
            )}
          </Droppable>
        </Grid.Row>
        <Grid.Row>
          <div
            style={{ borderRadius: "25px" }}
            className="playerStatusContainer"
            {...(mainPlayer!.isMyTurn && mainPlayer!.mustThrow && {
              className: "mustThrow playerStatusContainer",
            })}
            {...(mainPlayer!.isMyTurn && !mainPlayer!.mustThrow && {
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
                {isManualSort && (
                  <Button
                    className="actionButton"
                    circular
                    color="green"
                    onClick={autoSort}
                    content="SORT"
                  />
                )}

                <PlayerStatus player={mainPlayer} />
              </span>
            )}
          </div>
        </Grid.Row>
      </Grid.Column>
      <Grid.Column width={3}>
        <PlayerAction />
      </Grid.Column>
    </Grid>
  );
};
export default observer(MainPlayerSection);
