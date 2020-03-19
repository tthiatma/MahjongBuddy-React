import React, { useContext, useEffect } from "react";
import { Grid } from "semantic-ui-react";
import GameList from "./GameList";
import { observer } from "mobx-react-lite";
import GameStore from "../../../app/stores/gameStore";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";

const GameDashboard: React.FC = () => {
  const gameStore = useContext(GameStore);
  useEffect(() => {
    gameStore.loadGames();
  }, [gameStore]);

  if (gameStore.loadingInitial)
    return <LoadingComponent content="Loading games..." />;
    
  return (
    <Grid>
      <Grid.Column width={10}>
        <GameList />
      </Grid.Column>
      <Grid.Column width={6}>
        <h2>Game filters</h2>
      </Grid.Column>
    </Grid>
  );
};

export default observer(GameDashboard);
