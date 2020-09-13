import React from "react";
import { Container, Segment, Header, List } from "semantic-ui-react";

const RulesBasic = () => {
  return (
    <Container>
      <Segment>
        <Header size="medium">Supported Rule type</Header>
        <p>Hongkong Mahjong rule</p>
      </Segment>

      <Segment>
        <Header size="medium">Pre-Game</Header>
        <List bulleted className="ruleList">
          <List.Item>
            <p>Creating a game</p>
            <p>
              When creating a game, you can set{" "}
              <strong>min point and max point</strong> for the game. Also, you
              are now the "Host" of the game and has several responsibilities{" "}
            </p>
          </List.Item>
          <List.Item>
            <p>Host</p>
            <p>
              -Invite your buddy to your game by letting them know of the Game#
            </p>
            <p>
              -Once there are four players joined the game, you can either
              shuffle the players into random seats or ask them to pick a seat.
            </p>
            <p>
              -Everytime a round is over, the result of the round will be shown
              and only the host can move the game to the next round.
            </p>
          </List.Item>
        </List>
      </Segment>

      <Segment>
        <Header size="medium">Game</Header>
        <List bulleted className="ruleList">
          <List.Item>
            <p>Throwing a tile</p>
            <p>
              When a player throws a tile, the game will check if there is any
              "Action" such as chow/pong/kong from other players. Also, there
              will be a slight delay for every tile thrown to the board to mask
              a clue that other players might need the discarded tile.
            </p>
          </List.Item>
          <List.Item>
            <p>Action priority order</p>
            <p>
              Action has priority order and it goes by this order{" "}
              <strong>
                Win {">"} Pong/Kong {">"} Chow
              </strong>
              . if a player with high priority action pressed the "Skip" button,
              then it goes to lower priority action and the action will be shown
              to other players.
            </p>
          </List.Item>
          <List.Item>
            <p>Last Tile</p>
            <p>
              When there is only 1 tile left, the player will be presented an
              option to either pick the last tile or to give up (not picking the
              last tile)
            </p>
          </List.Item>

          <List.Item>
            <p>Bao (Penalty)</p>
            <p>Penalty applies to below scenarios:</p>
            <p>
              <strong>-AllOneSuit hand</strong>
            </p>
            <p>
              Let's say there are players named Mei and Peter. Mei showing 3
              melds with one suit. Peter discarded that same suit and Mei
              chow/pong/kong the tile. This means Peter has "Bao". If Mei able
              to self pick the winning tile(only applies to self pick) and it's
              a AllOneSuit hand, then Peter will have to pay all the losses to
              Mei (winning pts * 3).
            </p>
            <p>
              <strong>-SmallDragon or BigDragon hand</strong>
            </p>
            <p>
              Let's say there are players named Mei and Peter. Mei showing 1
              meld of a dragon suit. Peter discarded another dragon suit and Mei
              pong the dragon tile. This means Peter has "Bao". If Mei able to
              self pick the winning tile(only applies to self pick) and it's a
              SmallDragon or BigDragon Hand, then Peter will have to pay all the
              losses to Mei (winning pts * 3).
            </p>
          </List.Item>
        </List>
      </Segment>
    </Container>
  );
};

export default RulesBasic;
