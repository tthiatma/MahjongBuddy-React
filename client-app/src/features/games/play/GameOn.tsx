import React, { useContext, useEffect, Fragment } from "react";
import { Grid, Label, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import TileList from "./TileList";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";
import _ from "lodash";
import TileListBoard from "./TileListBoard";
import { TileStatus } from "../../../app/models/tileStatus";

interface DetailParams {
  roundId: string;
  id: string;
}

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const { user } = rootStore.userStore;
  const { loadingGameInitial, loadGame, game } = rootStore.gameStore;
  const {
    loadingRoundInitial,
    round,
    loadRound,
    roundTiles,
  } = rootStore.roundStore;
  const {
    throwTile,
    loading,
    createHubConnection,
    stopHubConnection,
    leaveGroup,
  } = rootStore.hubStore;

  const currentPlayerTiles = roundTiles
    ? roundTiles.filter((rt) => rt.owner === user?.userName)
    : null;

  const boardActiveTile = roundTiles
    ? roundTiles.find((rt) => rt.status === TileStatus.BoardActive)
    : null;

  const boardGraveyardTiles = roundTiles
    ? roundTiles.filter((rt) => rt.status === TileStatus.BoardGraveyard)
    : null;

  const leftPlayerTiles =
    roundTiles && round && round.leftPlayer
      ? roundTiles.filter((rt) => rt.owner === round?.leftPlayer?.userName)
      : null;

  const topPlayerTiles =
    roundTiles && round && round.topPlayer
      ? roundTiles.filter((rt) => rt.owner === round?.topPlayer?.userName)
      : null;

  const rightPlayerTiles =
    roundTiles && round && round.rightPlayer
      ? roundTiles.filter((rt) => rt.owner === round?.rightPlayer?.userName)
      : null;

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
    match.params,
    match.params.id,
    match.params.roundId,
  ]);

  if (loadingGameInitial || loadingRoundInitial || !game || !round || loading)
    return <LoadingComponent content="Loading round..." />;

  return (
    <Grid>
      {/* Top Player */}
      <Grid.Row className="zeroPadding">
        <Grid.Column width={3} />
        <Grid.Column width={10}>
          {round && round.topPlayer && (
            <Label>{round.topPlayer.userName}</Label>
          )}
          <TileList
            tileStyleName="tileHorizontal"
            containerStyleName="tileHorizontalContainer"
            roundTiles={topPlayerTiles!}
          />
        </Grid.Column>
        <Grid.Column width={3} />
      </Grid.Row>

      <Grid.Row className="zeroPadding">
        {/* Left Player */}
        <Grid.Column width={1}></Grid.Column>
        <Grid.Column width={1}>
        {round && round.leftPlayer && (
            <Label>{round.leftPlayer.userName}</Label>
          )}
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
          {round && <div><Label>Wind: {WindDirection[round.wind]}</Label></div>}
          {round && round.mainPlayer && (
            <div>
            <Label>
              Current User Wind: {WindDirection[round.mainPlayer.wind]}
            </Label>
            </div>
          )}
          {boardActiveTile && <div><img src={boardActiveTile.tile.imageSmall} /></div>}
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
          <Grid.Column width={1}>
          {round && round.rightPlayer && (
            <Label>{round.rightPlayer.userName}</Label>
          )}
          </Grid.Column>
          <Grid.Column width={1}></Grid.Column>
        </Grid.Column>
      </Grid.Row>

      {/* Main Player */}
      <Grid.Row className="zeroPadding">
        <Grid.Column width={3} />
        <Grid.Column width={10}>
          <Button loading={loading} onClick={throwTile}>
            Throw
          </Button>
          <TileList
            tileStyleName="tileHorizontal"
            containerStyleName="tileHorizontalContainer"
            roundTiles={currentPlayerTiles!}
          />
        </Grid.Column>
        <Grid.Column width={3} />
      </Grid.Row>
    </Grid>
  );
};

export default observer(GameOn);
