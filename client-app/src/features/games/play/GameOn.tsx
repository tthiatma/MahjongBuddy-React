import React, { useContext, useEffect } from "react";
import { Grid, Label } from "semantic-ui-react";
import GameStore from "../../../app/stores/gameStore";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import TileList from "../../tiles/TileList";
import { RootStoreContext } from "../../../app/stores/rootStore";

interface DetailParams {
  id: string;
}

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({
  match
}) => {
  const rootStore = useContext(RootStoreContext);
  const { game, loadGame, loadingInitial } = rootStore.gameStore;

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
        <TileList
            tileStyleName="tileHorizontal"
            containerStyleName="tileHorizontalContainer"
          />
        </Grid.Column>
        <Grid.Column width={3} />
      </Grid.Row>

      <Grid.Row>
        {/* Left Player */}
        <Grid.Column width={1}></Grid.Column>
        <Grid.Column width={1}></Grid.Column>
        <Grid.Column width={1}>
          <TileList
            tileStyleName="tileVertical"
            containerStyleName="tileVerticalContainer rotate90"
          />
        </Grid.Column>

        {/* Board */}
        <Grid.Column width={10}>
          <Label>Board</Label>
        </Grid.Column>

        {/* Right Player */}
        <Grid.Column width={1}>
          <TileList
            tileStyleName="tileVertical"
            containerStyleName="tileVerticalContainer rotateMinus90"
          />
        <Grid.Column width={1}></Grid.Column>
        <Grid.Column width={1}></Grid.Column>
        </Grid.Column>
      </Grid.Row>

      {/* Main Player */}
      <Grid.Row>
        <Grid.Column width={3} />
        <Grid.Column width={10}>
          <TileList
            tileStyleName="tileHorizontal"
            containerStyleName="tileHorizontalContainer"
          />
        </Grid.Column>
        <Grid.Column width={3} />
      </Grid.Row>
    </Grid>
  );
};

export default observer(GameOn);
