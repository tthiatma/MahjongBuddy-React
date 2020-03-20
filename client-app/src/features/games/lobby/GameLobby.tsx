import React, { useContext, useEffect } from "react";
import { Grid } from "semantic-ui-react";
import GameStore from "../../../app/stores/gameStore";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";

interface DetailParams {
  id: string;
}

const GameLobby: React.FC<RouteComponentProps<DetailParams>> = ({
  match
}) => {
  const gameStore = useContext(GameStore);
  const { game, loadGame, loadingInitial } = gameStore;

  useEffect(() => {
    loadGame(match.params.id);
  }, [loadGame, match.params.id]);

  if (loadingInitial || !game)
    return <LoadingComponent content="Loading game..." />;

  return (
    <Grid>
      <Grid.Column width={10}>
          {game.id} - {game.title}
      </Grid.Column>
      <Grid.Column width={6}>
      </Grid.Column>
    </Grid>
  );
};

export default observer(GameLobby);
