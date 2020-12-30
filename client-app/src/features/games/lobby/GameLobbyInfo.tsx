import React, { useState } from "react";
import { Segment, Grid, Icon, Label, Button, Popup } from "semantic-ui-react";
import { IGame } from "../../../app/models/game";
import copy from "clipboard-copy";

const GameLobbyInfo: React.FC<{ game: IGame }> = ({ game }) => {
  const [isOpen, setIsOpen ] = useState(false);
  const timeoutLength = 2500;
  let timeout: any;

  const handleOpen = () => {
    setIsOpen(true)
    timeout = setTimeout(() => {
      setIsOpen(false)
    }, timeoutLength)
  }

  const handleClose = () => {
    setIsOpen(false)
    clearTimeout(timeout)
  }
  return (
    <Segment.Group>
      <Segment attached="top">
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="teal" name="info" />
          </Grid.Column>
          <Grid.Column width={15}>
          <Popup
            trigger={<Button onClick={() => copy(game.code)} icon labelPosition='right'><Icon name='clipboard' />{game.code}</Button>}
            content={`Successfully copied to clipboard!`}
            on='click'
            open={isOpen}
            onClose={handleClose}
            onOpen={handleOpen}
            position='top right'
          />
            {" "}Tell your buddies to enter{" "}
            <strong>{game.code}</strong> to join your game
          </Grid.Column>
        </Grid>
      </Segment>

      <Segment>
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="teal" name="cog" />
          </Grid.Column>
          <Grid.Column width={15}>
            <strong>
              <Label>{`Min Point: ${game.minPoint} pts`}</Label>
              <Label>{`Max Point: ${game.maxPoint} pts`}</Label>
            </strong>
          </Grid.Column>
        </Grid>
      </Segment>
    </Segment.Group>
  );
};

export default GameLobbyInfo;
