import React, { useContext, useEffect, Fragment } from "react";
import { Grid } from "semantic-ui-react";
import GameList from "./GameList";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import GameListItemPlaceholder from "./GameListItemPlaceholder";

const GameDashboard: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { loadGames, loadingGameInitial: loadingInitial } = rootStore.gameStore;

  useEffect(() => {
    loadGames();
  }, [loadGames]);

  return (
    <Fragment>
      <Grid>
        <Grid.Column width={10}>
          {loadingInitial ? <GameListItemPlaceholder /> : <GameList />}
        </Grid.Column>
        <Grid.Column width={6}></Grid.Column>
      </Grid>
    </Fragment>
  );
};

export default observer(GameDashboard);
