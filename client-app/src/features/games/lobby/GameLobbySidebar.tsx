import React, { Fragment } from "react";
import {
  Segment,
  Image,
  Grid,
  Header,
  List,
  Item,
  Label,
} from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { WindDirection } from "../../../app/models/windEnum";
import GameLobbySeat from "./GameLobbySeat";
import { Link } from "react-router-dom";
import { IGamePlayer } from "../../../app/models/player";

interface IProps {
  players: IGamePlayer[];
}

const GameLobbySidebar: React.FC<IProps> = ({ players }) => {
  return (
    <Fragment>
      <Segment attached="top" inverted color="teal" style={{ border: "none" }}>
        <Header textAlign="center">Players {`${players.length}/4`}</Header>
      </Segment>
      <Segment attached>
        <List horizontal relaxed="very">
          {players.map((player) => (
            <Item key={player.userName} style={{ position: "relative" }}>
              <Image
                circular
                size="mini"
                src={player.image || "/assets/user.png"}
              />
              <Item.Content verticalAlign="middle">
                <Item.Header as="h3">
                  <Link to={`/profile/${player.userName}`}>
                    {player.displayName}
                  </Link>
                </Item.Header>
                {player.isHost && (
                  <Label color="orange" size="mini">
                    Host
                  </Label>
                )}
              </Item.Content>
            </Item>
          ))}
        </List>
      </Segment>
      {players.length === 4 && (
        <Fragment>
          <Segment
            attached="top"
            inverted
            color="teal"
            style={{ border: "none" }}
          >
            <Header textAlign="center">Table</Header>
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
      )}
    </Fragment>
  );
};

export default observer(GameLobbySidebar);
