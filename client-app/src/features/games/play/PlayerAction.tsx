import React, { Fragment, useContext, useState, SyntheticEvent } from "react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { IRoundTile, TileType } from "../../../app/models/tile";
import {
  GetPossibleKong,
  GetPossibleChow,
} from "../../../app/common/util/tileHelper";
import { toJS } from "mobx";
import _ from "lodash";
import { toast } from "react-toastify";
import {
  Button,
  Card,
  Image,
  CardProps,
  Transition,
  Container,
} from "semantic-ui-react";

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
    hubActionLoading,
    winRound,
    skipAction,
  } = rootStore.hubStore;

  const [kongOptions, setKongOptions] = useState<any[]>([]);
  const [chowOptions, setChowOptions] = useState<any[]>([]);

  const buttonAnimation = "jiggle";
  const buttonAnimationDuration = 500;

  const clearKongOptions = () => {
    setKongOptions([]);
  };

  const clearChowOptions = () => {
    setChowOptions([]);
  };

  const doPong = () => {
    clearChowOptions();
    pong();
  };

  const selectTilesToKong = (event: SyntheticEvent, data: any) => {
    try {
      kong(data.kongtiles[0], data.kongtiles[1]);
      clearKongOptions();
    } catch (ex) {
      toast.error(`failed konging`);
    }
  };

  const doChow = () => {
    let boardActiveTile = rootStore.roundStore.boardActiveTile;

    //if board tile is dragon or wind then user cant chow
    if (
      boardActiveTile?.tile.tileType === TileType.Dragon ||
      boardActiveTile?.tile.tileType === TileType.Wind ||
      boardActiveTile?.tile.tileType === TileType.Flower
    ) {
      toast.error(`can't chow`);
      return;
    }

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
      clearChowOptions();
    } catch (ex) {
      toast.error(`failed chowing`);
    }
  };

  const doKong = () => {
    let validTileForKongs: IRoundTile[] = GetPossibleKong(
      mainPlayer!.isMyTurn,
      mainPlayerTiles!,
      boardActiveTile!
    );
    clearChowOptions();

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
    <Container>
      {kongOptions.length > 0 && (
        <Card.Group centered itemsPerRow={4} items={kongOptions} />
      )}

      {chowOptions.length > 0 && (
        <Card.Group centered itemsPerRow={3} items={chowOptions} />
      )}

      {(hasWinAction || hasSelfWinAction) && (
        <Transition
          transitionOnMount={true}
          animation={buttonAnimation}
          duration={buttonAnimationDuration}
        >
          <Button
            className="actionButton"
            circular
            color="green"
            disabled={round!.isOver}
            loading={hubActionLoading}
            onClick={winRound}
            content="WIN"
          />
        </Transition>
      )}

      {mainPlayer?.hasAction && hasChowAction && chowOptions.length === 0 && (
        <Transition
          transitionOnMount={true}
          animation={buttonAnimation}
          duration={buttonAnimationDuration}
        >
          <Button
            className="actionButton"
            circular
            color="olive"
            disabled={mainPlayer!.mustThrow || round!.isOver}
            loading={hubActionLoading}
            onClick={doChow}
            content="CHOW"
          />
        </Transition>
      )}

      {chowOptions.length > 1 && (
        <Button onClick={clearChowOptions} content="Cancel CHOW" />
      )}

      {kongOptions.length > 1 && (
        <Button onClick={clearKongOptions} content="Cancel KONG" />
      )}

      {mainPlayer?.hasAction && hasPongAction && (
        <Transition
          transitionOnMount={true}
          animation={buttonAnimation}
          duration={buttonAnimationDuration}
        >
          <Button
            className="actionButton"
            circular
            color="orange"
            disabled={mainPlayer!.mustThrow || round!.isOver}
            loading={hubActionLoading}
            onClick={doPong}
            content="PONG"
          />
        </Transition>
      )}

      {((mainPlayer!.hasAction && hasKongAction) || hasSelfKongAction) && !round!.isEnding && (
        <Transition
          transitionOnMount={true}
          animation={buttonAnimation}
          duration={buttonAnimationDuration}
        >
          <Button
            className="actionButton"
            circular
            color="violet"
            disabled={round!.isOver}
            loading={hubActionLoading}
            onClick={doKong}
            content="KONG"
          />
        </Transition>
      )}

      {mainPlayer?.hasAction && !mainPlayer!.mustThrow && (
        <Transition
          transitionOnMount={true}
          animation={buttonAnimation}
          duration={buttonAnimationDuration}
        >
          <Button
            circular
            disabled={round!.isOver}
            loading={hubActionLoading}
            onClick={skipAction}
            content="SKIP"
          />
        </Transition>
      )}

      {hasGiveUpAction && !round!.isEnding && !mainPlayer!.mustThrow && (
        <Button.Group>
          <Button
            circular
            color="grey"
            disabled={round!.isOver}
            loading={hubActionLoading}
            onClick={endingRound}
            content="GIVE UP"
          />
          <Button.Or />
          <Button
            circular
            color="brown"
            disabled={round!.isOver}
            loading={hubActionLoading}
            onClick={pickTile}
            content="PICK"
          />
        </Button.Group>
      )}

      {!showResult && round!.isOver && (
        <Button onClick={openModal}>Result</Button>
      )}
    </Container>
  );
};

export default observer(PlayerAction);
