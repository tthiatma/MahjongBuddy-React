import React, { useContext, useEffect, Fragment } from "react";
import { Grid, Container } from "semantic-ui-react";
import GameList from "./GameList";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import GameListItemPlaceholder from "./GameListItemPlaceholder";
import NavBar from "../../nav/NavBar";

const GameDashboard: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { loadGames, loadingGameInitial: loadingInitial } = rootStore.gameStore;

  useEffect(() => {
    loadGames();
  }, [loadGames]);

  return (
    <Fragment>
      <NavBar />
      <Container style={{ paddingTop: "5em" }}>
        <Grid>
          <Grid.Column width={10}>
            {loadingInitial ? <GameListItemPlaceholder /> : <GameList />}
          </Grid.Column>
          <Grid.Column width={6}>
          </Grid.Column>
        </Grid>
      </Container>
    </Fragment>
  );
};

export default observer(GameDashboard);
