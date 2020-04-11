import React, { useContext, useEffect } from "react";
import { Grid, Label, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import TileList from "./TileList";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { WindDirection } from "../../../app/models/windEnum";
import { GetOtherUserTiles } from "../../../app/common/util/util";

interface DetailParams {
  roundId: string;
}

const GameOn: React.FC<RouteComponentProps<DetailParams>> = ({
  match
}) => {
        const rootStore = useContext(RootStoreContext);
        const {
          round,
          loadRound,
          loadingInitial,
          loading,
          throwTile
        } = rootStore.gameStore;
        const { user } = rootStore.userStore;

        const currentPlayerTiles = round?.roundTiles.filter(
          (rt) => rt.owner === user?.userName
        );
        useEffect(() => {
          loadRound(parseInt(match.params.roundId));          
        }, [loadRound, match.params.roundId]);

        if (loadingInitial || !round || loading)
          return <LoadingComponent content="Loading round..." />;

        return (
          <Grid>
            {/* Top Player */}
            <Grid.Row className="zeroPadding">
              <Grid.Column width={3} />
              <Grid.Column width={10}>
                <TileList
                  tileStyleName="tileHorizontal"
                  containerStyleName="tileHorizontalContainer"
                  roundTiles={GetOtherUserTiles(round, "top")}
                />
              </Grid.Column>
              <Grid.Column width={3} />
            </Grid.Row>

            <Grid.Row className="zeroPadding">
              {/* Left Player */}
              <Grid.Column width={1}></Grid.Column>
              <Grid.Column width={1}></Grid.Column>
              <Grid.Column width={1}>
                <TileList
                  tileStyleName="tileVertical"
                  containerStyleName="tileVerticalContainer rotate90"
                  roundTiles={GetOtherUserTiles(round, "left")}
                />
              </Grid.Column>

              {/* Board */}
              <Grid.Column width={10}>
                <Label>Wind: {WindDirection[round.wind]}</Label>
                <Label>
                  Current User Wind:{" "}
                  {WindDirection[round.currentRoundPlayer.wind]}
                </Label>
              </Grid.Column>

              {/* Right Player */}
              <Grid.Column width={1}>
                <TileList
                  tileStyleName="tileVertical"
                  containerStyleName="tileVerticalContainer rotateMinus90"
                  roundTiles={GetOtherUserTiles(round, "right")}
                />
                <Grid.Column width={1}></Grid.Column>
                <Grid.Column width={1}></Grid.Column>
              </Grid.Column>
            </Grid.Row>

            {/* Main Player */}
            <Grid.Row className="zeroPadding">
              <Grid.Column width={3} />
              <Grid.Column width={10}>
                <Button loading={loading} onClick={throwTile}>
                  Throw
                </Button>
                <TileList
                  tileStyleName="tileHorizontal"
                  containerStyleName="tileHorizontalContainer"
                  roundTiles={currentPlayerTiles!}
                />
              </Grid.Column>
              <Grid.Column width={3} />
            </Grid.Row>
          </Grid>
        );
      };

export default observer(GameOn);
