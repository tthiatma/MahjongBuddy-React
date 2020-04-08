import React, { Fragment, useContext } from 'react';
import { Segment, List, Item, Label, Image, Grid } from 'semantic-ui-react';
import { IPlayer } from '../../../app/models/game';
import { observer } from 'mobx-react-lite';
import { WindDirection } from '../../../app/models/windEnum';
import { RootStoreContext } from '../../../app/stores/rootStore';
import GameLobbySeat from './GameLobbySeat';

interface IProps{
  players: IPlayer[];
}

const GameLobbySidebar: React.FC<IProps> = ({players}) => {
  const rootStore = useContext(RootStoreContext);
  const {game} = rootStore.gameStore;

  return (
    <Fragment>
      <Segment
        textAlign="center"
        style={{ border: "none" }}
        attached="top"
        secondary
        inverted
        color="teal"
      >
        {players.length} {players.length === 1 ? "Player" : "Players"} Joined
      </Segment>

      <Grid verticalAlign='middle' centered >
        <Grid.Row>
          <Grid.Column width={4}></Grid.Column>
          <Grid.Column textAlign="center" width={8}>
            <GameLobbySeat wind={WindDirection.North} />
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
            <GameLobbySeat wind={WindDirection.South} />
          </Grid.Column>
          <Grid.Column width={4}></Grid.Column>
        </Grid.Row>
      </Grid>

      <Segment attached>
        <List relaxed divided>
          {players.map((player) => (
            <Item key={player.userName} style={{ position: "relative" }}>
              {player.isHost && (
                <Label
                  style={{ position: "absolute" }}
                  color="orange"
                  ribbon="right"
                >
                  Host
                </Label>
              )}
              <Image size="tiny" src={player.image || "/assets/user.png"} />
              <Item.Content verticalAlign="middle">
                <Item.Header as="h3">
                  {/* <Link to={`/profile/${player.userName}`}> */}
                  {player.displayName}
                  {/* </Link> */}
                </Item.Header>
                {/* {
                player.initialSeatWind &&
                <Item.Extra style={{ color: 'orange' }}>Starting Wind: {WindDirection[player.initialSeatWind]}</Item.Extra>
                } */}
                <Item.Extra style={{ color: "orange" }}>
                  Seat: {WindDirection[player.initialSeatWind]}
                </Item.Extra>
              </Item.Content>
            </Item>
          ))}
        </List>
      </Segment>
    </Fragment>
  );
};

export default observer(GameLobbySidebar);
