import React, { Fragment } from "react";
import { Item, Button, Segment, Icon, Label } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { IGame } from "../../../app/models/game";
import GameListItemPlayers from "./GameListItemPlayers";
import { format } from "date-fns";

const GameListItem: React.FC<{ game: IGame }> = ({ game }) => {
  const host = game.players.filter((p) => p.isHost)[0];
  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image
              size="tiny"
              circular
              src={host?.image || "/assets/user.png"}
              style={{ marginBottom: 3 }}
            />
            <Item.Content>
              <Item.Header as={Link} to={`/games/${game.id}`}>
                {game.title}
              </Item.Header>
              <Item.Description>
                {host && (
                  <Fragment>
                    Hosted by{" "}
                    <Link to={`profile/${host.userName}`}>
                      {host?.displayName}
                    </Link>
                  </Fragment>
                )}
              </Item.Description>
              {game.isHost && (
                <Item.Description>
                  <Label
                    basic
                    color="orange"
                    content="you are hosting this game"
                  />
                </Item.Description>
              )}
              {game.isCurrentPlayerConnected && !game.isHost && (
                <Item.Description>
                  <Label
                    basic
                    color="green"
                    content="you are connected to this game"
                  />
                </Item.Description>
              )}
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <Icon name="clock" /> {format(game.date, "h:mm a")}
      </Segment>
      <Segment secondary>
        <GameListItemPlayers players={game.players} />
      </Segment>
      <Segment clearing>
        <Button
          as={Link}
          to={`/games/${game.id}`}
          floated="right"
          content="View"
          color="blue"
        />
      </Segment>
    </Segment.Group>
  );
};

export default GameListItem;
