import React, { useContext, useEffect } from "react";
import { Grid, Label } from "semantic-ui-react";
import GameStore from "../../../app/stores/gameStore";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import TileList from "../../tiles/TileList";

interface DetailParams {
  id: string;
}

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({
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
      {/* Top Player */}
      <Grid.Row>
        <Grid.Column width={3} />
        <Grid.Column width={10}>
          <Label>Top Player</Label>
        </Grid.Column>
        <Grid.Column width={3} />
      </Grid.Row>

      <Grid.Row>
        {/* Left Player */}
        <Grid.Column width={3}>
          <Label>Left Player</Label>
        </Grid.Column>

        {/* Board */}
        <Grid.Column>
          <Label>Board</Label>
        </Grid.Column>

        {/* Right Player */}
        <Grid.Column width={3}>
          <Label>Right Player</Label>
        </Grid.Column>
      </Grid.Row>

      {/* Main Player */}
      <Grid.Row>
        <Grid.Column width={3} />
        <Grid.Column width={10}>
          <Label>Main Player</Label>
        </Grid.Column>
        <Grid.Column width={3} />
      </Grid.Row>
    </Grid>
  );
};

export default observer(GameOn);
