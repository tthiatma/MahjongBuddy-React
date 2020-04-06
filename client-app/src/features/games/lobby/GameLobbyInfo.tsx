import React from 'react';
import { Segment, Grid, Icon } from 'semantic-ui-react';
import { IGame } from '../../../app/models/game';
import {format} from 'date-fns';

const GameLobbyInfo: React.FC<{game: IGame}> = ({game}) => {
  return (
    <Segment.Group>
      <Segment attached='top'>
        <Grid>
          <Grid.Column width={1}>
            <Icon size='large' color='teal' name='info' />
          </Grid.Column>
          <Grid.Column width={15}>
            <p>{game.title}</p>
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment attached>
        <Grid verticalAlign='middle'>
          <Grid.Column width={1}>
            <Icon name='calendar' size='large' color='teal' />
          </Grid.Column>
          <Grid.Column width={15}>
            <span>{format(game.date, 'eeee do MMMM')} at {format(game.date!, 'h:mm a')}</span>
          </Grid.Column>
        </Grid>
      </Segment>
    </Segment.Group>
  );
};

export default GameLobbyInfo;
