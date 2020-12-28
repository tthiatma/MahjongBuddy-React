import React, { useContext, useEffect, Fragment } from "react";
import { Grid } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { RootStoreContext } from "../../../app/stores/rootStore";
import GameLobbyChat from "./GameLobbyChat";
import GameLobbyHeader from "./GameLobbyHeader";
import GameLobbySidebar from "./GameLobbySidebar";
import GameLobbyInfo from "./GameLobbyInfo";

interface DetailParams {
  code: string;
}

const GameLobby: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    game,
    latestRound,
    loadGame,
    getLatestRound,
    loadingLatestRoundInitial,
    loadingGameInitial,
  } = rootStore.gameStore;
    const { createHubConnection, hubLoading, leaveGroup } = rootStore.hubStore;

  useEffect(() => {
    getLatestRound(match.params.code);
  }, [getLatestRound, match.params.code]);

  useEffect(() => {
    loadGame(match.params.code).then(() => {
      createHubConnection(match.params.code);
    });
    return () => {
      leaveGroup(match.params.code);
    }
  }, [loadGame,createHubConnection, leaveGroup, match.params.code]);

  if (loadingGameInitial || loadingLatestRoundInitial || !game || hubLoading)
    return <LoadingComponent content="Loading game..." />;

  if (!game) return <h2>Game not found</h2>;

  return (
    <Fragment>
      <Grid>
        <Grid.Column width={8}>
          <GameLobbyHeader game={game} latestRound={latestRound} />
          <GameLobbyInfo game={game} />
          <GameLobbyChat />
        </Grid.Column>
        <Grid.Column width={8}>
          <GameLobbySidebar players={game.players} />
        </Grid.Column>
      </Grid>
    </Fragment>
  );
};

export default observer(GameLobby);
