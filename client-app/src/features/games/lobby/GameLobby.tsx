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
  id: string;
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
    getLatestRound(match.params.id);
  }, [getLatestRound, match.params.id]);

  useEffect(() => {
    loadGame(match.params.id).then(() => {
      createHubConnection(match.params.id);
    });
    return () => {
      leaveGroup(match.params.id);
    }
  }, [loadGame,createHubConnection, leaveGroup, match.params.id]);

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
          <GameLobbySidebar players={game.gamePlayers} />
        </Grid.Column>
      </Grid>
    </Fragment>
  );
};

export default observer(GameLobby);
