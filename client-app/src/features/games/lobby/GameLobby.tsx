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
  const { game, latestRound, loadGame, getLatestRound, loadingLatestRoundInitial, loadingGameInitial } = rootStore.gameStore;
  const {
    createHubConnection,
    loading,
    leaveGroup
  } = rootStore.hubStore;

  useEffect(() => {
    createHubConnection(match.params!.id);
    return () => {
      leaveGroup(match.params.id);
    };
  }, [createHubConnection, leaveGroup, match.params]);

  useEffect(() => {
    getLatestRound(match.params!.id);
  }, [getLatestRound, match.params, match.params.id]);


  useEffect(() => {
    loadGame(match.params!.id);
  }, [loadGame, match.params, match.params.id]);


  if (loadingGameInitial || loadingLatestRoundInitial || !game || loading)
    return <LoadingComponent content="Loading game..." />;

  if (!game) return <h2>Game not found</h2>;

  return (
    <Grid>
      <Grid.Column width={8}>
        <GameLobbyHeader game={game} latestRound={latestRound} />
        <GameLobbyChat />
      </Grid.Column>
      <Grid.Column width={8}>
        <GameLobbySidebar players={game.players} />
      </Grid.Column>
    </Grid>
  );
};

export default observer(GameLobby);
