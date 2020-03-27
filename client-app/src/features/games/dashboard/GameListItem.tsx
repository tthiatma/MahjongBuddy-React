import React from "react";
import { Item, Button,  Segment, Icon } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { IGame } from "../../../app/models/game";
import GameListItemPlayers from "./GameListItemPlayers";
import {format} from 'date-fns';

const GameListItem: React.FC<{ game: IGame }> = ({ game }) => {
  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image size="tiny" circular src="/assets/user.png" />
            <Item.Content>
              <Item.Header as="a">{game.title}</Item.Header>
              <Item.Description>Hosted by Tonny</Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <Icon name="clock" /> {format(game.date, 'h:mm a')}
      </Segment>
      <Segment secondary>
        <GameListItemPlayers players={game.players} />
      </Segment>
      <Segment clearing>
        <Button
          as={Link}
          to={`/lobby/${game.id}`}
          floated="right"
          content="View"
          color="blue"
        />
      </Segment>
    </Segment.Group>
  );
};

export default GameListItem;
