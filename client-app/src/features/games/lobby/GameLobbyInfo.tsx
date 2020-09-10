import React from "react";
import { Segment, Grid, Icon, Label } from "semantic-ui-react";
import { IGame } from "../../../app/models/game";

const GameLobbyInfo: React.FC<{ game: IGame }> = ({ game }) => {
  return (
    <Segment.Group>
      <Segment attached="top">
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="teal" name="cog" />
          </Grid.Column>
          <Grid.Column width={15}>
            <strong>
              <Label>
                {`Min Point: ${game.minPoint} pts`}
              </Label>
              <Label>
                {`Max Point: ${game.maxPoint} pts`}
              </Label>
            </strong>
          </Grid.Column>
        </Grid>
      </Segment>
    </Segment.Group>
  );
};

export default GameLobbyInfo;
