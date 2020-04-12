import React, { useContext, useEffect } from "react";
import { Grid } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { RootStoreContext } from "../../../app/stores/rootStore";
import GameLobbyChat from "./GameLobbyChat";
import GameLobbyHeader from "./GameLobbyHeader";
import GameLobbySidebar from "./GameLobbySidebar";

interface DetailParams {
  id: string;
}

const GameLobby: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { game, loadGame, loadingInitial } = rootStore.gameStore;
  const {
    stopHubConnection,
    createHubConnection,
    loading,
    leaveGroup
  } = rootStore.hubStore;
  useEffect(() => {
    loadGame(match.params.id);
    createHubConnection(match.params!.id);
    return () => {
      leaveGroup(match.params.id);
    };
  }, [
    createHubConnection,
    stopHubConnection,
    loadGame,
    match.params,
    match.params.id,
  ]);

  if (loadingInitial || !game || loading)
    return <LoadingComponent content="Loading game..." />;

  if (!game) return <h2>Game not found</h2>;

  return (
    <Grid>
      <Grid.Column width={8}>
        <GameLobbyHeader game={game} />
        <GameLobbyChat />
      </Grid.Column>
      <Grid.Column width={8}>
        <GameLobbySidebar players={game.players} />
      </Grid.Column>
    </Grid>
  );
};

export default observer(GameLobby);
