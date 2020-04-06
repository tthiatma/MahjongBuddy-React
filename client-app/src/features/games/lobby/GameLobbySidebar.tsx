import React, { Fragment } from 'react';
import { Segment, List, Item, Label, Image } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IPlayer } from '../../../app/models/game';
import { observer } from 'mobx-react-lite';

interface IProps{
  players: IPlayer[];
}

const GameLobbySidebar: React.FC<IProps> = ({players}) => {
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
      <Segment attached>
        <List relaxed divided>
          {players.map(player => (
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
                  <Link to={`/profile/${player.userName}`}>
                    {player.displayName}
                  </Link>
                </Item.Header>
              </Item.Content>
            </Item>
          ))}
        </List>
      </Segment>
    </Fragment>
  );
};

export default observer(GameLobbySidebar);
