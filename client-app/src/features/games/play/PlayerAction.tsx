import React, { Fragment, useContext, useState, SyntheticEvent } from "react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { IRoundTile } from "../../../app/models/tile";
import {
  GetPossibleKong,
  GetPossibleChow,
} from "../../../app/common/util/tileHelper";
import { toJS } from "mobx";
import _ from "lodash";
import { toast } from "react-toastify";
import { Button, Card, Image, CardProps } from "semantic-ui-react";

const PlayerAction: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    openModal,
    showResult,
    roundSimple: round,
    mainPlayer,
    mainPlayerTiles,
    boardActiveTile,
    hasChowAction,
    hasPongAction,
    hasKongAction,
    hasGiveUpAction,
    hasWinAction,
    hasSelfKongAction,
    hasSelfWinAction,
  } = rootStore.roundStore;

  const {
    pickTile,
    endingRound,
    pong,
    kong,
    chow,
    loading,
    winRound,
    skipAction,
    throwAllTile
  } = rootStore.hubStore;

  const [kongOptions, setKongOptions] = useState<any[]>([]);
  const [chowOptions, setChowOptions] = useState<any[]>([]);

  const selectTilesToKong = (event: SyntheticEvent, data: any) => {
    try {
      kong(data.kongtiles[0], data.kongtiles[1]);
      setKongOptions([]);
    } catch (ex) {
      toast.error(`failed konging`);
    }
  };

  const clearChowOptions = () => {
    setChowOptions([]);
  };
  
  const doPong = () => {
    clearChowOptions();
    pong();
  };

  const doChow = () => {
    let boardActiveTile = rootStore.roundStore.boardActiveTile;
    let playerActiveTiles = rootStore.roundStore.mainPlayerActiveTiles;
    let chowTilesOptions: Array<IRoundTile[]> = GetPossibleChow(
      boardActiveTile!,
      playerActiveTiles!
    );

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
          <Fragment>
            <Image src={tileOne.tile.imageSmall} />
            <Image src={tileTwo.tile.imageSmall} />
          </Fragment>
        );
        cardObj.onClick = selectTilesToChow;
        cardObj.chowtiles = [tileOne.id, tileTwo.id];
        cardDisplay.push(cardObj);
      });
      setChowOptions(cardDisplay);
    } else {
      toast.error(`can't chow`);
    }
  };

  const selectTilesToChow = (event: SyntheticEvent, data: any) => {
    try {
      chow(data.chowtiles);
      setChowOptions([]);
    } catch (ex) {
      toast.error(`failed chowing`);
    }
  };

  const doKong = () => {
    clearChowOptions();
    let validTileForKongs: IRoundTile[] = GetPossibleKong(
      mainPlayer!.isMyTurn,
      mainPlayerTiles!,
      boardActiveTile!
    );

    if (validTileForKongs.length === 1) {
      let kt = toJS(validTileForKongs[0]);
      kong(kt.tile.tileType, kt.tile.tileValue);
    } else if (validTileForKongs.length > 1) {
      let cardDisplay: CardProps[] = [];
      _.forEach(validTileForKongs, function (rt) {
        let cardObj: CardProps = {};
        cardObj.key = rt.id;
        cardObj.className = "raised";
        cardObj.content = <Image src={rt.tile.imageSmall} />;
        cardObj.onClick = selectTilesToKong;
        cardObj.kongtiles = [rt.tile.tileType, rt.tile.tileValue];
        cardDisplay.push(cardObj);
      });
      setKongOptions(cardDisplay);
    } else {
      toast.error(`can't kong`);
    }
  };
  return (
    <Fragment>
      {kongOptions.length > 0 && (
        <Card.Group centered itemsPerRow={4} items={kongOptions} />
      )}

      {chowOptions.length > 0 && (
        <Card.Group centered itemsPerRow={3} items={chowOptions} />
      )}

      {mainPlayer?.hasAction && hasChowAction && chowOptions.length === 0 && (
        <Button
          disabled={mainPlayer!.mustThrow || round!.isOver}
          loading={loading}
          onClick={doChow}
          content="Chow"
        />
      )}

      {chowOptions.length > 1 && (
        <Button onClick={clearChowOptions} content="Cancel Chow" />
      )}

      {mainPlayer?.hasAction && hasPongAction && (
        <Button
          disabled={mainPlayer!.mustThrow || round!.isOver}
          loading={loading}
          onClick={doPong}
          content="Pong"
        />
      )}

      {((mainPlayer?.hasAction && hasKongAction) || hasSelfKongAction) && (
        <Button
          disabled={round!.isOver}
          loading={loading}
          onClick={doKong}
          content="Kong"
        />
      )}

      {mainPlayer?.hasAction && (
        <Button
          disabled={round!.isOver}
          loading={loading}
          onClick={skipAction}
          content="Skip"
        />
      )}

      {((mainPlayer?.hasAction && hasWinAction) || hasSelfWinAction) && (
        <Button
          disabled={round!.isOver}
          loading={loading}
          onClick={winRound}
          content="Win"
        />
      )}

{/* {remainingTiles === 1 &&
        mainPlayerJustPickedTile!.length === 0 &&
        mainPlayer!.isMyTurn && (
          <Button
            disabled={!canPick || round!.isOver}
            loading={loading}
            onClick={endingRound}
          >
            Give Up {pickCounter > 0 && `(${pickCounter})`}
          </Button>
        )} */}


      {hasGiveUpAction && (
        <Fragment>
        <Button
          disabled={round!.isOver}
          loading={loading}
          onClick={endingRound}
          content="Give Up"
        />
        <Button
          disabled={round!.isOver}
          loading={loading}
          onClick={pickTile}
          content="Pick"
        />
        </Fragment>
      )}



      <Button loading={loading} onClick={throwAllTile}>
        ThrowAll
      </Button>

      {/* <Button
        disabled={
          !canPick ||
          mainPlayer!.mustThrow ||
          !mainPlayer!.isMyTurn ||
          round!.isOver ||
          mainPlayerJustPickedTile!.length > 0
        }
        loading={loading}
        onClick={pickTile}
      >
        Pick
        {pickCounter > 0 && `(${pickCounter})`}
      </Button> */}
{/* 
      {`mustThrow: ${mainPlayer!.mustThrow.toString()} `}
      {`hasAction: ${mainPlayer?.hasAction.toString()} `}
      {`hasChow: ${hasChowAction.toString()} `}
      {`hasPong: ${hasPongAction.toString()} `}
      {`hasKongAction: ${hasKongAction.toString()} `}
      {`hasSelfKongAction: ${hasSelfKongAction.toString()} `}
      {`hasWinAction: ${hasWinAction.toString()} `}
      {`hasSelfWinAction: ${hasSelfWinAction.toString()} `}
      {`hasGiveUpAction: ${hasGiveUpAction.toString()} `} */}

      {!showResult && round!.isOver && (
        <Button onClick={openModal}>Result</Button>
      )}
    </Fragment>
  );
};

export default observer(PlayerAction);
