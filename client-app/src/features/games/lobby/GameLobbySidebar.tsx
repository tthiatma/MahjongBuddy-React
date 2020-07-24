import React, { Fragment } from "react";
import {
  Segment,
  Image,
  Grid,
  Header,
} from "semantic-ui-react";
import { IPlayer } from "../../../app/models/game";
import { observer } from "mobx-react-lite";
import { WindDirection } from "../../../app/models/windEnum";
import GameLobbySeat from "./GameLobbySeat";

interface IProps {
  players: IPlayer[];
}

const GameLobbySidebar: React.FC<IProps> = () => {
  return (
    <Fragment>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>Table</Header>
      </Segment>
      <Segment attached>
        <Grid verticalAlign="middle" centered>
          <Grid.Row>
            <Grid.Column width={4}></Grid.Column>
            <Grid.Column textAlign="center" width={8}>
              <GameLobbySeat wind={WindDirection.South} />
            </Grid.Column>
            <Grid.Column width={4}></Grid.Column>
          </Grid.Row>

          <Grid.Row>
            <Grid.Column width={4} textAlign="center">
              <GameLobbySeat wind={WindDirection.West} />
            </Grid.Column>
            <Grid.Column width={8}>
              <Image size="medium" src={"/assets/wind_indicator.png"} />
            </Grid.Column>
            <Grid.Column width={4} textAlign="center">
              <GameLobbySeat wind={WindDirection.East} />
            </Grid.Column>
          </Grid.Row>

          <Grid.Row>
            <Grid.Column width={4}></Grid.Column>
            <Grid.Column textAlign="center" width={8}>
              <GameLobbySeat wind={WindDirection.North} />
            </Grid.Column>
            <Grid.Column width={4}></Grid.Column>
          </Grid.Row>
        </Grid>
      </Segment>
    </Fragment>
  );
};

export default observer(GameLobbySidebar);
