import React, { useContext } from 'react';
import { Segment, Item, Header, Button } from 'semantic-ui-react';
import { IGame } from '../../../app/models/game';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import {format} from 'date-fns';
import { RootStoreContext } from '../../../app/stores/rootStore';

const gameImageTextStyle = {
  position: 'absolute',
  bottom: '5%',
  left: '5%',
  width: '100%',
  height: 'auto',
  color: 'white'
};

const GameLobbyHeader: React.FC<{game: IGame}> = ({game}) => {
  const rootStore = useContext(RootStoreContext);
  const {connectToGame, disconnectFromGame, loading} = rootStore.gameStore;
  return (
    <Segment.Group>
      <Segment basic attached="top" style={{ padding: "0" }}>
        <Segment style={gameImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size="huge"
                  content={game.title}
                  style={{ color: "white" }}
                />
                <p>{format(game.date, "eeee do MMMM")}</p>
                <p>
                  Hosted by <strong>Bob</strong>
                </p>
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      <Segment clearing attached="bottom">
        {game.isHost ? (
          <Button
            as={Link}
            to={`/manage/${game.id}`}
            color="orange"
            floated="right"
          >
            Edit Game
          </Button>
        ) : game.isCurrentPlayerConnected ? (
          <Button loading={loading} onClick={disconnectFromGame}>
            Leave
          </Button>
        ) : (
          <Button loading={loading} onClick={connectToGame} color="teal">
            Join
          </Button>
        )}
      </Segment>
    </Segment.Group>
  );
};

export default observer(GameLobbyHeader);
