import React from 'react';
import { Segment, Item, Header, Button } from 'semantic-ui-react';
import { IGame } from '../../../app/models/game';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import {format} from 'date-fns';

const activityImageTextStyle = {
  position: 'absolute',
  bottom: '5%',
  left: '5%',
  width: '100%',
  height: 'auto',
  color: 'white'
};

const GameLobbyHeader: React.FC<{game: IGame}> = ({game}) => {
  return (
    <Segment.Group>
      <Segment basic attached='top' style={{ padding: '0' }}>
        <Segment style={activityImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size='huge'
                  content={game.title}
                  style={{ color: 'white' }}
                />
                <p>{format(game.date, 'eeee do MMMM')}</p>
                <p>
                  Hosted by <strong>Bob</strong>
                </p>
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      <Segment clearing attached='bottom'>
        <Button color='teal'>Join Game</Button>
        <Button>Cancel attendance</Button>
        <Button as={Link} to={`/manage/${game.id}`} color='orange' floated='right'>
          Manage Event
        </Button>
      </Segment>
    </Segment.Group>
  );
};

export default observer(GameLobbyHeader);
