import React, { Fragment, useContext } from 'react';
import { Segment, List, Item, Label, Image, Grid, Button } from 'semantic-ui-react';
import { IPlayer } from '../../../app/models/game';
import { observer } from 'mobx-react-lite';
import { WindDirection } from '../../../app/models/windEnum';
import { RootStoreContext } from '../../../app/stores/rootStore';

interface IProps{
  players: IPlayer[];
}

const GameLobbySidebar: React.FC<IProps> = ({players}) => {
  const rootStore = useContext(RootStoreContext);
  const {connectToGame, disconnectFromGame, loading, game} = rootStore.gameStore;

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

      <Grid celled="internally">
        <Grid.Row>
          <Grid.Column width={3}></Grid.Column>
          <Grid.Column textAlign="center" width={10}>
            North Seat
            {game?.initialSeatWind === WindDirection.North && (
              <Button loading={loading} onClick={disconnectFromGame}>
                Leave
              </Button>
            )}
            {!game?.isCurrentPlayerConnected && !game?.players.some(p => p.initialSeatWind === WindDirection.North) && (
              <Button
                loading={loading}
                onClick={() => connectToGame(WindDirection.North)}
                color="teal"
              >
                Sit
              </Button>
            )}
          </Grid.Column>
          <Grid.Column width={3}></Grid.Column>
        </Grid.Row>

        <Grid.Row>
          <Grid.Column width={3}>
            West Seat
            {game?.initialSeatWind === WindDirection.West && (
              <Button loading={loading} onClick={disconnectFromGame}>
                Leave
              </Button>
            )}
            {!game?.isCurrentPlayerConnected && !game?.players.some(p => p.initialSeatWind === WindDirection.West) && (
              <Button
                loading={loading}
                onClick={() => connectToGame(WindDirection.West)}
                color="teal"
              >
                Sit
              </Button>
            )}
          </Grid.Column>
          <Grid.Column width={10}>Image showing direction</Grid.Column>
          <Grid.Column width={3}>
            East Seat
            {game?.initialSeatWind === WindDirection.East && (
              <Button loading={loading} onClick={disconnectFromGame}>
                Leave
              </Button>
            )}
            {!game?.isCurrentPlayerConnected && !game?.players.some(p => p.initialSeatWind === WindDirection.East) &&(
              <Button
                loading={loading}
                onClick={() => connectToGame(WindDirection.East)}
                color="teal"
              >
                Sit
              </Button>
            )}
          </Grid.Column>
        </Grid.Row>

        <Grid.Row>
          <Grid.Column width={3}></Grid.Column>
          <Grid.Column textAlign="center" width={10}>
            South
            {game?.initialSeatWind === WindDirection.South && (
              <Button loading={loading} onClick={disconnectFromGame}>
                Leave
              </Button>
            )}
            {!game?.isCurrentPlayerConnected && !game?.players.some(p => p.initialSeatWind === WindDirection.South) && (
              <Button
                loading={loading}
                onClick={() => connectToGame(WindDirection.South)}
                color="teal"
              >
                Sit
              </Button>
            )}
          </Grid.Column>
          <Grid.Column width={3}></Grid.Column>
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
